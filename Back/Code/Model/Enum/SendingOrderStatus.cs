using System.ComponentModel;

namespace Models.Model.Enum
{
    public enum SendingOrderStatus
    {
        [Description("待发货")]
        WaitingShipment,

        [Description("已发货")]
        Shipment,

        [Description("取消发货")]
        CancelShipment,

        [Description("等通知发货")]
        WaitingNotification,
    }
}
