using System;
using Infrastructure.Domain;

namespace Infrastructure.Audits
{
    public interface IAudit
    {
        int AuditId { get; set; }

        string AuditUser { get; set; }

        byte[] IPv4Address { get; set; }

        AuditAction Action { get; set; }

        DateTime Time { get; set; }
    }

    public interface IAudit<T> : IAudit where T : IEntity
    {
        T Entity { get; set; }
    }

    public abstract class Audit<T> : IAudit<T> where T : IEntity
    {
        public virtual int AuditId { get; set; }

        public virtual string AuditUser { get; set; }

        public virtual AuditAction Action { get; set; }

        public virtual DateTime Time { get; set; }

        public virtual byte[] IPv4Address { get; set; }

        public virtual T Entity { get; set; }
    }
}
