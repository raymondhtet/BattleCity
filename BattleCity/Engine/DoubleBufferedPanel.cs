using System;
using System.Windows.Forms;

namespace BattleCity.Engine
{
    public class DoubleBufferedPanel : Panel
    {
        #region Constructor
        public DoubleBufferedPanel()
        {
            if (!this.DesignMode)
            {
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.DoubleBuffer, true);
                this.UpdateStyles();
            }
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        #endregion
    }
}
