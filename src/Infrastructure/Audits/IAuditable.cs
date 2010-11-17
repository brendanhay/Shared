using System.Collections.Generic;

namespace Infrastructure.Audits
{
    public interface IAuditable<T> : IAuditable
    {
        IList<T> Audits { get; }
    }

    public interface IAuditable
    {
        IAudit CreatedAudit { get; }

        IAudit LastAudit { get; }

        IAudit CreateAudit(AuditAction action);
    }
}
