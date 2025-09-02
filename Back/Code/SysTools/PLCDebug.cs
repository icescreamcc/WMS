using External.Socket.PLC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysTools
{
    public partial class PLCDebug : Form
    {
        private SiemensS7Client _siemensS7Client;

        public PLCDebug()
        {
            InitializeComponent();
        }
    }
}
