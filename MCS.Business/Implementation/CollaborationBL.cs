using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class CollaborationBL : BaseBL, ICollaborationBL
    {
        public void AddCollaboration(Collaboration conversation)
        {
            try
            {
                ICollaborationRepository conversationRepository = IoC.Resolve<CollaborationRepository>();

                conversationRepository.AddCollaboration(conversation);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public bool HasCollaboration(int toUserId, int transactionId)
        {
            try
            {
                ICollaborationRepository collaborationRepository = IoC.Resolve<CollaborationRepository>();

                int collaborationCount = collaborationRepository.GetCollaborationCount(c => ((c.ReceiverId == toUserId && c.SenderId == User.Id) || (c.SenderId == toUserId && c.ReceiverId == User.Id))
                                                                                     && c.TransactionId == transactionId);

                if (collaborationCount > 0)
                {
                    return true;
                }

                return false;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public ChatNotificationsInfo GetChatNotifications()
        {
            try
            {
                ChatNotificationsInfo chatNotificationsInfo = new ChatNotificationsInfo();

                ICollaborationRepository collaborationRepository = IoC.Resolve<CollaborationRepository>();

                IList<Collaboration> collaborations = collaborationRepository.GetCollaborations(c => c.ReceiverId == User.Id && c.Status == CollaborationMessageStatus.Unread).ToList();

                chatNotificationsInfo.TotalUserNotifications = collaborations.Count;

                return chatNotificationsInfo;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<CollaborationUserInfo> GetAllCollaborationUsers(string cultureName)
        {
            try
            {
                ICollaborationRepository collaborationRepository = IoC.Resolve<CollaborationRepository>();

                return collaborationRepository.GetAllCollaborationUsers(User.Id, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Collaboration> GetCollaboration(int toUserId, int pageSize, string cultureName)
        {
            try
            {
                ICollaborationRepository conversationRepository = IoC.Resolve<CollaborationRepository>();

                List<Collaboration> unReadCollaboration = conversationRepository.GetCollaborations(c => ((c.ReceiverId == toUserId && c.SenderId == User.Id) || (c.SenderId == toUserId && c.ReceiverId == User.Id))

                                                                                              && c.Status == CollaborationMessageStatus.Unread).ToList();


                {
                    foreach (Collaboration collaboration in unReadCollaboration)
                    {
                        collaboration.Status = CollaborationMessageStatus.Read;

                        conversationRepository.UpdateCollaboration(collaboration);
                    }


                }

                return conversationRepository.GetCollaborations(c => (c.ReceiverId == toUserId && c.SenderId == User.Id) || (c.SenderId == toUserId && c.ReceiverId == User.Id), pageSize, cultureName);

            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Collaboration> GetCollaboration(int toUserId, int pageSize, int startIndexId, string cultureName)
        {
            try
            {
                ICollaborationRepository conversationRepository = IoC.Resolve<CollaborationRepository>();

                return conversationRepository.GetCollaborations(c => ((c.ReceiverId == toUserId && c.SenderId == User.Id) || (c.SenderId == toUserId && c.ReceiverId == User.Id))
                                                   && (c.Id < startIndexId), pageSize, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        private int GetChatNotificationCount(int userId)
        {
            ICollaborationRepository collaborationRepository = IoC.Resolve<CollaborationRepository>();

            IList<Domain.Collaboration> collaborations =
                collaborationRepository.GetCollaborations(c => (c.ReceiverId == User.Id && c.SenderId == userId) &&
                    c.Status == CollaborationMessageStatus.Unread);

            if (collaborations == null)
            {
                return 0;
            }

            return collaborations.Count;
        }
    }
}
