using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Security;
using Audit.EntityFramework;
using MCS.Framework.Entities;
using MCS.Framework.Security;
using MCS.Framework.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MCS.Framework.Persistence
{
    public class DbContextBase  : AuditDbContext , IDbContext
    {
        private readonly List<IInterceptor> interceptors = new List<IInterceptor>();

        public DbContextBase() { }
        public DbContextBase(string connectionString) : base(connectionString) { }

        public List<IInterceptor> Interceptors
        {
            get { return this.interceptors; }
        }

        private void Intercept(IDbContext dbContext, List<Tuple<EntityState, DbEntityEntry, DbPropertyValues>> entities)
        {
            this.Interceptors.ForEach(intercept => intercept.DoWork(dbContext, entities));
        }

        public override int SaveChanges()
        {
            try
            {
                List<Tuple<EntityState, DbEntityEntry, DbPropertyValues>> entitiesBeforeSave = new List<Tuple<EntityState, DbEntityEntry, DbPropertyValues>>();
                IEnumerable<DbEntityEntry> entities = this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added ||
                    p.State == EntityState.Modified);

                if (entities.Count() > 0)
                {
                    IUser loggedInUser = UserContext.LoggedInUser;

                    foreach (DbEntityEntry entity in entities)
                    {
                        switch (entity.State)
                        {
                            case EntityState.Added:
                                {
                                    entitiesBeforeSave.Add(new Tuple<EntityState, DbEntityEntry, DbPropertyValues>(EntityState.Added, entity, null));
                                    ((EntityBase)entity.Entity).CreatedOn = DateTime.Now;
                                    ((EntityBase)entity.Entity).CreatedBy = (loggedInUser == null) ? -1 : loggedInUser.Id;
                                    break;
                                }
                            case EntityState.Modified:
                                {
                                    entitiesBeforeSave.Add(new Tuple<EntityState, DbEntityEntry, DbPropertyValues>(EntityState.Modified, entity, entity.GetDatabaseValues()));
                                    object createdBy = entity.OriginalValues.GetValue<object>("CreatedBy");

                                    ((EntityBase)entity.Entity).CreatedOn = entity.OriginalValues.GetValue<DateTime>("CreatedOn");
                                    ((EntityBase)entity.Entity).CreatedBy = (createdBy != null) ? (int)createdBy : -1;
                                    ((EntityBase)entity.Entity).ModefiedOn = DateTime.Now;
                                    ((EntityBase)entity.Entity).ModefiedBy = (loggedInUser == null) ? -1 : loggedInUser.Id;
                                    break;
                                }
                        }
                    }
                }

                int saveChanges = base.SaveChanges();

                Intercept(this, entitiesBeforeSave);

                return saveChanges;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
