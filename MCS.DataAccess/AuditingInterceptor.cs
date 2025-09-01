using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MCS.Framework.AuditTrail;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class AuditingInterceptor : IInterceptor
    {
        private IList<MCS.Domain.Audit> audits = null;

        public void DoWork(IDbContext dbContext, List<Tuple<EntityState, DbEntityEntry, DbPropertyValues>> entities)
        {
            try
            {
                MCSDbContext context = dbContext as MCSDbContext;

                AuditTrailFactory auditTrail = new AuditTrailFactory(context);

                //IEnumerable<DbEntityEntry> entityList = context.ChangeTracker.Entries().Where(p => p.State == EntityState.Added ||
                //    p.State == EntityState.Deleted || p.State == EntityState.Modified);

                audits = new List<MCS.Domain.Audit>();

                foreach (Tuple<EntityState, DbEntityEntry, DbPropertyValues> entity in entities)
                {
                    if (entity.Item2.Entity.GetType().GetInterfaces().Contains(typeof(IAuditable)))
                    {
                        AuditInfo auditInfo = auditTrail.GetAudit(entity);

                        MCS.Domain.Audit audit = new MCS.Domain.Audit()
                        {
                            UserId = auditInfo.UserId,
                            IPAddress = auditInfo.IPAddress,
                            OperationType = auditInfo.OperationType,
                            EntityName = auditInfo.EntityName,
                            PrimaryKeyValue = auditInfo.PrimaryKeyValue,
                            Date = DateTime.Now,
                            TransactionId = auditInfo.TransactionId
                        };

                        audit.Details = new List<AuditDetail>();

                        auditInfo.Details.ToList().ForEach(d =>
                        {
                            AuditDetail auditDetail = new AuditDetail
                            {
                                Audit = audit,
                                PropertyName = d.PropertyName,
                                PropertyOldValue = d.PropertyOldValue,
                                PropertyNewValue = d.PropertyNewValue
                            };

                            audit.Details.Add(auditDetail);
                        });

                        audits.Add(audit);
                    }
                }

                if (audits.Count > 0)
                {
                    context.Audits.AddRange(audits);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
