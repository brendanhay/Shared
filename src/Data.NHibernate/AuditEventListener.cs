using System;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using Infrastructure.Audits;

namespace Data.NHibernate
{
    internal class AuditEventListener : IPreInsertEventListener, IPreUpdateEventListener,
        IPreDeleteEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            InsertAudit(@event, AuditAction.Insert);

            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            InsertAudit(@event, AuditAction.Update);

            return false;
        }

        public bool OnPreDelete(PreDeleteEvent @event)
        {
            return false;
        }

        private static void InsertAudit(IPreDatabaseOperationEventArgs @event, AuditAction action)
        {
            var metadata = @event.Persister.ClassMetadata as SingleTableEntityPersister;

            if (metadata != null && metadata.TableName.EndsWith(AuditConstants.TABLE_SUFFIX,
                StringComparison.OrdinalIgnoreCase)) {
                return;
            }

            var auditable = @event.Entity as IAuditable;

            if (auditable == null) {
                return;
            }

            auditable.CreateAudit(action);
        }
    }
}
