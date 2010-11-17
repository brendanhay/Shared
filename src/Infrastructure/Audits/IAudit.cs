using System;

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
}
