using System;
using System.Collections.Generic;

namespace MCS.Common
{
    public enum LookupCategory
    {
        DeliveryMethod = 1,
        OutboundDocumentType = 3,
        TransactionCategories = 4,
        LinkingType = 5,
        InboundDocumentType = 6,
        AttachmentType = 7,
        Nationality = 8,
        Title = 9,
        TransactionCategory = 10,
        LetterListType = 11,
        Color = 12,
        Permission = 13,
        Department = 14,
        ActionType = 15,
        PartyType = 16,
        PriorityLevel = 17,
        Year = 18,
        CustomPermissionGroup = 19,
        BarcodeRefreanceType = 20,
        BarcodeDesignType = 21,
        TransactionAttachmentType = 22,
        SearchType = 23,
        Hour = 24,
        Minute = 25,
        InboundStatus = 26,
        OutboundStatus = 27,
        NotificationType = 28,
        TaskStatus = 29,
        TransactionStatus = 30,
        TrayType = 31,
        BarcodeReferenceType = 32,
        AuditingActionCode = 33,
        NotificationTemplateType = 34,
        NotificationEmailSubject = 35,
        TransactionDateType = 36,
        ExplanationsConfidentiality = 37,
        CultureName = 38,
        NotificationSubscriptions = 39,
        NotificationSource = 40,
        City = 41,
        RelativeRelation = 42,
        Gender = 43,
        Entity = 44,
        ReportType = 45,
        RepresentationType = 46,
        FollowUp = 47,
        DelegationStatus = 48,
        SupportType = 49,
        ProblemSupportType = 50,
        InquirySupportType = 51,
        NotificationWebSubject = 52,
        TransactionLogBasicInfo = 53,
        TransactionLogAttachments = 54,
        TransactionLogNames = 55,
        TransactionLogCopies = 56,
        TransactionLogExternalCopies = 57,
        TransactionLogExplanations = 58,
        TransactionLogFollowUps = 59,
        TransactionLogTasks = 60,
        DateType = 61,
        DateFormat = 62,
        NumberFormat = 63,
        LookupActiveStatus = 64,
        EscalationAction = 65,
        EscalationTo = 66,
        TransCopyStatus = 67,
        FollowUpStatus = 68,
        UserMobileClass = 69,
        SaveReason = 70

    }

    public enum SupportType
    {
        Problem = 1,
        Inquiry = 2
    }

    public enum ActionTransactionType
    {
        Withdraw = 7,
        Rejected = 8,
        Completed = 9,
        Saved = 10
    }

    public enum UsersWithGroupsGridColumn
    {
        
      UserName= 0, 
      GroupName =1,
      Name =2,
      OrgUnitName = 3,
      AdminUserName = 4,
    }
    public enum TransactionReportGridColumn
    {
        Number = 0,
        TransactionType = 1,
        OrgUnit = 2,
        Date = 3,
        SourceType = 4,
        TransactioDescription = 5,
        Confidentiality = 6,
        Priority = 7,
        SubjectClassification = 8,
        Remarks = 9,
        DeliveryMethod = 10,
        FullName = 11,
        CivilID = 12,
        MobileNumber = 13,
        ExternalParty = 14,
        InboundDateH = 15,
        FromEntity = 16,
        FromUser = 17,
        ToEntity = 18,
        ToUser = 19,
        CreatedOn = 20,
        RemindDate = 21,
        UserProfile = 22,
        TransactionStatus = 23,
        NumberWithDate = 24,
        AssignedDate = 25,
        DelayText = 26,
        SignedByUserText = 27
    }
    public enum PermissionGroupName
    {
        Inbound = 1,
        Link = 2,
        UserCategory = 3,
        Outbound = 4,
        Search = 5,
        Reports = 6,
        File = 7,
        AddExternalParty = 8,
        TransactiosConfidentiality = 9,
        ExplanationsConfidentiality = 10,
        ModulesLevel = 11,
        Archiving = 12,
        Assignments = 13,
        GeneralPermissions = 14,
        SystemAdministration = 15,
        OutboundTransactionsTypes = 16,
        InboundTransactionsTypes = 17,
        InternalOutboundTransactionsTypes = 18,
        TasksInsertion = 19,
        Names = 20,
        InternalCopies = 21,
        ExternalCopies = 22,
        Followups = 23,
        Explanations = 24,
        UserPrefreances = 25,
        TransactionLog = 26,



    }
    public enum SignType
    {
        KeepPresent = 1,
        UploadFile,
        SignOnScreen,
        Delete,
        Wacom
    }

