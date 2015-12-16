namespace TeamViewerPopupBlocker.Forms
{
    partial class AddWindowNameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddWindowNameForm));
            this.dgvWindowNames = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnInfo = new System.Windows.Forms.Button();
            this.cmsRowMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnViewWindows = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWindowNames)).BeginInit();
            this.cmsRowMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvWindowNames
            // 
            this.dgvWindowNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWindowNames.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvWindowNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWindowNames.Location = new System.Drawing.Point(13, 12);
            this.dgvWindowNames.MultiSelect = false;
            this.dgvWindowNames.Name = "dgvWindowNames";
            this.dgvWindowNames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWindowNames.Size = new System.Drawing.Size(417, 186);
            this.dgvWindowNames.TabIndex = 0;
            this.dgvWindowNames.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.WindowNamesDdvCellValueChanged);
            this.dgvWindowNames.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvWindowNames_RowsAdded);
            this.dgvWindowNames.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DgvWindowNamesMouseClick);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(293, 205);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(54, 39);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = global::TeamViewerPopupBlocker.Properties.Resources.AddWindowNameForm_InitializeComponent_Save;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.SaveBtnClick);
            // 
            // btnDefault
            // 
            this.btnDefault.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDefault.Location = new System.Drawing.Point(169, 205);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(118, 38);
            this.btnDefault.TabIndex = 2;
            this.btnDefault.Text = global::TeamViewerPopupBlocker.Properties.Resources.AddWindowNameForm_InitializeComponent_Default;
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(365, 218);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status:";
            // 
            // lblResult
            // 
            this.lblResult.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(411, 218);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(0, 13);
            this.lblResult.TabIndex = 4;
            // 
            // btnInfo
            // 
            this.btnInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnInfo.BackColor = System.Drawing.Color.Transparent;
            this.btnInfo.BackgroundImage = global::TeamViewerPopupBlocker.Properties.Resources.question_mark;
            this.btnInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInfo.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnInfo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnInfo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfo.ForeColor = System.Drawing.Color.Transparent;
            this.btnInfo.Location = new System.Drawing.Point(13, 205);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(39, 38);
            this.btnInfo.TabIndex = 5;
            this.btnInfo.UseVisualStyleBackColor = false;
            this.btnInfo.Click += new System.EventHandler(this.InfoBtnClick);
            // 
            // cmsRowMenu
            // 
            this.cmsRowMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopy,
            this.tsmiDelete});
            this.cmsRowMenu.Name = "cmsRowMenu";
            this.cmsRowMenu.Size = new System.Drawing.Size(108, 48);
            // 
            // tsmiCopy
            // 
            this.tsmiCopy.Image = global::TeamViewerPopupBlocker.Properties.Resources.copy;
            this.tsmiCopy.Name = "tsmiCopy";
            this.tsmiCopy.Size = new System.Drawing.Size(107, 22);
            this.tsmiCopy.Text = global::TeamViewerPopupBlocker.Properties.Resources.AddWindowNameForm_InitializeComponent_Copy;
            this.tsmiCopy.Click += new System.EventHandler(this.TsmiCopyClick);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Image = global::TeamViewerPopupBlocker.Properties.Resources.delete;
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(107, 22);
            this.tsmiDelete.Text = global::TeamViewerPopupBlocker.Properties.Resources.AddWindowNameForm_InitializeComponent_Delete;
            this.tsmiDelete.Click += new System.EventHandler(this.TsmiDeleteClick);
            // 
            // btnViewWindows
            // 
            this.btnViewWindows.Location = new System.Drawing.Point(58, 204);
            this.btnViewWindows.Name = "btnViewWindows";
            this.btnViewWindows.Size = new System.Drawing.Size(105, 39);
            this.btnViewWindows.TabIndex = 6;
            this.btnViewWindows.Text = global::TeamViewerPopupBlocker.Properties.Resources.AddWindowNameForm_InitializeComponent_View_Opened_Windows;
            this.btnViewWindows.UseVisualStyleBackColor = true;
            this.btnViewWindows.Click += new System.EventHandler(this.ViewWindowsBtnClick);
            // 
            // AddWindowNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(442, 256);
            this.Controls.Add(this.btnViewWindows);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvWindowNames);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(458, 294);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(458, 294);
            this.Name = "AddWindowNameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add TeamViewer Window Name";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddWindowNameFormFormClosing);
            this.Load += new System.EventHandler(this.AddWindowNameFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWindowNames)).EndInit();
            this.cmsRowMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvWindowNames;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.ContextMenuStrip cmsRowMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.Button btnViewWindows;
    }
}