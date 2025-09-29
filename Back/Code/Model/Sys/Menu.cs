using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class Menu
    {

        public string MenuId { get; set; }

        public string MenuName { get; set; }

        public string ParentId { get; set; }

        public string ParentName { get; set; }

        public string ParentType { get; set; }

        public int Rank { get; set; }

        public string Icon { get; set; }

        public string MenuNameEn { get; set; }

        public string RoutePath { get; set; }

        public string ComponentPath { get; set; }

        public string CtrlName { get; set; }

        public string ActionName { get; set; }

        public string Remark { get; set; }

        public bool IsValid { get; set; }

        public bool IsVisible { get; set; }

        public string MenuType { get; set; }

        public bool HideMenu { get; set; }

        public string MenuDisplay { get; set; }

        public string MenuLayout { get; set; }

        public string Url { get; set; }
    }
}
