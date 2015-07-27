using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Collections;

namespace BattleCity.Engine
{
    #region ItemList<T>
    internal class ItemList<T> : List<T>, IDisposable
    {
        #region Constructor
        public ItemList()
            : base()
        {
        }

        public ItemList(int capacity)
            : base(capacity)
        {
        }

        public ItemList(IEnumerable<T> collection)
            : base(collection)
        {
        }
        #endregion

        #region Properties
        public T this[int x, int y]
        {
            get
            {
                int index = x * y;

                if (index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return this[index];
            }
            set
            {
                int index = x * y;

                if (index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                this[index] = value;
            }
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            this.Clear();
        }
        #endregion
    }
    #endregion

    #region gObject
    internal enum FaceDirection : int
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }

    internal class gObject : IDisposable
    {
        public static void TRACE(string message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(message, Assembly.GetCallingAssembly().GetName().Name);
            //System.Diagnostics.Trace.TraceInformation(message);
#endif
        }

        #region Variables
        protected const string c_image = "BattleCity.Pictures.png";

        protected Bitmap m_image = null;
        protected FaceDirection m_face = FaceDirection.Up;

        protected int m_x = 0;
        protected int m_y = 0;
        protected int m_width = 0;
        protected int m_height = 0;

        protected int m_life = 1;
        #endregion

        #region Declartion Events
        public delegate bool gMoveEventHandler(object sender, gMoveEventArgs e);
        public event gMoveEventHandler Move = null;

        public delegate void gPaintEventHandler(object sender, gPaintEventArgs e);
        public event gPaintEventHandler Paint = null;

        public delegate void gPauseEventHandler(object sender, gPauseEventArgs e);
        public event gPauseEventHandler Pause = null;

        public delegate void gDirectionChangeEventHandler(object sender, gDirectionChangeEventArgs e);
        public event gDirectionChangeEventHandler DirectionChange = null;

        public delegate void gLifeChangeEventHandler(object sender, gLifeChangeEventArgs e);
        public event gLifeChangeEventHandler LifeChange = null;
        #endregion

        #region Constructor / Destructor
        public gObject()
        {
        }

        public gObject(int width, int height)
            : this(0, 0, width, height)
        {
        }

        public gObject(int x, int y, int width, int height)
            : this(0, 0, width, height, null)
        {
        }

        public gObject(int x, int y, int width, int height, Bitmap image)
        {
            this.m_x = x;
            this.m_y = y;
            this.m_width = width;
            this.m_height = height;

            this.m_image = image;
        }

        public gObject(gObject owner)
        {
            this.SetOwner(owner);
        }

        public gObject(gObject owner, int width, int height)
            : this(owner, 0, 0, width, height)
        {
        }

        public gObject(gObject owner, int x, int y, int width, int height)
            : this(owner, x, y, width, height, null)
        {
        }

        public gObject(gObject owner, int x, int y, int width, int height, Bitmap image)
            : this(owner)
        {
            this.m_x = x;
            this.m_y = y;
            this.m_width = width;
            this.m_height = height;

            this.m_image = image;
        }

        ~gObject()
        {
            this.Dispose();
        }
        #endregion

        #region Override Methods
        #endregion

        #region Protected Methods
        protected virtual Bitmap GetImage(int destWidth, int destHeight, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            Bitmap image = new Bitmap(destWidth, destHeight);
            try
            {
                using (Stream resStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(c_image))
                {
                    Bitmap resImage = (Bitmap)System.Drawing.Image.FromStream(resStream);
                    if (resImage != null)
                    {
                        Graphics graphics = Graphics.FromImage(image);
                        //graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                        graphics.DrawImage(resImage, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(srcX, srcY, srcWidth, srcHeight), GraphicsUnit.Pixel);
                        GC.Collect();
                    }
                }
            }
            catch
            {
            }

            return image;
        }

        protected virtual void LoadImage(int x, int y, int width, int height)
        {
            this.m_image = this.GetImage(this.m_width, this.m_height, x, y, width, height);
        }

        protected virtual void SetOwner(gObject owner)
        {
            if (owner != null)
            {
                owner.Paint += new gPaintEventHandler(this.owner_Paint);
                owner.Pause += new gPauseEventHandler(this.owner_Pause);
            }
        }

        protected void ZoomPositive(int factor)
        {
            if (factor <= 0) return;
            this.m_height = this.m_height + factor;
            this.m_width = this.m_width + factor;
        }