    public enum Gender
    {
        FeMale = 1,
        Male
    }
    public enum eFileStatus
    {
        Insert = 1,
        Delete = 2,
        Update = 3
    }
    public enum Confedentiality
    {
        /// <summary>
        /// this int refrence from permission id
        /// </summary>
        Normal = 27,  //عادي//
        Secret = 28,  //سري//
        HighConfidential = 29,  //سري للغاية//
        HandDelivered = 121
    }
    public enum LinkingType
    {
        WithReplyInbound = 1,
        WithReferenceInbound,
        WithReplyOutbound,
        WithReferenceOutbound,
        WithReferenceOutboundInternal,
        WithReplyOutboundInternal,
        WithInboundDocumentNumber
    }
    public enum TrayType
    {
        MyTransactions = 1,//معاملاتي
        SentTransactions = 2,
        DraftOutbound = 3,//مشروع الصادر
        Saved = 4,
        OrgUnit = 5,
        Manager = 6,
        Copies = 7,
        YESSER = 8,
        Tasks = 9,
        FollowUp = 10,
        Reservation = 11,
        CopiesOutbound = 16,
        OutboundExternal = 15,
        ReservedExternalOutbound = 17,
        Withdrawal = 18,
        Late = 19,
        HasDate = 20,
        Archives = 21,
        Classification = 22,
        FollowUpUnderProcess = 23,
        FollowUpComplete = 24,
        FollowUpLate = 25,
        FollowUpReminder = 27,
        FollowUpEscalation = 28,
        FollowUpCanceld = 29,
        ElcOutBound = 30,
        DeletedDraftOutbound = 31,
        Today = 32,
        InternalInboundCopies = 33,
        SavedCopies = 34,
        SpecialCopies = 35,
        Decisions = 36,
        Circulars = 37,
        SublimeMatter = 38,
    }
    public enum TransactionCategory
    {
        All = -1,
        None = 1295,
        Inbound = 1,
        ExternalOutbound = 2,
        InternalOutbound = 3,
        DraftOutbound = 4
    }
    public enum HubConstants
    {
        DeliveryMethodId = 229,
        TransactionTypeId = 1,
        LetterTypeId = 1,
        SourceTypeId = 43,
    }
    public enum HubTransactionStatus
    {
        Pending,
        Confirmed,
        Rejected
    }
    public enum OutboundClassification
    {
        Original,
        Copy
    }
    public enum NotificationType
    {
        Email = 1,
        SMS,
        Web
    }
    [Flags]
    public enum TransactionCategorieColor
    {
        None = 0,
        Outbound = 255,
        Inbound = 254,
        InternalOutbound = 256,
        DraftOutbound = 257
    }
    [Flags]
    public enum TransactionCategories
    {
        None = 0,
        Outbound = 1,
        Inbound = 2,
        InternalOutbound = 4,
        DraftOutbound = 8
    }
    [Flags]
    public enum LetterListType
    {
        None,
        Formal,
        Individuals
    }
    public enum StatusCode
    {
        Ok,
        UserNotAuthorised,
        UserNameAlreadyExist,
        UserEmailAlreadyExist,
        UserNameOrPasswordNotCorrect,
        ModelNotValid,
        GeneralError,
        UserNotFound,
        NameNotFound,
        TransactionNotFound,
        UserRequired,
        ExplanationConfidentialityRequired,
        OrgUnitRequired,
        NotSupported,
        InvalidOrgUnit,
        TransactionRequired,
        UpdateNotAllow, //Due To Printed Delivery Report
        InvalidUserTask,
        InvalidParentTask,//
        InvalidTask,
        InvalidTaskWorkflow,
        LimitaionSubTask,//
        TaskReminderNotAllow,
        MyTransactionNotAuthorized,
        DraftOutboundNotAuthorized,
        TransferredTransactionsNotAuthorized,
        SentTransactionsNotAuthorized,
        TransactionDraftAlreadyCreated,
        OrgUnitNotAuthorized,
        PermissionFile,
        PermissionEditor,
        PermissionEditorViewTransactions,
        PermissionEditorAssignments,
        PermissionEditorDraft,
        PermissionEditorExplanations,
        PermissionPrintDeliveryData,
        PermissionOutbound,
        PermissionOutboundAddNames,
        PermissionOutboundAddAttachments,
        PermissionOutboundAddCopies,
        PermissionLinkAddLink,
        PermissionOutboundEditOutbound,
        PermissionOutboundDeleteNames,
        PermissionOutboundDeleteCopies,
        PermissionOutboundDeleteAttachments,
        PermissionLinkDeleteLink,
        PermissionCreateInbound,
        PermissionInboundAddNames,
        PermissionInboundAddAttachments,
        PermissionInboundDeleteNames,
        PermissionInboundDeleteAttachments,
        PermissionEditInbound,
        PermissionOutboundOutboundDraft,
        PermissionTasksInsertionAddTask,
        PermissionTasksInsertionDeleteTask,
        PermissionTasksInsertionSequenceOfTask,
        PermissionTasksInsertionExclusive,
        PermissionTasksInsertionTasksTray,
        PermissionTasksInsertionTaskReminder,
        PermissionTasksInsertionEditTaskDate,
        PermissionTasksInsertionTaskDetails,
        PermissionTasksInsertionSubTasking,
        PermissionAssignmentsWithdrawTransaction,
        PermissionAssignments,
        InvalidActionType,
        UpdateNotAllowDueToActionAsCopy,
        NoCounter,
        TransactionAssignmentsCannotBeNullOrEmpty,
        DocumentNumberAlreadyExist,
        DocumentNumberRequired,
        ExteralPartyUsed,
        ManagerUsed,
        OrgUnitUsed,
        BarcodeNotAvailable,
        InvalidExtendTaskDate,
        ConnotUpdateTransactionBasicInfo,
        TransactionConfidentialityRequired,
        TransactionSourceTypeConfidentialityRequired,
        UserIdClaimNotProvided,
        ActionNotFound,
        AttachmentTypeNotFound,
        BarcodeNotFound,
        BarcodeDesignNotFound,
        CultureNotFound,
        CounterNotFound,
        CounterDetailNotFound,
        DocumentInfoNotFound,
        ExplanationNotFound,
        AssignmentPaperNotFound,
        ExternalPartyNotFound,
        ExternalPartyManagerNotFound,
        FormNotFound,
        PermissionAssignToOtherDepartment,
        PermissionAssignToEmployeeInOtherDepartment,
        PermissionAssignTo,
        PermissionWithdrawTransactionFromAllCabins,
        PermissionWithdrawTransactionFromTidyCabins,
        NoOrgUnitRootDefiened,
        TransactionCycleLinked,
        TransactionDoubleLinked,
        RevertAssignmentToYou,
        UsedCounter,
        AssignmentTasksNotCompleted,
        PermissionNotFound,
        DeletePermissionNotAllow,
        PermissionRelatedToTransactions,
        PermissionRelatedToExplanation,
        UserNameAndEmailNotMatchUserProfile,
        InvalidOldPassword,
        UsernameOrEmailNotCorrect,
        ResetPasswordOperationFailed,
        VisitTicketNotAvailable,
        RestCodeInvalid,
        ResetTokenInvalid,
        SubjectClassificationRelatedToTransactions,
        SuggestedTopicRelatedToTransactions,
        DuplicateUserDeligation,
        TransactionAlreadyExist,
        DraftAlreadyExist,
        TenantNotFound,
        TenantAlreadyExist,
        TenantNotActive,
        AssignmentGroupNotFound,
        ActionSendCopyToViewNotFound,
        MaxUsersReached,
        MaxOrgUnitsReached,
        SystemExpired,
        ActionSendMainTransactionNotFound,
        PasswordRequired,
        InvalidPasswordRequiredLength,
        PasswordRequireNonLetterOrDigit,
        PasswordRequireDigit,
        PasswordRequireLowercase,
        PasswordRequireUppercase,
        UserGroupNameAlreadyExist,
        CantReturnToSelf,
        Lockout,
        ActionIsUsed,
        RepoterExist,
        TransactionPathConfidentialityRequired,
        BadRequest,
        UnAuthorized = 401,
        NotFound = 404,
        InternalServerError = 500,
        SystemException = 0,
        CodeOK = 200,
        UserTenantAlreadyExist,
        ConnectWithUserTenant,
        TaskNotFound,
        DeleteCounterDetail,
        CounterExisted,
        OTPInvalid,
        OTPVeryOld,
        Ora031350,
        OrgUnitsHaveSameName,
        CanNotMoveEntityToItself,
        CanNotMoveParentEntityToChildEntity,
        AlreadyChildOfThisEntity,
        OneOfTheUsersIsMemberOfTwoEntities,
        CanNotMergeEntityToItself,
        CanNotMergeParentEntityToChildEntity,
        OrgUnitsToBeMergedHaveSameName,
        OrgUnitHasTransactionsFromExternalParties,
        ThisRoleIsAlreadyExist,
        UserUsedCanntDelete,
        InActiveUser,
        CanNotMoveExternalEntity,
        CanNotMergeExternalEntity,
        SignatureNotFound,
        TransactionHasActiveFollowup,
        SigFramIsMandatory,
        MarkFramIsMandatory,
        DuplicateFormName,
        ArchivesNotFound,
        ErrorNoPermissionToReceiveTransaction,
        WarningNoPermissionToReceiveTransaction
    }
    [Flags]
    public enum ExternalPartyType
    {
        None = 0,
        Official = 200,   //رسمية
        Individuals = 201,  //افراد
                            //  Both = 3           //(كلاهما)  
    }
    public enum TransactionDateType
    {
        Any = 1,
        Today,
        HasDate,
        Late,
        Decisions,
        Circulars,
        SublimeMatter
    }
    public enum TransactionStatus
    {
        InProcess = 1,
        TempSave,
        Outbound,
        Reserved,
        MultiOwnership,
        Saved,
        NotSent,
        Rejected,
        UnableToDeliver,
        Pending,
        Sent,
        Completed,
        Deleted
    }
    public enum OutboundDraftStatus
    {
        Accept,
        Reject,
    }
    public enum BarcodeDesignType
    {
        None,
        Inbound,
        Outbound,
        OutboundInternal,
        VisitTicket,
        Attachment
    }
    public enum TransactionAttachmentType
    {
        Main = 1,
        Attachment,
    }
    public enum TaskStatus
    {
        InProcess = 1,
        Reject,
        Complete,
        Late,
        Received,
        Sent,
        expired
    }
    public enum SearchType
    {
        SearchByInboundNumber = 1,
        SearchByOutboundNumber,
        SearchBySubject,
        SearchByOutboundInternalNumber,
        SearchByOutboundDraftNumber,
        SearchByEntity,
        SearchByCreator,
        SearchbyEncryptionCode,
        SearchByDocumentNumber,
        SearchByAssignTransaction,
        SearchByRecordNumber,
        SearchByNames,
        SearchDaily,
        SearchByAssignmentNote,
        SearchByManifestNumber,
        SearchByMilitaryNumberOrIdentity,
        SearchByTransactionNots,
        SearchByELcEmployee,
        SearchByExternalOutBoundOrManifestNumber,
        SearchByCopyAssignemnt,
        SearchBySubjectLetter,
        SearchByTransactionNumber,
        SearchByExternalPartyCopies = 27,
        All = -1
        //SearchbyTransactionNumberInboundOutbound,

    }
    public enum NotificationSource
    {
        None = 0,
        TaskReminder,
        TransactionAssignment,
        ResetPassword,
        NewUser,
        DeleteUser,//حذف مستخدم
        ModifiedUser,
        DisabledUser,
        EnabledUser,
        NewTask,
        DeleteTask,
        ResendTask,
        AcceptTask,
        RejectTask,
        ReplyTask,
        AssignTransaction,
        RevertRejectTransaction,
        RevertTransaction,
        ElectronicCopies,
        Viewed,
        Followup,
        CancelFollowup,
        EndFollowupPeriod,
        CancelFollowupSendToSaved,
        AddExplanation,
        ReceiveReport,
        AddDelegation,
        InProcessDelegation,
        ApprovedDelegation,
        RejectedDelegation,
        DisabledDelegation,
        EnableDelegation,
        OrgUnit,
        ReceiveSupport,
        ReminderBeforeTaskEnded,
        VerificationCode,
        EditElectronicCopies,
        DeleteElectronicCopies,
        LateTransaction,
        RoleRequest,
        CreateTransactionWithRemindDate = 43,
        VerificationTransactionCodeEmail = 44
    }
    public enum NotificationTemplateType
    {
        None = 0,
        TransactionAssignmentWeb,//إحالة معاملة
        TransactionAssignmentDraftWeb,//إحالة معاملة
        NewUserEmail,//اضافة مستخدم          
        ResetPasswordEmail,//اعادة تعيين كلمة المرور       
        OrgUnitWeb,
        NewTaskWeb,
        OTPCode,
        TaskReminderWeb,
        DeleteTaskWeb,
        ResendTaskWeb,
        AcceptTaskWeb,
        RejectTaskWeb,
        ReplyTaskWeb,
        AssignTransactionWeb,
        RevertRejectTransactionWeb,
        RevertTransactionWeb,
        ElectronicCopiesWeb,//وصول نسخة الكترونية من المعاملة 
        ViewedWeb,//تم الاطلاع على النسخة الإلكترونية لمعاملة 
        FollowupWeb,
        CancelFollowupWeb,
        EndFollowupPeriodWeb,
        CancelFollowupSendToSavedWeb,
        AddExplanationWeb,
        ReceiveReportWeb,//استلام بيان التسليم
        AddDelegationWeb,
        InProcessDelegationWeb,
        ApprovedDelegationWeb,
        RejectedDelegationWeb,
        DisabledDelegationWeb,
        EnableDelegationWeb,


