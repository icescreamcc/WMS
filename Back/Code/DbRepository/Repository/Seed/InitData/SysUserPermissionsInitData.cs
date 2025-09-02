using DbRepository.Repository.DbModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository.Repository.Seed.InitData
{
    internal class SysUserPermissionsInitData : IDbInitData
    {
        public void CreateTableInitData(SqlSugarClient db)
        {
            var userPermession = new List<SysUserPermissions>() {
                //系统信息管理
                new SysUserPermissions() { MenuId = "SYSINFO", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "USERMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "USERREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "USERADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "USERUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "USERDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "USEREXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "USERINITPASSWORD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "USERUPDATESTATUS", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "USERUPDATEROLE", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "ORGANIZATIONMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "ORGANIZATIONREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "ROLEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "ROLEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "ROLEDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "DEPTADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "DEPTUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "DEPTDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "COMPANYMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "COMPANYREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "COMPANYUPDATE", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "BUSINESSDICMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "BUSINESSDICREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "BUSINESSDICADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "BUSINESSDICUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "BUSINESSDICDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "COMPANYANNOUNCEMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "COMPANYANNOUNCEREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "COMPANYANNOUNCEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "COMPANYANNOUNCEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "COMPANYANNOUNCEDEL", RoleId = "ADMIN" },

                //系统设置
                new SysUserPermissions() { MenuId = "SYSSETUP", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MENUSMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MENUSREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MENUSADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MENUSUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MENUSDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "PROVIDERMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PROVIDERREAD", RoleId = "ADMIN" },
                 new SysUserPermissions() { MenuId = "PROVIDERADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PROVIDERUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PROVIDERDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "PERMISSIONSETUP", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PERMISSIONREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PERMISSIONUPDATE", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "SYSARGSSETUP", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SYSARGSREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SYSARGSUPDATE", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "IMPORTMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "IMPORTREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "IMPORTUPDATE", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "APPROVALSETUP", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "APPROVALREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "APPROVALUPDATE", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "LOGINFO", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "LOGREAD", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "FIELDSPERMISSIONSETUP", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FIELDSPERMISSIONREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FIELDSPERMISSIONUPDATE", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "FIELDSMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FIELDSREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FIELDSUPDATE", RoleId = "ADMIN" },

              

                //仓库管理
              
                new SysUserPermissions() { MenuId = "INVENTORYMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WAREHOUSEMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WAREHOUSEREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WAREHOUSEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WAREHOUSEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WAREHOUSEDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "BINMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "BINREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "BINADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "BINUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "BINDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SHELFADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SHELFUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SHELFDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "WORKBINMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKBINREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKBINUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKBINSPECADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKBINSPECUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKBINSPECDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "WAREHOUSELAYOUT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "LAYOUTREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "LAYOUTUNLOCK", RoleId = "ADMIN" },  

                new SysUserPermissions() { MenuId = "SCANINSTORAGE", RoleId = "ADMIN" }, 
                new SysUserPermissions() { MenuId = "SCANINSTORAGESUBMITSCAN", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SCANINSTORAGESUBMITCODE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SCANINSTORAGESELECTGOODS", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SCANINSTORAGEGOODSQUERY", RoleId = "ADMIN" },


                new SysUserPermissions() { MenuId = "STOCKMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "INSTORAGEMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "INSTORAGEORDERREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "INSTORAGEORDERADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "INSTORAGEORDERUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "INSTORAGEORDERDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "INSTORAGEORDEREXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "INSTORAGEORDERCONFIRM", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "OUTSTORAGEMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "OUTSTORAGEORDERREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "OUTSTORAGEORDERADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "OUTSTORAGEORDERUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "OUTSTORAGEORDERDEL", RoleId = "ADMIN" }, 
                new SysUserPermissions() { MenuId = "OUTSTORAGEORDEREXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "OUTSTORAGEORDERCONFIRM", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "STORAGEMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "STORAGEREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "STORAGEDETAILREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "STORAGEFLOWREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "STORAGEALLOCATION", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "STORAGEEXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "STORAGEDETAILEXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "STORAGEFLOWEXPORT", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "TAKESTOCKMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TAKESTOCKREAD", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "REQUISITIONMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REQUISITIONEREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REQUISITIONADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REQUISITIONUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REQUISITIONDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REQUISITIONRECEIVE", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "REQUISITIONCONSUMABLESMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REQUISITIONECONSUMABLESREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REQUISITIONCONSUMABLESADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REQUISITIONCONSUMABLESUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REQUISITIONCONSUMABLESDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REQUISITIONCONSUMABLESRECEIVE", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILEMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILEREAD", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILEBYGOODS", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILEBYGOODSREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILELOCKGOODS", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILEBYBIN", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILEBYBINREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILELOCKBIN", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILEINVENTORYBYGOODS", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILEINVENTORYBYGOODSREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TAKESTOCKBYGOODSMOBILEADD", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILEINVENTORYBYBIN", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TAKESTOCKMOBILEINVENTORYBYBINREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TAKESTOCKBYBINMOBILEADD", RoleId = "ADMIN" },

                 new SysUserPermissions() { MenuId = "SAFETYINVENTORYMGR", RoleId = "ADMIN" },
                 new SysUserPermissions() { MenuId = "SAFETYINVENTORYREAD", RoleId = "ADMIN" },
                 new SysUserPermissions() { MenuId = "SAFETYINVENTORYUPDATE", RoleId = "ADMIN" },
                 new SysUserPermissions() { MenuId = "SAFETYINVENTORYEXPORT", RoleId = "ADMIN" },
                 new SysUserPermissions() { MenuId = "SAFETYINVENTORYAPPROVAL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "GOODSMOBILEMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "GOODSMOBILEREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "GOODSMOBILEDETAIL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "GOODSMOBILEDETAILREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "GOODSMOBILEDETAILUPDATE", RoleId = "ADMIN" }, 

                //生产（成品下线）
                new SysUserPermissions() { MenuId = "PRODUCTIONMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PRODORDERMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PRODORDERREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PRODORDERADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PRODORDERUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PRODORDERCARCODECREATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PRODORDERPRINT", RoleId = "ADMIN" },
                  new SysUserPermissions() { MenuId = "PRODORDERCLOSED", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PRODORDERDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "PRODPACKAGEMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PRODPACKAGEREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PRODPACKAGECLEARMATCH", RoleId = "ADMIN" },

                //基础信息管理
                new SysUserPermissions() { MenuId = "GOODSINFOMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SAMPLEPIECEMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SAMPLEPIECEREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SAMPLEPIECEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SAMPLEPIECEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SAMPLEPIECEDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SAMPLEPIECEEXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SAMPLEPIECETYPEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SAMPLEPIECETYPEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SAMPLEPIECETYPEDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "SPAREPARTMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SPAREPARTREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SPAREPARTADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SPAREPARTUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SPAREPARTDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SPAREPARTEXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SPAERPARTTYPEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SPAERPARTTYPEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SPAERPARTTYPEDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "SEPARATORMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SEPARATORREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SEPARATORADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SEPARATORUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SEPARATORDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SEPARATOREXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SEPARATORTYPEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SEPARATORTYPEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SEPARATORTYPEDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "PACKINGMATERIALMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PACKINGMATERIALREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PACKINGMATERIALADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PACKINGMATERIALUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PACKINGMATERIALDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PACKINGMATERIALEXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PACKINGMATERIALTYPEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PACKINGMATERIALTYPEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PACKINGMATERIALTYPEDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "FINISHEDPRODUCTMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FINISHEDPRODUCTREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FINISHEDPRODUCTADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FINISHEDPRODUCTUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FINISHEDPRODUCTDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FINISHEDPRODUCTEXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FINISHEDPRODUCTBOM", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FINISHEDPRODUCTTYPEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FINISHEDPRODUCTTYPEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "FINISHEDPRODUCTTYPEDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "RAWMATERIALMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RAWMATERIALREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RAWMATERIALADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RAWMATERIALUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RAWMATERIALDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RAWMATERIALEXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RAWMATERIALTYPEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RAWMATERIALTYPEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RAWMATERIALTYPEDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "CONSUMABLESMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CONSUMABLESREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CONSUMABLESADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CONSUMABLESUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CONSUMABLESDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CONSUMABLESEXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CONSUMABLESTYPEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CONSUMABLESTYPEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CONSUMABLESTYPEDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "UNITMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "UNITREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "UNITADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "UNITUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "UNITDEL", RoleId = "ADMIN" },

                //标签管理
                new SysUserPermissions() { MenuId = "LABELMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "LABELDESIGNMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "LABELDESIGNREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "LABELDESIGNADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "LABELDESIGNUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "LABELDESIGNUPDATEDEFT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "LABELDESIGNDEL", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "LABELPRINTMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "LABELPRINT", RoleId = "ADMIN" },

                //客户供应商管理
                new SysUserPermissions() { MenuId = "CLIENTSUPPLIERMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CLIENTMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CLIENTREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CLIENTADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CLIENTUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CLIENTDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "CLIENTEXPORT", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "SUPPLIERMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SUPPLIERREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SUPPLIERADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SUPPLIERUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SUPPLIERDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SUPPLIEREXPORT", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "PDAMOBILEMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TRUCKLOADING", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TRUCKQUERY", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "TRUCKSUBMIT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PUTAWAY", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PUTAWAYQUERY", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PUTAWAYSUBMIT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PUTOUT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PUTOUTQUERY", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PUTOUTSUBMIT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "UNSTACK", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "UNSTACKQUERY", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "UNSTACKSUBMIT", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "QUERYORDER", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "QUERYORDERQUERY", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "QUERYORDERUNLOCK", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MESSAGE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MESSAGEREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MYSETTING", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MYSETTINGREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MESSAGESUBSCRIBE", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "RECEIVINGDELIVERYPLAN", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGAPPROVAL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGNOTIFICATION", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGAFFIRM", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGABNORMAL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGURGENCY", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGSAPIMPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGSAPEXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGDELIVERYORDERADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGDELIVERYORDERPRINT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGMATLABELPRINT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGUPLOAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGREADUPLOAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "RECEIVINGDELSENDINGFILES", RoleId = "ADMIN" },
                

                new SysUserPermissions() { MenuId = "SENDINGMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SENDINGREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SENDINGADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SENDINGUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SENDINGDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SENDINGIMPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SENDINGEXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SENDINGUPLOAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SENDINGREADUPLOAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "DELSENDINGFILES", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "SENDINGNOTIFICATION", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "MATERIALREQUIREMENTPLAN", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MATERIALREQUIREMENTPLANREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MATERIALREQUIREMENTPLANADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MATERIALREQUIREMENTPLANUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MATERIALREQUIREMENTPLANDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MATERIALREQUIREMENTPLANEXPORT", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "MATERIALREQUIREMENTPLAN", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "PURCHASEMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PURCHASEREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PURCHASEADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PURCHASEUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PURCHASEDEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PURCHASEAPPROVAL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PURCHASERECEIVEDWH", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PURCHASERECEIVEDWK", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PURCHASEUPDATEFLOWSTATUS", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "PURCHASEEXPORT", RoleId = "ADMIN" }, 

                new SysUserPermissions() { MenuId = "REPORTSMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REPORTSPAREPART", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REPORTSPAREPARTREAD", RoleId = "ADMIN" },

                new SysUserPermissions() { MenuId = "REPORTSAMPLEPIECE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "REPORTSAMPLEPIECEREAD", RoleId = "ADMIN" },
                 
                new SysUserPermissions() { MenuId = "WORKSHOPDEVICE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKSHOPDEVICEINFO", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKSHOPDEVICEINFOREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKSHOPDEVICEINFOADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKSHOPDEVICEINFOUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKSHOPDEVICEINFODEL", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKSHOPDEVICEBINADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKSHOPDEVICEBINUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "WORKSHOPDEVICEBINDEL", RoleId = "ADMIN" },
                 
                new SysUserPermissions() { MenuId = "AGVINFOMGR", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "AGVINFOREAD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "AGVINFOADD", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "AGVINFOUPDATE", RoleId = "ADMIN" },
                new SysUserPermissions() { MenuId = "AGVINFODEL", RoleId = "ADMIN" },
            };
            db.Insertable(userPermession).AddQueue();
        }
    }
}
