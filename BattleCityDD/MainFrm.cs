using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BattleCity.Engine;

namespace BattleCity
{
    public partial class MainFrm : Form
    {
        #region Variables
        private System.ComponentModel.IContainer components = null;

        private Canvas m_canvas = null;
        #endregion

        #region Constructor
        public MainFrm()
        {
            this.InitializeComponent();

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.BackColor = Color.FromArgb(0x74, 0x74, 0x74);
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.m_canvas = new Canvas(this);
        }
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.SuspendLayout();
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 447);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.Text = "Battle City";
            this.ResumeLayout(false);

        }
        #endregion

        #region Override Methods
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

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 160: //WM_NCMOUSEMOVE
                case 0xa1: //WM_NCLBUTTONDOWN
                case 0xa7: //WM_NCMBUTTONDOWN
                case 0xa4: //WM_NCRBUTTONDOWN
                case 0xab: //WM_NCXBUTTONDOWN
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;
                    break;

                case 0x10: // WM_CLOSE
                    this.Dispose();
                    return;
            }
            base.WndProc(ref m);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (this.m_canvas != null)
            {
                this.m_canvas.Initialize();
            }
        }
        #endregion

        #region Private Methods
        private new void Dispose()
        {
            if (this.m_canvas != null)
            {
                this.m_canvas.Dispose();
            }
            Application.Exit();
        }
        #endregion

        #region Public Methods
        #endregion

        #region Form Events
        #endregion

        #region Controls Events
        #endregion

        #region Properties
        #endregion

        #region Nested Types
        #endregion

    }
}