        TransactionAssignmentEmail,//احالة معاملة لصندوق => according to type
        AssignTransactionEmail,//استلام معاملة رقم
        RevertRejectTransactionEmail,//رفض استلام معاملة
        NewTaskEmail,//مهمة جديدة       
        TaskReminderEmail,//تذكير بمباشرة العمل على المهمة    
        DeleteTaskEmail,//حذف مهمة
        ResendTaskEmail,//اعادة ارسال مهمة
        AcceptTaskEmail,//قبول المهمة
        RejectTaskEmail,//رفض المهمة
        ReplyTaskEmail,//رد على المهمة
        DeleteUserEmail,//حذف مستخدم
        ModifiedUserEmail,
        DisabledUserEmail,
        EnabledUserEmail,
        ElectronicCopiesEmail,//وصول نسخة الكترونية
        ViewedEmail,//تم الاطلاع على النسخة الالكترونية
        RevertTransactionEmail,//راجاع معاملة
        FollowupEmail,//متابعة معاملة
        CancelFollowupEmail,//الغاء متابعة معاملة من قبل موظف
        EndFollowupPeriodEmail,//الغاء متابعة معاملة لإنتهاء مدة متابعتها
        CancelFollowupSendToSavedEmail,//الغاء متابعة معاملة وذلك لأنها ارسلت لصندوق المنجزة
        AddExplanationEmail,//اضافة شرح على نسخة إلكترونية
        ReceiveReportEmail,//استلام بيان التسليم
        AddDelegationEmail,//اضافة تفويض                       //InProcessDelegationWeb = 00,
        ApprovedDelegationEmail,//اضافة تفويض لك
        RejectedDelegationEmail,//تم رفض التفويض الذي أضفته ل 
        DisabledDelegationEmail,//الغاء التفويض
        OrgUnitEmail,//احالة معاملة لصندوق ==> Redirect to OrgUnit (استقبال الادارة)   
        SupportEmail,
        ReminderBeforeTaskEndedEmail,
        ReminderBeforeTaskEndedWeb,


