using System;
using System.Collections.Generic;
using System.Text;

using GameUtil;
using System.Drawing;
using BattleCity;

namespace BattleCity.Engine
{
    #region Text
    internal class TitleText : StaticObject
    {
        #region Constuctor
        public TitleText(DDGameGraphics graphic, DDGameSurface surface, PointF location)
            : base(graphic["Title"], surface, location)
        {
        }
        #endregion
    }

    internal class LoadingText : GameObject
    {
        #region Constuctor
        public LoadingText(DDGameGraphics graphic, DDGameSurface surface, PointF location)
            : base(graphic["Loading"], surface, location)
        {
            this.m_framesPerSecond = 7F;
        }
        #endregion
        
        #region Override Methods
        protected override void SetFrame(float ticksPerSecond)
        {
            this.m_currentFrame += ticksPerSecond * this.FramesPerSecond;

            // Number of movements is directly related to number of frames in a col.
            if (this.m_currentFrame >= this.m_graphic.FramesPerRow)
            {
                this.m_currentFrame = 0;
            }
        }
        #endregion
    }

    internal class GameOverText : StaticObject
    {
        #region Constuctor
        public GameOverText(DDGameGraphics graphic, DDGameSurface surface, PointF location)
            : base(graphic["GameOver"], surface, location)
        {
        }
        #endregion
    }
    #endregion

    #region Icons
    internal class FlagIcon : StaticObject
    {
        #region Constructor
        public FlagIcon(DDGameGraphics graphic, DDGameSurface surface, PointF location)
            : base(graphic["InfoIcons"], surface, location, 0)
        {
        }
        #endregion
    }

    internal class PlayerLifeIcon : StaticObject
    {
        #region Constructor
        public PlayerLifeIcon(DDGameGraphics graphic, DDGameSurface surface, PointF location, bool secondPlayer)
            : base(graphic["InfoIcons"], surface, location, (secondPlayer) ? 2 : 1)
        {
        }
        #endregion
    }

    internal class EnemyIcon : StaticObject
    {
        #region Constructor
        public EnemyIcon(DDGameGraphics graphic, DDGameSurface surface, PointF location)
            : base(graphic["InfoIcons"], surface, location, 3)
        {
        }
        #endregion
    }

    internal class TotalTankIcon : StaticObject
    {
        #region Constructor
        public TotalTankIcon(DDGameGraphics graphic, DDGameSurface surface, PointF location, int type)
            : base(graphic["InfoIcons"], surface, location, type + 3)
        {
        }
        #endregion
    }
    #endregion

    #region Map Item
    internal enum MapItemTypes : short
    {
        blank = 0,
        brick = 1,
        ferum = 2,
        garden = 3,
        water = 4,
        ice = 5
    }

    internal class MapItem : GameObject
    {
        #region Variables
        private MapItemTypes m_types = MapItemTypes.blank;
        #endregion

        #region Constructor
        public MapItem(DDGameGraphics graphic, DDGameSurface surface, PointF location)
            : base(graphic["MapItems"], surface, location)
        {
            this.m_framesPerSecond = 16F;
        }
        #endregion

        #region Override Methods
        protected override void SetFrame(float ticksPerSecond)
        {
            base.SetFrame(ticksPerSecond);
        }

        public override bool Render()
        {
            return base.Render();
        }
        #endregion

        #region Properties
        public MapItemTypes MapItemType
        {
            get { return this.m_types; }
            set { this.m_types = value; }
        }
        #endregion
    }

    internal class HomeEagle : GameObject
    {
        #region Variables
        private bool m_death = false;
        #endregion

        #region Constructor
        public HomeEagle(DDGameGraphics graphic, DDGameSurface surface, PointF location)
            : base(graphic["HomeEagle"], surface, location)
        {
        }
        #endregion

        #region Override Methods
        protected override void SetFrame(float ticksPerSecond)
        {
            this.m_currentFrame = (this.m_death) ? 1 : 0;
        }
        #endregion

        #region Properties
        public bool Dead
        {
            get { return this.m_death; }
            set { this.m_death = value; }
        }
        #endregion
    }
    #endregion

    #region Bonus
    internal enum BonusTypes : short
    {
        star = 0,
        spade = 1,
        grenade = 2,
        clock = 3,
        lifeAdd = 4,
        helmet = 5,
        rocket = 6
    }

    internal class BonusItem : GameObject
    {
        #region Variables
        #endregion

        #region Constructor
        public BonusItem(DDGameGraphics graphic, DDGameSurface surface, PointF location)
            : base(graphic["Bonus"], surface, location)
        {
        }
        #endregion

        #region Override Methods
        protected override void SetFrame(float ticksPerSecond)
        {
            base.SetFrame(ticksPerSecond);
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        #endregion

        #region Properties
        #endregion
    }
    #endregion

    #region Effect
    public enum EffectTypes : short
    {
        portal,
        armor,
        explosive
    }

    internal class Effect : GameObject
    {
        #region Variables
        private EffectTypes m_types = EffectTypes.portal;
        #endregion

        #region Constructor
        public Effect(DDGameGraphics graphic, DDGameSurface surface, PointF location)
            : base(graphic["Effect"], surface, location)
        {
            this.m_framesPerSecond = 12F;
        }
        #endregion

        #region Override Methods
        protected override void SetFrame(float ticksPerSecond)
        {
            this.m_currentFrame += ticksPerSecond * this.FramesPerSecond;
            float startFrame = 0f;
            float frameCount = 0f;

            switch (this.m_types)
            {
                case EffectTypes.armor:
                    startFrame = 3f;
                    frameCount = 2f;
                    break;

                case EffectTypes.explosive:
                    startFrame = 5f;
                    frameCount = 2f;
                    break;

                default:
                    startFrame = 0f;
                    frameCount = 3f;
                    break;
            }

            this.m_currentFrame += startFrame;
            if (this.m_currentFrame >= (frameCount + startFrame))
            {
                this.m_currentFrame = startFrame;
            }
        }
        #endregion

        #region Public Methods
        #endregion

        #region Properties
        public EffectTypes EffectType
        {
            get { return this.m_types; }
            set { this.m_types = value; }
        }
        #endregion
    }

    internal class EffectBigExplosive : GameObject
    {
        #region Variables
        #endregion

        #region Constructor
        public EffectBigExplosive(DDGameGraphics graphic, DDGameSurface surface, PointF location)
            : base(graphic["BigExplosive"], surface, location)
        {
            this.m_framesPerSecond = 12F;
        }
        #endregion

        #region Override Methods
        protected override void SetFrame(float ticksPerSecond)
        {
            this.m_currentFrame += ticksPerSecond * this.FramesPerSecond;

            if (this.m_currentFrame >= this.m_graphic.FramesPerCol)
            {
                this.m_currentFrame = 0;
            }
        }
        #endregion
    }
    #endregion
}
