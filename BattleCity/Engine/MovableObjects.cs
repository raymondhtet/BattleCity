using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using System.Threading;

namespace BattleCity.Engine
{
    #region Player
    internal class Player : gObject
    {
        #region Constance
        private const int c_armorLife = 800;
        private const int c_portalLife = 100;
        #endregion

        #region Variables
        private Bitmap[] m_bmp = null;
        private Canvas m_canvas = null;
        private bool m_secondPlayer = false;
        private int m_portal = c_portalLife;
        private Effect m_effect = null;
        private int m_level = 0;
        private bool m_alt = false;
        private int m_armor = c_armorLife;

        private Bullet[] m_bullets = null;
        private int m_bulletLoad = 0;
        private bool m_pause = false;

        private Thread m_processThread = null;
        #endregion

        #region Constructor
        public Player()
            : base(32, 32)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(gPaintEventArgs e)
        {
            if (this.m_bmp != null)
            {
                int index = 0;
                switch (this.m_face)
                {
                    case FaceDirection.Down:
                        index = 2;
                        break;
                    case FaceDirection.Left:
                        index = 4;
                        break;
                    case FaceDirection.Right:
                        index = 6;
                        break;
                    default:
                        break;
                }

                if (this.m_alt)
                {
                    index++;
                }

                try
                {
                    this.m_image = this.m_bmp[index];
                    this.m_alt = !this.m_alt;
                }
                catch
                {
                }
            }

            base.OnPaint(e);
        }

        protected override void OnLifeChange(gLifeChangeEventArgs e)
        {
            base.OnLifeChange(e);

            //if (this.m_level != (e.Value - 1))
            //{
            //    this.Level = e.Value - 1;
            //    return;
            //}

            if (e.Value == 0)
            {
                if (this.m_effect == null)
                {
                    this.m_effect = new Effect();
                    this.m_effect.End += new EventHandler(this.effect_End);
                }

                this.m_effect.Start(this, EffectTypes.explosive, 20, false);
            }

            if ((e.Value < 0) && (this.m_canvas != null))
            {
                this.m_canvas.OnPlayerDead(this);
            }

            if (e.Value < 1)
            {
                if ((this.m_processThread != null) && ((this.m_processThread.ThreadState & ThreadState.Running) == ThreadState.Running))
                {
                    this.m_processThread.Abort();
                    this.m_processThread.Join();
                }
            }
        }

        protected override bool OnMove(gMoveEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.m_canvas.MapItemsIce.Count; i++)
                {
                    MapItem item = this.m_canvas.MapItemsIce[i];
                    if ((item != null) && (item.Rectangle.Contains(this.m_x + (this.m_width / 2), this.m_y + (this.m_height / 2))))
                    {
                        e.Value += 2;
                    }
                }

                for (int i = 0; i < this.m_canvas.BonusItems.Count; i++)
                {
                    BonusItem item = this.m_canvas.BonusItems[i];
                    if ((item != null) && (item.Life > 0))
                    {
                        if ((item.Rectangle.Contains(this.m_x, this.m_y))
                            || (item.Rectangle.Contains(this.m_x, this.m_y + this.m_height))
                            || (item.Rectangle.Contains(this.m_x + this.m_width, this.m_y))
                            || (item.Rectangle.Contains(this.m_x + this.m_width, this.m_y + this.m_height))
                            )
                        {
                            this.m_canvas.HitBonus(this, item);
                            break;
                        }
                    }
                }
            }
            catch
            {
            }

            if (this.m_effect.Type == EffectTypes.armor)
            {
                this.m_effect.SetLocation(this);
            }