        VerificationCodeEmail,
        EditElectronicCopiesWeb,//تم تعدبل نسخة الكترونية من المعاملة 
        EditElectronicCopiesEmail,//تم تعديل نسخة الكترونية
        DeleteElectronicCopiesWeb,//تم حذف نسخة الكترونية من المعاملة       
        DeleteElectronicCopiesEmail,//تم حذف نسخة الكترونية
        LateTransactionWeb = 70,
        LateTransactionEmail,
        NewUserRequest,
        RoleRequest,
        CreateTransactionWithRemindDate = 75,
        VerificationTransactionCodeEmail = 76

    }
    public enum NotificationWebSubject
    {
        None = 0,
        TransactionAssignment,//احالة معاملة لصندوق => according to type
        TransactionAssignmentDraft,

        TaskReminder,//تذكير بمباشرة العمل على المهمة
        NewTask,//مهمة جديدة
        DeleteTask,//حذف مهمة
        ResendTask,//اعادة ارسال مهمة
        AcceptTask,//قبول المهمة
        RejectTask,//رفض المهمة
        ReplyTask,//رد على المهمة

        AssignTransaction,//استلام معاملة رقم
        RevertRejectTransaction,//رفض استلام معاملة
        RevertTransaction,//راجاع معاملة

        ElectronicCopies,//وصول نسخة الكترونية
        Viewed,//تم الاطلاع على النسخة الالكترونية

