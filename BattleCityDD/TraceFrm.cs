using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace BattleCity
{
    public partial class TraceFrm : Form
    {
        #region Variables
        private static TraceFrm m_instance = null;
        private ControlTraceListener m_traceListener = null;
        #endregion

        #region Constructor
        static TraceFrm()
        {
        }

        public TraceFrm()
        {
            InitializeComponent();

            this.m_traceListener = new ControlTraceListener(this.lstTrace, true);
        }
        #endregion

        #region Override Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        private void Clear()
        {
            this.lstTrace.Items.Clear();
        }

        private void Write(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(this.Write), message);
                return;
            }

            if (this.lstTrace.Items.Count > 500)
            {
                this.Clear();
            }

            if (this.m_traceListener != null)
            {
                this.m_traceListener.Write(message);
            }
            else
            {
                this.lstTrace.Items.Add(message);

                if (this.lstTrace.Items.Count > 0)
                {
                    this.lstTrace.SelectedIndex = this.lstTrace.Items.Count - 1;
                }
            }
        }
        #endregion

        #region Static Methods
        public static void TRACE(string message)
        {
            if (TraceFrm.m_instance == null)
            {
                TraceFrm.m_instance = new TraceFrm();
                TraceFrm.m_instance.Show();
            }

            TraceFrm.m_instance.Write(message);
        }

        public static void TRACE(Form owner, string message)
        {
            if (TraceFrm.m_instance == null)
            {
                TraceFrm.m_instance = new TraceFrm();

                if(owner != null)
                {
                    TraceFrm.m_instance.Location = new Point(owner.Right + (SystemInformation.BorderSize.Width * 2), owner.Top);
                    owner.Activate();
                }
                TraceFrm.m_instance.Show();
            }

            TraceFrm.m_instance.Write(message);
        }
        #endregion

        #region Nested Types
        private class ControlTraceListener : TraceListener
        {
            #region Variables
            private Control m_traceCtrl = null;
            private ConsoleListener m_consoleListener = null;
            protected delegate void Delegate();
            #endregion

            #region Constructor/Destructor
            public ControlTraceListener(Control ctrl)
                : this(ctrl, false)
            {
            }

            public ControlTraceListener(Control ctrl, bool withConsole)
            {
                this.m_traceCtrl = ctrl;
                TraceListenerCollection traceListeners = Trace.Listeners;
                if (!traceListeners.Contains(this))
                {
                    traceListeners.Add(this);
                }

                if (withConsole)
                {
                    this.m_consoleListener = new ConsoleListener(this);
                    Console.SetOut(this.m_consoleListener);
                }
            }
            #endregion

            #region Override Methods
            public override void Write(string message)
            {
                this.RunOnUiThread(delegate
                {
                    this.WriteInUiThread(message);
                }
                );
            }

            public override void WriteLine(string message)
            {
                this.Write(message + Environment.NewLine);
            }
            #endregion

            #region Private Methods
            private void WriteInUiThread(string message)
            {
                // accessing textBox.Text is not fast but works for small trace logs

                if (this.m_traceCtrl is TextBox)
                {
                    TextBox traceCtrl = (TextBox)this.m_traceCtrl;
                    traceCtrl.Text = traceCtrl.Text + message;
                    traceCtrl.SelectionLength = 0;
                    traceCtrl.SelectionStart = traceCtrl.Text.Length;
                }
                else if (this.m_traceCtrl is ListBox)
                {
                    ListBox traceCtrl = (ListBox)this.m_traceCtrl;

                    try
                    {
                        using (StringReader reader = new StringReader(message))
                        {
                            string line = reader.ReadLine();
                            while (line != null)
                            {
                                traceCtrl.Items.Add(line);
                                line = reader.ReadLine();
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    if (traceCtrl.Items.Count > 0)
                    {
                        traceCtrl.SelectedIndex = traceCtrl.Items.Count - 1;
                    }
                }
            }
            #endregion

            #region Protected Methods
            protected virtual void RunOnUiThread(Delegate method)
            {
                try
                {
                    this.m_traceCtrl.Invoke(method);
                }
                catch (System.Exception)
                {
                    try
                    {
                        this.m_traceCtrl.BeginInvoke(method);
                    }
                    catch (System.Exception)
                    {
                    }
                }
            }
            #endregion

            #region Properties
            private string Text
            {
                get 
                {
                    if (this.m_traceCtrl is TextBox)
                    {
                        TextBox traceCtrl = (TextBox)this.m_traceCtrl;
                        return traceCtrl.Text;
                    }
                    else if (this.m_traceCtrl is ListBox)
                    {
                        ListBox traceCtrl = (ListBox)this.m_traceCtrl;
                        string text = "";
                        for (int i = 0; i < traceCtrl.Items.Count; i++)
                        {
                            text += traceCtrl.Items[i].ToString() + Environment.NewLine;
                        }

                        return text;
                    }

                    return ""; 
                }
            }
            #endregion

            #region Nested Types
            private class ConsoleListener : System.IO.StringWriter
            {
                #region Variables
                private ControlTraceListener m_listener = null;
                #endregion

                #region Constructor
                public ConsoleListener(ControlTraceListener listener)
                {
                    this.m_listener = listener;
                }
                #endregion

                #region Override Methods
                public override System.Text.StringBuilder GetStringBuilder()
                {
                    if (this.m_listener != null)
                    {
                        return new System.Text.StringBuilder(this.m_listener.Text);
                    }

                    return new System.Text.StringBuilder();
                }

                public override void Write(char value)
                {
                    if (this.m_listener != null)
                    {
                        this.m_listener.Write(value.ToString());
                    }
                }

                public override void Write(string value)
                {
                    this.m_listener.Write(value);
                }

                public override void Write(char[] buffer, int index, int count)
                {
                    if (buffer == null)
                    {
                        throw new ArgumentNullException("buffer");
                    }
                    if (index < 0)
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }
                    if (count < 0)
                    {
                        throw new ArgumentOutOfRangeException("count");
                    }
                    if ((buffer.Length - index) < count)
                    {
                        throw new ArgumentException();
                    }

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(buffer, index, count);
                    this.m_listener.Write(sb.ToString());
                }
                #endregion
            }
            #endregion
        }
        #endregion
    }
}
