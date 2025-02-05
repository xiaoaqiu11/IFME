﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IFME
{
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {

        }

        private void frmOptions_Shown(object sender, EventArgs e)
        {
            txtTempPath.Text = Properties.Settings.Default.FolderTemporary;
            txtPrefix.Text = Properties.Settings.Default.PrefixText;
            txtPostfix.Text = Properties.Settings.Default.PostfixText;

            switch (Properties.Settings.Default.PrefixMode)
            {
                case 0:
                    rdoPrefixNone.Checked = true;
                    break;
                case 1:
                    rdoPrefixDateTime.Checked = true;
                    break;
                default:
                    rdoPrefixCustom.Checked = true;
                    break;
            }

            switch (Properties.Settings.Default.PostfixMode)
            {
                case 0:
                    rdoPostfixNone.Checked = true;
                    break;
                case 1:
                    rdoPostfixDateTime.Checked = true;
                    break;
                default:
                    rdoPostfixCustom.Checked = true;
                    break;
            }

            var disabled = Properties.Settings.Default.PluginsDisabled.Split(',').Select(Guid.Parse).ToList();
            foreach (var item in Plugins.Items.Lists)
            {
                lstPlugins.Items.Add(new ListViewItem(new[]
                {
                    item.Value.Name,
                    item.Value.X64 ? "64-bit" : "32-bit",
                    item.Value.Version,
                    item.Value.Author.Developer
                })
                {
                    Tag = item.Key,
                    Checked = !disabled.Contains(item.Key)
                });
            }

            chkSkipTest.Checked = Properties.Settings.Default.TestEncoder;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FolderTemporary = txtTempPath.Text;
            Properties.Settings.Default.PrefixText = txtPrefix.Text;
            Properties.Settings.Default.PostfixText = txtPostfix.Text;

            if (rdoPrefixNone.Checked)
                Properties.Settings.Default.PrefixMode = 0;

            if (rdoPrefixDateTime.Checked)
                Properties.Settings.Default.PrefixMode = 1;

            if (rdoPrefixCustom.Checked)
                Properties.Settings.Default.PrefixMode = 2;

            if (rdoPostfixNone.Checked)
                Properties.Settings.Default.PostfixMode = 0;

            if (rdoPostfixDateTime.Checked)
                Properties.Settings.Default.PostfixMode = 1;

            if (rdoPostfixCustom.Checked)
                Properties.Settings.Default.PostfixMode = 2;

            var disabled = new List<Guid>();
            foreach (ListViewItem item in lstPlugins.Items)
            {
                if (!item.Checked)
                    disabled.Add((Guid)item.Tag);
            }
            Properties.Settings.Default.PluginsDisabled = string.Join(",", disabled);

            if (string.IsNullOrEmpty(Properties.Settings.Default.PluginsDisabled))
                Properties.Settings.Default.PluginsDisabled = "00000000-0000-0000-0000-000000000000,aaaaaaaa-0000-0000-0000-000000000000,ffffffff-ffff-ffff-ffff-ffffffffffff";

            Properties.Settings.Default.TestEncoder = chkSkipTest.Checked;

            Properties.Settings.Default.Save();
            
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTempBrowse_Click(object sender, EventArgs e)
        {
            var fbd = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = false,
                Title = "Select desire temporary folder (must be empty!)",
                FileName = "TEMP",
                InitialDirectory = txtTempPath.Text,
                AutoUpgradeEnabled = true
            };

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                var fPath = Path.GetDirectoryName(fbd.FileName);
                txtTempPath.Text = fPath;
            }
        }

        private void txtPrefix_TextChanged(object sender, EventArgs e)
        {
            if ((sender as TextBox).Focused)
            {
                if (string.IsNullOrEmpty((sender as TextBox).Text))
                {
                    rdoPrefixNone.Checked = true;
                }
                else
                {
                    rdoPrefixCustom.Checked = true;
                }
            }
        }

        private void txtPostfix_TextChanged(object sender, EventArgs e)
        {
            if ((sender as TextBox).Focused)
            {
                if (string.IsNullOrEmpty((sender as TextBox).Text))
                {
                    rdoPostfixNone.Checked = true;
                }
                else
                {
                    rdoPostfixCustom.Checked = true;
                }
            }
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstPlugins.Items)
            {
                item.Checked = true;
            }
        }

        private void btnOnly265_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstPlugins.Items)
            {
                if (((Guid)item.Tag).Equals(new Guid("deadbeef-0265-0265-0265-026502650265")))
                    item.Checked = true;
                else
                    item.Checked = false;
            }
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstPlugins.Items)
            {
                item.Checked = false;
            }
        }

        private void chkSkipTest_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender as CheckBox).Checked)
            {
                var msg = MessageBox.Show("Disable encoder test could lead to broken results, glitch, instability, incompatible host & CPU\n\nUSE AT YOUR OWN RISK, NO SUPPORT AFTER THIS!", "You are about to disable encoder test!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (msg == DialogResult.No)
                {
                    chkSkipTest.Checked = true;
                }
                else
                {
                    var final = MessageBox.Show("ARE YOU SURE?", "FINAL WARNING!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                    if (final == DialogResult.No)
                    {
                        chkSkipTest.Checked = true;
                    }
                }
            }
        }
    }
}