        Followup,//متابعة معاملة
        CancelFollowup,//الغاء متابعة معاملة من قبل موظف
        EndFollowupPeriod,//الغاء متابعة معاملة لإنتهاء مدة متابعتها
        CancelFollowupSendToSaved,//الغاء متابعة معاملة وذلك لأنها ارسلت لصندوق المنجزة

        AddExplanation,//اضافة شرح على نسخة إلكترونية
        ReceiveReport,//استلام بيان التسليم

        AddDelegation,//اضافة تفويض
        InProcessDelegation,
        ApprovedDelegation,//اضافة تفويض لك
        RejectedDelegation,//تم رفض التفويض الذي أضفته ل 
        DisabledDelegation,//الغاء التفويض
        EnableDelegation,//

        OrgUnit,//احالة معاملة لصندوق ==> Redirect to OrgUnit (استقبال الادارة)
        ReminderBeforeTaskEnded,
        VerificationCode,
        EditElectronicCopies,//تم تعديل نسخة الكترونية
        DeleteElectronicCopies,//تم حذف نسخة الكترونية
        LateTransaction,
        NewUser,
        RoleRequest,
        CreateTransactionWithRemindDate = 75
    }
    public enum NotificationEmailSubject
    {
        None = 0,
        NewUserEmail,
        ResetPasswordEmail,
        TransactionAssignmentEmail,
        TransactionAssignmentDraftEmail,
        TaskReminderEmail,
        NewTaskEmail,
        DeleteTaskEmail,
        ResendTaskEmail,
        AcceptTaskEmail,
        RejectTaskEmail,
        ReplyTaskEmail,
        AssignTransactionEmail,
        RevertRejectTransactionEmail,
        RevertTransactionEmail,
        ElectronicCopiesEmail,
        ViewedEmail,
        FollowupEmail,
        CancelFollowupEmail,
        EndFollowupPeriodEmail,
        CancelFollowupSendToSavedEmail,
        AddExplanationEmail,
        ReceiveReportEmail,//استلام بيان التسليم
        AddDelegationEmail,
        InProcessDelegationEmail,
        ApprovedDelegationEmail,
        RejectedDelegationEmail,
        DisabledDelegationEmail,
        EnableDelegationEmail,
        OrgUnitEmail,
        DeleteUser,//حذف مستخدم
        ModifiedUser,
        DisabledUser,
        EnabledUser,
        ReminderBeforeTaskEndedEmail,
        VerificationCodeEmail,
        EditElectronicCopiesEmail,
        DeleteElectronicCopiesEmail,
        FollowUpRecieveEmail,
        LateTransaction = 41,
        RoleRequest,
        CreateTransactionWithRemindDate = 44,
        VerificationTransactionCodeEmail = 45
    }
    public enum ActionType
    {
        SendMainTransaction = 1,
        SendCopyToTakeAction = 2,
        SendCopyToView = 3,
    }
    public enum Color
    {
        Green = 1,
        Red,
        Blue
    }
    public enum BarcodePrintType
    {
        Transaction = 1520,
        Copy = 1521,
        Attachment = 1522,
        VisitTicket
    }
    public enum BarcodeReferenceType
    {
        MainTransaction = 1,
        Copy,
        Attachment
    }
    public enum AuditingActionCode
    {
        None = 0,
        AddNewExternalParty,
        CreateTransaction,
        ViewBasicInformation,
        ViewTransactionArchiving,
        ViewTransactionNames,
        ViewTransactionLinks,
        //ViewTrasnactionAssigments = 1566,
        ViewTransactionCopies,
        ViewTransactionAttachmentsArchiving,
        OpenEditor,
        UpadteTransaction,
        PrintVisitTicket,
        ViewBarcodes,
        ViewCertificate,
        PrintDeliveryReport,
        PrintBarcode,
        AdvanceQuery,
        PrintWithoutWatermark,
        AcknowledgeElcOutBound,
        ViewTransaction

    }
    public enum EditorType
    {
        TextEditor = 1,
        Scanning,
        Text,
        File
    }
    public enum TrayActionType
    {
        Save = 1,
        Assign,
        Revert,
        SaveRevert,
        DeleteDraft,
        CreateOutbound,
        Viewed,
        ManagerRevert,
        ManagerSave,
        ManagerAssign,
        RejectRevert,
        RejectRevertToCreator,
        DeleteCopy,
        Complete,
        UndoDeleteCopy

    }
    public enum LinkType //LinkingType
    {
        ByInboundNumber = 1,
        ByOutboundNumber = 2,
    }
    public enum CollaborationMessageStatus
    {
        Read = 1,
        Unread = 2,
    }
    /// <summary>
    /// 0, 2^n
    /// </summary>
    [Flags]
    public enum NotificationSubscriptions
    {
        None = 0,
        MyTransactions = 1,
        OutboundDraft = 2,
        Tasks = 4,
        ElectronicCopies = 8,
        Followup = 16,
        Explanation = 32,
        ReceiveReport = 64,
        Delegation = 128,
        OrgUnit = 256,
        VerificationCode = 512
    }
    public enum NotificationSubscription //NotificationSubscriptions
    {
        None,
        MyTransactions = 425,
        OutboundDraft = 426,
        Tasks = 427,
        ElectronicCopies = 528,
        Followup = 529,
        Explanation = 530,
        ReceiveReport = 531,
        Delegation = 532,
        OrgUnit = 533,
        VerificationCode
    }
    public enum Title
    {
        Mr = 1,
        VIPUser = 8
    }
    public enum YesserTypesMapping
    {
        MsgPriority = 1,
        MsgSecrecy,
        MsgSubject,
        OrgUnitId,//4: Represents the current (local) entity for any request context (Originates from OrgUnits table)
        DestinationId, //5: Represents the remote entity for any request context (Originates from ExternalParties table)
        AttachementType
    }
    public enum DeliveryReportType
    {
        DeliveryReport = 1,
        LetterOfficialMail = 2,
        PackageOfficialMail = 3
    }
    public enum DeliveryMethodType
    {
        Paper = 1,
        Electronic,
        ElectronicPaper
    }
    public enum TransactionReportTypes
    {
        SearchInboundTransaction = 3285,
        SearchCreatedTransaction = 3276,
        SearchOutboundTransaction = 3277,
        SearchMergedTransaction = 3278,
        SearchLinkedTransaction = 3279,
        SearchCurrentTransaction = 3280,
        SearchDelayedTransaction = 3281,
        SearchCopiesTransaction = 3282,
        SearchRejectedTransaction = 3283,
        SearchSavedTransaction = 3284
    }
    public enum ReceivedTasksType
    {
        AcceptedTasks = 0,
        NewTasks = 1,
        EndTasks = 2
    }
    public enum TaskAcceptanceStatus
    {
        Accept = 0,
        Reject = 1
    }
    public enum Status
    {
        Failure = 0,
        UnableToDeliver,
        Success,
        Sent
    }
    public enum CultureType
    {
        Arabic = 1,
        English = 2
    }
    public enum CopiesActions
    {
        ToView = 1
    }
    public enum EntitiesType
    {
        Entities = 1,
        Individual
    }
    public enum SearchChosser
    {
        Common,
        Names,
        External,
        Inbound,
        Internal,
        Draft,
        Transferrd,
        Employees
    }
    public enum AttachmentSource
    {
        Scanned,
        Uploaded
    }
    public enum TransactionReportType
    {
        PerformanceMeasurementDepartment = 1,//463,
        PerformanceMeasurementStaff = 2//464
    }
    public enum RepresentationReportType
    {
        Table = 1,
        Barchart
    }
    public enum DelegationStatus
    {
        InProcess = 1,
        Approved,
        Rejected,
        Disabled
    }
    public enum AuditFor
    {
        MainDataAuditDetails = 1,
        AssignmentAuditDetails = 2,
        AttachmentsAuditDetails = 3,
        NamesAuditDetails = 4,
        ExplanationsAuditDetails = 5,
        DocumentInfoAuditDetails = 6,
        MainAudit = 7,
        Copies = 8,
        ExternalCopies = 9,
        Links = 10,
        Tasks = 11,
        FollowUp = 12,
        Print = 13
    }
    public enum UserStatus
    {
        Offline = 0,
        Active = 1,
        Inactive = 2,
    }
    public enum PasswordType
    {
        None = 0,
        Delete = 1,
        Edit
    }

