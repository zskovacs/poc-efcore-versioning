namespace EntityFrameworkVersioning.Persistence;

public class VersionInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _dateTime;

    public VersionInterceptor(TimeProvider dateTime)
    {
        _dateTime = dateTime;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        ApplyVersioningConcept(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyVersioningConcept(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    private void ApplyVersioningConcept(DbContext? context)
    {
        if (context == null) 
            return;
        
        foreach (var entry in context.ChangeTracker.Entries().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                {
                    ApplyVersioningConceptForModifiedEntity(entry, context);
                    break;
                }
                case EntityState.Added:
                {
                    ApplyVersioningConceptForAddedEntity(entry, context);
                    break;
                }
                case EntityState.Deleted:
                {
                    ApplyVersioningConceptForDeletedEntity(entry);
                    break;
                };
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void ApplyVersioningConceptForDeletedEntity(EntityEntry entry)
    {
        if (entry.Entity is not IVersionEntity entity) 
            return;
        
        entry.Reload();
        entity.ValidTo = _dateTime.GetUtcNow().DateTime;
        entry.State = EntityState.Modified;
    }
    
    private void ApplyVersioningConceptForModifiedEntity(EntityEntry entry, DbContext context)
    {
        if (entry.Entity is not IVersionEntity entity)
            return;

        var newEntity = (entity.Clone() as IVersionEntity)!;
        newEntity.ValidFrom = _dateTime.GetUtcNow().DateTime;
        newEntity.ValidTo = DateTime.MaxValue;
        newEntity.Revision++;
        context.Add(newEntity);
        
        entry.Reload();
        entity.ValidTo = _dateTime.GetUtcNow().DateTime;
        entry.State = EntityState.Modified;
    }
    
    private void ApplyVersioningConceptForAddedEntity(EntityEntry entry, DbContext context)
    {
        if (entry.Entity is not IVersionEntity entity)
            return;

        var baseInstance = (IVersionBaseEntity)entry.Entity.GetType().GetProperty("Base").GetValue(entity, null);
        baseInstance.Id = baseInstance.Id == Guid.Empty ? Guid.NewGuid() : baseInstance.Id;
        
        context.Add(baseInstance);

        entity.BaseId = baseInstance.Id;
        entity.ValidFrom = _dateTime.GetUtcNow().DateTime;
        entity.ValidTo = DateTime.MaxValue;
        entity.Revision = 1;
    }
}