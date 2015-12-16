//-----------------------------------------------------------------------// <copyright file="AvailableWindowsForm.cs" company="Zhivko Kabaivanov">//     Copyright (c) Zhivko Kabaivanov. All rights reserved.// </copyright>// <author>Zhivko Kabaivanov</author>//-----------------------------------------------------------------------
namespace TeamViewerPopupBlocker.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using Classes;

    /// <summary>
    /// Class for showing the available opened window names along with their icons.
    /// </summary>
    public partial class AvailableWindowsForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableWindowsForm"/> class.
        /// </summary>
        public AvailableWindowsForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the data source for the <see cref="dgvAvaliableWindows"/>.
        /// </summary>
        private static DataTable AvaliableWindowsDataTable { get; set; }

        /// <summary>
        /// Initialization for the <see cref="AvaliableWindowsDataTable"/>.
        /// </summary>
        private static void InitDataTableColumns()
        {
            DataColumn windoNameColumn, iconColumn;
            AvaliableWindowsDataTable = null;

            using (AvaliableWindowsDataTable = new DataTable())
            using (iconColumn = new DataColumn())
            using (windoNameColumn = new DataColumn())
            {
                AvaliableWindowsDataTable.Locale = CultureInfo.InvariantCulture;

                iconColumn.ColumnName = "Icon";
                iconColumn.DataType = typeof(Bitmap);
                AvaliableWindowsDataTable.Columns.Add(iconColumn);

                windoNameColumn.ColumnName = "Window name";
                windoNameColumn.DataType = typeof(string);
                AvaliableWindowsDataTable.Columns.Add(windoNameColumn);
            }
        }

        /// <summary>
        /// Initializes the <see cref="AvaliableWindowsDataTable"/>.
        /// </summary>
        private static void InitDataTableRows()
        {
            AvaliableWindowsDataTable.Rows.Clear();
            IEnumerable<Window> windows = WindowOperations.InitOpenWindows(true);

            foreach (Window window in windows)
            {
                DataRow row = AvaliableWindowsDataTable.NewRow();

                row[0] = window.WindowIcon;
                row[1] = window.WindowName;

                AvaliableWindowsDataTable.Rows.Add(row);
            }
        }

        /// <summary>
        /// Initialization for the <see cref="dgvAvaliableWindows"/>.
        /// </summary>
        private void InitDataGridView()
        {
            this.dgvAvaliableWindows.DataSource = null;
            this.dgvAvaliableWindows.DataSource = AvaliableWindowsDataTable;

            this.dgvAvaliableWindows.Columns[0].Width = 45;
            this.dgvAvaliableWindows.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
        }

        /// <summary>
        /// Selecting the whole row instead of only one cell.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="DataGridViewRowPrePaintEventArgs"/> e.</param>
        private void AvailableWindowsDgvRowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
        }

        /// <summary>
        /// Refreshing the opened window names and refreshing the <see cref="dgvAvaliableWindows"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void RefreshListBtnClick(object sender, System.EventArgs e)
        {
            InitDataTableRows();
        }

        /// <summary>
        /// Handle right click mouse event to show the <see cref="ctxmsMenu"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> e.</param>
        private void AvaliableWindowsDgvMouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        var hti = this.dgvAvaliableWindows.HitTest(e.X, e.Y);
                        this.dgvAvaliableWindows.ClearSelection();
                        this.dgvAvaliableWindows.Rows[hti.RowIndex].Selected = true;

                        this.ctxmsMenu.Show(this.dgvAvaliableWindows, e.X, e.Y);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Handle click event for <see cref="tsmiCopyWindowName"/> to add the selected value from the <see cref="dgvAvaliableWindows"/> to the <see cref="Clipboard"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void CopyWindowNameTsmiClick(object sender, EventArgs e)
        {
            string windowName = this.GetSelectedRowValue();

            Clipboard.SetText(windowName);
        }

        /// <summary>
        /// Handle click event for <see cref="tsmiAddWindowName"/> to add the selected value from the <see cref="dgvAvaliableWindows"/> to the <see cref="AddWindowNameForm"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void AddWindowNameTsmiClick(object sender, EventArgs e)
        {
            string windowName = this.GetSelectedRowValue();

            AddWindowNameForm.AddRowToDataGridView(windowName);
        }

        /// <summary>
        /// Get the value of the selected row from the <see cref="dgvAvaliableWindows"/>.
        /// </summary>
        /// <returns> Returns the value from the selected row from <see cref="dgvAvaliableWindows"/>. </returns>
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

        /// <summary>
        /// Handling the double click event on a cell of the <see cref="dgvAvaliableWindows"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> e.</param>
        private void AvaliableWindowsDgvCellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
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

        /// <summary>
        /// Loading event for the <see cref="AvailableWindowsForm"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void AvaliableWindowsFormLoad(object sender, EventArgs e)
        {
            InitDataTableColumns();
            InitDataTableRows();
            this.InitDataGridView();
        }

        /// <summary>
        /// Handle the closing event for just hiding instead of closing.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> e.</param>
        private void AvaliableWindowsFormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.Parent = null;

            e.Cancel = true;
        }
    }
}
