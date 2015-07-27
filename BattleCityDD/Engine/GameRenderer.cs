using System;
using System.Collections.Generic;
using System.Text;
using GameUtil;

namespace BattleCity.Engine
{
    #region RunningStates
    internal enum RunningStates
    {
        Initialize,
        Loading,
        Playing,
        Pause,
        HighScore,
        GameOver
    }
    #endregion

    internal class GameRenderer : IGameObject
    {
        #region Variables
        private DDGameGraphics m_graphics = null;
        private DDGameSurface m_surface = null;
        private RunningStates m_runningState = RunningStates.Initialize;

        private LoadingPane m_loadingPane = null;
        #endregion

        #region Constructor
        public GameRenderer(DDGameGraphics graphic, DDGameSurface surface)
        {
            this.m_graphics = graphic;
            this.m_surface = surface;

            this.Initialize();
        }
        #endregion

        #region Private Methods
        private void Initialize()
        {
            this.m_loadingPane = new LoadingPane(this.m_graphics, this.m_surface, this.m_surface.Window.Size);
        }
        #endregion

        #region Public Methods
        public void Update(float ticksPerSecond)
        {
            if (this.m_runningState == RunningStates.Loading)
            {
                this.m_loadingPane.Update(ticksPerSecond);    
            }
            else if (this.m_runningState == RunningStates.Playing)
            {
            }
            else if (this.m_runningState == RunningStates.Pause)
            {
            }
            else if (this.m_runningState == RunningStates.HighScore)
            {
            }
            else if (this.m_runningState == RunningStates.GameOver)
            {
            }
        }

        public bool Render()
        {
            if (this.m_runningState == RunningStates.Loading)
            {
                this.m_loadingPane.Render();    
            }
            else if (this.m_runningState == RunningStates.Playing)
            {
            }
            else if (this.m_runningState == RunningStates.Pause)
            {
            }
            else if (this.m_runningState == RunningStates.HighScore)
            {
            }
            else if (this.m_runningState == RunningStates.GameOver)
            {
            }
            return true;
        }
        #endregion

        #region Properties
        public RunningStates RunningState
        {
            get { return this.m_runningState; }
            set { this.m_runningState = value; }
        }

        public virtual DDGameGraphics GameGraphic
        {
            get { return this.m_graphics; }
            set { this.m_graphics = value; }
        }

        public virtual DDGameSurface GameSurface
        {
            get { return this.m_surface; }
        }
        #endregion
    }
}