    public enum SettingType
    {
        GeneralSettings = 1,
        OrgUnit = 2,
        ReportCounter = 3,
        DraftCounter = 4,
        CompanyName = 5,
        DefaultPassword = 6,
        Tenant = 7,
        Tray = 8,
        Search = 9,
        SMS = 10,
        SmartPhone = 11,
        Email = 12,
        Version = 13,
        DateAndNumbers = 14,
        Agency = 15,
        SystemConfiguration = 16,
        Counter = 17,
        ERPSyncTimestamp = 19
    }
    public enum ControlType
    {
        Text,
        Numeric,
        Checkbox,
        RadioButton,
        Password,
        Dropdown,
        Textarea,
        ImageUpload
    }
    public enum DateType
    {
        Ummalqura = 1,
        Gregorian
    }
    public enum ConnectionProtocolType
    {
        Wrapper = 0,
        HTTP = 1
    }
    public enum LookupType
    {
        Form,
        Link,
        AttachmentType,
        Actions,
        Correspondent,
        FollowUpPriorityType,
        FollowUpMethod,
        FollowUpProccess,
        FollowUpSource,
        ConfidentialityAcknowledgments,
        SaveReason,
    }
    public enum LookupOperationType
    {
        Delete,
        Edit,
        Lock,
        UnLock,
        Active,
        Deactivate
    }
    public enum TransCopyStatus
    {
        NotViewed = 1,
        Viewed,
        Delete
    }