        protected void ZoomNegative(int factor)
        {
            if (factor <= 0) return;
            this.m_height = this.m_image.Height - factor;
            this.m_width = this.m_image.Width - factor;
        }

        protected void Rotate()
        {
            this.m_image.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        protected virtual bool CanMove(int x, int y)
        {
            return (this.m_life > 0);
        }

        protected virtual void HitCanvas(Rectangle rect, int x, int y, ref bool canMove)
        {
            canMove = (canMove && ((rect.Left - 3) < x));
            canMove = (canMove && ((rect.Top - 3) < y));
            canMove = (canMove && ((rect.Right + 2) > (x + this.m_width)));
            canMove = (canMove && ((rect.Bottom + 2) > (y + this.m_height)));
        }

        protected virtual bool HitTest(Rectangle rect, int x, int y, ref bool canMove)
        {
            bool hit = ((rect.Left + 3 > (x + this.m_width)) || (rect.Right - 3 < x) || (rect.Top + 3 > (y + this.m_height)) || (rect.Bottom - 3 < y));
            canMove = (canMove && hit);
            return !hit;
        }

        protected void ObjectDispose(IDisposable obj)
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }

        protected void ObjectDispose(params IDisposable[] obj)
        {
            if (obj != null)
            {
                try
                {
                    for (int i = 0; i < obj.Length; i++)
                    {
                        if (obj[i] != null)
                        {
                            obj[i].Dispose();
                            obj[i] = null;
                        }
                    }
                }
                catch
                {
                }

                obj = null;
            }
        }

        protected virtual bool OnMove(gMoveEventArgs e)
        {
            int speed = e.Value;
            if (speed > 15)
                speed = 15;
            if (speed < 0)
                speed = 0;

            int x = this.m_x;
            int y = this.m_y;

            switch (this.m_face)
            {
                case FaceDirection.Up:
                    y -= speed;
                    break;
                case FaceDirection.Down:
                    y += speed;
                    break;
                case FaceDirection.Right:
                    x += speed;
                    break;
                case FaceDirection.Left:
                    x -= speed;
                    break;
            }

            if (this.CanMove(x, y))
            {
                this.m_x = x;
                this.m_y = y;

                if (this.Move != null)
                {
                    return this.Move(this, e);
                }

                return true;
            }

            return false;
        }

        protected virtual void OnPaint(gPaintEventArgs e)
        {
            if ((this.m_image != null) && (this.m_life > 0))
            {
                e.Value.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                e.Value.DrawImageUnscaledAndClipped(this.m_image, this.Rectangle);
            }

            if (this.Paint != null)
            {
                this.Paint(this, e);
            }
        }

        protected virtual void OnPause(gPauseEventArgs e)
        {
            if (this.Pause != null)
            {
                this.Pause(this, e);
            }
        }

        protected virtual void OnDirectionChange(gDirectionChangeEventArgs e)
        {
            if (this.DirectionChange != null)
            {
                this.DirectionChange(this, e);
            }
        }

        protected virtual void OnLifeChange(gLifeChangeEventArgs e)
        {
            if (this.LifeChange != null)
            {
                this.LifeChange(this, e);
            }
        }
        #endregion

        #region Private Methods
        private void owner_Paint(object sender, gPaintEventArgs e)
        {
            this.DoPaint(e.Value);
        }
        
        private void owner_Pause(object sender, gPauseEventArgs e)
        {
            this.DoPause(e.Value);
        }
        #endregion

        #region Public Methods
        public virtual void SetLocation(int x, int y)
        {
            this.m_x = x;
            this.m_y = y;
        }

        public virtual bool DoMove(int speed)
        {
            return this.OnMove(new gMoveEventArgs(speed));
        }

        public virtual void DoPaint(Graphics graphics)
        {
            this.OnPaint(new gPaintEventArgs(graphics));
        }

        public virtual void DoPause(bool pause)
        {
            this.OnPause(new gPauseEventArgs(pause));
        }

        public virtual void Dispose()
        {
            this.m_life = -1;
            this.ObjectDispose(this.m_image);
        }
        #endregion

        #region Properties
        public int X
        {
            get { return this.m_x; }
        }
        public int Y
        {
            get { return this.m_y; }
        }
        public int Width
        {
            get { return this.m_width; }
        }
        public int Height
        {
            get { return this.m_height; }
        }

        public Point Location
        {
            get { return new Point(this.m_x, this.m_y); }
        }

