namespace TeamViewerPopupBlocker.Forms
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using Classes;
    using Properties;
    using Settings = Classes.Settings;

    public partial class AddWindowNameForm : Form
    {
        /// <summary>
        /// Column names for the <see cref="dgvWindowNames"/>.
        /// </summary>
        private string[] ColumnNames { get; } = 
        {
            Resources.AddWindowNameForm_ColumnNames_TeamViewer_Window_Name
        };

        /// <summary>
        /// The <see cref="DataTable"/> for populating the <see cref="dgvWindowNames"/>.
        /// </summary>
        private static DataTable DataTable { get; set; }

        /// <summary>
        /// Stores the <see cref="AvailableWindowsForm"/> form for showing.
        /// </summary>
        private static  AvailableWindowsForm AvailableWindowForm { get; set; }

        /// <summary>
        /// Collection of the window names loaded from <see cref="Settings.WindowNames"/>.
        /// </summary>
        private static Collection<string> WindowNames { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AddWindowNameForm"/>.
        /// </summary>
        public AddWindowNameForm()
        {
            InitializeComponent();

            AvailableWindowForm = new AvailableWindowsForm();
            WindowNames = Settings.Instance.WindowNames;
        }

        /// <summary>
        /// Adding a window name to the <see cref="DataTable"/>.
        /// </summary>
        /// <param name="windowName">The input window name that will be added.</param>
        public static void AddRowToDataGridView(string windowName)
        {
            DataTable.Rows.Add(windowName);
        }

        /// <summary>
        /// Sets a star symbol the the form title so the user be notified that there have been made a change in <see cref="dgvWindowNames"/>.
        /// </summary>
        /// <returns>True for containt a star in title, false for not containing.</returns>
        private bool IsSetModifiedTitle()
        {
            bool result = this.Text.Contains("*");

            if (result)
            {
                return true;
            }

            this.Text += '*';
            return false;
        }
        
        /// <summary>
        /// Removing the star symbol from the title of the form.
        /// </summary>
        private void RemoveModifiedTitle()
        {
            bool result = IsSetModifiedTitle();

            if (!result) return;

            this.Text = this.Text.Replace("*", string.Empty);
        }

        #region DataGrivView Methods

        /// <summary>
        /// Initializes the <see cref="DataTable"/>.
        /// </summary>
        private void InitializeDataTable()
        {
            CreateColumnForDataTable();

            PopulateDataTable();

            InitDataGridView();
        }

        /// <summary>
        /// Adding the <see cref="WindowNames"/> to the <see cref="DataTable"/>.
        /// </summary>
        private static void PopulateDataTable()
        {
            foreach (string windowName in WindowNames)
            {
                DataTable.Rows.Add(windowName);
            }
        }

        /// <summary>
        /// Initializes the <see cref="dgvWindowNames"/>.
        /// </summary>
        private void InitDataGridView()
        {
            this.dgvWindowNames.DataSource = null;
            this.dgvWindowNames.DataSource = DataTable;

            DataGridViewColumn dataGridViewColumn = this.dgvWindowNames.Columns[ColumnNames[0]];

            if (dataGridViewColumn != null)
            {
                dataGridViewColumn.Width = 400;
            }
        }

        /// <summary>
        /// Creating the columns for the <see cref="DataTable"/> from <see cref="ColumnNames"/>.
        /// </summary>
        private void CreateColumnForDataTable()
        {
            DataTable = new DataTable();
            DataTable.Locale = CultureInfo.InvariantCulture;

            using (DataColumn column = new DataColumn())
            {
                column.ColumnName = ColumnNames[0];
                column.DataType = typeof (string);
                
                DataTable.Columns.Add(column);
            }
        }

        /// <summary>
        /// Saving the <see cref="WindowNames"/> to the <see cref="Settings"/> collection.
        /// </summary>
        private void SaveWindowNamesToSettings()
        {
            WindowNames.Clear();

            foreach (DataGridViewRow row in this.dgvWindowNames.Rows)
            {
                string cellValue = Convert.ToString(row.Cells[ColumnNames[0]].Value, CultureInfo.InvariantCulture);

                if (string.IsNullOrEmpty(cellValue))
                {
                    continue;
                }

                WindowNames.Add(cellValue);
            }

            Settings.Instance.SaveWindowNames(WindowNames);
            Settings.Instance.Save();
        }

        /// <summary>
        /// Click event for reseting the window names to application default.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void btnDefault_Click(object sender, EventArgs e)
        {
            ReturnCollectionToDefault();

            try
            {
                ClearDataGridView();
                InitializeDataTable();
                SetResultLabel(Resources.AddWindowNameForm_SaveBtnClick_OK_, Color.GreenYellow);
            }
            catch (ArgumentNullException argumentNullException)
            {
                SetResultLabel(Resources.AddWindowNameForm_btnDefault_Click_Error_, Color.Red);
                LogSystem.Instance.AddToLog(argumentNullException, false);
            }
        }

        /// <summary>
        /// Reseting <see cref="WindowNames"/> to default values.
        /// </summary>
        private void ReturnCollectionToDefault()
        {
            Collection<string> defaultWindowsNames = new Collection<string>()
            {
                Resources.TeamViewer_WindowName_Commercial_use,
                Resources.TeamViewer_WindowName_Sponssored_Session,
                Resources.TeamViewer_WindowName_Commercial_use_suspected,
                Resources.TeamViewer_WindowName_Commercial_use_detected,
                Resources.TeamViewer_WindowName_Unable_to_connect,
            };

            WindowNames = defaultWindowsNames;
            ClearDataGridView();
            InitializeDataTable();
        }

        /// <summary>
        /// Clearing and releasing all resources of <see cref="dgvWindowNames"/>.
        /// </summary>
        private void ClearDataGridView()
        {
            this.dgvWindowNames.DataSource = null;
            this.dgvWindowNames.Rows.Clear();
            DataTable = new DataTable();
            DataTable.Locale = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Tracking the changes for the <see cref="dgvWindowNames"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void WindowNamesDdvCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            IsSetModifiedTitle();
        }

        /// <summary>
        /// Right mouse click event for showing the <see cref="cmsRowMenu"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void DgvWindowNamesMouseClick(object sender, MouseEventArgs e)
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

        /// <summary>
        /// Click event for the <see cref="ToolStripMenuItem"/> trigerring copy to <see cref="Clipboard"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void TsmiCopyClick(object sender, EventArgs e)
        {
            string value = GetSelectedRowValue();

            Clipboard.SetText(value);
        }

        /// <summary>
        /// Getting the selected row value.
        /// </summary>
        /// <returns>Returns a <see cref="string"/> with the value.</returns>
        private string GetSelectedRowValue()
        {
            if (dgvWindowNames.SelectedCells.Count <= 0)
            {
                return string.Empty;
            }

            int selectedRowIndex = dgvWindowNames.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dgvWindowNames.Rows[selectedRowIndex];

            string value = Convert.ToString(selectedRow.Cells[0].Value, CultureInfo.InvariantCulture);
            return value;
        }

        /// <summary>
        /// Click event for the <see cref="ToolStripMenuItem"/> trigerring delete row from <see cref="dgvWindowNames"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void TsmiDeleteClick(object sender, EventArgs e)
        {
            DeleteRow();
        }

        /// <summary>
        /// Deleting a row from the <see cref="dgvWindowNames"/>.
        /// </summary>
        private void DeleteRow()
        {
            var row = this.dgvWindowNames.CurrentRow;

            if (row != null)
            {
                this.dgvWindowNames.Rows.Remove(row);
            }
        }

        /// <summary>
        /// Setting the text and the <see cref="Control.BackColor"/> of the <see cref="lblResult"/>.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="backColor"></param>
        private void SetResultLabel(string text, Color backColor)
        {
            this.lblResult.Text = text;
            this.lblResult.BackColor = backColor;
        }

        #endregion

        #region Button Click Events

        /// <summary>
        /// Show info message how to use form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoBtnClick(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.AddWindowNam_InfoBtnClick,
                Resources.Program_Name,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }

        /// <summary>
        /// Click event to show the <see cref="AvailableWindowForm"/> form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewWindowsBtnClick(object sender, EventArgs e)
        {
            if (AvailableWindowForm.Visible)
            {
                AvailableWindowForm.Focus();
                return;
            }

            AvailableWindowForm.Show();
            AvailableWindowForm.Location = new Point(this.Left + this.Width, this.Top);
        }

        /// <summary>
        /// Saves the current state of t
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtnClick(object sender, EventArgs e)
        {
            try
            {
                RemoveModifiedTitle();

                SaveWindowNamesToSettings();
                SetResultLabel(Resources.AddWindowNameForm_SaveBtnClick_OK_, Color.GreenYellow);

                MessageBox.Show(Resources.AddWindowNameForm_SaveBtnClick,
                    Resources.Program_Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (ArgumentNullException argumentNullException)
            {
                SetResultLabel(Resources.AddWindowNameForm_btnDefault_Click_Error_, Color.Red);
                LogSystem.Instance.AddToLog(argumentNullException, false);
            }
        }

        #endregion

        #region Form events

        /// <summary>
        /// Occurs before a form is displayed for the first time.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void AddWindowNameFormLoad(object sender, EventArgs e)
        {
            InitializeDataTable();
        }

        /// <summary>
        /// Closing <see cref="AddWindowNameForm"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddWindowNameFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AvailableWindowForm.Visible)
            {
                return;
            }

            bool result = IsSetModifiedTitle();

            if (result)
            {
                DialogResult dialogResult = MessageBox.Show(
                    Resources.AddWindowNameForm_AddWindowNameFormFormClosing_Do_you_want_to_save_the_changes_,
                    Resources.Program_Name,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);

                switch (dialogResult)
                {
                    case DialogResult.Yes:
                    {
                        RemoveModifiedTitle();
                        break;
                    }

                    case DialogResult.None:
                    case DialogResult.OK:
                    case DialogResult.Cancel:
                    case DialogResult.Abort:
                    case DialogResult.Retry:
                    case DialogResult.Ignore:
                    case DialogResult.No:
                    default:
                    {
                        break;
                    }
                }
            }

            AvailableWindowForm.Close();

            this.Hide();
            this.Parent = null;
            e.Cancel = true;
        }

        #endregion

        /// <summary>
        /// When the ESC key is press, the window is closed.
        /// </summary>
        /// <param name="msg">The <see cref="Message"/> message.</param>
        /// <param name="keyData">The <see cref="Keys"/> key.</param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.Close();
            bool res = base.ProcessCmdKey(ref msg, keyData);
            return res;
        }

        private void dgvWindowNames_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (this.Visible)
            {
                return;
            }

            IsSetModifiedTitle();
        }
    }
}
