using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using TeamViewerPopupBlocker.Classes;

namespace TeamViewerPopupBlocker.Forms
{
    public partial class AvailableWindowsForm : Form
    {
        private static DataTable DataTable { get; set; }

        public AvailableWindowsForm()
        {
            InitializeComponent();
        }

        private void AvaliableWindowsForm_Load(object sender, System.EventArgs e)
        {
            InitDataTableColumns();
            InitDataTableRows();
            InitDataGridView();
        }

        private void InitDataGridView()
        {
            this.dgvAvaliableWindows.DataSource = null;
            this.dgvAvaliableWindows.DataSource = DataTable;
            
            this.dgvAvaliableWindows.Columns[0].Width = 45;
            this.dgvAvaliableWindows.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
        }

        private static void InitDataTableColumns()
        {
            DataColumn windoNameColumn, iconColumn;
            DataTable = null;

            using (DataTable = new DataTable())
            using (iconColumn = new DataColumn())
            using (windoNameColumn = new DataColumn())
            {
                DataTable.Locale = CultureInfo.InvariantCulture;

                iconColumn.ColumnName = "Icon";
                iconColumn.DataType = typeof(Bitmap);
                DataTable.Columns.Add(iconColumn);

                windoNameColumn.ColumnName = "Window WindowName";
                windoNameColumn.DataType = typeof (string);
                DataTable.Columns.Add(windoNameColumn);
            }
        }

        private static void InitDataTableRows()
        {
            DataTable.Rows.Clear();
            IEnumerable<Window> windows = WinApi.InitOpenWindows(true);

            foreach (Window window in windows)
            {
                DataRow row = DataTable.NewRow();

                row[0] = window.WindowIcon;
                row[1] = window.WindowName;

                DataTable.Rows.Add(row);
            }
        }

        private void dgv_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
        }

        private void btnRefreshList_Click(object sender, System.EventArgs e)
        {
            InitDataTableRows();
        }

        private void dgvAvaliableWindows_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        var hti = this.dgvAvaliableWindows.HitTest(e.X, e.Y);
                        this.dgvAvaliableWindows.ClearSelection();
                        this.dgvAvaliableWindows.Rows[hti.RowIndex].Selected = true;

                        this.ctxmsMenu.Show(dgvAvaliableWindows, e.X, e.Y);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void tsmiAddWindowName_Click(object sender, System.EventArgs e)
        {
            string windowName = GetSelectedRowValue();

            AddWindowNameForm.AddRowToDataGridView(windowName);
        }

        private void tsmiCopyWindowName_Click(object sender, EventArgs e)
        {
            string windowName = GetSelectedRowValue();

            Clipboard.SetText(windowName);
        }

        private string GetSelectedRowValue()
        {
            if (dgvAvaliableWindows.SelectedCells.Count <= 0)
            {
                return string.Empty;
            }

            int selectedRowIndex = dgvAvaliableWindows.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dgvAvaliableWindows.Rows[selectedRowIndex];

            string windowName = Convert.ToString(selectedRow.Cells[1].Value, CultureInfo.InvariantCulture);
            return windowName;
        }

        private void dgvAvaliableWindows_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            if (rowIndex < 0)
            {
                return;
            }

            string windowName = Convert.ToString(this.dgvAvaliableWindows.Rows[rowIndex].Cells[1].Value, CultureInfo.InvariantCulture);

            if (string.IsNullOrEmpty(windowName))
            {
                return;
            }

            AddWindowNameForm.AddRowToDataGridView(windowName);
        }

        private void AvaliableWindowsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.Parent = null;
            e.Cancel = true;
        }
    }
}
