using SqlSugar;

namespace DbRepository.Repository.DbModels
{
    [SugarTable("SysMenus", "系统菜单表")]
   public class SysMenus
    {
        [SugarColumn( Length = 50, IsPrimaryKey = true, ColumnDescription = "菜单ID")]
        public string MenuId { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "父级菜单ID")]
        public string ParentId { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "菜单名称")]
        public string MenuName { get; set; }

        [SugarColumn( ColumnDescription = "界面展示排序")]
        public int Rank { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "控制器名")]
        public string CtrlName { get; set; }

        [SugarColumn( Length = 50, ColumnDescription = "Action方法名")]
        public string ActionName { get; set; }

        [SugarColumn( Length = 100, IsNullable = true, ColumnDescription = "菜单图标")]
        public string Icon { get; set; }

        [SugarColumn( Length = 20, ColumnDescription = "菜单类型")]
        public string MenuType { get; set; }

        [SugarColumn( Length = 200, IsNullable = true, ColumnDescription = "备注")]
        public string Remark { get; set; }

        [SugarColumn( Length = 50, IsNullable = true, ColumnDescription = "菜单英文名")]
        public string MenuNameEn { get; set; }

        [SugarColumn( Length = 100, IsNullable = true, ColumnDescription = "前端路由")]
        public string RoutePath { get; set; }

        [SugarColumn( Length = 100, IsNullable = true, ColumnDescription = "前端组件")]
        public string ComponentPath { get; set; }

        [SugarColumn( ColumnDescription = "是否隐藏菜单")]
        public bool HideMenu { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "是否为移动端主菜单")]
        public string MenuLayout { get; set; }

        [SugarColumn(Length = 20, IsNullable = true, ColumnDescription = "展示端")]
        public string MenuDisplay { get; set; }

        [SugarColumn( ColumnDescription = "是否有效")]
        public bool IsValid { get; set; }

        [SugarColumn(ColumnDescription = "是否可见")]
        public bool IsVisible { get; set; }

        [SugarColumn(Length =500,IsNullable =true, ColumnDescription ="外部链接")]
        public  string Url { get; set; }
    }
}
