namespace ArithmeticsTest
{
    partial class CreatedStackForm
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
            this.gvbPatterns = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.btnCreatedStack = new Telerik.WinControls.UI.RadButton();
            this.gvbStackDetail = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gvbStack = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreatedStack)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // gvbPatterns
            // 
            this.gvbPatterns.AllowAddNewRow = false;
            this.gvbPatterns.AllowCheckSort = false;
            this.gvbPatterns.AllowDeleteRow = false;
            this.gvbPatterns.AllowEditRow = true;
            this.gvbPatterns.AllowSelectAll = true;
            this.gvbPatterns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbPatterns.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbPatterns.DataSource = null;
            this.gvbPatterns.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbPatterns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvbPatterns.EnableFiltering = true;
            this.gvbPatterns.EnablePaging = false;
            this.gvbPatterns.EnableSorting = true;
            this.gvbPatterns.Location = new System.Drawing.Point(3, 3);
            this.gvbPatterns.Name = "gvbPatterns";
            this.gvbPatterns.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbPatterns.PageSize = 20;
            this.gvbPatterns.ReadOnly = false;
            this.gvbPatterns.ShowCheckBox = false;
            this.gvbPatterns.ShowRowHeaderColumn = false;
            this.gvbPatterns.ShowRowNumber = true;
            this.gvbPatterns.Size = new System.Drawing.Size(394, 623);
            this.gvbPatterns.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(26, 28);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(56, 24);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCreatedStack
            // 
            this.btnCreatedStack.Location = new System.Drawing.Point(204, 27);
            this.btnCreatedStack.Name = "btnCreatedStack";
            this.btnCreatedStack.Size = new System.Drawing.Size(73, 24);
            this.btnCreatedStack.TabIndex = 2;
            this.btnCreatedStack.Text = "创建垛";
            this.btnCreatedStack.Click += new System.EventHandler(this.btnCreatedStack_Click);
            // 
            // gvbStackDetail
            // 
            this.gvbStackDetail.AllowAddNewRow = false;
            this.gvbStackDetail.AllowCheckSort = false;
            this.gvbStackDetail.AllowDeleteRow = false;
            this.gvbStackDetail.AllowEditRow = true;
            this.gvbStackDetail.AllowSelectAll = true;
            this.gvbStackDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbStackDetail.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbStackDetail.DataSource = null;
            this.gvbStackDetail.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbStackDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvbStackDetail.EnableFiltering = true;
            this.gvbStackDetail.EnablePaging = false;
            this.gvbStackDetail.EnableSorting = true;
            this.gvbStackDetail.Location = new System.Drawing.Point(891, 3);
            this.gvbStackDetail.Name = "gvbStackDetail";
            this.gvbStackDetail.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbStackDetail.PageSize = 20;
            this.gvbStackDetail.ReadOnly = true;
            this.gvbStackDetail.ShowCheckBox = false;
            this.gvbStackDetail.ShowRowHeaderColumn = false;
            this.gvbStackDetail.ShowRowNumber = true;
            this.gvbStackDetail.Size = new System.Drawing.Size(483, 623);
            this.gvbStackDetail.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.gvbPatterns, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gvbStackDetail, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.gvbStack, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 58);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1377, 629);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // gvbStack
            // 
            this.gvbStack.AllowAddNewRow = false;
            this.gvbStack.AllowCheckSort = false;
            this.gvbStack.AllowDeleteRow = false;
            this.gvbStack.AllowEditRow = true;
            this.gvbStack.AllowSelectAll = true;
            this.gvbStack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbStack.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbStack.DataSource = null;
            this.gvbStack.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbStack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvbStack.EnableFiltering = true;
            this.gvbStack.EnablePaging = false;
            this.gvbStack.EnableSorting = true;
            this.gvbStack.Location = new System.Drawing.Point(403, 3);
            this.gvbStack.Name = "gvbStack";
            this.gvbStack.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbStack.PageSize = 20;
            this.gvbStack.ReadOnly = true;
            this.gvbStack.ShowCheckBox = false;
            this.gvbStack.ShowRowHeaderColumn = false;
            this.gvbStack.ShowRowNumber = true;
            this.gvbStack.Size = new System.Drawing.Size(482, 623);
            this.gvbStack.TabIndex = 4;
            // 
            // CreatedStackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1391, 693);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnCreatedStack);
            this.Controls.Add(this.btnSearch);
            this.Name = "CreatedStackForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "CreatedStackFrom";
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreatedStack)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbPatterns;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadButton btnCreatedStack;
        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbStackDetail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbStack;
    }
}
