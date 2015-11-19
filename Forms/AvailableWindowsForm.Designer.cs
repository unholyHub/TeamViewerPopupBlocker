using TeamViewerPopupBlocker.Properties;

namespace TeamViewerPopupBlocker.Forms
{
    partial class AvailableWindowsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AvailableWindowsForm));
            this.dgvAvaliableWindows = new System.Windows.Forms.DataGridView();
            this.btnRefreshList = new System.Windows.Forms.Button();
            this.ctxmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAddWindowName = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopyWindowName = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvaliableWindows)).BeginInit();
            this.ctxmsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvAvaliableWindows
            // 
            this.dgvAvaliableWindows.AllowUserToAddRows = false;
            this.dgvAvaliableWindows.AllowUserToDeleteRows = false;
            this.dgvAvaliableWindows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvaliableWindows.Location = new System.Drawing.Point(13, 55);
            this.dgvAvaliableWindows.MultiSelect = false;
            this.dgvAvaliableWindows.Name = "dgvAvaliableWindows";
            this.dgvAvaliableWindows.ReadOnly = true;
            this.dgvAvaliableWindows.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAvaliableWindows.Size = new System.Drawing.Size(321, 188);
            this.dgvAvaliableWindows.TabIndex = 0;
            this.dgvAvaliableWindows.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAvaliableWindows_CellContentDoubleClick);
            this.dgvAvaliableWindows.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgv_RowPrePaint);
            this.dgvAvaliableWindows.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvAvaliableWindows_MouseClick);
            // 
            // btnRefreshList
            // 
            this.btnRefreshList.Location = new System.Drawing.Point(13, 12);
            this.btnRefreshList.Name = "btnRefreshList";
            this.btnRefreshList.Size = new System.Drawing.Size(321, 37);
            this.btnRefreshList.TabIndex = 1;
            this.btnRefreshList.Text = global::TeamViewerPopupBlocker.Properties.Resources.AvailableWindowsForm_InitializeComponent_Refresh_List;
            this.btnRefreshList.UseVisualStyleBackColor = true;
            this.btnRefreshList.Click += new System.EventHandler(this.btnRefreshList_Click);
            // 
            // ctxmsMenu
            // 
            this.ctxmsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddWindowName,
            this.tsmiCopyWindowName});
            this.ctxmsMenu.Name = "ctxmsMenu";
            this.ctxmsMenu.Size = new System.Drawing.Size(185, 48);
            // 
            // tsmiAddWindowName
            // 
            this.tsmiAddWindowName.Image = global::TeamViewerPopupBlocker.Properties.Resources.add_window_name;
            this.tsmiAddWindowName.Name = "tsmiAddWindowName";
            this.tsmiAddWindowName.Size = new System.Drawing.Size(184, 22);
            this.tsmiAddWindowName.Text = global::TeamViewerPopupBlocker.Properties.Resources.AvailableWindowsForm_InitializeComponent_Add_Window_Name;
            this.tsmiAddWindowName.Click += new System.EventHandler(this.tsmiAddWindowName_Click);
            // 
            // tsmiCopyWindowName
            // 
            this.tsmiCopyWindowName.Image = global::TeamViewerPopupBlocker.Properties.Resources.copy;
            this.tsmiCopyWindowName.Name = "tsmiCopyWindowName";
            this.tsmiCopyWindowName.Size = new System.Drawing.Size(184, 22);
            this.tsmiCopyWindowName.Text = global::TeamViewerPopupBlocker.Properties.Resources.AvailableWindowsForm_InitializeComponent_Copy_Window_Name;
            this.tsmiCopyWindowName.Click += new System.EventHandler(this.tsmiCopyWindowName_Click);
            // 
            // AvailableWindowsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 255);
            this.Controls.Add(this.btnRefreshList);
            this.Controls.Add(this.dgvAvaliableWindows);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(362, 294);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(362, 294);
            this.Name = "AvailableWindowsForm";
            this.Text = "Available Windows";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AvaliableWindowsForm_FormClosing);
            this.Load += new System.EventHandler(this.AvaliableWindowsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvaliableWindows)).EndInit();
            this.ctxmsMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAvaliableWindows;
        private System.Windows.Forms.Button btnRefreshList;
        private System.Windows.Forms.ContextMenuStrip ctxmsMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddWindowName;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyWindowName;
    }
}