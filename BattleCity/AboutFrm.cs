using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MouseKeyboardLibrary;

namespace BattleCity
{
    public partial class AboutFrm : Form
    {
        private KeyboardHook keyHook = new KeyboardHook();

        public AboutFrm()
        {
            InitializeComponent();

            this.keyHook.KeyDown += new KeyEventHandler(this.keyHook_KeyDown);
        }

        private void keyHook_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("keyHook_KeyDown");
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.keyHook.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.keyHook.Stop();
        }
    }
}
