using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using Infrastructure.Extensions;

namespace Infrastructure.Audits
{
    public abstract class AuditableEntity<T>
        : Entity, IAudit, IAuditable<T> where T : IAudit, new()
    {
        private readonly IList<T> _audits = new List<T>();

        #region IAudit

        int IAudit.AuditId { get; set; }

        string IAudit.AuditUser { get; set; }

        AuditAction IAudit.Action { get; set; }

        DateTime IAudit.Time { get; set; }

        byte[] IAudit.IPv4Address { get; set; }

        #endregion

        #region IAuditable<T> Members

        public virtual IList<T> Audits { get { return _audits; } }

        #endregion

        #region IAuditable Members

        public virtual IAudit CreatedAudit
        {
            get { return Audits.First(audit => audit.Action == AuditAction.Insert); }
        }

        public virtual IAudit LastAudit
        {
            get { return Audits.Last(); }
        }

        IAudit IAuditable.CreateAudit(AuditAction action)
        {
            var audit = new T {
                Action = action,
                Time = DateTime.Now,
                AuditUser = GetCurrentIdentity().Name,
                IPv4Address = GetCurrentIPv4Address()
            };

            this.Extend(audit, 1);

            Audits.Add(audit);

            return audit;
        }

        #endregion

        private static byte[] GetCurrentIPv4Address()
        {
            var request = HttpContext.Current.Request;
            var address = request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                ?? request.UserHostAddress;

            return !string.IsNullOrEmpty(address)
                ? IPAddress.Parse(address).GetAddressBytes()
                : new byte[0];
        }

        private static IIdentity GetCurrentIdentity()
        {
            IIdentity identity = WindowsIdentity.GetCurrent();

            if (HttpContext.Current != null) {
                identity = HttpContext.Current.User.Identity;
            }

            return identity;
        }
    }
}
