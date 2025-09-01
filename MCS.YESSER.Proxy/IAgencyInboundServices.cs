namespace MCS.YESSER.Proxy
{




    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", ConfigurationName = "IAgencyInboundServices")]
    public interface IAgencyInboundServices
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveOutboundRq", ReplyAction = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveOutboundRs")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Status")]
        Status_Type ReceiveOutbound(out string ErrorCode, out string ErrorMessage, ReceiveOutboundOutboundRec OutboundRec);

        [System.ServiceModel.OperationContractAttribute(Action = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveRejectOutboundRq", ReplyAction = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveRejectOutboundRs")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Status")]
        Status_Type ReceiveRejectOutbound(out string ErrorCode, out string ErrorMessage, string From, string To, string OutboundDocumentNumber, System.DateTime RejectionDate, string RejectionCode, string RejectionReason);

        [System.ServiceModel.OperationContractAttribute(Action = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveStatusInquiryRq", ReplyAction = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveStatusInquiryRs")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        void ReceiveStatusInquiry(string From, string To, ref string OutboundDocumentNumber, out string InboundDocumentNumber, out string InboundCreationTimestamp, out MsgStatus_Type Status, out System.DateTime Timestamp, out string ErrorCode, out string ErrorMessage);

        // CODEGEN: Parameter 'OSRec' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveOSUpdateRq", ReplyAction = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveOSUpdateRs")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name = "Status")]
        ReceiveOSUpdateResponse ReceiveOSUpdate(ReceiveOSUpdateRequest request);

        // CODEGEN: Generating message contract since the wrapper name (ReceiveConfirmTransactionReponse) of message ReceiveConfirmTransactionResponse does not match the default value (ReceiveConfirmTransaction)
        [System.ServiceModel.OperationContractAttribute(Action = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveConfirmTransactionRq", ReplyAction = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveConfirmTransactionRs")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        ReceiveConfirmTransactionResponse ReceiveConfirmTransaction(ReceiveConfirmTransactionRequest request);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1")]
    public partial class ReceiveOutboundOutboundRec : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ReceiveOutboundOutboundRecOutboundInfo outboundInfoField;

        private ReceiveOutboundOutboundRecRoutingInfo routingInfoField;

        private System.Collections.Generic.List<ReceiveOutboundOutboundRecAttachmentRec> attachmentRecField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public virtual ReceiveOutboundOutboundRecOutboundInfo OutboundInfo
        {
            get
            {
                return this.outboundInfoField;
            }
            set
            {
                this.outboundInfoField = value;
                this.RaisePropertyChanged("OutboundInfo");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public virtual ReceiveOutboundOutboundRecRoutingInfo RoutingInfo
        {
            get
            {
                return this.routingInfoField;
            }
            set
            {
                this.routingInfoField = value;
                this.RaisePropertyChanged("RoutingInfo");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AttachmentRec", Order = 2)]
        public virtual System.Collections.Generic.List<ReceiveOutboundOutboundRecAttachmentRec> AttachmentRec
        {
            get
            {
                return this.attachmentRecField;
            }
            set
            {
                this.attachmentRecField = value;
                this.RaisePropertyChanged("AttachmentRec");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1")]
    public partial class ReceiveOutboundOutboundRecOutboundInfo : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string outboundDocNoField;

        private Msg_Subject outboundTypeField;

        private System.DateTime outboundGDateField;

        private string outboundHDateField;

        private string outboundSubjectField;

        private OutboundCategory_Type outboundCategoryField;

        private string outboundRemarksField;

        private string sPTrackingNumberField;

        private string outboundHDueDateField;

        private System.DateTime? outboundGDueDateField;

        private bool outboundGDueDateFieldSpecified;

        private OutboundClassification_Type outboundClassificationField;

        private System.Collections.Generic.List<ReceiveOutboundOutboundRecOutboundInfoRelatedPersonsInfo> relatedPersonsInfoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public virtual string OutboundDocNo
        {
            get
            {
                return this.outboundDocNoField;
            }
            set
            {
                this.outboundDocNoField = value;
                this.RaisePropertyChanged("OutboundDocNo");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public virtual Msg_Subject OutboundType
        {
            get
            {
                return this.outboundTypeField;
            }
            set
            {
                this.outboundTypeField = value;
                this.RaisePropertyChanged("OutboundType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public virtual System.DateTime OutboundGDate
        {
            get
            {
                return this.outboundGDateField;
            }
            set
            {
                this.outboundGDateField = value;
                this.RaisePropertyChanged("OutboundGDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public virtual string OutboundHDate
        {
            get
            {
                return this.outboundHDateField;
            }
            set
            {
                this.outboundHDateField = value;
                this.RaisePropertyChanged("OutboundHDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public virtual string OutboundSubject
        {
            get
            {
                return this.outboundSubjectField;
            }
            set
            {
                this.outboundSubjectField = value;
                this.RaisePropertyChanged("OutboundSubject");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public virtual OutboundCategory_Type OutboundCategory
        {
            get
            {
                return this.outboundCategoryField;
            }
            set
            {
                this.outboundCategoryField = value;
                this.RaisePropertyChanged("OutboundCategory");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public virtual string OutboundRemarks
        {
            get
            {
                return this.outboundRemarksField;
            }
            set
            {
                this.outboundRemarksField = value;
                this.RaisePropertyChanged("OutboundRemarks");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public virtual string SPTrackingNumber
        {
            get
            {
                return this.sPTrackingNumberField;
            }
            set
            {
                this.sPTrackingNumberField = value;
                this.RaisePropertyChanged("SPTrackingNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public virtual string OutboundHDueDate
        {
            get
            {
                return this.outboundHDueDateField;
            }
            set
            {
                this.outboundHDueDateField = value;
                this.RaisePropertyChanged("OutboundHDueDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public virtual System.DateTime? OutboundGDueDate
        {
            get
            {
                return this.outboundGDueDateField;
            }
            set
            {
                this.outboundGDueDateField = value;
                this.RaisePropertyChanged("OutboundGDueDate");
                this.OutboundGDueDateSpecified = true;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public virtual bool OutboundGDueDateSpecified
        {
            get
            {
                return this.outboundGDueDateFieldSpecified;
            }
            set
            {
                this.outboundGDueDateFieldSpecified = value;
                this.RaisePropertyChanged("OutboundGDueDateSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public virtual OutboundClassification_Type OutboundClassification
        {
            get
            {
                return this.outboundClassificationField;
            }
            set
            {
                this.outboundClassificationField = value;
                this.RaisePropertyChanged("OutboundClassification");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RelatedPersonsInfo", Order = 11)]
        public virtual System.Collections.Generic.List<ReceiveOutboundOutboundRecOutboundInfoRelatedPersonsInfo> RelatedPersonsInfo
        {
            get
            {
                return this.relatedPersonsInfoField;
            }
            set
            {
                this.relatedPersonsInfoField = value;
                this.RaisePropertyChanged("RelatedPersonsInfo");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum Msg_Subject
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("01")]
        Item01,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02")]
        Item02,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03")]
        Item03,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("04")]
        Item04,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("05")]
        Item05,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06")]
        Item06,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07")]
        Item07,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08")]
        Item08,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("09")]
        Item09,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11")]
        Item11,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("12")]
        Item12,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13")]
        Item13,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("14")]
        Item14,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15")]
        Item15,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("16")]
        Item16,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("17")]
        Item17,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("18")]
        Item18,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("19")]
        Item19,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20")]
        Item20,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21")]
        Item21,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("22")]
        Item22,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("23")]
        Item23,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("24")]
        Item24,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("25")]
        Item25,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("26")]
        Item26,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("27")]
        Item27,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("28")]
        Item28,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("29")]
        Item29,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("30")]
        Item30,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("31")]
        Item31,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("32")]
        Item32,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("33")]
        Item33,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("34")]
        Item34,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("35")]
        Item35,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("36")]
        Item36,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("37")]
        Item37,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("38")]
        Item38,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("39")]
        Item39,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("40")]
        Item40,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("41")]
        Item41,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("42")]
        Item42,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("43")]
        Item43,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("44")]
        Item44,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("45")]
        Item45,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("46")]
        Item46,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("47")]
        Item47,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("48")]
        Item48,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("49")]
        Item49,
        [System.Xml.Serialization.XmlEnumAttribute("50")]
        Item50,

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum OutboundCategory_Type
    {
        /// <remarks/>
        Financial,
        /// <remarks/>
        Management,
        /// <remarks/>
        Judicial,
        /// <remarks/>
        Penal,
        /// <remarks/>
        General,
        /// <remarks/>
        Ministerial,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum OutboundClassification_Type
    {

        /// <remarks/>
        Original,

        /// <remarks/>
        Copy,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1")]
    public partial class ReceiveOutboundOutboundRecOutboundInfoRelatedPersonsInfo : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string personFullNameField;

        private string personIDField;

        private string personAddressField;

        private string personEmailField;

        private string personMobileNumberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public virtual string PersonFullName
        {
            get
            {
                return this.personFullNameField;
            }
            set
            {
                this.personFullNameField = value;
                this.RaisePropertyChanged("PersonFullName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public virtual string PersonID
        {
            get
            {
                return this.personIDField;
            }
            set
            {
                this.personIDField = value;
                this.RaisePropertyChanged("PersonID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public virtual string PersonAddress
        {
            get
            {
                return this.personAddressField;
            }
            set
            {
                this.personAddressField = value;
                this.RaisePropertyChanged("PersonAddress");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public virtual string PersonEmail
        {
            get
            {
                return this.personEmailField;
            }
            set
            {
                this.personEmailField = value;
                this.RaisePropertyChanged("PersonEmail");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public virtual string PersonMobileNumber
        {
            get
            {
                return this.personMobileNumberField;
            }
            set
            {
                this.personMobileNumberField = value;
                this.RaisePropertyChanged("PersonMobileNumber");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1")]
    public partial class ReceiveOutboundOutboundRecRoutingInfo : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string fromField;

        private string toField;

        private Msg_Secrecy secrecyLevelField;

        private Msg_Priority priorityField;

        private OutboundDelivery_Type deliveryTypeField;

        private OutboundSender_Type senderTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public virtual string From
        {
            get
            {
                return this.fromField;
            }
            set
            {
                this.fromField = value;
                this.RaisePropertyChanged("From");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public virtual string To
        {
            get
            {
                return this.toField;
            }
            set
            {
                this.toField = value;
                this.RaisePropertyChanged("To");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public virtual Msg_Secrecy SecrecyLevel
        {
            get
            {
                return this.secrecyLevelField;
            }
            set
            {
                this.secrecyLevelField = value;
                this.RaisePropertyChanged("SecrecyLevel");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public virtual Msg_Priority Priority
        {
            get
            {
                return this.priorityField;
            }
            set
            {
                this.priorityField = value;
                this.RaisePropertyChanged("Priority");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public virtual OutboundDelivery_Type DeliveryType
        {
            get
            {
                return this.deliveryTypeField;
            }
            set
            {
                this.deliveryTypeField = value;
                this.RaisePropertyChanged("DeliveryType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public virtual OutboundSender_Type SenderType
        {
            get
            {
                return this.senderTypeField;
            }
            set
            {
                this.senderTypeField = value;
                this.RaisePropertyChanged("SenderType");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum Msg_Secrecy
    {

        /// <remarks/>
        S,

        /// <remarks/>
        TS,

        /// <remarks/>
        N,

        /// <remarks/>
        NN,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum Msg_Priority
    {

        /// <remarks/>
        L,

        /// <remarks/>
        N,

        /// <remarks/>
        H,

        /// <remarks/>
        C,

        /// <remarks/>
        I,

        /// <remarks/>
        BH,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum OutboundDelivery_Type
    {

        /// <remarks/>
        M,

        /// <remarks/>
        E,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum OutboundSender_Type
    {

        /// <remarks/>
        GOVT,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1")]
    public partial class ReceiveOutboundOutboundRecAttachmentRec : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string attachmentIdField;

        private byte[] attachmentBase64Field;

        private string remarksField;

        private Attachment_Type attachementTypeField;

        private AttachmentContent_Type attachmentContentTypeField;

        private string attachmentFileNameField;

        private string attachementURLField;

        private bool isObjectField;

        private byte[] attachmentBarcodeField;

        private Content_Classification contentClassificationField;

        private bool contentClassificationFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public virtual string AttachmentId
        {
            get
            {
                return this.attachmentIdField;
            }
            set
            {
                this.attachmentIdField = value;
                this.RaisePropertyChanged("AttachmentId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 1)]
        public virtual byte[] AttachmentBase64
        {
            get
            {
                return this.attachmentBase64Field;
            }
            set
            {
                this.attachmentBase64Field = value;
                this.RaisePropertyChanged("AttachmentBase64");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public virtual string Remarks
        {
            get
            {
                return this.remarksField;
            }
            set
            {
                this.remarksField = value;
                this.RaisePropertyChanged("Remarks");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public virtual Attachment_Type AttachementType
        {
            get
            {
                return this.attachementTypeField;
            }
            set
            {
                this.attachementTypeField = value;
                this.RaisePropertyChanged("AttachementType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public virtual AttachmentContent_Type AttachmentContentType
        {
            get
            {
                return this.attachmentContentTypeField;
            }
            set
            {
                this.attachmentContentTypeField = value;
                this.RaisePropertyChanged("AttachmentContentType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public virtual string AttachmentFileName
        {
            get
            {
                return this.attachmentFileNameField;
            }
            set
            {
                this.attachmentFileNameField = value;
                this.RaisePropertyChanged("AttachmentFileName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public virtual string AttachementURL
        {
            get
            {
                return this.attachementURLField;
            }
            set
            {
                this.attachementURLField = value;
                this.RaisePropertyChanged("AttachementURL");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public virtual bool IsObject
        {
            get
            {
                return this.isObjectField;
            }
            set
            {
                this.isObjectField = value;
                this.RaisePropertyChanged("IsObject");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 8)]
        public virtual byte[] AttachmentBarcode
        {
            get
            {
                return this.attachmentBarcodeField;
            }
            set
            {
                this.attachmentBarcodeField = value;
                this.RaisePropertyChanged("AttachmentBarcode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public virtual Content_Classification ContentClassification
        {
            get
            {
                return this.contentClassificationField;
            }
            set
            {
                this.contentClassificationField = value;
                this.RaisePropertyChanged("ContentClassification");
                this.ContentClassificationSpecified = true;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public virtual bool ContentClassificationSpecified
        {
            get
            {
                return this.contentClassificationFieldSpecified;
            }
            set
            {
                this.contentClassificationFieldSpecified = value;
                this.RaisePropertyChanged("ContentClassificationSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum Attachment_Type
    {

        /// <remarks/>
        MAIN,
        /// <remarks/>
        SUB,
        /// <remarks/>
        COPY,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum AttachmentContent_Type
    {
        /// <remarks/>
        PNG,
        /// <remarks/>
        JPEG,
        /// <remarks/>
        BMP,
        /// <remarks/>
        GIF,
        /// <remarks/>
        PDF,
        /// <remarks/>
        TIF,
        /// <remarks/>
        DOC,
        /// <remarks/>
        DOCX,
        /// <remarks/>
        XLS,
        /// <remarks/>
        XLSX,
        /// <remarks></remarks>
        OBJ,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum Content_Classification
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("01")]
        Item01,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02")]
        Item02,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03")]
        Item03,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("04")]
        Item04,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("05")]
        Item05,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06")]
        Item06,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07")]
        Item07,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08")]
        Item08,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("09")]
        Item09,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11")]
        Item11,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("12")]
        Item12,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13")]
        Item13,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("14")]
        Item14,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15")]
        Item15,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("16")]
        Item16,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("17")]
        Item17,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("18")]
        Item18,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("19")]
        Item19,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20")]
        Item20,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21")]
        Item21,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("22")]
        Item22,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("23")]
        Item23,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("24")]
        Item24,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("25")]
        Item25,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("26")]
        Item26,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("27")]
        Item27,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("28")]
        Item28,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("29")]
        Item29,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("30")]
        Item30,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("31")]
        Item31,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("32")]
        Item32,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("33")]
        Item33,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("34")]
        Item34,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("35")]
        Item35,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("36")]
        Item36,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("37")]
        Item37,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("38")]
        Item38,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("39")]
        Item39,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("40")]
        Item40,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("41")]
        Item41,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("42")]
        Item42,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("43")]
        Item43,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("44")]
        Item44,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("45")]
        Item45,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("46")]
        Item46,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("47")]
        Item47,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("48")]
        Item48,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("49")]
        Item49,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("50")]
        Item50,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("51")]
        Item51,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("52")]
        Item52,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("53")]
        Item53,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("54")]
        Item54,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("55")]
        Item55,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("56")]
        Item56,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("57")]
        Item57,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("58")]
        Item58,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("59")]
        Item59,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("60")]
        Item60,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("61")]
        Item61,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("62")]
        Item62,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("63")]
        Item63,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("64")]
        Item64,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("65")]
        Item65,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("66")]
        Item66,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("67")]
        Item67,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("68")]
        Item68,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("69")]
        Item69,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("70")]
        Item70,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("71")]
        Item71,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("72")]
        Item72,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("73")]
        Item73,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("74")]
        Item74,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("75")]
        Item75,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("76")]
        Item76,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("77")]
        Item77,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("78")]
        Item78,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum Status_Type
    {

        /// <remarks/>
        Success,

        /// <remarks/>
        Failed,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum MsgStatus_Type
    {

        /// <remarks/>
        Pending,

        /// <remarks/>
        Accepted,

        /// <remarks/>
        Rejected,

        /// <remarks/>
        NotFound,

        /// <remarks/>
        Failed,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1")]
    public partial class ReceiveOSUpdateOSRec : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string mainIdField;

        private string subIdField;

        private string aRNameField;

        private string eNNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public virtual string MainId
        {
            get
            {
                return this.mainIdField;
            }
            set
            {
                this.mainIdField = value;
                this.RaisePropertyChanged("MainId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public virtual string SubId
        {
            get
            {
                return this.subIdField;
            }
            set
            {
                this.subIdField = value;
                this.RaisePropertyChanged("SubId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public virtual string ARName
        {
            get
            {
                return this.aRNameField;
            }
            set
            {
                this.aRNameField = value;
                this.RaisePropertyChanged("ARName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public virtual string ENName
        {
            get
            {
                return this.eNNameField;
            }
            set
            {
                this.eNNameField = value;
                this.RaisePropertyChanged("ENName");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ReceiveOSUpdate", WrapperNamespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", IsWrapped = true)]
    public partial class ReceiveOSUpdateRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("OSRec")]
        public System.Collections.Generic.List<ReceiveOSUpdateOSRec> OSRec;

        public ReceiveOSUpdateRequest()
        {
        }

        public ReceiveOSUpdateRequest(System.Collections.Generic.List<ReceiveOSUpdateOSRec> OSRec)
        {
            this.OSRec = OSRec;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ReceiveOSUpdateResponse", WrapperNamespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", IsWrapped = true)]
    public partial class ReceiveOSUpdateResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 0)]
        public Status_Type Status;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 1)]
        public string ErrorCode;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 2)]
        public string ErrorMessage;

        public ReceiveOSUpdateResponse()
        {
        }

        public ReceiveOSUpdateResponse(Status_Type Status, string ErrorCode, string ErrorMessage)
        {
            this.Status = Status;
            this.ErrorCode = ErrorCode;
            this.ErrorMessage = ErrorMessage;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    public enum DeliveryStatus_Type
    {

        /// <remarks/>
        Delivered,

        /// <remarks/>
        UnableToDeliver,
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ReceiveConfirmTransaction", WrapperNamespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", IsWrapped = true)]
    public partial class ReceiveConfirmTransactionRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 0)]
        public string From;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 1)]
        public string To;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 2)]
        public string OutboundDocumentNo;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 3)]
        public DeliveryStatus_Type Status;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 4)]
        public string ErrorCode;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 5)]
        public string ErrorMessage;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 6)]
        public System.DateTime Timestamp;

        public ReceiveConfirmTransactionRequest()
        {
        }

        public ReceiveConfirmTransactionRequest(string From, string To, string OutboundDocumentNo, DeliveryStatus_Type Status, string ErrorCode, string ErrorMessage, System.DateTime Timestamp)
        {
            this.From = From;
            this.To = To;
            this.OutboundDocumentNo = OutboundDocumentNo;
            this.Status = Status;
            this.ErrorCode = ErrorCode;
            this.ErrorMessage = ErrorMessage;
            this.Timestamp = Timestamp;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ReceiveConfirmTransactionReponse", WrapperNamespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", IsWrapped = true)]
    public partial class ReceiveConfirmTransactionResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 0)]
        public Status_Type Status;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 1)]
        public string ErrorCode;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 2)]
        public string ErrorMessage;

        public ReceiveConfirmTransactionResponse()
        {
        }

        public ReceiveConfirmTransactionResponse(Status_Type Status, string ErrorCode, string ErrorMessage)
        {
            this.Status = Status;
            this.ErrorCode = ErrorCode;
            this.ErrorMessage = ErrorMessage;
        }
    }

    //[System.ServiceModel.ServiceBehaviorAttribute(InstanceContextMode = System.ServiceModel.InstanceContextMode.PerCall, ConcurrencyMode = System.ServiceModel.ConcurrencyMode.Single)]
    //public class AgencyInboundServices : IAgencyInboundServices
    //{

    //    public virtual Status_Type ReceiveOutbound(out string ErrorCode, out string ErrorMessage, ReceiveOutboundOutboundRec OutboundRec)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public virtual Status_Type ReceiveRejectOutbound(out string ErrorCode, out string ErrorMessage, string From, string To, string OutboundDocumentNumber, System.DateTime RejectionDate, string RejectionCode, string RejectionReason)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public virtual void ReceiveStatusInquiry(string From, string To, ref string OutboundDocumentNumber, out string InboundDocumentNumber, out string InboundCreationTimestamp, out MsgStatus_Type Status, out System.DateTime Timestamp, out string ErrorCode, out string ErrorMessage)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public virtual ReceiveOSUpdateResponse ReceiveOSUpdate(ReceiveOSUpdateRequest request)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public virtual ReceiveConfirmTransactionResponse ReceiveConfirmTransaction(ReceiveConfirmTransactionRequest request)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    //[System.ServiceModel.ServiceContractAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", ConfigurationName = "IAgencyInboundServices")]
    //public interface IAgencyInboundServices
    //{

    //    [System.ServiceModel.OperationContractAttribute(Action = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveOutboundRq", ReplyAction = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1/IAgencyInboundServi" +
    //        "ces/ReceiveOutboundResponse")]
    //    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    //    [return: System.ServiceModel.MessageParameterAttribute(Name = "Status")]
    //    string ReceiveOutbound(ReceiveOutboundOutboundRec OutboundRec, out string ErrorCode, out string ErrorMessage);


    //    [System.ServiceModel.OperationContractAttribute(Action = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveRejectOutboundRq", ReplyAction = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1/IAgencyInboundServi" +
    //        "ces/ReceiveRejectOutboundResponse")]
    //    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    //    [return: System.ServiceModel.MessageParameterAttribute(Name = "Status")]
    //    string ReceiveRejectOutbound(out string ErrorCode, out string ErrorMessage, string From, string To, string OutboundDocumentNumber, System.DateTime RejectionDate, string RejectionCode, string RejectionReason);

    //    [System.ServiceModel.OperationContractAttribute(Action = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveStatusInquiryRq", ReplyAction = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1/IAgencyInboundServi" +
    //        "ces/ReceiveStatusInquiryResponse")]
    //    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    //        string ReceiveStatusInquiry(string From, string To, ref string OutboundDocumentNumber, out string InboundDocumentNumber, out string InboundCreationTimestamp, out string Status, out System.DateTime Timestamp, out string ErrorCode, out string ErrorMessage);

    //    // CODEGEN: Parameter 'OSRec' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
    //    [System.ServiceModel.OperationContractAttribute(Action = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveOSUpdateRq", ReplyAction = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1/IAgencyInboundServi" +
    //        "ces/ReceiveOSUpdateResponse")]
    //    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    //    [return: System.ServiceModel.MessageParameterAttribute(Name = "Status")]
    //        ReceiveOSUpdateResponse ReceiveOSUpdate(ReceiveOSUpdateRequest request);

    //    // CODEGEN: Generating message contract since the wrapper name (ReceiveConfirmTransactionReponse) of message ReceiveConfirmTransactionResponse does not match the default value (ReceiveConfirmTransaction)
    //    [System.ServiceModel.OperationContractAttribute(Action = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1:ReceiveConfirmTransactionRq", ReplyAction = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1/IAgencyInboundServi" +
    //        "ces/ReceiveConfirmTransactionResponse")]
    //    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    //    ReceiveConfirmTransactionResponse ReceiveConfirmTransaction(ReceiveConfirmTransactionRequest request);
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1")]
    //public partial class ReceiveOutboundOutboundRec
    //{

    //    private ReceiveOutboundOutboundRecOutboundInfo outboundInfoField;

    //    private ReceiveOutboundOutboundRecRoutingInfo routingInfoField;

    //    private ReceiveOutboundOutboundRecAttachmentRec[] attachmentRecField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
    //    public ReceiveOutboundOutboundRecOutboundInfo OutboundInfo
    //    {
    //        get
    //        {
    //            return this.outboundInfoField;
    //        }
    //        set
    //        {
    //            this.outboundInfoField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
    //    public ReceiveOutboundOutboundRecRoutingInfo RoutingInfo
    //    {
    //        get
    //        {
    //            return this.routingInfoField;
    //        }
    //        set
    //        {
    //            this.routingInfoField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
    //    public ReceiveOutboundOutboundRecAttachmentRec[] AttachmentRec
    //    {
    //        get
    //        {
    //            return this.attachmentRecField;
    //        }
    //        set
    //        {
    //            this.attachmentRecField = value;
    //        }
    //    }
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1")]
    //public partial class ReceiveOutboundOutboundRecOutboundInfo
    //{

    //    private string outboundDocNoField;

    //    private string outboundTypeField;

    //    private System.DateTime outboundGDateField;

    //    private string outboundHDateField;

    //    private string outboundSubjectField;

    //    private OutboundCategory_Type outboundCategoryField;

    //    private string outboundRemarksField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
    //    public string OutboundDocNo
    //    {
    //        get
    //        {
    //            return this.outboundDocNoField;
    //        }
    //        set
    //        {
    //            this.outboundDocNoField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
    //    public string OutboundType
    //    {
    //        get
    //        {
    //            return this.outboundTypeField;
    //        }
    //        set
    //        {
    //            this.outboundTypeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
    //    public System.DateTime OutboundGDate
    //    {
    //        get
    //        {
    //            return this.outboundGDateField;
    //        }
    //        set
    //        {
    //            this.outboundGDateField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
    //    public string OutboundHDate
    //    {
    //        get
    //        {
    //            return this.outboundHDateField;
    //        }
    //        set
    //        {
    //            this.outboundHDateField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
    //    public string OutboundSubject
    //    {
    //        get
    //        {
    //            return this.outboundSubjectField;
    //        }
    //        set
    //        {
    //            this.outboundSubjectField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
    //    public OutboundCategory_Type OutboundCategory
    //    {
    //        get
    //        {
    //            return this.outboundCategoryField;
    //        }
    //        set
    //        {
    //            this.outboundCategoryField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
    //    public string OutboundRemarks
    //    {
    //        get
    //        {
    //            return this.outboundRemarksField;
    //        }
    //        set
    //        {
    //            this.outboundRemarksField = value;
    //        }
    //    }
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    //public enum OutboundCategory_Type
    //{

    //    /// <remarks/>
    //    Financial,

    //    /// <remarks/>
    //    Management,
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1")]
    //public partial class ReceiveOutboundOutboundRecRoutingInfo
    //{

    //    private string fromField;

    //    private string toField;

    //    private string secrecyLevelField;

    //    private string priorityField;

    //    private OutboundDelivery_Type deliveryTypeField;

    //    private OutboundSender_Type senderTypeField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
    //    public string From
    //    {
    //        get
    //        {
    //            return this.fromField;
    //        }
    //        set
    //        {
    //            this.fromField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
    //    public string To
    //    {
    //        get
    //        {
    //            return this.toField;
    //        }
    //        set
    //        {
    //            this.toField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
    //    public string SecrecyLevel
    //    {
    //        get
    //        {
    //            return this.secrecyLevelField;
    //        }
    //        set
    //        {
    //            this.secrecyLevelField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
    //    public string Priority
    //    {
    //        get
    //        {
    //            return this.priorityField;
    //        }
    //        set
    //        {
    //            this.priorityField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
    //    public OutboundDelivery_Type DeliveryType
    //    {
    //        get
    //        {
    //            return this.deliveryTypeField;
    //        }
    //        set
    //        {
    //            this.deliveryTypeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
    //    public OutboundSender_Type SenderType
    //    {
    //        get
    //        {
    //            return this.senderTypeField;
    //        }
    //        set
    //        {
    //            this.senderTypeField = value;
    //        }
    //    }
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    //public enum OutboundDelivery_Type
    //{

    //    /// <remarks/>
    //    M,

    //    /// <remarks/>
    //    E,
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    //public enum OutboundSender_Type
    //{

    //    /// <remarks/>
    //    GOVT,
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1")]
    //public partial class ReceiveOutboundOutboundRecAttachmentRec
    //{

    //    private string attachmentIdField;

    //    private byte[] attachmentBase64Field;

    //    private string remarksField;

    //    private Attachment_Type attachementTypeField;

    //    private AttachmentContent_Type attachmentContentTypeField;

    //    private string attachmentFileNameField;

    //    private string attachementURLField;

    //    private bool isObjectField;

    //    private byte[] attachmentBarcodeField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
    //    public string AttachmentId
    //    {
    //        get
    //        {
    //            return this.attachmentIdField;
    //        }
    //        set
    //        {
    //            this.attachmentIdField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 1)]
    //    public byte[] AttachmentBase64
    //    {
    //        get
    //        {
    //            return this.attachmentBase64Field;
    //        }
    //        set
    //        {
    //            this.attachmentBase64Field = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
    //    public string Remarks
    //    {
    //        get
    //        {
    //            return this.remarksField;
    //        }
    //        set
    //        {
    //            this.remarksField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
    //    public Attachment_Type AttachementType
    //    {
    //        get
    //        {
    //            return this.attachementTypeField;
    //        }
    //        set
    //        {
    //            this.attachementTypeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
    //    public AttachmentContent_Type AttachmentContentType
    //    {
    //        get
    //        {
    //            return this.attachmentContentTypeField;
    //        }
    //        set
    //        {
    //            this.attachmentContentTypeField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
    //    public string AttachmentFileName
    //    {
    //        get
    //        {
    //            return this.attachmentFileNameField;
    //        }
    //        set
    //        {
    //            this.attachmentFileNameField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
    //    public string AttachementURL
    //    {
    //        get
    //        {
    //            return this.attachementURLField;
    //        }
    //        set
    //        {
    //            this.attachementURLField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
    //    public bool IsObject
    //    {
    //        get
    //        {
    //            return this.isObjectField;
    //        }
    //        set
    //        {
    //            this.isObjectField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 8)]
    //    public byte[] AttachmentBarcode
    //    {
    //        get
    //        {
    //            return this.attachmentBarcodeField;
    //        }
    //        set
    //        {
    //            this.attachmentBarcodeField = value;
    //        }
    //    }
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    //public enum Attachment_Type
    //{

    //    /// <remarks/>
    //    ORIGINAL,

    //    /// <remarks/>
    //    COPY,
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://yefi.gov.sa/CSIHUBDataTypes/xml/schemas/version1.0")]
    //public enum AttachmentContent_Type
    //{

    //    /// <remarks/>
    //    PDF,

    //    /// <remarks/>
    //    TIF,

    //    /// <remarks/>
    //    JPG,

    //    /// <remarks/>
    //    BMP,

    //    /// <remarks/>
    //    MP3,

    //    /// <remarks/>
    //    MP4,
    //}

    ///// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1")]
    //public partial class ReceiveOSUpdateOSRec
    //{

    //    private string mainIdField;

    //    private string subIdField;

    //    private string aRNameField;

    //    private string eNNameField;

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
    //    public string MainId
    //    {
    //        get
    //        {
    //            return this.mainIdField;
    //        }
    //        set
    //        {
    //            this.mainIdField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
    //    public string SubId
    //    {
    //        get
    //        {
    //            return this.subIdField;
    //        }
    //        set
    //        {
    //            this.subIdField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
    //    public string ARName
    //    {
    //        get
    //        {
    //            return this.aRNameField;
    //        }
    //        set
    //        {
    //            this.aRNameField = value;
    //        }
    //    }

    //    /// <remarks/>
    //    [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
    //    public string ENName
    //    {
    //        get
    //        {
    //            return this.eNNameField;
    //        }
    //        set
    //        {
    //            this.eNNameField = value;
    //        }
    //    }
    //}

    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    //[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    //[System.ServiceModel.MessageContractAttribute(WrapperName = "ReceiveOSUpdate", WrapperNamespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", IsWrapped = true)]
    //public partial class ReceiveOSUpdateRequest
    //{

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 0)]
    //    [System.Xml.Serialization.XmlElementAttribute("OSRec")]
    //    public System.Collections.Generic.List<ReceiveOSUpdateOSRec> OSRec;

    //    public ReceiveOSUpdateRequest()
    //    {
    //    }

    //    public ReceiveOSUpdateRequest(System.Collections.Generic.List<ReceiveOSUpdateOSRec> OSRec)
    //    {
    //        this.OSRec = OSRec;
    //    }
    //}

    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    //[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    //[System.ServiceModel.MessageContractAttribute(WrapperName = "ReceiveOSUpdateResponse", WrapperNamespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", IsWrapped = true)]
    //public partial class ReceiveOSUpdateResponse
    //{

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 0)]
    //    public string Status;

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 1)]
    //    public string ErrorCode;

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 2)]
    //    public string ErrorMessage;

    //    public ReceiveOSUpdateResponse()
    //    {
    //    }

    //    public ReceiveOSUpdateResponse(string Status, string ErrorCode, string ErrorMessage)
    //    {
    //        this.Status = Status;
    //        this.ErrorCode = ErrorCode;
    //        this.ErrorMessage = ErrorMessage;
    //    }
    //}

    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    //[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    //[System.ServiceModel.MessageContractAttribute(WrapperName = "ReceiveConfirmTransaction", WrapperNamespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", IsWrapped = true)]
    //public partial class ReceiveConfirmTransactionRequest
    //{

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 0)]
    //    public string From;

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 1)]
    //    public string To;

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 2)]
    //    public string OutboundDocumentNo;

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 3)]
    //    public string Status;

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 4)]
    //    public string ErrorCode;

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 5)]
    //    public string ErrorMessage;

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 6)]
    //    public System.DateTime Timestamp;

    //    public ReceiveConfirmTransactionRequest()
    //    {
    //    }

    //    public ReceiveConfirmTransactionRequest(string From, string To, string OutboundDocumentNo, string Status, string ErrorCode, string ErrorMessage, System.DateTime Timestamp)
    //    {
    //        this.From = From;
    //        this.To = To;
    //        this.OutboundDocumentNo = OutboundDocumentNo;
    //        this.Status = Status;
    //        this.ErrorCode = ErrorCode;
    //        this.ErrorMessage = ErrorMessage;
    //        this.Timestamp = Timestamp;
    //    }
    //}

    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    //[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    //[System.ServiceModel.MessageContractAttribute(WrapperName = "ReceiveConfirmTransactionReponse", WrapperNamespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", IsWrapped = true)]
    //public partial class ReceiveConfirmTransactionResponse
    //{

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 0)]
    //    public string Status;

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 1)]
    //    public string ErrorCode;

    //    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://yesser.gov.sa/NCS/CSIAgencyInboundServices/version/0.1", Order = 2)]
    //    public string ErrorMessage;

    //    public ReceiveConfirmTransactionResponse()
    //    {
    //    }

    //    public ReceiveConfirmTransactionResponse(string Status, string ErrorCode, string ErrorMessage)
    //    {
    //        this.Status = Status;
    //        this.ErrorCode = ErrorCode;
    //        this.ErrorMessage = ErrorMessage;
    //    }
    //}


}
