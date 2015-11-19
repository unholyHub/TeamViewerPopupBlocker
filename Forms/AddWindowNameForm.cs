using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using TeamViewerPopupBlocker.Classes;
using TeamViewerPopupBlocker.Properties;
using Settings = TeamViewerPopupBlocker.Classes.Settings;

namespace TeamViewerPopupBlocker.Forms
{
    public partial class AddWindowNameForm : Form
    {
        private string[] ColumnNames { get; } = 
        {
            string.Empty,
            Resources.AddWindowNameForm_ColumnNames_TeamViewer_Window_Name
        };

        private static DataTable DataTable { get; set; }

        private static  AvailableWindowsForm AvailableWindowForm { get; set; }

        private Collection<string> WindowNames { get; set; } = Settings.Instance.WindowNames;

        public AddWindowNameForm()
        {
            InitializeComponent();
            AvailableWindowForm = new AvailableWindowsForm();
        }

        public static void AddRowToDataGridView(string windowName)
        {
            DataTable.Rows.Add(windowName);
        }

        private void AddWindowNameForm_Load(object sender, EventArgs e)
        {
            InitializeDataTable();
        }

        private void InitializeDataTable()
        {
            CreateColumnForDataTable();

            foreach (string windowName in this.WindowNames)
            {
                DataTable.Rows.Add(windowName);
            }

            this.dgvWindowNames.DataSource = null;
            this.dgvWindowNames.DataSource = DataTable;

            DataGridViewColumn dataGridViewColumn = this.dgvWindowNames.Columns[ColumnNames[1]];

            if (dataGridViewColumn != null)
            {
                dataGridViewColumn.Width = 400;
            }
        }

        private void CreateColumnForDataTable()
        {
            DataTable = new DataTable();
            DataTable.Locale = CultureInfo.InvariantCulture;

            using (DataColumn column = new DataColumn())
            {
                column.ColumnName = ColumnNames[1];
                column.DataType = typeof (string);
                
                DataTable.Columns.Add(column);
            }
        }

        private void SaveBtnClick(object sender, EventArgs e)
        {
            try
            {
                SaveToList();
                SetResultLabel(Resources.AddWindowNameForm_SaveBtnClick_OK_, Color.GreenYellow);
                MessageBox.Show(Resources.AddWindowNameForm_SaveBtnClick,
                                Settings.Instance.ProgramName,
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information, 
                                MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                SetResultLabel(Resources.AddWindowNameForm_btnDefault_Click_Error_, Color.Red);
                LogSystem.Instance.AddToLog(ex);
            }
        }

        private void SaveToList()
        {
            StringBuilder testString = new StringBuilder();
            WindowNames.Clear();

            foreach (DataGridViewRow row in this.dgvWindowNames.Rows)
            {
                string cellValue = Convert.ToString(row.Cells[ColumnNames[1]].Value, CultureInfo.InvariantCulture);

                if (string.IsNullOrEmpty(cellValue))
                {
                    continue;
                }

                testString.AppendLine(cellValue);
                WindowNames.Add(cellValue);
            }

            Settings.Instance.WindowNames = WindowNames;
            Settings.Instance.Save();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            ReturnListToDefault();

            try
            {
                ClearDataGridView();
                InitializeDataTable();
                SetResultLabel(Resources.AddWindowNameForm_btnDefault_Click_OK, Color.GreenYellow);
            }
            catch (Exception ex)
            {
                SetResultLabel(Resources.AddWindowNameForm_btnDefault_Click_Error_, Color.Red);
                LogSystem.Instance.AddToLog(ex);
            }
        }

        private void ReturnListToDefault()
        {
            Collection<string> defaultWindowsNames = new Collection<string>()
            {
                Resources.TeamViewer_WindowName_Commercial_use,
                Resources.TeamViewer_WindowName_Sponssored_Session,
                Resources.TeamViewer_WindowName_Commercial_use_suspected,
                Resources.TeamViewer_WindowName_Unable_to_connect
            };

            this.WindowNames = defaultWindowsNames;
            ClearDataGridView();
            InitializeDataTable();
        }

        private void ClearDataGridView()
        {
            this.dgvWindowNames.DataSource = null;
            this.dgvWindowNames.Rows.Clear();
            DataTable = new DataTable();
            DataTable.Locale = CultureInfo.InvariantCulture;
        }

        private void SetResultLabel(string text, Color color)
        {
            this.lblResult.Text = text;
            this.lblResult.BackColor = color;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.Close();
            bool res = base.ProcessCmdKey(ref msg, keyData);
            return res;
        }

        private void InfoBtnClick(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.AddWindowNam_InfoBtnClick,
                            Settings.Instance.ProgramName,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
        }

        private void dgvWindowNames_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                {
                    var hti = this.dgvWindowNames.HitTest(e.X, e.Y);
                    this.dgvWindowNames.ClearSelection();
                    this.dgvWindowNames.Rows[hti.RowIndex].Selected = true;

                    cmsRowMenu.Show(dgvWindowNames, e.X, e.Y);
                    break;
                }

                default:
                {
                    break;
                }
            }
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            DeleteRow();
        }

        private void DeleteRow()
        {
            foreach (DataGridViewRow item in this.dgvWindowNames.SelectedRows)
            {
                if (!item.IsNewRow)
                {
                    this.dgvWindowNames.Rows.Remove(item);
                }
            }
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            if (dgvWindowNames.SelectedCells.Count <= 0)
            {
                return;
            }

            int selectedRowIndex = dgvWindowNames.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dgvWindowNames.Rows[selectedRowIndex];

            string value = Convert.ToString(selectedRow.Cells[0].Value, CultureInfo.InvariantCulture);

            Clipboard.SetText(value);
        }

        private void btnViewWindows_Click(object sender, EventArgs e)
        {
            if (AvailableWindowForm.Visible)
            {
                AvailableWindowForm.Focus();
                return;
            }

            AvailableWindowForm.Show();
            AvailableWindowForm.Location = new Point(this.Left + this.Width, this.Top);

        }

        private void dgvWindowNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
            {
                return;
            }

            DeleteRow();
        }

        private void AddWindowNameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AvailableWindowForm.Visible)
            {
                AvailableWindowForm.Close();
            }

            this.Hide();
            this.Parent = null;
            e.Cancel = true;
        }
    }
}
