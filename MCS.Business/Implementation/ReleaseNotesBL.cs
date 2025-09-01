using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using ReleaseNote =  MCS.Domain.ReleaseNote;
using MCS.Framework.Persistence;

namespace MCS.Business
{
    public class ReleaseNotesBL : BaseBL, IReleaseNotesBL
    {
        public int ReleaseNotesAdd(ReleaseNote release)
        {
            try
            {
                IReleaseNotesRepository oReleaseNotesRepository = IoC.Resolve<IReleaseNotesRepository>();

                release.ReleaseDate = DateTimeUtility.ConvertToDate(release.DateHj);

                return oReleaseNotesRepository.ReleaseNotesAdd(release);
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

        public void ReleaseNotesUpdate(ReleaseNote obj)
        {
            try
            {
                {
                    IReleaseNotesRepository rep = IoC.Resolve<IReleaseNotesRepository>();
                    obj.ReleaseDate = DateTimeUtility.ConvertToDate(obj.DateHj);
                    rep.ReleaseNotesUpdate(obj);
                }
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

        public void ReleaseNotesDelete(IList<int> ids, out IList<int> actionesCannotBeDeleted)
        {
            try
            {
                IReleaseNotesRepository rep = IoC.Resolve<IReleaseNotesRepository>();

                actionesCannotBeDeleted = new List<int>();

                foreach (var id in ids)
                {
                    if (rep.ReleaseNotesCheckIfUsed(id))
                    {
                        actionesCannotBeDeleted.Add(id);

                        continue;
                    }

                    rep.ReleaseNotesDelete(id);
                }
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

        public IList<ReleaseNote> ReleaseNotesSelect(SearchCriteria searchCriteria, out int rowsCount, string cultureName)
        {
            try
            {
                IReleaseNotesRepository rep = IoC.Resolve<IReleaseNotesRepository>();

                return rep.ReleaseNotesSelect(searchCriteria, out rowsCount, cultureName);
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

        public ReleaseNote ReleaseNotesSelectById(int noteId)
        {
            try
            {
                IReleaseNotesRepository rep = IoC.Resolve<IReleaseNotesRepository>();


                return rep.Get(noteId);
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
