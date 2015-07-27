using System;
using System.Collections.Generic;
using System.Text;

using GameUtil;
using System.Drawing;

namespace BattleCity.Engine
{
    internal enum FaceDirections : int
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }

    internal class GameObject : IGameObject
    {
        #region Variables
        protected float m_framesPerSecond = 12.0F;

        protected DDGraphic m_graphic = null;
        protected DDGameSurface m_surface = null;

        protected PointF m_location = PointF.Empty;

        protected FaceDirections m_direction = FaceDirections.Up;
        protected float m_currentFrame = 0F;

        protected bool m_visible = true;
        #endregion

        #region Constructor
        public GameObject(DDGraphic graphic, DDGameSurface surface, PointF location)
        {
            this.m_graphic = graphic;
            this.m_surface = surface;
            this.m_location = location;
        }
        #endregion

        #region Protected Methods
        protected virtual void SetFrame(float ticksPerSecond)
        {
            this.m_currentFrame += ticksPerSecond * this.m_framesPerSecond;

            // Number of movements is directly related to number of frames in a col.
            if (this.m_currentFrame >= this.m_graphic.FramesPerCol)
            {
                this.m_currentFrame = 0F;
            }
        }
        #endregion

        #region Public Methods
        public virtual void Update(float ticksPerSecond)
        {
            this.SetFrame(ticksPerSecond);
        }

        public virtual bool Render()
        {
            return this.Render(this.m_currentFrame);
        }

        public virtual bool Render(float frame)
        {
            return this.Render((int)frame);
        }

        public virtual bool Render(int frame)
        {
            if (!this.m_visible)
            {
                return false;
            }

            // Don't draw if not within surface bounds.
            if (!this.m_surface.Window.ClientRectangle.IntersectsWith(this.Area))
            {
                return false;
            }

            // Draw this Character.
            this.m_surface.DrawTransparent((int)this.m_location.X, (int)this.m_location.Y, this.m_graphic, this.m_graphic.GetFrameArea(frame));

            return true;
        }
        #endregion

        #region Properties
        public virtual DDGraphic Graphic
        {
            get { return this.m_graphic; }
            set { this.m_graphic = value; }
        }

        public virtual DDGameSurface GameSurface
        {
            get { return this.m_surface; }
        }

        public virtual Point CenterPoint
        {
            get
            {
                // Get center location.
                int x = (int)this.m_location.X - (this.Size.Width / 2);
                int y = (int)this.m_location.Y - (this.Size.Height / 2);

                return new Point(x, y);
            }
        }

        public virtual PointF Location
        {
            get { return this.m_location; }
            set { this.m_location = value; }
        }

        public virtual Size Size
        {
            get { return this.m_graphic.FrameSize; }
        }

        public virtual Rectangle Area
        {
            get { return new Rectangle(new Point((int)this.m_location.X, (int)this.m_location.Y), this.Size); }
        }

        public virtual FaceDirections Direction
        {
            get { return this.m_direction; }
            set { this.m_direction = value; }
        }

        public virtual float FramesPerSecond
        {
            get { return this.m_framesPerSecond; }
            set { this.m_framesPerSecond = value; }
        }

        public virtual float CurrentFrame
        {
            get { return this.m_currentFrame; }
            set { this.m_currentFrame = value; }
        }

        public virtual bool Visible
        {
            get { return this.m_visible; }
            set { this.m_visible = value; }
        }
        #endregion
    }

    internal class StaticObject : GameObject
    {
        #region Variables
        protected int m_defaultFrame = 0;
        #endregion

        #region Constuctor
        public StaticObject(DDGraphic graphic, DDGameSurface surface, PointF location)
            : base(graphic, surface, location)
        {
        }

        public StaticObject(DDGraphic graphic, DDGameSurface surface, PointF location, int defaultFrame)
            : base(graphic, surface, location)
        {
            this.m_defaultFrame = defaultFrame;
        }
        #endregion

        #region Override Methods
        protected override void SetFrame(float ticksPerSecond)
        {
            this.m_currentFrame = (float)this.m_defaultFrame;
        }
        #endregion
    }

    internal class MovableObject : GameObject
    {
        #region Variables
        protected float m_movementPerSecond = 64.0F;

        protected bool m_moving = false;
        protected float m_movementFrame = 0;
        #endregion

        #region Constructor
        public MovableObject(DDGraphic graphic, DDGameSurface surface, PointF location)
            : base(graphic, surface, location)
        {
        }
        #endregion

        #region Protected Methods
        protected virtual void ValidateLocation()
        {
        }

        protected virtual void Move(float ticksPerSecond)
        {
            // Get the amount this Character moved.
            float movementAmount = ticksPerSecond * this.m_movementPerSecond;

            // Move character based on Direction.
            switch (this.m_direction)
            {
                case FaceDirections.Up:
                    this.m_location.Y -= movementAmount;
                    break;

                case FaceDirections.Down:
                    this.m_location.Y += movementAmount;
                    break;

                case FaceDirections.Left:
                    this.m_location.X -= movementAmount;
                    break;

                case FaceDirections.Right:
                    this.m_location.X += movementAmount;
                    break;

                default:
                    // No other directions to handle.
                    break;
            }
        }
        #endregion

        #region Public Methods
        public virtual new void Update(float ticksPerSecond)
        {
            if (this.m_moving)
            {
                this.Move(ticksPerSecond);
                base.Update(ticksPerSecond);
                this.m_moving = false;
            }

            this.ValidateLocation();
        }

        public virtual void Move(FaceDirections direction)
        {
            this.m_direction = direction;
            this.m_moving = true;
        }
        #endregion

        #region Properties
        public virtual bool Moving
        {
            get { return this.m_moving; }
            set { this.m_moving = value; }
        }

        public virtual float MovementFrame
        {
            get { return this.m_movementFrame; }
            set { this.m_movementFrame = value; }
        }

        public virtual float MovementPerSecond
        {
            get { return this.m_movementPerSecond; }
            set { this.m_movementPerSecond = value; }
        }
        #endregion
    }
}
