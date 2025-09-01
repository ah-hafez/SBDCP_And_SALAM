using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class LinkBL : BaseBL, ILinkBL
    {
        public int AddLink(Link link)
        {
            try
            {
                ILinkRepository linkRepository = IoC.Resolve<LinkRepository>();
                var addLink = linkRepository.AddLink(link);
                CacheHelper.Remove(CachedObjectsKey.Links, "ar");
                CacheHelper.Remove(CachedObjectsKey.Links, "en");
                return addLink;
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

        public void UpdateLink(Link link)
        {
            try
            {
                ILinkRepository linkRepository = IoC.Resolve<LinkRepository>();
                linkRepository.UpdateLink(link);
                CacheHelper.Remove(CachedObjectsKey.Links, "ar");
                CacheHelper.Remove(CachedObjectsKey.Links, "en");
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

        public Link GetLinkById(int linkId)
        {
            try
            {
                ILinkRepository linkRepository = IoC.Resolve<LinkRepository>();
                return linkRepository.Get(linkId);
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

        public void DeleteLinks(IList<int> ids, out IList<int> linkTypesCannotBeDeleted)
        {
            try
            {
                ILinkRepository linkRepository = IoC.Resolve<LinkRepository>();
                linkTypesCannotBeDeleted = new List<int>();

                foreach (int id in ids)
                {
                    if (linkRepository.CheckIfLinkTypeUsed(id))
                    {
                        linkTypesCannotBeDeleted.Add(id);

                        continue;
                    }
                    linkRepository.DeleteLink(id);
                }
                CacheHelper.Remove(CachedObjectsKey.Links, "ar");
                CacheHelper.Remove(CachedObjectsKey.Links, "en");
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

        public IList<Link> GetLinks(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                ILinkRepository linkRepository = IoC.Resolve<LinkRepository>();
                return linkRepository.GetLinks(searchCriteria, out rowsCount);
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

        public IList<Link> GetLinks(TransactionCategories transactionCategories, string cultureName)
        {
            try
            {
                IList<Link> links = CacheHelper.Get(CachedObjectsKey.Links, cultureName) as IList<Link>;
                if (links == null || links.Count == 0)
                {
                    ILinkRepository linkRepository = IoC.Resolve<LinkRepository>();

                    links = linkRepository.GetLinks(transactionCategories, cultureName);

                    CacheHelper.Insert(CachedObjectsKey.Links, links, cultureName);
                }
                return links;
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
    }
}