        public Size Size
        {
            get { return new Size(this.m_width, this.m_height); }
        }

        public Rectangle Rectangle
        {
            get { return new Rectangle(this.m_x, this.m_y, this.m_width, this.m_height); }
        }

        public Bitmap Image
        {
            get { return this.m_image; }
        }

        public FaceDirection Face
        {
            get { return this.m_face; }
            set
            {
                if (this.m_face != value)
                {
                    this.m_face = value;
                    this.OnDirectionChange(new gDirectionChangeEventArgs(value));
                }
            }
        }

        public int Life
        {
            get { return this.m_life; }
            set
            {
                if (this.m_life != value)
                {
                    this.m_life = value;
                    this.OnLifeChange(new gLifeChangeEventArgs(value));
                }
            }
        }
        #endregion
    }

    #region EventArgs
    internal class gEventArgs<T> : EventArgs
    {
        #region Variables
        private T m_value = default(T);
        #endregion

        #region Constructor
        public gEventArgs(T value)
        {
            this.m_value = value;
        }
        #endregion

        #region Properties
        public T Value
        {
            get { return this.m_value; }
            set { this.m_value = value; }
        }
        #endregion
    }

    internal class gLifeChangeEventArgs : gEventArgs<int>
    {
        #region Constructor
        public gLifeChangeEventArgs(int life)
            : base(life)
        {
        }
        #endregion
    }

    internal class gDirectionChangeEventArgs : gEventArgs<FaceDirection>
    {
        #region Constructor
        public gDirectionChangeEventArgs(FaceDirection face)
            : base(face)
        {
        }
        #endregion
    }

    internal class gPaintEventArgs : gEventArgs<Graphics>
    {
        #region Constructor
        public gPaintEventArgs(Graphics graphics)
            : base(graphics)
        {
        }
        #endregion
    }

    internal class gPauseEventArgs : gEventArgs<bool>
    {
        #region Constructor
        public gPauseEventArgs(bool pause)
            : base(pause)
        {
        }
        #endregion
    }

    internal class gMoveEventArgs : gEventArgs<int>
    {
        #region Constructor
        public gMoveEventArgs(int speed)
            : base(speed)
        {
        }
        #endregion
    }
    #endregion

    #endregion

    #region gFont
    internal class gFont : gObject
    {
        #region Variables
        private const int c_size = 16;
        private const string c_charList = "|!<©ǁ.-0123456789ABCDEFGHILMNOPRSTUVY";
        private const string c_charListBlack = "-0123456789";
        
        private int m_startX = 0;
        private Bitmap[] m_characters = null;
        private bool m_black = false;
        #endregion

        #region Constructor
        public gFont(bool black)
            : base(c_size, c_size)
        {
            this.m_black = black;
            this.Initialize(black);
        }
        #endregion

        #region Override Methods
        public override void Dispose()
        {
            base.Dispose();
            this.ObjectDispose(this.m_characters);
        }
        #endregion

        #region Private Methods
        private void Initialize(bool black)
        {
            if (black)
            {
                int count = c_charListBlack.Length;
                this.m_characters = new Bitmap[count];

                int cursor = 192;
                for (int i = 0; i < count; i++)
                {
                    this.m_characters[i] = this.GetImage(c_size, c_size, cursor, 18, c_size, c_size);
                    cursor += c_size;
                }
            }
            else
            {
                int count = c_charList.Length;
                this.m_characters = new Bitmap[count];

                int cursor = 0;
                for (int i = 0; i < count; i++)
                {
                    this.m_characters[i] = this.GetImage(c_size, c_size, cursor, 0, c_size, c_size);
                    cursor += c_size;
                }
            }
        }
        #endregion

        #region Public Methods
        public void Write(Graphics g, string str, Point pos)
        {
            this.m_x = pos.X;
            this.m_y = pos.Y;

            try
            {
                for (int i = 0; i < str.Length; i++)
                {
                    int index = -1;
                    if (this.m_black)
                    {
                        index = c_charListBlack.IndexOf(str[i]);
                    }
                    else
                    {
                        index = c_charList.IndexOf(str[i]);
                    }

                    if ((index != -1) && (index < this.m_characters.Length))
                    {
                        this.m_image = this.m_characters[index];
                        this.DoPaint(g);
                        this.m_x += c_size;
                    }
                }
            }
            catch
            {
            }
        }

        public void Newline()
        {
            this.m_x = this.m_startX;
            this.m_y += c_size;
        }
        #endregion
    }

    #endregion
}
