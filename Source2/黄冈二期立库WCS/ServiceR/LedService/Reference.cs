﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCS.LedService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.primeton.com/LEDMsgQueryService", ConfigurationName="LedService.LEDMsgQueryService")]
    public interface LEDMsgQueryService {
        
        // CODEGEN: 操作 queryHBZNCStationTasks 以后生成的消息协定不是 RPC，也不是换行文档。
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        WCS.LedService.queryHBZNCStationTasksResponse1 queryHBZNCStationTasks(WCS.LedService.queryHBZNCStationTasks1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<WCS.LedService.queryHBZNCStationTasksResponse1> queryHBZNCStationTasksAsync(WCS.LedService.queryHBZNCStationTasks1 request);
        
        // CODEGEN: 操作 queryOutsideHBZNCStationTasks 以后生成的消息协定不是 RPC，也不是换行文档。
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        WCS.LedService.queryOutsideHBZNCStationTasksResponse1 queryOutsideHBZNCStationTasks(WCS.LedService.queryOutsideHBZNCStationTasks1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<WCS.LedService.queryOutsideHBZNCStationTasksResponse1> queryOutsideHBZNCStationTasksAsync(WCS.LedService.queryOutsideHBZNCStationTasks1 request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.primeton.com/LEDMsgQueryService")]
    public partial class queryHBZNCStationTasks : object, System.ComponentModel.INotifyPropertyChanged {
        
        private StageInfo in0Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public StageInfo in0 {
            get {
                return this.in0Field;
            }
            set {
                this.in0Field = value;
                this.RaisePropertyChanged("in0");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ledMsg.autowarehouse.wms.sfy.com")]
    public partial class StageInfo : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string invcodeField;
        
        private string stationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public string invcode {
            get {
                return this.invcodeField;
            }
            set {
                this.invcodeField = value;
                this.RaisePropertyChanged("invcode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string station {
            get {
                return this.stationField;
            }
            set {
                this.stationField = value;
                this.RaisePropertyChanged("station");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ledMsg.autowarehouse.wms.sfy.com")]
    public partial class OutsideTaskInfoRows : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string act_plate_numberField;
        
        private string areaField;
        
        private string noptstationField;
        
        private string responserField;
        
        private string telephoneField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public string act_plate_number {
            get {
                return this.act_plate_numberField;
            }
            set {
                this.act_plate_numberField = value;
                this.RaisePropertyChanged("act_plate_number");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string area {
            get {
                return this.areaField;
            }
            set {
                this.areaField = value;
                this.RaisePropertyChanged("area");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=2)]
        public string noptstation {
            get {
                return this.noptstationField;
            }
            set {
                this.noptstationField = value;
                this.RaisePropertyChanged("noptstation");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public string responser {
            get {
                return this.responserField;
            }
            set {
                this.responserField = value;
                this.RaisePropertyChanged("responser");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=4)]
        public string telephone {
            get {
                return this.telephoneField;
            }
            set {
                this.telephoneField = value;
                this.RaisePropertyChanged("telephone");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ledMsg.autowarehouse.wms.sfy.com")]
    public partial class OutsideTaskInfo : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string countField;
        
        private OutsideTaskInfoRows[] rows_2Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public string count {
            get {
                return this.countField;
            }
            set {
                this.countField = value;
                this.RaisePropertyChanged("count");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true, Order=1)]
        public OutsideTaskInfoRows[] rows_2 {
            get {
                return this.rows_2Field;
            }
            set {
                this.rows_2Field = value;
                this.RaisePropertyChanged("rows_2");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ledMsg.autowarehouse.wms.sfy.com")]
    public partial class TaskInfoRows : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string alltasksField;
        
        private string areaField;
        
        private string customerField;
        
        private string customer_numberField;
        
        private string delivery_numberField;
        
        private string downtasksField;
        
        private string notsubmitField;
        
        private string pendingtasksField;
        
        private string platnumberField;
        
        private string responserField;
        
        private string stageField;
        
        private string totalpacksField;
        
        private string warehousekeeperField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public string alltasks {
            get {
                return this.alltasksField;
            }
            set {
                this.alltasksField = value;
                this.RaisePropertyChanged("alltasks");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string area {
            get {
                return this.areaField;
            }
            set {
                this.areaField = value;
                this.RaisePropertyChanged("area");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=2)]
        public string customer {
            get {
                return this.customerField;
            }
            set {
                this.customerField = value;
                this.RaisePropertyChanged("customer");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public string customer_number {
            get {
                return this.customer_numberField;
            }
            set {
                this.customer_numberField = value;
                this.RaisePropertyChanged("customer_number");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=4)]
        public string delivery_number {
            get {
                return this.delivery_numberField;
            }
            set {
                this.delivery_numberField = value;
                this.RaisePropertyChanged("delivery_number");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=5)]
        public string downtasks {
            get {
                return this.downtasksField;
            }
            set {
                this.downtasksField = value;
                this.RaisePropertyChanged("downtasks");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=6)]
        public string notsubmit {
            get {
                return this.notsubmitField;
            }
            set {
                this.notsubmitField = value;
                this.RaisePropertyChanged("notsubmit");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=7)]
        public string pendingtasks {
            get {
                return this.pendingtasksField;
            }
            set {
                this.pendingtasksField = value;
                this.RaisePropertyChanged("pendingtasks");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=8)]
        public string platnumber {
            get {
                return this.platnumberField;
            }
            set {
                this.platnumberField = value;
                this.RaisePropertyChanged("platnumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=9)]
        public string responser {
            get {
                return this.responserField;
            }
            set {
                this.responserField = value;
                this.RaisePropertyChanged("responser");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=10)]
        public string stage {
            get {
                return this.stageField;
            }
            set {
                this.stageField = value;
                this.RaisePropertyChanged("stage");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=11)]
        public string totalpacks {
            get {
                return this.totalpacksField;
            }
            set {
                this.totalpacksField = value;
                this.RaisePropertyChanged("totalpacks");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=12)]
        public string warehousekeeper {
            get {
                return this.warehousekeeperField;
            }
            set {
                this.warehousekeeperField = value;
                this.RaisePropertyChanged("warehousekeeper");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ledMsg.autowarehouse.wms.sfy.com")]
    public partial class TaskInfo : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string countField;
        
        private TaskInfoRows[] rowsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public string count {
            get {
                return this.countField;
            }
            set {
                this.countField = value;
                this.RaisePropertyChanged("count");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true, Order=1)]
        public TaskInfoRows[] rows {
            get {
                return this.rowsField;
            }
            set {
                this.rowsField = value;
                this.RaisePropertyChanged("rows");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.primeton.com/LEDMsgQueryService")]
    public partial class queryHBZNCStationTasksResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private TaskInfo out1Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public TaskInfo out1 {
            get {
                return this.out1Field;
            }
            set {
                this.out1Field = value;
                this.RaisePropertyChanged("out1");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class queryHBZNCStationTasks1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.primeton.com/LEDMsgQueryService", Order=0)]
        public WCS.LedService.queryHBZNCStationTasks queryHBZNCStationTasks;
        
        public queryHBZNCStationTasks1() {
        }
        
        public queryHBZNCStationTasks1(WCS.LedService.queryHBZNCStationTasks queryHBZNCStationTasks) {
            this.queryHBZNCStationTasks = queryHBZNCStationTasks;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class queryHBZNCStationTasksResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.primeton.com/LEDMsgQueryService", Order=0)]
        public WCS.LedService.queryHBZNCStationTasksResponse queryHBZNCStationTasksResponse;
        
        public queryHBZNCStationTasksResponse1() {
        }
        
        public queryHBZNCStationTasksResponse1(WCS.LedService.queryHBZNCStationTasksResponse queryHBZNCStationTasksResponse) {
            this.queryHBZNCStationTasksResponse = queryHBZNCStationTasksResponse;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.primeton.com/LEDMsgQueryService")]
    public partial class queryOutsideHBZNCStationTasks : object, System.ComponentModel.INotifyPropertyChanged {
        
        private StageInfo in0Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public StageInfo in0 {
            get {
                return this.in0Field;
            }
            set {
                this.in0Field = value;
                this.RaisePropertyChanged("in0");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.primeton.com/LEDMsgQueryService")]
    public partial class queryOutsideHBZNCStationTasksResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private OutsideTaskInfo out1Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public OutsideTaskInfo out1 {
            get {
                return this.out1Field;
            }
            set {
                this.out1Field = value;
                this.RaisePropertyChanged("out1");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class queryOutsideHBZNCStationTasks1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.primeton.com/LEDMsgQueryService", Order=0)]
        public WCS.LedService.queryOutsideHBZNCStationTasks queryOutsideHBZNCStationTasks;
        
        public queryOutsideHBZNCStationTasks1() {
        }
        
        public queryOutsideHBZNCStationTasks1(WCS.LedService.queryOutsideHBZNCStationTasks queryOutsideHBZNCStationTasks) {
            this.queryOutsideHBZNCStationTasks = queryOutsideHBZNCStationTasks;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class queryOutsideHBZNCStationTasksResponse1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.primeton.com/LEDMsgQueryService", Order=0)]
        public WCS.LedService.queryOutsideHBZNCStationTasksResponse queryOutsideHBZNCStationTasksResponse;
        
        public queryOutsideHBZNCStationTasksResponse1() {
        }
        
        public queryOutsideHBZNCStationTasksResponse1(WCS.LedService.queryOutsideHBZNCStationTasksResponse queryOutsideHBZNCStationTasksResponse) {
            this.queryOutsideHBZNCStationTasksResponse = queryOutsideHBZNCStationTasksResponse;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface LEDMsgQueryServiceChannel : WCS.LedService.LEDMsgQueryService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LEDMsgQueryServiceClient : System.ServiceModel.ClientBase<WCS.LedService.LEDMsgQueryService>, WCS.LedService.LEDMsgQueryService {
        
        public LEDMsgQueryServiceClient() {
        }
        
        public LEDMsgQueryServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LEDMsgQueryServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LEDMsgQueryServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LEDMsgQueryServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WCS.LedService.queryHBZNCStationTasksResponse1 WCS.LedService.LEDMsgQueryService.queryHBZNCStationTasks(WCS.LedService.queryHBZNCStationTasks1 request) {
            return base.Channel.queryHBZNCStationTasks(request);
        }
        
        public WCS.LedService.queryHBZNCStationTasksResponse queryHBZNCStationTasks(WCS.LedService.queryHBZNCStationTasks queryHBZNCStationTasks1) {
            WCS.LedService.queryHBZNCStationTasks1 inValue = new WCS.LedService.queryHBZNCStationTasks1();
            inValue.queryHBZNCStationTasks = queryHBZNCStationTasks1;
            WCS.LedService.queryHBZNCStationTasksResponse1 retVal = ((WCS.LedService.LEDMsgQueryService)(this)).queryHBZNCStationTasks(inValue);
            return retVal.queryHBZNCStationTasksResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WCS.LedService.queryHBZNCStationTasksResponse1> WCS.LedService.LEDMsgQueryService.queryHBZNCStationTasksAsync(WCS.LedService.queryHBZNCStationTasks1 request) {
            return base.Channel.queryHBZNCStationTasksAsync(request);
        }
        
        public System.Threading.Tasks.Task<WCS.LedService.queryHBZNCStationTasksResponse1> queryHBZNCStationTasksAsync(WCS.LedService.queryHBZNCStationTasks queryHBZNCStationTasks) {
            WCS.LedService.queryHBZNCStationTasks1 inValue = new WCS.LedService.queryHBZNCStationTasks1();
            inValue.queryHBZNCStationTasks = queryHBZNCStationTasks;
            return ((WCS.LedService.LEDMsgQueryService)(this)).queryHBZNCStationTasksAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WCS.LedService.queryOutsideHBZNCStationTasksResponse1 WCS.LedService.LEDMsgQueryService.queryOutsideHBZNCStationTasks(WCS.LedService.queryOutsideHBZNCStationTasks1 request) {
            return base.Channel.queryOutsideHBZNCStationTasks(request);
        }
        
        public WCS.LedService.queryOutsideHBZNCStationTasksResponse queryOutsideHBZNCStationTasks(WCS.LedService.queryOutsideHBZNCStationTasks queryOutsideHBZNCStationTasks1) {
            WCS.LedService.queryOutsideHBZNCStationTasks1 inValue = new WCS.LedService.queryOutsideHBZNCStationTasks1();
            inValue.queryOutsideHBZNCStationTasks = queryOutsideHBZNCStationTasks1;
            WCS.LedService.queryOutsideHBZNCStationTasksResponse1 retVal = ((WCS.LedService.LEDMsgQueryService)(this)).queryOutsideHBZNCStationTasks(inValue);
            return retVal.queryOutsideHBZNCStationTasksResponse;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WCS.LedService.queryOutsideHBZNCStationTasksResponse1> WCS.LedService.LEDMsgQueryService.queryOutsideHBZNCStationTasksAsync(WCS.LedService.queryOutsideHBZNCStationTasks1 request) {
            return base.Channel.queryOutsideHBZNCStationTasksAsync(request);
        }
        
        public System.Threading.Tasks.Task<WCS.LedService.queryOutsideHBZNCStationTasksResponse1> queryOutsideHBZNCStationTasksAsync(WCS.LedService.queryOutsideHBZNCStationTasks queryOutsideHBZNCStationTasks) {
            WCS.LedService.queryOutsideHBZNCStationTasks1 inValue = new WCS.LedService.queryOutsideHBZNCStationTasks1();
            inValue.queryOutsideHBZNCStationTasks = queryOutsideHBZNCStationTasks;
            return ((WCS.LedService.LEDMsgQueryService)(this)).queryOutsideHBZNCStationTasksAsync(inValue);
        }
    }
}
