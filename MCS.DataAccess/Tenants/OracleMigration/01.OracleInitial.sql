set scan off;

DECLARE
V_CultureID1 Number;
V_CultureID2 Number;
V_Lookup_Culture1 Number;
V_Lookup_Culture2 Number;
V_TenantLocalizationIdentifiers_1 Number;
V_TenantLocalizationIdentifiers_2 Number;

BEGIN

INSERT INTO "AspNetUsers" ("Id","Email", "EmailConfirmed", "PasswordHash", "SecurityStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEndDateUtc", "LockoutEnabled", "AccessFailedCount", "UserName") 
VALUES (N'4562b74d-1016-42db-8bac-5cbe0efc5743', N'ahmadas@sssprocess.com', 1, N'AI1iSpKiUQHWVkYWPU7Rm+Xbv5IM/SuB90dx4b137Ei6blRqQ/vsT7tls4IzIbJHRw==', N'1ffb1533-dbed-4e8c-aed8-3a97d8414202', N'962796958630', 0, 0, NULL, 1, 0, N'Admin');

-------------- Cultures

INSERT INTO  "TenantLookups" ( "CategoryId", "IsActive", "Sort", "EnumReference", "CreatedOn", "CreatedBy","ModefiedOn", "ModefiedBy") 
VALUES ( 38, 1, 0, NULL, sysdate, -1, NULL, NULL)
	returning "Id" INTO V_Lookup_Culture1;
Update   "TenantLookups" Set "EnumReference" = V_Lookup_Culture1
where "Id" = V_Lookup_Culture1;

INSERT INTO  "TenantCultures" ( "ShortName", "NameId", "CreatedOn", "CreatedBy", "ModefiedOn", "ModefiedBy") 
VALUES (N'ar', V_Lookup_Culture1, sysdate, NULL, NULL, NULL)
	returning "Id" INTO V_CultureID1;

INSERT INTO  "TenantLookups" ("CategoryId", "IsActive", "Sort", "EnumReference", "CreatedOn", "CreatedBy","ModefiedOn", "ModefiedBy") 
VALUES ( 38, 1, 1, NULL, sysdate, -1, NULL, NULL)
	returning "Id" INTO V_Lookup_Culture2;
	Update   "TenantLookups" Set "EnumReference" = V_Lookup_Culture2
	where "Id" = V_Lookup_Culture2;

INSERT INTO  "TenantCultures" ("ShortName", "NameId", "CreatedOn", "CreatedBy", "ModefiedOn", "ModefiedBy") 
VALUES ( N'en', V_Lookup_Culture2, sysdate, NULL, NULL, NULL)
	returning "Id" INTO V_CultureID2;
-----------------------------------------
INSERT INTO "TenantLocalizationIdentifiers" ("CreatedOn", "CreatedBy", "ModefiedOn", "ModefiedBy") VALUES (sysdate, -1, NULL, NULL)
returning "Id" INTO V_TenantLocalizationIdentifiers_1;
INSERT INTO "TenantLocalizationIdentifiers" ("CreatedOn", "CreatedBy", "ModefiedOn", "ModefiedBy") VALUES (sysdate, -1, NULL, NULL)
returning "Id" INTO V_TenantLocalizationIdentifiers_2;

INSERT INTO "Tenants" ("DatabaseName", "HostName", "FromDate", "FromDateH", "ToDate", "ToDateH", "OrgUnitsCount", "UsersCount", "DelegatedUserName", "DelegatedEmail", "DelegatedMobile", "IsDeleted", "IsActive", "YesserCertificate", "YesserCode", "YesserSourceID", "YesserServiceID", "YesserSourceName", "CreatedOn", "CreatedBy", "ModefiedOn", "ModefiedBy", "DelegatedName_Id", "Name_Id") 
VALUES (N'DB_1', N'HN_1', sysdate, N'2015/01/01', sysdate, N'2015/01/01', NULL, NULL, N'Test_1', N'ttt@t.com', N'34534534534', 0, 1, NULL, NULL, NULL, NULL, NULL, sysdate, -1, NULL, NULL, V_TenantLocalizationIdentifiers_1, V_TenantLocalizationIdentifiers_2);


INSERT INTO "TenantLocalizations" ("CultureId", "Text", "CreatedOn", "CreatedBy", "ModefiedOn", "ModefiedBy",  "LocalizationIdentifierId") 
VALUES (V_CultureID1, N'Database_1', sysdate, -1, NULL, NULL, V_TenantLocalizationIdentifiers_2);

INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Name.ar', N'الإسم بالعربي', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Name.ar', N'Arabic Name', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Name.en', N'الإسم بالإنجليزي', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Name.en', N'English Name', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.NameRequired.ar', N'أدخل الإسم بالعربي', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.NameRequired.ar', N'Enter Arabic Name', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.NameRequired.en', N'أدخل الإسم بالإنجليزي', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.NameRequired.en', N'Enter English Name', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.MandatoryFields', N'أدخل جميع الحقول الإلزامية', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.MandatoryFields', N'Enter All Mandatory Fields', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DateBetween', N'التاريخ بين', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DateBetween', N'Date Between', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedUserInformations', N'بيانات الشخص المفوض', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedUserInformations', N'Delegated User Informations', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedName.ar', N'إسم الشخص بالعربي ', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedName.ar', N'Arabic Delegated Name', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedName.en', N'إسم الشخص بالإنجليزي', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedName.en', N'English Delegated Name', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedNameRequired.ar', N'أدخل إسم الشخص بالعربي', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedNameRequired.ar', N'Enter Arabic Delegated  Name', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedNameRequired.en', N'أدخل إسم الشخص بالإنجليزي', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedNameRequired.en', N'Enter English Delegated  Name', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Save', N'حفظ', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Save', N'Save', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetButton', N'أعد الكتابة', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetButton', N'Clear Inputs', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Edit', N'تعديل', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Edit', N'Edit', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Cancel', N'إلغاء', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Cancel', N'Cancel', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Name', N'الإسم', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Name', N'Name', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Number', N'الرقم', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Number', N'Number', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ToDateCompare', N'بداية التاريخ يجب أن تكون أقل من نهاية التاريخ', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ToDateCompare', N'Begin date must be less than end date', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ToDateRequired', N'أدخل تاريخ النهاية', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ToDateRequired', N'Enter end date', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.FromDateRequired', N'أدخل تاريخ البداية', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.FromDateRequired', N'Enter begin date', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.OrgUnitsCount', N'عدد الإدارات', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.OrgUnitsCount', N'Departments Count', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.UsersCount', N'عدد المستخدمين', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.UsersCount', N'Users Count', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedUserName', N'إسم الدخول إلى النظام', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedUserName', N'Login To System UserName', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedUserNameRequired', N' أدخل إسم الدخول إلى النظام', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedUserNameRequired', N'Enter login to system userName', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedMobile', N'رقم الجوال', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedMobile', N'Mobile Number', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedMobileRequired', N' أدخل رقم الجوال', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedMobileRequired', N'Enter mobile number', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedEmail', N'البريد الإلكتروني', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedEmail', N'Email', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedEmailRequired', N'أدخل البريد الإلكتروني', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DelegatedEmailRequired', N'Enter email', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tanant.DelegatedEmailExpresssion', N'أدخل البريد الإلكتروني بالشكل صحيح', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tanant.DelegatedEmailExpresssion', N'Correct the email', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Grid.Delete', N'حذف', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Grid.Delete', N'Delete', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Tenant', N'العميل', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Tenant', N'Client', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Tenants', N'العملاء', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Tenants', N'Clients', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.HostName', N'إسم المضيف', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.HostName', N'Host Name', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.HostNameRequired', N'أدخل إسم المضيف', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.HostNameRequired', N'Enter host name', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DeleteSucceeded', N'تمت عملية الحذف بنجاح', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DeleteSucceeded', N'Delete Succeeded', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.UpdateSucceeded', N'تم عملية التعديل بنجاح', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.UpdateSucceeded', N'Update Succeeded', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.AddSucceeded', N'تمت عملية الإضافة بنجاح', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.AddSucceeded', N'Add Succeeded', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.Wellcome', N'مرحبا بك', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.Wellcome', N'Wellcome', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.UserName', N'اسم المستخدم', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.UserName', N'User Name', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.Password', N'رمز الدخول', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.Password', N'Password', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.ForgetPassword', N'نسيت كلمة المرور', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.ForgetPassword', N'Forget Password', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.Enter', N'الدخول', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.Enter', N'Enter', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.Title', N'مراسلات', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.Title', N'eMorasalate', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.Reset', N'إعادة تعيين', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.Reset', N'Reset', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.Password', N'كلمة المرور', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.Password', N'Password', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.NewPassword', N'رمز الدخول الجديد', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.NewPassword', N'New Password', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.ReWritePassword', N'إعادة كتابة رمز الدخول الجديد', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.ReWritePassword', N'Confirm New Password', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.Code', N'رمز التوثيق', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.Code', N'Varification Code', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.Reset', N'إعادة تعيين', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepTwo.Reset', N'Reset', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.Cancel', N'إلغاء', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.Cancel', N'Cancel', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.ResetPassword', N'إعادة تعيين كلمة المرور', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.ResetPassword', N'Reset Password', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.Request', N'طلب', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.Request', N'Request', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.Username', N'اسم المستخدم', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.Username', N'Username', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.Email', N'البريد الإلكتروني', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.Email', N'Email', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.Reset', N'إعادة تعيين', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.ResetPasswordStepOne.Reset', N'Reset', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.RememberMe', N'حفظ معلومات الدخول', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.RememberMe', N'Remember Me', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.PleaseEnter', N'، الرجاء الدخول', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.PleaseEnter', N'Enter', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Layout.Year', N'1437', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Layout.Year', N'1437', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.AboutUs', N'من نحن', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.AboutUs', N'About Us', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.Privacy', N'الخصوصية', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.Privacy', N'Privacy', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.ContactUs', N'اتصل بنا', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Login.ContactUs', N'Contact Us', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Dialog.Close', N'Close', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Dialog.Close', N'إغلاق', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tanent.Login.InvalidCredentials', N'الرجاء التأكد من أسم المستخدم أو رمز الدخول', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tanent.Login.InvalidCredentials', N'Please make sure the user name and access code', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Login.PasswordRequired', N'أدخل رمز الدخول', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Login.PasswordRequired', N'Enter Password', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Login.UserNameRequired', N'أدخل اسم المستخدم', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Login.UserNameRequired', N' Enter UserName', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.EmailRequired', N'أدخل البريد الالكتروني', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.EmailRequired', N'Enter Email ', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPasswordStepTwo.EmailSent', N'تم ارسال الطلب الى البريد الالكتروني', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPasswordStepTwo.EmailSent', N'Request Sent to the Email', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPasswordStepTwo.Cancel', N'إلغاء', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPasswordStepTwo.Cancel', N'Cancel', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.NewPasswordRequierd', N'أدخل رمز الدخول الجديد', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.NewPasswordRequierd', N'Enter New Password', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.ReNewPasswordRequierd', N'أدخل تأكيد رمز الدخول الجديد', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.ReNewPasswordRequierd', N'Enter Confirm Password', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.CodeRequierd', N'أدخل رمز التوثيق', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.CodeRequierd', N'Enter the varification code', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.ReNewPasswordCompare', N'غير مطابق لرمز الدخول الجديد', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.ReNewPasswordCompare', N'Not Match with the New Password', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPasswordStepTwo.Succeeded', N'تمت العملية بنجاح', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPasswordStepTwo.Succeeded', N'operation accomplished successfully', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Localization.TextExpression', N'يجب إدخال أحرف فقط', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Localization.TextExpression', N'Must enter just characters', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DeleteMessage', N'هل أنت متأكد من عملية الحذف؟', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.DeleteMessage', N'Are you sure from delete operation?', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Grid.DeleteMassage', N'هل أنت متأكد من عملية الحذف؟', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Grid.DeleteMassage', N'Are you sure from delete operation?', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Grid.EmptyText', N'لا يوجد نتائج للعرض', N'ar', N'Resources', N'', NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Grid.EmptyText', N'There are no items to display', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.UsernameRequired', N'أدخل اسم المستخدم', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.UsernameRequired', N'Enter Username', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Dialog.Yes', N'نعم', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Dialog.Yes', N'Yes', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Dialog.No', N'لا', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.Dialog.No', N'No', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.CopyRightFor', N'جميع الحقوق محفوظة لدى شركة ', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.CopyRightFor', N'Copyright for', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.CompanyName', N'أنظمة الخدمات الامنة لتقنية المعلومات المحدودة', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.CompanyName', N'SSSIT', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.HostNameExpresssion', N'أدخل إسم المضيف بالشكل الصحيح', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.HostNameExpresssion', N'Correct the host name ', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.SendResetEmail', N'إعادة إرسال تعيين كلمة السر', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.SendResetEmail', N'Resend Reset Password', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.SendResetEmailSucceed', N'تمت عملية الإرسال بنجاح', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.SendResetEmailSucceed', N'Sent operation succeeded', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Activate', N'تفعيل', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tenant.Activate', N'Activate', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.UserNameNotValid', N'اسم المستخدم غير صحيح', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.UserNameNotValid', N'User name not valid', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.UserEmailNotValid', N'البريد الالكتروني غير صحيح', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Global.ResetPassword.UserEmailNotValid', N'Email not valid', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tanent.Login.Lockout', N'تم إغلاق هذا الحساب ، يرجى إعادة المحاولة لاحقًا.', N'ar', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'Tanent.Login.Lockout', N'This account has been locked out, please try again later.', N'en', N'Validation', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'User.Tenant.UploadLogo', N'تحميل الشعار', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'User.Tenant.UploadLogo', N'Upload Logo', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'User.Tenant.Logo', N'الشعار', N'ar', N'Resources', NULL, NULL, NULL, NULL, NULL);
INSERT INTO "Resources" ("ResourceId", "Value", "Culture", "ResourceSet", "Type", "BinFile", "TextFile", "Filename", "Comment") VALUES (N'User.Tenant.Logo', N'Logo', N'en', N'Resources', NULL, NULL, NULL, NULL, NULL);
END;



