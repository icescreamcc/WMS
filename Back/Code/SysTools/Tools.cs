using External.Common;
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
    public partial class Tools : Form
    {
        public Tools()
        {
            InitializeComponent();
        }

        private void Tools_Load(object sender, EventArgs e)
        {
            cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbType.Items.Clear();
            cbType.Items.Add("MD5");
            cbType.Items.Add("SHA256");
            cbType.Items.Add("Des");
            cbType.Items.Add("Base64");
            cbType.SelectedIndex = 0;
            tbKey.Text = EncryptionHelper.EncryptionKey;
            lbMsg.Text = "";
           
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbSourceStr.Text))
            {
                lbMsg.Text = "请输入原字符串";
                return;
            }
            var type = cbType.SelectedItem.ToString();
            var res = "";
            try
            {
                if (type == "MD5")
                {
                    res = EncryptionHelper.MD5Encrypt(tbSourceStr.Text);
                }
                else if (type == "SHA256")
                {
                    res = EncryptionHelper.SHA256Encrypt(tbSourceStr.Text);
                }
                else if (type == "Des")
                {
                    if (string.IsNullOrEmpty(tbKey.Text))
                    {
                        lbMsg.Text = "请输入秘钥";
                        return;
                    }
                    res = EncryptionHelper.DesEncrypt(tbSourceStr.Text, tbKey.Text);
                }
                else
                {
                    res = EncryptionHelper.Base64Encrypt(tbSourceStr.Text);
                }
            }
            catch(Exception ex)
            {
                lbMsg.Text = ex.Message;
            }
            tbResult.Text = res;
        }

        private void tbDescrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbSourceStr.Text))
            {
                lbMsg.Text = "请输入原字符串";
                return;
            }
            var type = cbType.SelectedItem.ToString();
            var res = "";
            try
            {
                if (type == "MD5")
                {
                    lbMsg.Text = "暂不支持MD5解密";
                    return;
                }
                else if (type == "SHA256")
                {
                    lbMsg.Text = "暂不支持SHA256解密";
                    return;
                }
                else if (type == "Des")
                {
                    if (string.IsNullOrEmpty(tbKey.Text))
                    {
                        lbMsg.Text = "请输入秘钥";
                        return;
                    }
                    res = EncryptionHelper.DesDecrypt(tbSourceStr.Text, tbKey.Text);
                }
                else
                {
                    res = EncryptionHelper.Base64Decrypt(tbSourceStr.Text);
                }
            }
            catch (Exception ex)
            {
                lbMsg.Text = ex.Message;
            }
            tbResult.Text = res;
        }
    }
}
