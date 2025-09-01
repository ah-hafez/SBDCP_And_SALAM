using MCS.Common.TransactionContext;
using MCS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MCS.DataAccess
{
    public class OnlineUserRepository : BaseRepository<OnlineUser>, IOnlineUserRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public OnlineUserRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public bool AddUserOnline(int userId, int OrgUnitId, string connectionId)
        {
            try
            {
                var userOnline = _oMCSDbContext.OnlineUsers.Where(x => x.UserId == userId).FirstOrDefault();
                if (userOnline != null)
                {
                    userOnline.ConnectionId = connectionId;
                    userOnline.ModefiedBy = userId;
                    userOnline.ModefiedOn = DateTime.Now;
                }
                else
                {
                    userOnline = new OnlineUser
                    {
                        ConnectionId = connectionId,
                        UserId = userId,
                        OrgUnitId = OrgUnitId,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now
                    };

                    _oMCSDbContext.OnlineUsers.Add(userOnline);
                }

                _oMCSDbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public bool UpdateUserOnline(int userId, int OrgUnitId)
        {
            try
            {
                var onlineUserByRef = _oMCSDbContext.OnlineUsers.FirstOrDefault(x => x.UserId == userId);
                if (onlineUserByRef != null)
                {
                    onlineUserByRef.OrgUnitId = OrgUnitId;
                    _oMCSDbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool DeleteOnlineUser(string connectionId)
        {
            try
            {
                var onlineUserByRef = _oMCSDbContext.OnlineUsers.FirstOrDefault(x => x.ConnectionId == connectionId);
                if (onlineUserByRef != null)
                {
                    _oMCSDbContext.OnlineUsers.Remove(onlineUserByRef);
                    _oMCSDbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<OnlineUser> GetOnlineUser()
        {
            try
            {
                var onlineUserByRef = _oMCSDbContext.OnlineUsers.ToList();
                return onlineUserByRef;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion Methods
    }
}
