namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    partial class LayerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerForm));
            this.rollerUserControl1 = new NJIS.FPZWS.LineControl.Drilling.Client.Forms.RollerUserControl();
            this.SuspendLayout();
            // 
            // rollerUserControl1
            // 
            this.rollerUserControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rollerUserControl1.BackgroundImage")));
            this.rollerUserControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rollerUserControl1.Location = new System.Drawing.Point(152, 144);
            this.rollerUserControl1.Name = "rollerUserControl1";
            this.rollerUserControl1.Size = new System.Drawing.Size(92, 45);
            this.rollerUserControl1.TabIndex = 0;
            // 
            // LayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 769);
            this.Controls.Add(this.rollerUserControl1);
            this.Name = "LayerForm";
            this.Text = "LayerForm";
            this.ResumeLayout(false);

        }

        #endregion

        private RollerUserControl rollerUserControl1;
    }
}