using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Reflection;
using System.Threading;

using BattleCity.Engine;
using MouseKeyboardLibrary;

namespace BattleCity
{
    public class MainFrm : Form
    {
        #region Variables
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private LoadingPane m_loading = null;
        private Map m_map = null;
        private Canvas m_canvas = null;

        private DoubleBufferedPanel panelCanvas = null;

        private Thread m_processThread = null;
        private System.Windows.Forms.Timer m_timer = null;

        private bool m_loaded = false;
        private bool m_pause = false;

        private KeyboardHook m_keyHook = null;

        private string m_currentState = "1";

        private delegate void ProcessCompleteCallback();
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

            this.m_loading = new LoadingPane(this.panelCanvas.Width, this.panelCanvas.Height);
            this.m_map = new Map();
            this.m_canvas = new Canvas(16, 16);

            this.m_timer = new System.Windows.Forms.Timer()
            {
                Interval = 5
            };
            this.m_timer.Tick += new EventHandler(this.timer_Tick);

            this.m_keyHook = new KeyboardHook();
            this.m_keyHook.KeyDown += new KeyEventHandler(this.keyHook_KeyDown);
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
            this.panelCanvas = new BattleCity.Engine.DoubleBufferedPanel();
            this.SuspendLayout();
            // 
            // panelCanvas
            // 
            this.panelCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCanvas.Location = new System.Drawing.Point(0, 0);
            this.panelCanvas.Name = "panelCanvas";
            this.panelCanvas.Size = new System.Drawing.Size(527, 447);
            this.panelCanvas.TabIndex = 2;
            this.panelCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.panelCanvas_Paint);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(527, 447);
            this.Controls.Add(this.panelCanvas);
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
            this.m_timer.Stop();

            if (this.m_processThread != null)
            {
                this.m_processThread.Abort();
                this.m_processThread = null;
            }

            if (this.m_keyHook != null)
            {
                this.m_keyHook.Stop();
                this.m_keyHook = null;
            }

            if (this.m_loading != null)
            {
                this.m_loading.Dispose();
                this.m_loading = null;
            }

            if (this.m_canvas != null)
            {
                this.m_canvas.Dispose();
                this.m_canvas = null;
            }

            if (this.m_map != null)
            {
                this.m_map.Dispose();
                this.m_map = null;
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.m_loading.Show(true);

            this.m_processThread = new Thread(new ThreadStart(this.DoProcess));
            this.m_processThread.IsBackground = true;
            this.m_processThread.Start();

            this.m_timer.Start();
            this.m_keyHook.Start();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0x74, 0x74, 0x74)), this.ClientRectangle);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            if (this.m_loaded)
            {
                //this.m_pause = 1;
                //this.Pause();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!this.m_loaded) return;
            
            switch (e.KeyCode)
            {
                case Keys.P:
                case Keys.Escape:
                    this.m_pause = !this.m_pause;
                    this.Pause();
                    break;
                default:
                    break;
            }
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
            }
            base.WndProc(ref m);
        }
        #endregion

        #region Private Methods
        private void DoProcess()
        {
            Map.StateData state = this.m_map[this.m_currentState];

            if (!state.IsEmpty)
            {
                this.m_canvas.Create(state);
            }

            Thread.Sleep(50);

            ProcessCompleteCallback callback = new ProcessCompleteCallback(this.ProcessCompleted);
            callback.Invoke();
        }

        private void ProcessCompleted()
        {
            this.m_loaded = true;
            this.m_loading.Show(false);
            this.m_canvas.Show(true);

            System.Diagnostics.Debug.WriteLine("Load complete!");
        }

        private void keyHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (!this.m_loaded) return;

            if (this.m_canvas.Player1 != null)
            {
                bool isMove = false;
                switch (e.KeyCode)
                {
                    case Keys.W:
                    case Keys.Up:
                        this.m_canvas.Player1.Face = FaceDirection.Up;
                        isMove = true;
                        break;

                    case Keys.S:
                    case Keys.Down:
                        this.m_canvas.Player1.Face = FaceDirection.Down;
                        isMove = true;
                        break;

                    case Keys.D:
                    case Keys.Right:
                        this.m_canvas.Player1.Face = FaceDirection.Right;
                        isMove = true;
                        break;

                    case Keys.A:
                    case Keys.Left:
                        this.m_canvas.Player1.Face = FaceDirection.Left;
                        isMove = true;
                        break;

                    case Keys.F:
                    case Keys.Space:
                        this.m_canvas.Player1.Fire();
                        break;
                }
                if (isMove) this.m_canvas.Player1.DoMove(3);
            }
        }

        private void Pause()
        {
            if (this.m_pause)
            {
                this.Text = "Battle City [ pause ]";

                if (this.m_canvas != null)
                {
                    this.m_canvas.DoPause(true);
                }

                if (this.m_timer != null) this.m_timer.Stop();
                if (this.m_keyHook != null) this.m_keyHook.Stop();
            }
            else
            {
                this.Text = "Battle City";
                
                if (this.m_canvas != null)
                {
                    this.m_canvas.DoPause(false);
                }

                if (this.m_timer != null) this.m_timer.Start();
                if (this.m_keyHook != null) this.m_keyHook.Start();
            }
        }
        #endregion

        #region Public Methods
        #endregion

        #region Form Events
        #endregion

        #region Controls Events
        private void timer_Tick(object sender, EventArgs e)
        {
            this.panelCanvas.Invalidate();
        }

        private void panelCanvas_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            this.m_loading.DoPaint(e.Graphics);
            this.m_canvas.DoPaint(e.Graphics);
        }
        #endregion

        #region Properties
        #endregion
    }
}
