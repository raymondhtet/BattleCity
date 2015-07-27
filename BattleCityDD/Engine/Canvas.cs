using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Microsoft.DirectX.DirectInput;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Threading;

using GameUtil;
using BattleCity;

namespace BattleCity.Engine
{
    internal class Canvas : IDisposable
    {
        #region Variables
        private Form m_form = null;

        private DDGameSurface m_surface = null;
        private DDGameGraphics m_graphics = null;

        private bool m_running = false;
        private Thread m_processThread = null;

        private DIKeyboard m_keyboard = null;

        private GameRenderer m_game = null;
        #endregion

        #region Constructor
        public Canvas(Form form)
        {
            this.m_form = form;
        }
        #endregion

        #region Override Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        private void ProcessKeyboard()
        {
            this.m_keyboard.Update();

            // Move character.
            if (this.m_keyboard[Key.Down])
            {
            }
            else if (this.m_keyboard[Key.Up])
            {
            }
            else if (this.m_keyboard[Key.Left])
            {
            }
            else if (this.m_keyboard[Key.Right])
            {
            }

            Type keyType = typeof(Key);
            foreach (string keyName in Enum.GetNames(keyType))
            {
                Key key = (Key)Enum.Parse(keyType, keyName);
                if (this.m_keyboard[key])
                {
                }
            }
        }

        private void UpdateGameLogic(float ticksPerSecond)
        {
            this.m_game.Update(ticksPerSecond);
        }

        private void Render()
        {
            // Make sure we can actually draw on the surface.
            if (this.m_surface.CanDraw)
            {
                // Create some form of grass-like background.
                this.m_surface.Clear(Color.FromArgb(0x74, 0x74, 0x74));

                if (this.m_game != null)
                {
                    this.m_game.Render();
                }
            }
        }

        private void DoProcess()
        {
            do
            {
                System.Diagnostics.Trace.Write("Process Start.");
                Thread.Sleep(5000);
                System.Diagnostics.Trace.Write("Process Complete.");
                this.m_game.RunningState = RunningStates.Playing;
            } while (this.m_processThread.IsAlive);
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
            // Initialize game components.
            this.m_surface = new DDGameSurface(this.m_form);

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BattleCity.Resources.Graphics.xml");
            this.m_graphics = new DDGameGraphics(stream, this.m_surface);
            stream.Close();

            this.m_keyboard = new DIKeyboard(this.m_form);

            this.m_game = new GameRenderer(this.m_graphics, this.m_surface);

            TraceFrm.TRACE(this.m_form, "Initialize : OK");

            if (this.m_processThread == null)
            {
                this.m_processThread = new Thread(new ThreadStart(this.DoProcess));
                this.m_processThread.IsBackground = true;
                this.m_processThread.Start();
            }

            System.Diagnostics.Trace.Write("RunGame");

            this.m_game.RunningState = RunningStates.Loading;
            this.RunGame();
        }

        public void Dispose()
        {
            this.m_running = false;

            if (this.m_processThread != null)
            {
                if ((this.m_processThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                    this.m_processThread.Abort();
                this.m_processThread = null;
            }

            try
            {
                // Free managed resources.
                this.m_graphics.Dispose();
                this.m_surface.Dispose();
                this.m_keyboard.Dispose();
            }
            catch
            {
            }
        }

        public void RunGame()
        {
            // Game is now running.
            this.m_running = true;

            // Create and start timer.
            QPTimer timer = new QPTimer();
            timer.Start(QPTimer.Counter);

            //this.m_processThread.Start();

            // Update game until it is no longer running.
            while (this.m_running)
            {
                // Update timer.
                timer.Record();

                // Update game state.
                this.ProcessKeyboard();
                this.UpdateGameLogic(timer.TicksPerSecond);
                this.Render();

                // Display game in its current state.
                this.m_surface.Display();

                // Restart timer.
                timer.Start(timer.RecordTime);

                // Process events.
                Application.DoEvents();
            }
        }
        #endregion

        #region Properties
        #endregion
    }
}
