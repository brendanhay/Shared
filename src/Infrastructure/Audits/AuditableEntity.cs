using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using Infrastructure.Domain;

namespace Infrastructure.Audits
{
    public abstract class AuditableEntity<T, TAudit> : Entity, IAuditable<TAudit>
        where TAudit : IAudit<T>, new()
        where T : class, IEntity
    {
        private readonly IList<TAudit> _audits = new List<TAudit>();

        #region IAuditable<T> Members

        public virtual IList<TAudit> Audits { get { return _audits; } }

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
            var audit = new TAudit {
                Action = action,
                Time = DateTime.Now,
                AuditUser = GetCurrentIdentity().Name,
                IPv4Address = GetCurrentIPv4Address(),
                Entity = this as T
            };

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
