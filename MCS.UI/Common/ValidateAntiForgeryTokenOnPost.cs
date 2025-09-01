using MCS.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MCS.UI.Common
{
    public class ValidateAntiForgeryTokenOnPost : IAuthorizationFilter
    {
        public ValidateAntiForgeryTokenOnPost()
        {
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                List<string> skippedMethods = new List<string>
            {
                "UpdateTrayTransactions","GetTasksByFilter",
                "CheckIfHasMainArchive","UploadFile",
                "UploadExternalCopyAttachments","AddExternalCopy",
                "MoveElectronicTransactionsCopies","MoveTransaction",
                "SendTaskReply","FollowUpAddNote","DeleteExternalParty",
                "ExportToPdf","UploadMainDocAttachments","GetImagePath",
                "DeletePage","ExportXml","ExportAnnotations","GetImagePath",
                "DeleteImageFile", "DeleteDocumentTemp", "ExportUsersWithGroupsReportToExcel","UsersReportExportToPdf",
                "UpdateGridUserProfile", "UpdateUserCategoriesGrid", "UpdateAttachmentTypeGrid",
                "UpdateLinkGrid", "UpdateTransactionTypeGrid", "UpdateLetterTypeGrid", "UpdatePriorityGrid",
                "UpdateGroupsGrid", "UpdateActionGrid", "UpdateManagersGrid", "UpdateGridOrgUnitUser", "UpdateGridOrgUnitLink",
                "LogTransactionAction","SearchTransactionLinks" , "GetExternalSearchById","GetDirectedToUsersByOrgUnitId", "GetUsersByOrgUnitId", "GetOrgStructureSearchById",
                "GetDeliveryReportByTransactionId", "UpdateGridReservations", "SecretaryTransactionSearch", "HideTransactionAssignment","ChangePageOrder","UpdateDocument","MarkDocumentAsRead","GetDesign","UploadExplanationAttachments","SaveAssignmentPaper","PdfAssignmentPaper","Approved","UploadAttachments"
            };

                if (filterContext.HttpContext.Request.HttpMethod != "GET" &&
                    skippedMethods.All(s => !filterContext.HttpContext.Request.Path.Contains(s)))
                {
                    AntiForgery.Validate();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionWithMessage(ex, "Url Exception :" + filterContext.HttpContext.Request.Path);
                throw;
            }

        }
    }
}