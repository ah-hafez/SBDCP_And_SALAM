using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MCS.Framework.Controls;
using MCS.Framework.Localization;
using MCS.Common;
using MCS.UI.Areas.Admin.Models.OrgUnit;
using MCS.UI.Areas.User.Models.ExternalParties;
using UserOrgUnit = MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI
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

        public static TreeViewModel BulidTree(List<OrgUnitVM> orgUnitVMs, int selectedOrgUnitId = -1, bool maintainOrgUnitLinks = false)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            if (orgUnitVMs == null)
            {
                return tree;
            }

            if (orgUnitVMs.Count == 0)
            {
                return tree;
            }

            OrgUnitVM userOrgUnit = new OrgUnitVM();
            userOrgUnit = orgUnitVMs.Where(o => o.Id == SessionInfo.OrgUnitId).SingleOrDefault();

            tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };

            OrgUnitVM root = orgUnitVMs.Where(o => o.ParentId == -1).SingleOrDefault();

            TreeNode baseTreeNode = new TreeNode()
            {
                DepartmentNumber = root.Number.ToString(),
                IsSelected = false,
                Selectable = false,
                Name = root.Name,
                Id = root.Id,
                HasChilds = root.HasChilds
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


            orgUnitVMs.Where(o => o.ParentId == root.Id).ToList().ForEach(d =>
            {
                baseTreeNode.Childs.Add(AddChilds(orgUnitVMs, d, userOrgUnit, selectedOrgUnitId, maintainOrgUnitLinks));
            });

            tree.RootNode.Childs.Add(baseTreeNode);

            return tree;
        }

        private static TreeNode AddChilds(List<OrgUnitVM> orgUnitVMs, OrgUnitVM orgUnitVM, OrgUnitVM userOrgUnit, int selectedOrgUnitId, bool maintainOrgUnitLinks)
        {
            TreeNode treeNode = new TreeNode()
            {
                DepartmentNumber = orgUnitVM.Number.ToString(),
                IsSelected = orgUnitVM.IsSelected,
                Selectable = maintainOrgUnitLinks,
                Name = orgUnitVM.Name,
                Id = orgUnitVM.Id,
                HasChilds = orgUnitVM.HasChilds
            };

            if (orgUnitVM.Id == selectedOrgUnitId)
            {
                treeNode.IsSelected = true;
            }

            if (userOrgUnit != null)
            {

                userOrgUnit.LinkUnitsKeys.ForEach(l =>
                {
                    if (l == orgUnitVM.Id)
                    {
                        treeNode.Selectable = true;
                    }
                });

                if (userOrgUnit.Number.ToString() == treeNode.DepartmentNumber)
                {
                    treeNode.Selectable = true;
                }

            }

            orgUnitVMs.Where(o => o.ParentId == orgUnitVM.Id).ToList().ForEach(d =>
            {
                treeNode.Childs.Add(AddChilds(orgUnitVMs, d, userOrgUnit, selectedOrgUnitId, maintainOrgUnitLinks));
            });

            return treeNode;
        }

        public static TreeViewModel BulidExternalPartiesTree(List<ExternalPartyVM> externalPartyVMs, int selectedOrgUnitId = -1)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };

            List<ExternalPartyVM> roots = externalPartyVMs.Where(o => !o.ParentId.HasValue).ToList();

            roots.ForEach(r =>
            {
                tree.RootNode.Childs.Add(AddExternalPartyChilds(externalPartyVMs, r, selectedOrgUnitId));
            });
            return tree;
        }

        private static TreeNode AddExternalPartyChilds(List<ExternalPartyVM> externalPartyVMs, ExternalPartyVM externalPartyVM, int selectedOrgUnitId)
        {
            TreeNode treeNode = new TreeNode()
            {
                DepartmentNumber = externalPartyVM.Number,
                IsSelected = externalPartyVM.IsSelected,
                Selectable = externalPartyVM.IsVirtual ? false : true,
                //Name = externalPartyVM.Name.Where(n => n.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text,
                Name = externalPartyVM.LocalName,
                Id = externalPartyVM.Id,
                HasChilds = externalPartyVM.HasChilds,
                IsYesserRegistered = externalPartyVM.YasserRegistered
            };

            if (externalPartyVM.Id == selectedOrgUnitId)
            {
                treeNode.IsSelected = true;
            }

            externalPartyVMs.Where(o => o.ParentId == externalPartyVM.Id).ToList().ForEach(p =>
            {
                treeNode.Childs.Add(AddExternalPartyChilds(externalPartyVMs, p, selectedOrgUnitId));
            });

            return treeNode;
        }
        public static TreeViewModel BulidTree(List<UserOrgUnit.OrgUnitVM> orgUnitVMs, int selectedOrgUnitId = -1, bool maintainOrgUnitLinks = false)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            if (orgUnitVMs == null)
            {
                return tree;
            }

            if (orgUnitVMs.Count == 0)
            {
                return tree;
            }

            tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };

            UserOrgUnit.OrgUnitVM root = orgUnitVMs.Where(o => o.ParentId == -1).SingleOrDefault();

            TreeNode baseTreeNode = new TreeNode()
            {
                DepartmentNumber = root.Number.ToString(),
                IsSelected = false,
                Selectable = true,
                Name = root.Name,
                Id = root.Id,
                HasChilds = root.HasChilds,
   
            };

            orgUnitVMs.Where(o => o.ParentId == root.Id).ToList().ForEach(d =>
            {
                baseTreeNode.Childs.Add(AddChilds(orgUnitVMs, d, selectedOrgUnitId, maintainOrgUnitLinks));
            });

            tree.RootNode.Childs.Add(baseTreeNode);

            return tree;
        }

        private static TreeNode AddChilds(List<UserOrgUnit.OrgUnitVM> orgUnitVMs, UserOrgUnit.OrgUnitVM orgUnitVM, int selectedOrgUnitId, bool maintainOrgUnitLinks)
        {
            TreeNode treeNode = new TreeNode()
            {
                DepartmentNumber = orgUnitVM.Number.ToString(),
                IsSelected = orgUnitVM.IsSelected,
                Selectable = maintainOrgUnitLinks,
                Name = orgUnitVM.Name,
                Id = orgUnitVM.Id,
                HasChilds = orgUnitVM.HasChilds,
            };

            if (orgUnitVM.Id == selectedOrgUnitId)
            {
                treeNode.IsSelected = true;
            }
            treeNode.Selectable = true;

            orgUnitVMs.Where(o => o.ParentId == orgUnitVM.Id).ToList().ForEach(d =>
            {
                treeNode.Childs.Add(AddChilds(orgUnitVMs, d, selectedOrgUnitId, maintainOrgUnitLinks));
            });

            return treeNode;
        }

        public static TreeViewModel BuildInternalEntitiesTree(List<UserOrgUnit.OrgUnitVM> orgUnitVMs, int selectedOrgUnitId = -1)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            UserOrgUnit.OrgUnitVM root = orgUnitVMs.Where(o => o.ParentId == -1).SingleOrDefault();

            TreeNode baseTreeNode = new TreeNode()
            {
                DepartmentNumber = root.Number.ToString(),
                IsSelected = false,
                Selectable = true,
                Name = root.Name,
                Id = root.Id,
                HasChilds = true
            };

            tree.RootNode = baseTreeNode;

            List<UserOrgUnit.OrgUnitVM> rootOrgUnitsVMs = orgUnitVMs.Where(o => o.ParentId != -1).ToList();

            rootOrgUnitsVMs.ForEach(r =>
            {
                tree.RootNode.Childs.Add(AddInternalEntitiesChilds(orgUnitVMs, r, selectedOrgUnitId));
            });

            return tree;
        }

        private static TreeNode AddInternalEntitiesChilds(List<UserOrgUnit.OrgUnitVM> orgUnitVMs, UserOrgUnit.OrgUnitVM orgUnitVM, int selectedOrgUnitId)
        {
            TreeNode treeNode = new TreeNode()
            {
                DepartmentNumber = orgUnitVM.Number.ToString(),
                IsSelected = orgUnitVM.IsSelected,
                Selectable = true,
                Name = orgUnitVM.Name,
                Id = orgUnitVM.Id,
                HasChilds = orgUnitVM.HasChilds,
            };

            if (orgUnitVM.Id == selectedOrgUnitId)
            {
                treeNode.IsSelected = true;
            }

            orgUnitVMs.Where(o => o.ParentId == orgUnitVM.Id).ToList().ForEach(p =>
            {
                treeNode.Childs.Add(AddInternalEntitiesChilds(orgUnitVMs, p, selectedOrgUnitId));
            });

            return treeNode;
        }

        public static AutoCompleteDataSource GetDefaultSelect()
        {
            if (SessionInfo.CultureShortName == "en")
            {
                return new AutoCompleteDataSource()
                {
                    Value = "-1",
                    Label = DbRes.TResource("User.TransactionType.All"),
                };
            }
            else
            {
                return new AutoCompleteDataSource()
                {
                    Value = "-1",
                    Label = DbRes.TResource("User.TransactionType.All"),
                };
            }
        }
    }
}