    public enum OrgUnitTreeMode
    {
        Search = 1,
        Admin,
        User
    }

    public enum HubDeliveryType
    {
        M = 3268,
        E = 3267,
    }
    public enum HubDocumentType
    {
        ExternalAttachment = 58
    }

    public enum DefaultCategoryTypes
    {
        Confedentiality,
        TransactionSourceType,
        BasicDeliveryMethod,
        PriorityLevel,
        InboundDocumentType
    }
    public enum PriorityType
    {
        Normal = 1,
        Urgent,
        VeryUrgent,
        Now,
        Immediately,
        HasDate
    }


    public enum SigntureType
    {
        Electronic = 1,
        Command,
        Behalf,
        Message,
        Seal,
        Marking
    }

    public enum DefaultDisplay
    {
        Cards = 1,
        Table
    }

    public enum TransactionFollowupStatus
    {
        Invalid = -1,
        UnderFollowup = 537,
        Closed = 538,
        Sent = 539
    }

    public enum FollowUpDurationUnit
    {
        Day = 518,
        Month = 517,
        Year = 516
    }
    public enum FollowupStatus
    {
        All = -1,
        New = 1,
        UnderFollowup = 2,
        Completed = 3,
        Delayed = 4,
        Cancled = 5,
        UnderFollowupSecondLevel = 6,
        UnLockFollowup = 7,
        WithDrow = 8,
        ReActiveParent = 9,
        EnsureComplition = 10,
    }

