using System;
using System.Collections.Generic;
using System.Text;

using GameUtil;
using System.Drawing;
using BattleCity;

namespace BattleCity.Engine
{
    #region LoadingPane
    internal class LoadingPane : IGameObject
    {
        #region Variables
        private TitleText m_titleText = null;
        private LoadingText m_loadingText = null;
        #endregion

        #region Constructor
        public LoadingPane(DDGameGraphics graphic, DDGameSurface surface, Size size)
        {
            this.m_titleText = new TitleText(graphic, surface, new PointF((size.Width - 390) / 2, 100));
            this.m_loadingText = new LoadingText(graphic, surface, new PointF(200F, 300F));
        }
        #endregion

        #region Override Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        public void Update(float ticksPerSecond)
        {
            this.m_loadingText.Update(ticksPerSecond);
        }

        public bool Render()
        {
            this.m_titleText.Render();
            this.m_loadingText.Render();
            return true;
        }
        #endregion

        #region Properties
        #endregion
    }
    #endregion

    #region StatePane
    internal class StatePane : IGameObject
    {
        #region Variables
        #endregion

        #region Constructor
        public StatePane(DDGameGraphics graphic, DDGameSurface surface)
        {
        }
        #endregion

        #region Public Methods
        public void Update(float ticksPerSecond)
        {
        }

        public bool Render()
        {
            return true;
        }
        #endregion
    }
    #endregion

    #region InfoPane
    internal class InfoPane : IGameObject
    {
        #region Variables
        #endregion

        #region Constructor
        public InfoPane(DDGameGraphics graphic, DDGameSurface surface)
        {
        }
        #endregion

        #region Override Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        public void Update(float ticksPerSecond)
        {
        }

        public bool Render()
        {
            return true;
        }
        #endregion

        #region Properties
        #endregion
    }
    #endregion

    #region HighScorePane
    internal class HighScorePane : IGameObject
    {
        #region Variables
        #endregion

        #region Constructor
        public HighScorePane(DDGameGraphics graphic, DDGameSurface surface)
        {
        }
        #endregion

        #region Override Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        public void Update(float ticksPerSecond)
        {
        }

        public bool Render()
        {
            return true;
        }
        #endregion

        #region Properties
        #endregion
    }
    #endregion
}
