using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Repository.Entity;

namespace Repository.Interceptor;

public class InsertInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is null) return result;

        var changeTracker = eventData.Context.ChangeTracker;
        
        // Added interceptor
        var addedList = changeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Added).ToList();
        foreach (var entry in addedList)
        {
            entry.Property(entity => entity.CreatedDate).CurrentValue = DateTime.Now;
            entry.Property(entity => entity.UpdatedDate).CurrentValue = entry.Property(entity => entity.CreatedDate).CurrentValue;
            entry.Property(entity => entity.DelFlag).CurrentValue = false;
        }
        
        // Updated interceptor
        var modifiedList = changeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Modified).ToList();
        foreach (var entry in modifiedList)
        {
            entry.Property(entity => entity.UpdatedDate).CurrentValue = DateTime.Now;
        }
        return result;
    }
}