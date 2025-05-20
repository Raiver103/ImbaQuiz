using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ImbaQuiz.Domain.Interfaces;

namespace ImbaQuiz.infrastructure.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
    private readonly ILogSender _logSender;

        public AuditInterceptor(ILogSender logSender)
        {
            _logSender = logSender;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;

            if (context == null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var entityName = entry.Entity.GetType().Name;
                var state = entry.State.ToString();
                var log = $"EF Change: Entity={entityName}, State={state}"; 
                _logSender.SendLog(log);
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}