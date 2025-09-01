using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;
using MCS.Common;
using MCS.DTO;

namespace MCS.UI.TenantsAdmin
{
    public static class UIHelper
    {
        public static string SystemDateFormat
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SystemDateFormat"]))
                {
                    return ConfigurationManager.AppSettings["SystemDateFormat"];
                }

                throw new ConfigurationErrorsException("SystemDateFormat not defiened in the Web.config file");
            }
        }
       
        public static int PageSize
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["GridPageSize"]))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["GridPageSize"]);
                }

                throw new Exception("GridPageSize not configured in the web config file");
            }


        }

        public static string RenderRazorViewToHtml(ControllerContext context, string viewName, object model)
        {            
            context.Controller.ViewData.Model = model;

            using (StringWriter stringWriter = new StringWriter())
            {
                ViewEngineResult viewResult =
                    ViewEngines.Engines.FindPartialView(context, viewName);

                ViewContext viewContext =
                    new ViewContext(context, viewResult.View, context.Controller.ViewData, context.Controller.TempData, stringWriter);

                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(context, viewResult.View);

                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public static TreeViewModel BulidTree(List<OrgUnitDTO> orgUnitDTOs, int selectedOrgUnitId = -1)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            if (orgUnitDTOs == null)
            {
                return tree;
            }

            if (orgUnitDTOs.Count == 0)
            {
                return tree;
            }

            OrgUnitDTO userOrgUnit = new OrgUnitDTO();
            userOrgUnit = orgUnitDTOs.Where(o => o.Id == SessionInfo.OrgUnit).SingleOrDefault();

            tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };

            OrgUnitDTO root = orgUnitDTOs.Where(o => o.ParentId == -1).SingleOrDefault();

            TreeNode baseTreeNode = new TreeNode()
            {
                DepartmentNumber = root.Number.ToString(),
                IsSelected = false,
                Selectable = false,
                Name = root.Name,
                Id = root.Id
            };

            if (root.Id == selectedOrgUnitId)
            {
                baseTreeNode.IsSelected = true;
            }

            if (userOrgUnit != null)
            {

                userOrgUnit.LinkUnitsKeys.ForEach(l =>
                {
                    if (l == root.Id)
                    {
                        baseTreeNode.Selectable = true;
                    }
                });

                if (userOrgUnit.Number.ToString() == baseTreeNode.DepartmentNumber)
                {
                    baseTreeNode.Selectable = true;
                }

            }


            orgUnitDTOs.Where(o => o.ParentId == root.Id).ToList().ForEach(d =>
            {
                baseTreeNode.Childs.Add(AddChilds(orgUnitDTOs, d, userOrgUnit, selectedOrgUnitId));
            });

            tree.RootNode.Childs.Add(baseTreeNode);

            return tree;
        }

        private static TreeNode AddChilds(List<OrgUnitDTO> orgUnitDTOs, OrgUnitDTO orgUnitDTO, OrgUnitDTO userOrgUnit, int selectedOrgUnitId)
        {
            TreeNode treeNode = new TreeNode()
            {
                DepartmentNumber = orgUnitDTO.Number.ToString(),
                IsSelected = orgUnitDTO.IsSelected,
                Selectable = false,
                Name = orgUnitDTO.Name,
                Id = orgUnitDTO.Id
            };

            if (orgUnitDTO.Id == selectedOrgUnitId)
            {
                treeNode.IsSelected = true;
            }

            if (userOrgUnit != null)
            {

                userOrgUnit.LinkUnitsKeys.ForEach(l =>
                {
                    if (l == orgUnitDTO.Id)
                    {
                        treeNode.Selectable = true;
                    }
                });

                if (userOrgUnit.Number.ToString() == treeNode.DepartmentNumber)
                {
                    treeNode.Selectable = true;
                }

            }

            orgUnitDTOs.Where(o => o.ParentId == orgUnitDTO.Id).ToList().ForEach(d =>
            {
                treeNode.Childs.Add(AddChilds(orgUnitDTOs, d, userOrgUnit, selectedOrgUnitId));
            });

            return treeNode;
        }

        public static TreeViewModel BulidExternalPartiesTree(List<ExternalPartyDTO> externalPartyDTOs, int selectedOrgUnitId = -1)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };

            List<ExternalPartyDTO> roots = externalPartyDTOs.Where(o => o.ParentId == 0).ToList();

            roots.ForEach(r =>
            {
                tree.RootNode.Childs.Add(AddExternalPartyChilds(externalPartyDTOs, r, selectedOrgUnitId));
            });
            return tree;
        }

        private static TreeNode AddExternalPartyChilds(List<ExternalPartyDTO> externalPartyDTOs, ExternalPartyDTO externalPartyDTO, int selectedOrgUnitId)
        {
            TreeNode treeNode = new TreeNode()
            {
                DepartmentNumber = externalPartyDTO.Id.ToString(),
                IsSelected = externalPartyDTO.IsSelected,
                Selectable = true,
                //Name = externalPartyDTO.Name.Where(n => n.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text,
                Name = externalPartyDTO.LocalName,
                Id = externalPartyDTO.Id
            };

            if (externalPartyDTO.Id == selectedOrgUnitId)
            {
                treeNode.IsSelected = true;
            }

            externalPartyDTOs.Where(o => o.ParentId == externalPartyDTO.Id).ToList().ForEach(p =>
            {
                treeNode.Childs.Add(AddExternalPartyChilds(externalPartyDTOs, p, selectedOrgUnitId));
            });

            return treeNode;
        }

    }
}