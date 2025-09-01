using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MCS.Framework.Security;
using MCS.Framework.Web;

namespace MCS.Framework.AuditTrail
{
    public enum AuditActions
    {
        Insert,
        Update,
        Delete
    }

    public class AuditTrailFactory
    {
        private DbContext _dbContext;

        public AuditTrailFactory(DbContext context)
        {
            _dbContext = context;
        }

        public AuditInfo GetAudit(Tuple<EntityState, DbEntityEntry, DbPropertyValues> entry)
        {
            IUser loggedInUser = UserContext.LoggedInUser;

            AuditInfo auditInfo = new AuditInfo();

            if (loggedInUser != null)
            {
                auditInfo.UserId = loggedInUser.Id;
                auditInfo.IPAddress = loggedInUser.IPAddress;
            }

            auditInfo.Id = Guid.NewGuid();
            auditInfo.EntityName = ObjectContext.GetObjectType(entry.Item2.Entity.GetType()).ToString();
            auditInfo.PrimaryKeyValue = GetKeyValue(entry.Item2);
            auditInfo.Date = DateTime.Now;
            auditInfo.TransactionId = GetTransactionIdValue(entry.Item2);

            switch (entry.Item1)
            {
                case EntityState.Added:
                    {
                        auditInfo.Details = SetAddedProperties(entry.Item2);
                        auditInfo.OperationType = OperationType.Insert;
                    }
                    break;
                case EntityState.Deleted:
                    {
                        auditInfo.Details = SetDeletedProperties(entry.Item3);
                        auditInfo.OperationType = OperationType.Delete;
                    }
                    break;
                case EntityState.Modified:
                    {
                        auditInfo.Details = SetModifiedProperties(entry.Item2, entry.Item3);
                        auditInfo.OperationType = OperationType.Update;
                    }
                    break;
            }

            return auditInfo;
        }

        private IList<AuditInfoDetail> SetAddedProperties(DbEntityEntry entry)
        {
            IList<AuditInfoDetail> auditInfoDetails = new List<AuditInfoDetail>();

            foreach (string propertyName in entry.CurrentValues.PropertyNames)
            {
                if (IsKeyControlProperty(propertyName))
                    continue;

                AuditInfoDetail auditInfoDetail = new AuditInfoDetail();

                object newVal = entry.CurrentValues[propertyName];

                if (newVal != null)
                {
                    auditInfoDetail.PropertyName = propertyName;
                    auditInfoDetail.PropertyNewValue = newVal.ToString();

                    auditInfoDetails.Add(auditInfoDetail);
                }
            }

            return auditInfoDetails;
        }

        private IList<AuditInfoDetail> SetDeletedProperties(DbPropertyValues dbValues)
        {
            IList<AuditInfoDetail> auditInfoDetails = new List<AuditInfoDetail>();

            //DbPropertyValues dbValues = entry.GetDatabaseValues();

            foreach (var propertyName in dbValues.PropertyNames)
            {
                if (IsKeyControlProperty(propertyName))
                    continue;

                AuditInfoDetail auditInfoDetail = new AuditInfoDetail();

                object oldVal = dbValues[propertyName];

                if (oldVal != null)
                {
                    auditInfoDetail.PropertyName = propertyName;
                    auditInfoDetail.PropertyOldValue = oldVal.ToString();

                    auditInfoDetails.Add(auditInfoDetail);
                }
            }

            return auditInfoDetails;
        }

        private IList<AuditInfoDetail> SetModifiedProperties(DbEntityEntry entry, DbPropertyValues dbValues)
        {
            IList<AuditInfoDetail> auditInfoDetails = new List<AuditInfoDetail>();

            //DbPropertyValues dbValues = entry.GetDatabaseValues();

            foreach (var propertyName in entry.OriginalValues.PropertyNames)
            {
                if (IsKeyControlProperty(propertyName))
                    continue;

                AuditInfoDetail auditInfoDetail = new AuditInfoDetail();

                object oldVal = dbValues[propertyName];
                object newVal = entry.CurrentValues[propertyName];

                if (oldVal != null && newVal != null && !Equals(oldVal, newVal))
                {
                    auditInfoDetail.PropertyName = propertyName;
                    auditInfoDetail.PropertyOldValue = oldVal.ToString();
                    auditInfoDetail.PropertyNewValue = newVal.ToString();

                    auditInfoDetails.Add(auditInfoDetail);
                }
            }

            return auditInfoDetails;
        }

        public string GetKeyValue(DbEntityEntry entry)
        {
            ObjectStateEntry objectStateEntry =
                ((IObjectContextAdapter)_dbContext).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);

            string id = null;

            if (objectStateEntry.EntityKey.EntityKeyValues != null &&
                objectStateEntry.EntityKey.EntityKeyValues.Count() > 0)
            {
                id = objectStateEntry.EntityKey.EntityKeyValues[0].Value.ToString();
            }

            return id;
        }

        public int GetTransactionIdValue(DbEntityEntry entry)
        {
            int transactionId = -1;

            if (GetTableName(entry).ToLower() == "transaction")
            {
                transactionId = Convert.ToInt32(GetKeyValue(entry));
                return transactionId;
            }

            foreach (string propertyName in entry.CurrentValues.PropertyNames)
            {
                if (propertyName.ToLower() == "transactionid")
                {
                    transactionId = entry.CurrentValues[propertyName] != null ? (int)entry.CurrentValues[propertyName]  : transactionId;
                    break;
                }
            }

            return transactionId;
        }

        private string GetTableName(DbEntityEntry dbEntry)
        {
            TableAttribute tableAttribute =
                dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            string tableName = (tableAttribute != null) ? tableAttribute.Name : dbEntry.Entity.GetType().Name;

            return tableName;
        }

        private bool IsKeyControlProperty(string propertyName)
        {
            string[] controlKeys = new string[] { "CreatedOn", "CreatedBy", "ModefiedOn", "ModefiedBy" };

            return controlKeys.Contains(propertyName);
        }
    }
}
