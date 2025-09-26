using External.Common;
using External.HIK_RCS;
using External.OpenAI;
using External.Socket.PLC;
using External.Socket.Socket;
using Logic.Authentication;
using Logic.AutomationDevice;
using Logic.BaseInfo;
using Logic.Inventory;
using Logic.Label;
using Logic.LogicBase.CacheService;
using Logic.LogicBase.MQService;
using Logic.LogicCommon;
using Logic.PlanMaterial;
using Logic.ProductOffLine;
using Logic.Purchase;
using Logic.Order;
using Logic.Report;
using Logic.ScheduleJob;
using Logic.ScheduleJob.Job;
using Logic.Sys;
using Microsoft.Extensions.DependencyInjection; 
using Quartz.Impl;
using Quartz.Spi; 
using System.Linq;
using System.Net.Http; 

namespace Logic.LogicDependencyInjection
{
   public class ProjectInjectionService
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(SysMappingProfile)); 
            services.AddAutoMapper(typeof(InvMappingProfile));
            services.AddAutoMapper(typeof(ProdMappingProfile));
            services.AddAutoMapper(typeof(BaseMappingProfile));
            services.AddAutoMapper(typeof(LabelMappingProfile));
            services.AddAutoMapper(typeof(PurchaseMappingProfile));
            services.AddAutoMapper(typeof(PlanMappingProfile));
            services.AddAutoMapper(typeof(AutomationMappingProfile));
            services.AddTransient<HttpClient>();
            services.AddTransient<EmailService>();  
            services.AddTransient<HttpHelperAsync>();
            services.AddSingleton<WebSocketService>();
            services.AddSingleton<RCSService>();

            services.AddTransient<UserDataSync>();
            services.AddTransient<FactoryDataSyncMgr>();
            services.AddSingleton(typeof(StdSchedulerFactory));
            services.AddSingleton<IJobFactory, JobFactory>(); 
            services.AddTransient<UserDataSyncJob>();  
            services.AddTransient<FactoryDataSyncJob>();
            services.AddTransient<FinishedProductOrdersDataSyncJob>();
            services.AddTransient<SafetyInvenstoryWarningJob>();
            services.AddSingleton<JobCreator>();
            services.AddSingleton<JobLauncher>();
            services.AddTransient<ImportIBDDataSyncJob>();
            services.AddTransient<ImportIBDDataSync>();

            services.AddTransient<BusinessLogService>();
            services.AddTransient<SysArgsService>();
            services.AddTransient<SelectOptionsService>();

            services.AddTransient<GoodsMgr>();
            services.AddTransient<SamplePieceMgr>();
            services.AddTransient<SparePartMgr>();
            services.AddTransient<SeparatorMgr>();
            services.AddTransient<PackingMaterialMgr>(); 
            services.AddTransient<RawMaterialMgr>(); 
            services.AddTransient<FinishedProductMgr>();
            services.AddTransient<ConsumablesMgr>();
            services.AddTransient<UnitMgr>();
            services.AddTransient<ClientMgr>();
            services.AddTransient<SupplierMgr>();
             
            services.AddTransient<WindowsUserAuth>();
            services.AddTransient<AccountPasswordAuth>();
            services.AddTransient<ExternalProviderMgr>(); 
            services.AddTransient<WarehouseMgr>();
            services.AddTransient<ShelfBinMgr>();
            services.AddTransient<LayoutMgr>();
            services.AddTransient<InStorageMgr>();
            services.AddTransient<OutStorageMgr>();
            services.AddTransient<StorageMgr>();
            services.AddTransient<WorkbinMgr>();
            services.AddTransient<TakeStockMgr>();
            services.AddTransient<RequisitionMgr>();
            services.AddTransient<InStorageLabelsMgr>();
            services.AddTransient<WarehousePickDashboardMgr>();
            services.AddTransient<SafetyWarningRecordMgr>();

            services.AddTransient<BusinessCacheItem>();
            services.AddTransient<BusinessCacheService>();
            services.AddTransient<UserMgr>();
            services.AddTransient<OrganizationMgr>();
            services.AddTransient<MenusMgr>();
            services.AddTransient<UserPermissionMgr>();
            services.AddTransient<SysArgsMgr>();
            services.AddTransient<BusinessDicMgr>();
            services.AddTransient<MessageCompanyMgr>();
            services.AddTransient<FieldManageMgr>();
            services.AddTransient<FieldPermissionMgr>();
            services.AddTransient<SysLogMgr>();
            services.AddTransient<ApprovalMgr>(); 
            services.AddTransient<BusinessMQService>();
            services.AddTransient<MessageService>();

            services.AddTransient<PDAPermissionMgr>();
            services.AddTransient<PDACarLoadMgr>();
            services.AddTransient<PDAPutawayMgr>();
            services.AddTransient<PDAPutoutMgr>();
            services.AddTransient<PDAMessageMgr>();
            services.AddTransient<PDAOrderQueryMgr>();
            services.AddTransient<PDAUnstackMgr>();
            services.AddTransient<ProductionOrderMgr>();
            services.AddTransient<BufferWarehouseDashboardMgr>();
            services.AddTransient<ProductPackageMgr>();

            services.AddTransient<ReceivingOrderMgr>();
            services.AddTransient<SendingOrderMgr>();
            services.AddTransient<PurchaseOrderMgr>();

            services.AddTransient<OrderPlanMgr>();
            services.AddTransient<ShipmentMgr>();

            services.AddTransient<MaterialLabelDesignMgr>();
            services.AddTransient<LabelPrintMgr>();

            services.AddTransient<MaterialRequirementPlanMgr>();
            services.AddTransient<ReportSparepartMgr>();

            services.AddTransient<WorkShopDeviceInfoMgr>(); 
            services.AddTransient<AGVSettingMgr>();
            services.AddTransient<AutoTransportHandler>(); 
            services.AddTransient<AutoTransportInfrastructureMgr>();
        }
    }
}
