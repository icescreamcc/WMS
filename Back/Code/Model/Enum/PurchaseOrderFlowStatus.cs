using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Enum
{
    public enum PurchaseOrderFlowStatus
    {
        [Description("申请SAP号")]
        ApplySAPNumber,

        [Description("申请SAP号，待回复")]
        ApplySAPNumber_WaitingReply,

        [Description("维护SAP")]
        EditSAP,

        [Description("已维护SAP，待采购询价")]
        EditedSAP_WaitingInquiry,

        [Description("已发询价，待采购反馈")]
        SendInquiry_WaitingFeedback,

        [Description("采购已反馈，可下单")]
        Feedbacked_AllowPlaceOrder,

        [Description("待登记信息")]
        WaitingInfoRegister,

        [Description("待RUN PO")]
        WaitingRunPO,
         
        [Description("待收货")]
        WaitingReceiving,

        [Description("已入库")]
        InStoraged,

        [Description("其他异常情况")]
        Abnormal_Other,

        [Description("取消采买异常")]
        Abnormal_ProcureCancel,

        [Description("型号异常")]
        Abnormal_Model,

        [Description("SAP号申请异常")]
        Abnormal_ApplySAPNumber,

        [Description("SAP维护异常")]
        Abnormal_EditSAP,

        [Description("供应商异常")]
        Abnormal_Supplier,

        [Description("短期重复采买异常")]
        Abnormal_ProcureRepet,

        [Description("采购异常")]
        Abnormal_Procure,

        [Description("完结")]
        Finished 
    }
}
