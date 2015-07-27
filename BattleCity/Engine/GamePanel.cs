using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace BattleCity.Engine
{
    #region LoadingPane
    internal class LoadingPane : gObject
    {
        #region Variables
        private gFont m_font = null;
        private TitleText m_titleText = null;
        private int m_tick = 0;
        private string sLoading = "LOADING";
        #endregion

        #region Constructor
        public LoadingPane(int width, int height)
            : base(width, height)
        {
            this.m_life = 0;
            this.m_titleText = new TitleText(this);
            this.m_titleText.Life = 0;
            this.m_titleText.SetLocation((width - this.m_titleText.Width) / 2, 100);

            this.m_font = new gFont(false);
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(gPaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.m_life > 0)
            {
                Point pText = new Point(180, 300);

                if (this.m_tick < 10)
                    this.m_font.Write(e.Value, sLoading, pText);
                else if ((this.m_tick >= 10) && (this.m_tick < 20))
                    this.m_font.Write(e.Value, sLoading + ".", pText);
                else if ((this.m_tick >= 20) && (this.m_tick < 30))
                    this.m_font.Write(e.Value, sLoading + "..", pText);
                else if (this.m_tick > 30)
                    this.m_font.Write(e.Value, sLoading + "...", pText);

                this.m_tick++;
                if (this.m_tick == 40) this.m_tick = 0;
            }
        }
        #endregion

        #region Public Methods
        public void Show(bool show)
        {
            this.m_titleText.Life = (show) ? 1 : 0;
            this.m_life = (show) ? 1 : 0;
        }
        #endregion
    }
    #endregion

    #region InfoPane
    internal class InfoPane : gObject
    {
        #region Variables
        private ItemList<EnemyIcon> m_enemy = null;
        private PlayerLifeIcon m_player1 = null;
        private PlayerLifeIcon m_player2 = null;
        private short m_lifeCount1 = 2;
        private short m_lifeCount2 = 2;
        private string m_state = "";

        private gFont m_font = null;
        private FlagIcon m_flagIcon = null;
        #endregion

        #region Constructor
        public InfoPane(gObject owner)
            : base(owner, 46, 350)
        {
            this.m_enemy = new ItemList<EnemyIcon>();
            for (int i = 0; i < 20; i++)
            {
                this.m_enemy.Add(new EnemyIcon(this));
            }

            this.m_font = new gFont(true);

            this.m_player1 = new PlayerLifeIcon(this, false);
            this.m_flagIcon = new FlagIcon(this);
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(gPaintEventArgs e)
        {
            Graphics graphics = e.Value;

            base.OnPaint(e);

            this.m_font.Write(graphics, this.m_lifeCount1.ToString(), new Point(this.m_x + 24, this.m_y + 216));

            if (this.m_player2 != null)
            {
                this.m_font.Write(graphics, this.m_lifeCount2.ToString(), new Point(this.m_x + 24, this.m_y + 256));
            }

            if (!string.IsNullOrEmpty(this.m_state)) this.m_font.Write(graphics, this.m_state, new Point(this.m_x + 24, this.m_y + 314));

        }

        public override void SetLocation(int x, int y)
        {
            base.SetLocation(x, y);
            this.SetIconsLocation(x, y);
        }

        public override void Dispose()
        {
            this.ObjectDispose(this.m_enemy);
            this.ObjectDispose(this.m_player1);
            this.ObjectDispose(this.m_player2);
            this.ObjectDispose(this.m_flagIcon);
            this.ObjectDispose(this.m_font);

            base.Dispose();
        }
        #endregion

        #region Private Methods
        private void SetIconsLocation(int x, int y)
        {
            int eX = x + 2;
            int eY = y + 2;

            for (int i = 0; i < this.m_enemy.Count; i++)
            {
                this.m_enemy[i].SetLocation(eX, eY);

                if (eX == this.X + 2)
                {
                    eX += 18;
                }
                else
                {
                    eX = x + 2;
                    eY += 18;
                }
            }

            this.m_player1.SetLocation(x + 4, y + 200);

            if (this.m_player2 != null)
            {
                this.m_player2.SetLocation(x + 4, y + 240);
            }

            this.m_flagIcon.SetLocation(x + 4, y + 290);
        }
        #endregion

        #region Public Methods
        public short GetLifeCount(bool secondPlayer)
        {
            if ((this.m_player2 != null) && secondPlayer)
                return this.m_lifeCount2;
            return this.m_lifeCount1;
        }

        public void IncreaseLife(bool secondPlayer)
        {
            if ((this.m_player2 != null) && secondPlayer)
                this.m_lifeCount2 += 1;
            else
                this.m_lifeCount1 += 1;
        }

        public bool DecreaseLife(bool secondPlayer)
        {
            if ((this.m_player2 != null) && secondPlayer)
            {
                this.m_lifeCount2 -= 1;
                return (this.m_lifeCount2 == 0);
            }
            else
            {
                this.m_lifeCount1 -= 1;
                return (this.m_lifeCount1 == 0);
            }
        }

        public void AddSecondPlayer()
        {
            this.m_lifeCount2 = 2;
            this.m_player2 = new PlayerLifeIcon(this, true);
            this.m_player2.SetLocation(this.m_x + 4, this.m_y + 240);
        }

        public void RemoveSecondPlayer()
        {
            this.m_lifeCount2 = 0;
            this.m_player2.Dispose();
            this.m_player2 = null;
        }

        public void AddEnemy(int value)
        {
            if ((this.m_enemy.Count + value) > 20) return;

            for (int i = 0; i < value; i++)
            {
                this.m_enemy.Add(new EnemyIcon(this));
                this.SetIconsLocation(this.m_x, this.m_y);
            }
        }

        public bool DecreaseEnemy()
        {
            int index = this.m_enemy.Count - 1;
            this.m_enemy[index].Dispose();
            this.m_enemy.RemoveAt(index);
            return (this.m_enemy.Count == 0);
        }
        #endregion

        #region Properties
        public int EnemyCount
        {
            get { return this.m_enemy.Count; }
        }

        public string StateID
        {
            get { return this.m_state; }
            set { this.m_state = value; }
        }

        public int Player1Life
        {
            get { return this.m_lifeCount1; }
        }

        public int Player2Life
        {
            get { return this.m_lifeCount2; }
        }
        #endregion
    }
    #endregion

    #region HighScorePane
    internal class HighScorePane : gObject
    {
        #region Variables
        #endregion

        #region Constructor
        public HighScorePane()
        {
        }
        #endregion

        #region Override Methods
        #endregion

        #region Properties
        #endregion
    }
    #endregion
}