    public enum FollowupType
    {
        Privet = 1,
        Public = 2,
        Secondary = 3
    }



    public enum FollowupAuditProcess
    {
        AddAssignFollowup = 1,
        AddPublicFollowup = 2,
        ReciveFollowup = 3,
        CancelFollowup = 4,
        UnLoackFollowup = 5,
        WithDrawFollowup = 6,
        LockFollowup = 7,
        UnderProcessingFollowup = 8,
        AddSecondaryFollowup = 9,
        CompletionFollowup = 10,
        InsureCompletionFollowup = 11,
        UnLockCompletionFollowup = 12,
        WithDrawSecondaryFollowup = 13,
        UnderSecondaryFollowup = 14,
        ReminderFollowup = 15,
        ScaltFollowup = 17,
        LinkingFollowup = 18,
        AssignmentCompliteFollowupAndClosing = 19,
        SaveCompliteFollowupAndClosing = 20,
        AddPrivetFollowup = 22,
        ConvertToOutBoundCompliteFollowupAndClosing = 23,
        LinkWithTransaction = 24,
    }

    public enum TrayProcedureId
    {
        Necessary = 3,
        Opinion = 11,
        Follow = 12,
        Reviews = 13,
        OthersAll = 0
    }

    public enum TrayProcedureFilter
    {
        Necessary = 101,
        Opinion = 102,
        Follow = 103,
        Reviews = 104,
        OthersAll = 105
    }

    public enum InquiryType
    {
        TransactionNumber = 1,
        InboundDocumentNumber,
        SubjectSearch,
        Name,
        Remarks
    }

    public enum PrivacyOfTransactions
    {
        Private = 1,
        Limited = 2,
        OpenByHand = 3
    }

    public enum SurveyAnswers
    {
        VeryBad = 1,
        Bad = 2,
        Good = 3,
        VeryGood = 4,
        Excellent = 5,
    }
    public enum EncryptionChannel
    {
        Sms = 1,
        Email = 2
    }
    public enum UserMobileClass
    {
        VipUser = 1,
        NormalUser = 2,
        ReporterUser = 3
    }
}