            return base.OnMove(e);
        }

        protected override void OnPause(gPauseEventArgs e)
        {
            base.OnPause(e);

            this.m_pause = e.Value;

            if (e.Value)
            {
                if ((this.m_processThread != null) && ((this.m_processThread.ThreadState & ThreadState.Running) == ThreadState.Running))
                {
                    this.m_processThread.Abort();
                    this.m_processThread.Join();
                    this.m_processThread = null;
                }
            }
            else
            {
                this.m_processThread = new Thread(new ThreadStart(this.DoProcess));
                this.m_processThread.Start();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            TRACE("Player.Dispose");

            this.ObjectDispose(this.m_effect);

            if (this.m_bullets != null)
            {
                this.ObjectDispose(this.m_bullets);
            }

            if (this.m_bmp != null)
            {
                this.ObjectDispose(this.m_bmp);
            }

            if (this.m_processThread != null)
            {
                if ((this.m_processThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                {
                    this.m_processThread.Abort();
                    this.m_processThread.Join();
                }
                this.m_processThread = null;
            }
        }

        protected override bool CanMove(int x, int y)
        {
            if (this.m_pause) return false;

            if ((this.m_portal < 0) && (this.m_canvas != null))
            {
                bool canMove = true;
                try
                {
                    this.HitCanvas(this.m_canvas.Rectangle, x, y, ref canMove);

                    if ((this.m_canvas.Home != null) && (this.m_canvas.Home.Life > 0))
                    {
                        this.HitTest(this.m_canvas.Home.Rectangle, x, y, ref canMove);
                    }

                    for (int i = 0; i < this.m_canvas.MapItemsHomeCover.Length; i++)
                    {
                        MapItem item = this.m_canvas.MapItemsHomeCover[i];
                        if ((item != null) && (item.Life > 0))
                        {
                            this.HitTest(item.Rectangle, x, y, ref canMove);
                        }
                    }

                    for (int i = 0; i < this.m_canvas.MapItemsBrick.Count; i++)
                    {
                        MapItem item = this.m_canvas.MapItemsBrick[i];
                        if ((item != null) && (item.Life > 0))
                        {
                            this.HitTest(item.Rectangle, x, y, ref canMove);
                        }
                    }

                    for (int i = 0; i < this.m_canvas.MapItemsFerum.Count; i++)
                    {
                        MapItem item = this.m_canvas.MapItemsFerum[i];
                        if ((item != null) && (item.Life > 0))
                        {
                            this.HitTest(item.Rectangle, x, y, ref canMove);
                        }
                    }

                    for (int i = 0; i < this.m_canvas.MapItemsWater.Count; i++)
                    {
                        MapItem item = this.m_canvas.MapItemsFerum[i];
                        if ((item != null) && (item.Life > 0))
                        {
                            this.HitTest(item.Rectangle, x, y, ref canMove);
                        }
                    }
                }
                catch
                {
                    return false;
                }

                return (canMove && base.CanMove(x, y));
            }
            return false;
        }
        #endregion

        #region Private Methods
        private void Initialize(bool secondPlayer, int level)
        {
            if ((level < 0) || (level > 3)) return;

            this.m_level = level;

            int cursorX = 0;
            int cursorY = 104;

            switch (level)
            {
                case 1:
                    cursorX = 256;
                    cursorY = 104;
                    break;
                case 2:
                    cursorX = 0;
                    cursorY = 138;
                    break;
                case 3:
                    cursorX = 256;
                    cursorY = 138;
                    break;
            }

            if (secondPlayer)
            {
                cursorY += 68;
            }

            this.m_bmp = new Bitmap[8];
            for (int i = 0; i < 8; i++)
            {
                this.m_bmp[i] = this.GetImage(32, 32, cursorX, cursorY, 32, 32);
                cursorX += 32;
            }
        }

        private void DoProcess()
        {
            try
            {
                do
                {
                    if (this.m_portal > 0)
                        this.m_portal--;
                    else if (this.m_portal == 0)
                    {
                        this.Life = this.m_level + 1;
                        this.m_effect.Stop();
                        this.SetArmor();
                        this.m_portal--;
                    }
                    else
                    {
                        if (this.m_armor > 0)
                            this.m_armor--;
                        else
                            this.m_effect.Stop();

                        if (this.m_bulletLoad > 0)
                            this.m_bulletLoad--;
                    }

                    Thread.Sleep(20);
                } while (this.m_processThread.IsAlive);
            }
            catch
            {
            }
        }

        private void effect_End(object sender, EventArgs e)
        {
            if (((Effect)sender).Type == EffectTypes.explosive)
            {
                this.Life = -1;
            }
        }
        #endregion

        #region Public Methods
        public void Start(Canvas canvas, bool secondPlayer, int level)
        {
            this.Dispose();

            this.m_canvas = canvas;
            this.SetOwner(canvas);

            this.m_secondPlayer = secondPlayer;

            this.Initialize(secondPlayer, level);

            if (secondPlayer)
            {
                this.m_x = canvas.X + 256;
                this.m_y = canvas.Y + 384;
            }
            else
            {
                this.m_x = canvas.X + 128;
                this.m_y = canvas.Y + 384;
            }

            TRACE("Player Started!");
            this.m_face = FaceDirection.Up;
            this.m_life = 0;
            this.m_pause = false;
            this.m_portal = c_portalLife;
            this.m_armor = c_armorLife;
            this.m_bulletLoad = 0;

            this.m_effect = new Effect();
            this.m_effect.End += new EventHandler(this.effect_End);
            this.m_effect.Start(this, EffectTypes.portal, 5, true);

            this.m_processThread = new Thread(new ThreadStart(this.DoProcess));
            this.m_processThread.Start();
        }

        public void Fire()
        {
            if (this.m_pause) return;

            if (this.m_level > 0)
            {
                try
                {
                    if (this.m_bulletLoad < 1)
                    {
                        int index = -1;
                        if (this.m_bullets == null)
                        {
                            this.m_bullets = new Bullet[5];
                            index = 0;
                        }
                        else
                        {
                            for (int i = 0; i < this.m_bullets.Length; i++)
                            {
                                if ((this.m_bullets[i] != null) && (this.m_bullets[i].Life < 0))
                                {
                                    this.ObjectDispose(this.m_bullets[i]);
                                    index = i;
                                    break;
                                }

                                if (this.m_bullets[i] == null)
                                {
                                    index = i;
                                    break;
                                }
                            }
                        }

                        if (index > -1)
                        {
                            this.m_bullets[index] = new Bullet(this.m_canvas, this, this.m_level);
                            this.m_bulletLoad = 20;
                        }
                    }
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    if (this.m_bullets == null)
                    {
                        this.m_bullets = new Bullet[1];
                        this.m_bullets[0] = new Bullet(this.m_canvas, this, this.m_level);
                    }
                    else if (this.m_bullets[0] == null)
                    {
                        this.m_bullets[0] = new Bullet(this.m_canvas, this, this.m_level);
                    }
                    else if (this.m_bullets[0].Life < 0)
                    {
                        this.ObjectDispose(this.m_bullets[0]);
                        this.m_bullets[0] = new Bullet(this.m_canvas, this, this.m_level);
                    }
                }
                catch
                {
                }
            }
        }

        public void SetArmor()
        {
            this.m_armor = c_armorLife;
            if (this.m_effect == null)
            {
                this.m_effect = new Effect();
            }
            this.m_effect.Start(this, EffectTypes.armor, 5, true);
        }
        #endregion

        #region Properties
        public bool IsSecondPlayer
        {
            get { return this.m_secondPlayer; }
        }

        public int Level
        {
            get { return this.m_level; }
            set
            {
                if (this.m_level != value) 
                {
                    this.Initialize(this.m_secondPlayer, value);
                }
            }
        }

        public bool HasArmor
        {
            get { return (this.m_armor > 0); }
        }

        public bool IsPortal
        {
            get { return (this.m_portal > 0); }
        }
        #endregion
    }
    #endregion

    #region Enemy
    internal class Enemies : gObject
    {
        #region Constance
        private const int c_portalLife = 100;
        #endregion

        #region Variables
        private Canvas m_canvas = null;
        private int m_level = 0;
        private int m_type = 0;
        private bool m_withBonus = false;
        private Bitmap[] m_bmp = null;
        private Bitmap[] m_bmpBonus = null;
        private int m_portal = c_portalLife;
        private Effect m_effect = null;
        private bool m_alt = false;

        private System.Random m_random = null;
        private Thread m_processThread = null;

        private Bullet m_bullet = null;
        private int m_delay = 50;
        private int m_bulletDelay = 20;
        private int m_bonusAlt = 0;
        #endregion

        #region Constructor
        public Enemies(Canvas canvas, int level, int type, bool withBonus, int spwanLoc)
            : base(canvas, 32, 32)
        {
            this.m_canvas = canvas;
            this.m_life = 0;
            this.Initialize(level, type, withBonus);

            this.m_x = canvas.X;
            this.m_y = canvas.Y;
            if (spwanLoc == 1)
                this.m_x += 192;
            else if (spwanLoc == 2)
                this.m_x += 384;

            this.m_face = FaceDirection.Down;

            this.m_random = new Random();

            this.m_effect = new Effect();
            this.m_effect.End += new EventHandler(this.effect_End);
            this.m_effect.Start(this, EffectTypes.portal, 5, true);

            this.m_processThread = new Thread(new ThreadStart(this.DoProcess));
            this.m_processThread.Start();
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(gPaintEventArgs e)
        {
            int index = 0;
            switch (this.m_face)
            {
                case FaceDirection.Down:
                    index = 2;
                    break;
                case FaceDirection.Left:
                    index = 4;
                    break;
                case FaceDirection.Right:
                    index = 6;
                    break;
                default:
                    break;
            }

            if (this.m_alt)
            {
                index++;
            }

            try
            {
                if ((this.m_withBonus) && (this.m_bonusAlt > 1))
                {
                    if (this.m_bmpBonus != null) this.m_image = this.m_bmpBonus[index];
                }
                else
                {
                    if (this.m_bmp != null) this.m_image = this.m_bmp[index];
                }

                this.m_alt = !this.m_alt;

                this.m_bonusAlt++;
                if (this.m_bonusAlt > 3) this.m_bonusAlt = 0;
            }
            catch (Exception ex)
            {
                TRACE(ex.Message);
            }

            base.OnPaint(e);
        }

        protected override bool OnMove(gMoveEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.m_canvas.MapItemsIce.Count; i++)
                {
                    MapItem item = this.m_canvas.MapItemsIce[i];
                    if ((item != null) && (item.Rectangle.Contains(this.m_x + (this.m_width / 2), this.m_y + (this.m_height / 2))))
                    {
                        e.Value += 2;
                    }
                }
            }
            catch
            {
            }

            return base.OnMove(e);
        }

        protected override void OnPause(gPauseEventArgs e)
        {
            base.OnPause(e);

            if (e.Value)
            {
                if ((this.m_processThread != null) && ((this.m_processThread.ThreadState & ThreadState.Running) == ThreadState.Running))
                {
                    this.m_processThread.Abort();
                    this.m_processThread.Join();
                    this.m_processThread = null;
                }
            }
            else
            {
                this.m_processThread = new Thread(new ThreadStart(this.DoProcess));
                this.m_processThread.Start();
            }
        }

        protected override void OnLifeChange(gLifeChangeEventArgs e)
        {
            base.OnLifeChange(e);

            if (e.Value == 0)
            {
                if (this.m_effect == null)
                {
                    this.m_effect = new Effect();
                    this.m_effect.End += new EventHandler(this.effect_End);
                }

                this.m_effect.Start(this, EffectTypes.explosive, 20, false);
            }
            else if ((e.Value < 0) && (this.m_canvas != null))
            {
                this.m_canvas.OnEnemyDead(this);
            }
        }

        protected override bool CanMove(int x, int y)
        {
            if ((this.m_portal < 0) && (this.m_canvas != null))
            {
                bool canMove = true;

                try
                {
                    this.HitCanvas(this.m_canvas.Rectangle, x, y, ref canMove);

                    if ((this.m_canvas.Home != null) && (this.m_canvas.Home.Life > 0))
                    {
                        this.HitTest(this.m_canvas.Home.Rectangle, x, y, ref canMove);
                    }

                    for (int i = 0; i < this.m_canvas.MapItemsHomeCover.Length; i++)
                    {
                        MapItem item = this.m_canvas.MapItemsHomeCover[i];
                        if ((item != null) && (item.Life > 0))
                        {
                            this.HitTest(item.Rectangle, x, y, ref canMove);
                        }
                    }

                    for (int i = 0; i < this.m_canvas.MapItemsBrick.Count; i++)
                    {
                        MapItem item = this.m_canvas.MapItemsBrick[i];
                        if ((item != null) && (item.Life > 0))
                        {
                            this.HitTest(item.Rectangle, x, y, ref canMove);
                        }
                    }

                    for (int i = 0; i < this.m_canvas.MapItemsFerum.Count; i++)
                    {
                        MapItem item = this.m_canvas.MapItemsFerum[i];
                        if ((item != null) && (item.Life > 0))
                        {
                            this.HitTest(item.Rectangle, x, y, ref canMove);
                        }
                    }

                    for (int i = 0; i < this.m_canvas.MapItemsWater.Count; i++)
                    {
                        MapItem item = this.m_canvas.MapItemsFerum[i];
                        if ((item != null) && (item.Life > 0))
                        {
                            this.HitTest(item.Rectangle, x, y, ref canMove);
                        }
                    }

                    if ((this.m_canvas.Player1 != null) && (this.m_canvas.Player1.Life > 0))
                    {
                        this.HitTest(this.m_canvas.Player1.Rectangle, x, y, ref canMove);
                    }
                }
                catch
                {
                    return false;
                }

                return (canMove && base.CanMove(x, y));
            }

            return false;
        }

        public override void Dispose()
        {
            base.Dispose();

            TRACE("Enemies.Dispose");

            if (this.m_random != null)
            {
                this.m_random = null;
            }

            this.ObjectDispose(this.m_effect);
            this.ObjectDispose(this.m_bmp);
            this.ObjectDispose(this.m_bmpBonus);
            this.ObjectDispose(this.m_bullet);

            if (this.m_processThread != null)
            {
                if ((this.m_processThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                {
                    this.m_processThread.Abort();
                    this.m_processThread.Join();
                }
                this.m_processThread = null;
            }
        }
        #endregion

        #region Private Methods
        private void Initialize(int level, int type, bool withBonus)
        {
            this.m_level = level;
            this.m_type = type;
            this.m_withBonus = withBonus;

            if (level > 3) this.m_level = 0;

            int cursorX = 0;
            int cursorY = 240;

            switch (level)
            {
                case 1:
                    cursorX = 256;
                    cursorY = 240;
                    break;
                case 2:
                    cursorX = 0;
                    cursorY = 274;
                    break;
                case 3:
                    cursorX = 256;
                    cursorY = 274;
                    break;
            }

            switch (type)
            {
                case 1:
                    cursorY += 136;
                    break;
                case 2:
                    cursorY += 204;
                    break;
            }

            this.m_bmp = new Bitmap[8];
            for (int i = 0; i < 8; i++)
            {
                this.m_bmp[i] = this.GetImage(32, 32, cursorX, cursorY, 32, 32);
                cursorX += 32;
            }

            if (withBonus)
            {
                cursorX = 0;
                cursorY = 308;

                switch (level)
                {
                    case 1:
                        cursorX = 256;
                        cursorY = 308;
                        break;
                    case 2:
                        cursorX = 0;
                        cursorY = 342;
                        break;
                    case 3:
                        cursorX = 256;
                        cursorY = 342;
                        break;
                }

                this.m_bmpBonus = new Bitmap[8];
                for (int i = 0; i < 8; i++)
                {
                    this.m_bmpBonus[i] = this.GetImage(32, 32, cursorX, cursorY, 32, 32);
                    cursorX += 32;
                }
            }
        }

        private void DoProcess()
        {
            try
            {
                do
                {
                    if (this.m_portal > 0)
                    {
                        this.m_portal--;
                    }
                    else if (this.m_portal == 0)
                    {
                        this.Life = this.m_type + 1;
                        this.m_effect.Stop();
                        this.m_portal--;
                    }
                    else
                    {
                        this.ChangeDirection();

                        if (this.m_delay > -2)
                            this.m_delay -= 2;

                        if (this.m_bulletDelay > 0)
                            this.m_bulletDelay--;

                        this.DoMove(2);
                        this.Fire();

                        if (this.m_level != 1)
                            Thread.Sleep(30);
                    }

                    Thread.Sleep(20);
                } while (this.m_processThread.IsAlive);
            }
            catch
            {
            }
        }

        private void ChangeDirection()
        {
            if ((this.m_delay < 0) && (this.m_random != null))
            {
                FaceDirection newFace = this.m_face;
                int rFace = this.m_random.Next(10);

                if (rFace < 2)
                {
                    newFace = this.m_face = FaceDirection.Down; // 2
                }
                else if (rFace < 4)
                {
                    if (this.m_x > (this.m_canvas.X + ((this.m_canvas.Width - this.m_width) / 2))) // half of canvas [104]
                    {
                        newFace = FaceDirection.Left; // 3
                    }
                    else
                    {
                        newFace = FaceDirection.Right; // 4
                    } // end else if
                }
                else
                {
                    newFace = (FaceDirection)this.m_random.Next(4);
                } // end else if

                this.m_face = newFace;
                this.m_delay = this.m_random.Next(50) + 10;
            }
        }

        private void Fire()
        {
            if (this.m_bulletDelay == 0)
            {
                if ((this.m_bullet != null) && (this.m_bullet.Life < 0))
                {
                    this.m_bullet.Dispose();
                    this.m_bullet = null;
                }

                if (this.m_bullet == null)
                {
                    this.m_bullet = new Bullet(this.m_canvas, this, 1);
                }

                this.m_bulletDelay = 20;
            }
        }

        private void effect_End(object sender, EventArgs e)
        {
            if (((Effect)sender).Type == EffectTypes.explosive)
            {
                this.Life = -1;
            }
        }
        #endregion

        #region Properties
        public Bullet Bullet
        {
            get { return this.m_bullet; }
        }

        public bool IsPortal
        {
            get { return (this.m_portal > 0); }
        }

        public int Type
        {
            get { return this.m_type; }
        }

        public bool WithBonus
        {
            get { return this.m_withBonus; }
            set { this.m_withBonus = value; }
        }
        #endregion
    }
    #endregion

    #region Bullet
    internal class Bullet : gObject
    {
        #region Variables
        private Canvas m_canvas = null;
        private gObject m_owner = null;

        private Effect m_effect = null;
        private int m_level = 0;
        private Thread m_processThread = null;

        private Bullet m_secondBullet = null;
        private int m_secondBulletLoad = 10;
        #endregion

        #region Constructor
        public Bullet(Canvas canvas, gObject owner, int level)
            : base(canvas, 16, 16)
        {
            this.m_canvas = canvas;
            this.m_owner = owner;
            this.m_level = level;

            if (owner != null)
            {
                this.m_face = owner.Face;

                int cursorX = 0;
                switch (owner.Face)
                {
                    case FaceDirection.Up:
                        {
                            this.m_x = (owner.X + (owner.Width / 2)) - 8;
                            this.m_y = owner.Y - 4;
                            cursorX = 112;
                        }
                        break;
                    case FaceDirection.Down:
                        {
                            this.m_x = (owner.X + (owner.Width / 2)) - 8;
                            this.m_y = (owner.Y + owner.Height) - 4;
                            cursorX = 128;
                        }
                        break;
                    case FaceDirection.Left:
                        {
                            this.m_x = owner.X - 4;
                            this.m_y = (owner.Y + (owner.Height / 2)) - 8;
                            cursorX = 144;
                        }
                        break;
                    case FaceDirection.Right:
                        {
                            this.m_x = (owner.X + owner.Width) - 4;
                            this.m_y = (owner.Y + (owner.Height / 2)) - 8;
                            cursorX = 160;
                        }
                        break;
                }

                this.LoadImage(cursorX, 18, 16, 16);
                this.m_effect = new Effect();
                this.m_effect.End += new EventHandler(this.effect_End);

                this.m_processThread = new Thread(new ThreadStart(this.DoProcess));
                this.m_processThread.Start();
            }
        }
        #endregion

        #region Override Methods
        protected override void OnPause(gPauseEventArgs e)
        {
            base.OnPause(e);

            if (e.Value)
            {
                if ((this.m_processThread != null) && ((this.m_processThread.ThreadState & ThreadState.Running) == ThreadState.Running))
                {
                    this.m_processThread.Abort();
                    this.m_processThread.Join();
                    this.m_processThread = null;
                }
            }
            else
            {
                this.m_processThread = new Thread(new ThreadStart(this.DoProcess));
                this.m_processThread.Start();
            }
        }

        protected override bool OnMove(gMoveEventArgs e)
        {
            if (base.OnMove(e))
            {
                return true;
            }
            else
            {
                this.Life = 0;
                return false;
            }
        }

        protected override void OnLifeChange(gLifeChangeEventArgs e)
        {
            base.OnLifeChange(e);

            if (e.Value == 0)
            {
                if (this.m_effect == null)
                {
                    this.m_effect = new Effect();
                    this.m_effect.End += new EventHandler(this.effect_End);
                }

                this.m_effect.Start(this, EffectTypes.explosive, 5, false);
            }

            if (e.Value < 1)
            {
                if ((this.m_processThread != null) && ((this.m_processThread.ThreadState & ThreadState.Running) == ThreadState.Running))
                {
                    this.m_processThread.Abort();
                    this.m_processThread.Join();
                }
            }
        }

        protected override bool CanMove(int x, int y)
        {
            if (this.m_canvas != null)
            {
                bool canMove = true;
                try
                {
                    bool hit = false;

                    this.HitCanvas(this.m_canvas.Rectangle, x, y, ref canMove);

                    if ((this.m_canvas.Home != null) && (this.m_canvas.Home.Life > 1))
                    {
                        hit = this.HitTest(this.m_canvas.Home.Rectangle, x, y, ref canMove);
                        if (hit)
                        {
                            this.m_canvas.Home.Life = 1;
                        }
                    }

                    for (int i = 0; i < this.m_canvas.MapItemsHomeCover.Length; i++)
                    {
                        MapItem item = this.m_canvas.MapItemsHomeCover[i];
                        if ((item != null) && (item.Life > 0))
                        {
                            hit = this.HitTest(item.Rectangle, x, y, ref canMove);
                            if (hit)
                            {
                                if (item.Type == MapItemTypes.brick)
                                {
                                    this.m_canvas.MapItemsHomeCover[i].Face = this.m_face;
                                    if (this.m_level < 2)
                                    {
                                        this.m_canvas.MapItemsHomeCover[i].Life = item.Life - 1;
                                    }
                                    else
                                    {
                                        this.m_canvas.MapItemsHomeCover[i].Life = 0;
                                    }
                                }
                                else if ((item.Type == MapItemTypes.ferum) && (this.m_level == 3))
                                {
                                    this.m_canvas.MapItemsHomeCover[i].Face = this.m_face;
                                    this.m_canvas.MapItemsHomeCover[i].Life = 0;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < this.m_canvas.MapItemsBrick.Count; i++)
                    {
                        MapItem item = this.m_canvas.MapItemsBrick[i];
                        if ((item != null) && (item.Life > 0))
                        {
                            hit = this.HitTest(item.Rectangle, x, y, ref canMove);
                            if (hit)
                            {
                                this.m_canvas.MapItemsBrick[i].Face = this.m_face;
                                if (this.m_level < 2)
                                {
                                    this.m_canvas.MapItemsBrick[i].Life = item.Life - 1;
                                }
                                else
                                {
                                    this.m_canvas.MapItemsBrick[i].Life = 0;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < this.m_canvas.MapItemsFerum.Count; i++)
                    {
                        MapItem item = this.m_canvas.MapItemsFerum[i];
                        if ((item != null) && (item.Life > 0))
                        {
                            hit = this.HitTest(item.Rectangle, x, y, ref canMove);
                            if ((hit) && (this.m_level == 3))
                            {
                                this.m_canvas.MapItemsFerum[i].Face = this.m_face;
                                this.m_canvas.MapItemsFerum[i].Life = 0;
                            }
                        }
                    }

                    if (this.m_owner is Player)
                    {
                        for (int e = 0; e < this.m_canvas.Enemy.Length; e++)
                        {
                            if ((this.m_canvas.Enemy[e] != null) && (!this.m_canvas.Enemy[e].IsPortal) && (this.m_canvas.Enemy[e].Life > 0))
                            {
                                if ((this.m_canvas.Enemy[e].Bullet != null) && (this.m_canvas.Enemy[e].Bullet.Life > 1))
                                {
                                    hit = this.HitTest(this.m_canvas.Enemy[e].Bullet.Rectangle, x, y, ref canMove);
                                    if (hit)
                                    {
                                        this.m_canvas.Enemy[e].Bullet.Life = -1;
                                        this.Life = -1;
                                        break;
                                    }
                                }

                                hit = this.HitTest(this.m_canvas.Enemy[e].Rectangle, x, y, ref canMove);
                                if (hit)
                                {
                                    this.m_canvas.Enemy[e].Life -= 1;
                                    this.Life = -1;

                                    if (this.m_secondBullet != null)
                                    {
                                        this.m_secondBullet.Dispose();
                                        this.m_secondBullet = null;
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    else if (this.m_owner is Enemies)
                    {
                        if ((this.m_canvas.Player1 != null) && (!this.m_canvas.Player1.IsPortal) && (this.m_canvas.Player1.Life > 0))
                        {
                            hit = this.HitTest(this.m_canvas.Player1.Rectangle, x, y, ref canMove);
                            if (hit)
                            {
                                TRACE("Hit on Player! Life : " + this.m_canvas.Player1.Life.ToString());

                                if (!this.m_canvas.Player1.HasArmor)
                                {
                                    this.m_canvas.Player1.Life = this.m_canvas.Player1.Life - 1;
                                }

                                this.Life = -1;
                            }
                        }
                    }

                    canMove = (canMove && base.CanMove(x, y));
                }
                catch
                {
                    return false;
                }

                return canMove;
            }

            return false;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.ObjectDispose(this.m_effect);

            if (this.m_processThread != null)
            {
                if ((this.m_processThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                {
                    this.m_processThread.Abort();
                    this.m_processThread.Join();
                }
                this.m_processThread = null;
            }
        }
        #endregion

        #region Private Methods
        private void DoProcess()
        {
            try
            {
                do
                {
                    this.DoMove(2);

                    if ((this.m_level == 3) && (this.m_secondBullet == null))
                    {
                        if (this.m_secondBulletLoad > 0)
                        {
                            this.m_secondBulletLoad--;
                        }
                        else
                        {
                            this.m_secondBullet = new Bullet(this.m_canvas, this.m_owner, 2);
                        }
                    }

                    Thread.Sleep(5);
                } while (this.m_processThread.IsAlive);
            }
            catch
            {
            }
        }

        private void effect_End(object sender, EventArgs e)
        {
            this.Life = -1;
        }
        #endregion

        #region Properties
        public int Level
        {
            get { return this.m_level; }
            set { this.m_level = value; }
        }
        #endregion
    }
    #endregion
}
