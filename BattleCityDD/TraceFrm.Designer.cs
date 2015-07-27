namespace BattleCity
{
    partial class TraceFrm
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
            this.lstTrace = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstTrace
            // 
            this.lstTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTrace.FormatString = "TRACE : {0}";
            this.lstTrace.FormattingEnabled = true;
            this.lstTrace.HorizontalScrollbar = true;
            this.lstTrace.IntegralHeight = false;
            this.lstTrace.Location = new System.Drawing.Point(0, 0);
            this.lstTrace.Name = "lstTrace";
            this.lstTrace.ScrollAlwaysVisible = true;
            this.lstTrace.Size = new System.Drawing.Size(353, 459);
            this.lstTrace.TabIndex = 0;
            // 
            // TraceFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 459);
            this.Controls.Add(this.lstTrace);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "TraceFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Trace Window";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstTrace;
    }
}