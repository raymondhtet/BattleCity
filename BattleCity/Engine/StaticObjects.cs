using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BattleCity.Engine
{
    #region Text
    internal class TitleText : gObject
    {
        #region Constructor
        public TitleText(gObject owner)
            : base(owner, 384, 144)
        {
            this.LoadImage(0, 512, 384, 144);
        }
        #endregion
    }

    internal class GameOverText : gObject
    {
        #region Constructor
        public GameOverText(gObject owner)
            : base(owner, 64, 32)
        {
            this.m_life = 0;
            this.LoadImage(384, 512, 64, 32);
        }
        #endregion

        #region Public Methods
        public void Show(bool show)
        {
            this.m_life = (show) ? 1 : 0;
        }
        #endregion
    }
    #endregion

    #region Icon
    internal class EnemyIcon : gObject
    {
        #region Constructor
        public EnemyIcon(gObject owner)
            : base(owner, 16, 16)
        {
            this.LoadImage(176, 18, 16, 16);
        }
        #endregion
    }

    internal class FlagIcon : gObject
    {
        #region Constructor
        public FlagIcon(gObject owner)
            : base(owner, 32, 32)
        {
            this.LoadImage(160, 70, 32, 32);
        }
        #endregion
    }

    internal class PlayerLifeIcon : gObject
    {
        #region Constructor
        public PlayerLifeIcon(gObject owner, bool secondPlayer)
            : base(owner, 32, 32)
        {
            if (!secondPlayer)
                this.LoadImage(192, 70, 32, 32);
            else
                this.LoadImage(224, 70, 32, 32);
        }
        #endregion
    }

    internal class TotalTankIcon : gObject
    {
        #region Constructor
        public TotalTankIcon(gObject owner, int type)
            : base(owner, 32, 32)
        {
            this.LoadImage(288 + (type * 32), 70, 32, 32);
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

    internal class MapItem : gObject
    {
        #region Variables
        private Bitmap[] m_bmp = null;
        private MapItemTypes m_type = 0;
        private bool m_isHomeCover = false;
        private int m_saveX = 0;
        private int m_saveY = 0;
        private int m_blocked = 0;
        #endregion

        #region Constructor
        public MapItem(gObject owner, MapItemTypes type, bool isHomeCover, int x, int y)
            : base(owner, x, y, 16, 16)
        {
            this.m_saveX = x;
            this.m_saveY = y;
            this.m_isHomeCover = isHomeCover;
            this.Initialize(type, isHomeCover);
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(gPaintEventArgs e)
        {
            if ((this.m_isHomeCover) && (this.m_bmp != null) && (this.m_life > 0))
            {
                if (this.m_blocked < 0)
                {
                    this.m_type = MapItemTypes.ferum;
                    this.m_image = this.m_bmp[1];
                }
                else if (this.m_blocked > 0)
                {
                    if ((this.m_blocked % 16) < 8)
                    {
                        this.m_type = MapItemTypes.brick;
                        this.m_image = this.m_bmp[0];
                    }
                    else
                    {
                        this.m_type = MapItemTypes.ferum;
                        this.m_image = this.m_bmp[1];
                    }
                    this.m_blocked--;
                }
                else
                {
                    this.m_type = MapItemTypes.brick;
                    this.m_image = this.m_bmp[0];
                }
            }
            
            if ((this.m_type == MapItemTypes.water) && (this.m_bmp != null))
            {
                this.m_image = (this.m_life <= 20) ? this.m_bmp[0] : this.m_bmp[1];
                this.m_life++;
                if (this.m_life > 40) this.m_life = 1;
            }
            else if ((this.m_type == MapItemTypes.brick) && (this.m_life == 1))
            {
                switch (this.m_face)
                {
                    case FaceDirection.Up:
                        this.m_height = 8;
                        break;
                    case FaceDirection.Down:
                        if (this.m_height != 8)
                            this.m_y += 8;
                        this.m_height = 8;
                        break;
                    case FaceDirection.Left:
                        this.m_width = 8;
                        break;
                    case FaceDirection.Right:
                        if (this.m_width != 8)
                            this.m_x += 8;
                        this.m_width = 8;
                        break;
                }
            }

            base.OnPaint(e);
        }

        public override void SetLocation(int x, int y)
        {
            base.SetLocation(x, y);

            this.m_saveX = x;
            this.m_saveY = y;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.ObjectDispose(this.m_bmp);
        }
        #endregion

        #region Private Methods
        private void Initialize(MapItemTypes type, bool isHomeCover)
        {
            if (isHomeCover)
            {
                this.m_life = 2;
                this.m_bmp = new Bitmap[2];
                this.m_bmp[0] = this.GetImage(16, 16, 16, 18, 16, 16);
                this.m_bmp[1] = this.GetImage(16, 16, 32, 18, 16, 16);
                this.m_image = this.m_bmp[0];
                this.m_type = MapItemTypes.brick;
            }
            else
            {
                this.m_type = type;

                switch (type)
                {
                    case MapItemTypes.brick:
                    case MapItemTypes.ferum:
                        this.m_life = 2;
                        this.LoadImage((short)type * 16, 18, 16, 16);
                        break;

                    case MapItemTypes.garden:
                        this.LoadImage((short)type * 16, 18, 16, 16);
                        break;

                    case MapItemTypes.water:
                        {
                            this.m_bmp = new Bitmap[2];
                            this.m_bmp[0] = this.GetImage(16, 16, 64, 18, 16, 16);
                            this.m_bmp[1] = this.GetImage(16, 16, 80, 18, 16, 16);
                            this.m_image = this.m_bmp[0];
                        }
                        break;

                    case MapItemTypes.ice:
                        this.LoadImage(((short)type + 1) * 16, 18, 16, 16);
                        break;

                    default:
                        break;
                }
            }
        }
        #endregion

        #region Public Methods
        public void Block(bool value)
        {
            if (!this.m_isHomeCover) return;

            this.m_blocked = (value) ? -1 : 160;
            this.m_width = this.m_height = 16;

            if (value)
            {
                this.m_life = 2;
                this.m_x = this.m_saveX;
                this.m_y = this.m_saveY;
            }
        }
        #endregion

        #region Properties
        public MapItemTypes Type
        {
            get { return this.m_type; }
        }
        #endregion
    }

    internal class HomeEagle : gObject
    {
        #region Variables
        private Bitmap[] m_bmp = null;
        #endregion

        #region Constructor
        public HomeEagle(gObject owner)
            : base(owner, 32, 32)
        {
            this.m_bmp = new Bitmap[2];
            this.m_bmp[0] = this.GetImage(32, 32, 224, 36, 32, 32);
            this.m_bmp[1] = this.GetImage(32, 32, 256, 36, 32, 32);

            this.m_image = this.m_bmp[0];
            this.m_life = 2;
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(gPaintEventArgs e)
        {
            if (this.m_bmp != null)
            {
                try
                {
                    this.m_image = (this.m_life == 2) ? this.m_bmp[0] : this.m_bmp[1];
                    base.OnPaint(e);
                }
                catch
                {
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            this.ObjectDispose(this.m_bmp);
        }
        #endregion
    }
    #endregion

    #region Bonus
    public enum BonusTypes : short
    {
        star = 0,
        spade = 1,
        grenade = 2,
        clock = 3,
        lifeAdd = 4,
        helmet = 5,
        rocket = 6
    }

    internal class BonusItem : gObject
    {
        #region Variables
        private BonusTypes m_type = BonusTypes.star;
        private int m_alt = 0;
        #endregion

        #region Constructor
        public BonusItem(gObject owner, BonusTypes type)
            : base(owner, 32, 32)
        {
            this.m_type = type;
            this.LoadImage((short)type * 32, 36, 32, 32);
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(gPaintEventArgs e)
        {
            if (this.m_alt < 20) base.OnPaint(e);
            this.m_alt++;
            if (this.m_alt == 40) this.m_alt = 0;
        }
        #endregion

        #region Properties
        public BonusTypes Type
        {
            get { return this.m_type; }
        }
        #endregion
    }
    #endregion

    #region Effect
    public enum EffectTypes : short
    {
        portal,
        armor,
        explosive,
        bigExplosive
    }

    internal class Effect : gObject
    {
        #region Variables
        //private GameObject m_owner = null;

        private Bitmap[] m_bmp = null;
        private EffectTypes m_type = EffectTypes.portal;
        private bool m_loop = false;
        private int m_speed = 9;
        private int m_speedCount = 0;

        public event EventHandler End = null;
        #endregion

        #region Constructor
        public Effect()
            : base(32, 32)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(gPaintEventArgs e)
        {
            if ((this.m_bmp != null) && (this.m_life > 0))
            {
                try
                {
                    int index = this.m_bmp.Length - this.m_life;
                    this.m_image = this.m_bmp[index];
                    base.OnPaint(e);
                }
                catch
                {
                }

                this.m_speedCount--;
                if (this.m_speedCount < 1)
                {
                    this.m_speedCount = this.m_speed;
                    this.m_life--;

                    if (this.m_life == 0)
                    {
                        if (this.m_loop)
                        {
                            this.m_life = this.m_bmp.Length;
                        }
                        else
                        {
                            this.Stop();
                        }
                    }
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            try
            {
                this.ObjectDispose(this.m_bmp);
            }
            catch
            {
            }
        }
        #endregion

        #region Private Methods
        private void Initialize(EffectTypes type)
        {
            this.m_type = type;
            if (this.m_bmp != null)
            {
                this.Dispose();
            }

            this.m_width = this.m_height = 32;

            switch (type)
            {
                case EffectTypes.portal:
                    {
                        this.m_life = 3;
                        this.m_bmp = new Bitmap[3];
                        this.m_bmp[0] = this.GetImage(32, 32, 32, 70, 32, 32);
                        this.m_bmp[1] = this.GetImage(32, 32, 64, 70, 32, 32);
                        this.m_bmp[2] = this.GetImage(32, 32, 0, 70, 32, 32);
                    }
                    break;

                case EffectTypes.armor:
                    {
                        this.m_life = 2;
                        this.m_bmp = new Bitmap[2];
                        this.m_bmp[0] = this.GetImage(32, 32, 96, 70, 32, 32);
                        this.m_bmp[1] = this.GetImage(32, 32, 128, 70, 32, 32);
                    }
                    break;

                case EffectTypes.explosive:
                    {
                        this.m_life = 2;
                        this.m_bmp = new Bitmap[2];
                        this.m_bmp[0] = this.GetImage(32, 32, 288, 36, 32, 32);
                        this.m_bmp[1] = this.GetImage(32, 32, 320, 36, 32, 32);
                    }
                    break;

                case EffectTypes.bigExplosive:
                    {
                        this.m_life = 3;
                        this.m_width = this.m_height = 64;

                        this.m_bmp = new Bitmap[3];
                        this.m_bmp[0] = this.GetImage(32, 32, 352, 36, 32, 32);
                        this.m_bmp[1] = this.GetImage(32, 32, 384, 36, 32, 32);
                        this.m_bmp[2] = this.GetImage(64, 64, 416, 36, 64, 64);
                    }
                    break;
            }
        }

        private void OnEnd()
        {
            if (this.End != null)
            {
                this.End(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Public Methods
        public void SetLocation(gObject owner)
        {
            this.m_x = (owner.X + (owner.Width / 2)) - (this.m_width / 2);
            this.m_y = (owner.Y + (owner.Height / 2)) - (this.m_height / 2);
        }

        public void Start(gObject owner, EffectTypes type, int speed, bool loop)
        {
            this.Dispose();

            this.Initialize(type);
            this.m_loop = loop;

            if (speed > 0)
            {
                this.m_speed = speed;
                this.m_speedCount = speed;
            }

            this.SetOwner(owner);

            this.m_x = (owner.X + (owner.Width / 2)) - (this.m_width / 2);
            this.m_y = (owner.Y + (owner.Height / 2)) - (this.m_height / 2);
        }

        public void Stop()
        {
            this.m_loop = false;
            this.Life = 0;

            this.OnEnd();
            this.Dispose();
        }
        #endregion

        #region Properties
        public EffectTypes Type
        {
            get { return this.m_type; }
            set { if (this.m_type != value) this.Initialize(value); }
        }

        public int Speed
        {
            get { return this.m_speed; }
            set 
            {
                if ((value > 0) && (value < 11))
                {
                    this.m_speed = value;
                }
            }
        }
        #endregion
    }
    #endregion
}
