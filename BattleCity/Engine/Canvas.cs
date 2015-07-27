using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using System.Threading;

namespace BattleCity.Engine
{
    #region Canvas
    internal class Canvas : gObject
    {
        #region Variables
        private Map.StateData m_state = null;
        private HomeEagle m_home = null;

        private ItemList<MapItem> m_mapItemsBrick = null;
        private ItemList<MapItem> m_mapItemsFerum = null;
        private ItemList<MapItem> m_mapItemsGarden = null;
        private ItemList<MapItem> m_mapItemsWater = null;
        private ItemList<MapItem> m_mapItemsIce = null;
        private MapItem[] m_homeCover = null;

        private ItemList<BonusItem> m_bonusItem = null;
        private Enemies[] m_enemy = null;
        private Player m_player1 = null;
        
        private InfoPane m_infoPane = null;
        private GameOverText m_gameOver = null;

        private int m_enemyLoad = 5;
        private int m_enemyCount = 0;
        private int m_enemySpwanLoc = 0;
        private int m_bonusStopTime = -1;
        private int m_bonusBlockStuff = -1;

        private int m_playerSwpan = -1;
        private int m_stateWait = -1;
        private int m_score = 0;

        private Thread m_createEnemyThread = null;
        private Thread m_processThread = null;
        #endregion

        #region Declartion Events
        //public delegate void StateChangeEventHandler(object sender, StateChangeEventArgs e);
        //public event StateChangeEventHandler StateChange = null;
        #endregion

        #region Constructor
        public Canvas(int x, int y)
        {
            this.m_x = x;
            this.m_y = y;

            this.m_infoPane = new InfoPane(this);
            this.m_enemy = new Enemies[6];
            this.m_player1 = new Player();

            this.m_bonusItem = new ItemList<BonusItem>();
            this.m_homeCover = new MapItem[20];

            this.m_gameOver = new GameOverText(this);
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(gPaintEventArgs e)
        {
            if ((this.m_life > 0) && (this.m_state != null) && (!this.m_state.IsEmpty))
            {
                e.Value.FillRectangle(new SolidBrush(Color.Black), this.Rectangle);
                base.OnPaint(e);

                if ((this.m_gameOver != null) && (this.m_gameOver.Life > 0))
                {
                    this.m_gameOver.DoPaint(e.Value);
                }

                GC.Collect();
            }
        }

        protected override void OnPause(gPauseEventArgs e)
        {
            if (e.Value)
            {
                if ((this.m_createEnemyThread != null) && ((this.m_createEnemyThread.ThreadState & ThreadState.Running) == ThreadState.Running))
                {
                    this.m_createEnemyThread.Abort();
                    this.m_createEnemyThread.Join();
                    this.m_createEnemyThread = null;
                }
            }
            else
            {
                this.m_createEnemyThread = new Thread(new ThreadStart(this.CreateEnemy));
                this.m_createEnemyThread.Start();
            }

            base.OnPause(e);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (this.m_processThread != null)
            {
                if ((this.m_processThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                    this.m_processThread.Abort();
                this.m_processThread = null;
            }

            if (this.m_createEnemyThread != null)
            {
                if ((this.m_createEnemyThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                    this.m_createEnemyThread.Abort();
                this.m_createEnemyThread = null;
            }

            this.ObjectDispose(this.m_mapItemsBrick);
            this.ObjectDispose(this.m_mapItemsFerum);
            this.ObjectDispose(this.m_mapItemsGarden);
            this.ObjectDispose(this.m_mapItemsWater);
            this.ObjectDispose(this.m_mapItemsIce);
            this.ObjectDispose(this.m_homeCover);
            this.ObjectDispose(this.m_home);
            this.ObjectDispose(this.m_bonusItem);
            this.ObjectDispose(this.m_enemy);
            this.ObjectDispose(this.m_player1);
        }
        #endregion

        #region Private Methods
        private void DoProcess()
        {
            do
            {
                if (this.m_bonusStopTime > 0)
                {
                    this.m_bonusStopTime--;
                }
                else if (this.m_bonusStopTime == 0)
                {
                    if (this.m_enemy != null)
                    {
                        for (int i = 0; i < this.m_enemy.Length; i++)
                        {
                            if ((this.m_enemy[i] != null) && (this.m_enemy[i].Life > 0))
                            {
                                this.m_enemy[i].DoPause(false);
                            }
                        }
                    }
                    this.m_bonusStopTime--;
                }

                if ((this.m_bonusBlockStuff > 0) && (this.m_bonusBlockStuff < 500)) 
                {
                    this.m_bonusBlockStuff--;
                }
                else if (this.m_bonusBlockStuff == 500)
                {
                    for (int i = 0; i < this.m_homeCover.Length; i++)
                    {
                        this.m_homeCover[i].Block(true);
                    }

                    this.m_bonusBlockStuff--;
                }
                else if (this.m_bonusBlockStuff == 0)
                {
                    for (int i = 0; i < this.m_homeCover.Length; i++)
                    {
                        if ((this.m_homeCover[i] != null) && (this.m_homeCover[i].Life > 0))
                        {
                            this.m_homeCover[i].Block(false);
                        }
                    }

                    this.m_bonusBlockStuff--;
                }

                if ((this.m_gameOver != null) && (this.m_gameOver.Life > 0))
                {
                    if (((this.m_height / 2) + this.m_y) < this.m_gameOver.Y)
                    {
                        this.m_gameOver.DoMove(2);
                    }
                    else
                    {
                        this.OnGameOvered();
                    }
                }

                if (this.m_playerSwpan > 0)
                {
                    this.m_playerSwpan--;
                }
                else if (this.m_playerSwpan == 0)
                {
                    this.m_playerSwpan--;
                    if (this.m_infoPane.Player1Life > 0)
                    {
                        this.m_player1.Start(this, false, 0);
                        this.m_infoPane.DecreaseLife(false);
                    }
                    else
                    {
                        this.DoGameOver();
                    }
                }

                if (this.m_stateWait > 0)
                {
                    this.m_stateWait--;
                }
                else if (this.m_stateWait == 0)
                {
                    this.OnStateCompleted();
                }

                Thread.Sleep(50);
            } while (this.m_processThread.IsAlive);
        }

        private void DoGameOver()
        {
            if (this.m_gameOver.Life > 0) return;

            this.m_gameOver.SetLocation(((this.m_width - this.m_gameOver.Width) / 2) + this.m_x, ((this.m_height - this.m_gameOver.Height) - 10) + this.m_y);
            this.m_gameOver.Face = FaceDirection.Up;
            this.m_gameOver.Show(true);

            if ((this.m_createEnemyThread != null) && ((this.m_createEnemyThread.ThreadState & ThreadState.Running) == ThreadState.Running))
            {
                this.m_createEnemyThread.Abort();
                this.m_createEnemyThread.Join();
                this.m_createEnemyThread = null;
            }

            if (this.m_player1 != null)
            {
                this.m_player1.DoPause(true);
            }
        }

        private void OnGameOvered()
        {
        }

        private void OnStateCompleted()
        {
        }

        private void CreateMapItems()
        {
            this.ObjectDispose(this.m_mapItemsBrick);
            this.ObjectDispose(this.m_mapItemsFerum);
            this.ObjectDispose(this.m_mapItemsGarden);
            this.ObjectDispose(this.m_mapItemsWater);
            this.ObjectDispose(this.m_mapItemsIce);
            this.ObjectDispose(this.m_homeCover);
            this.ObjectDispose(this.m_home);

            this.m_mapItemsBrick = new ItemList<MapItem>();
            this.m_mapItemsFerum = new ItemList<MapItem>();
            this.m_mapItemsGarden = new ItemList<MapItem>();
            this.m_mapItemsWater = new ItemList<MapItem>();
            this.m_mapItemsIce = new ItemList<MapItem>();
            this.m_home = new HomeEagle(this);
            this.m_homeCover = new MapItem[20];
            this.m_home.LifeChange += new gLifeChangeEventHandler(this.home_LifeChange);

            int homeX = 0;
            int homeY = 0;
            int indexHC = 0;
            Point ptHome = Point.Empty;

            for (int y = 0; y < this.m_state.Canvas.RowCount; y++)
            {
                short[] row = this.m_state.Canvas.GetRow(y);
                for (int x = 0; x < row.Length; x++)
                {
                    if ((row[x] < 6) && (row[x] > 0))
                    {
                        int cursorX = (x * 16) + this.X;
                        int cursorY = (y * 16) + this.Y;

                        MapItemTypes type = (MapItemTypes)row[x];

                        if ((y > 21) && ((x > 9) && (x < 16)))
                        {
                            MapItem item = new MapItem(this, type, true, cursorX, cursorY);
                            this.m_homeCover[indexHC] = item;
                            indexHC++;
                        }
                        else
                        {
                            MapItem item = new MapItem(this, type, false, cursorX, cursorY);

                            switch (type)
                            {
                                case MapItemTypes.brick:
                                    this.m_mapItemsBrick.Add(item);
                                    break;
                                case MapItemTypes.ferum:
                                    this.m_mapItemsFerum.Add(item);
                                    break;
                                case MapItemTypes.garden:
                                    this.m_mapItemsGarden.Add(item);
                                    break;
                                case MapItemTypes.water:
                                    this.m_mapItemsWater.Add(item);
                                    break;
                                case MapItemTypes.ice:
                                    this.m_mapItemsIce.Add(item);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if ((row[x] == 9) && (ptHome == Point.Empty))
                        {
                            homeX = x;
                            homeY = y;
                            ptHome = new Point((x * 16) + this.X, (y * 16) + this.Y);
                        }
                    }
                }
            }

            if (ptHome != Point.Empty)
            {
                this.m_home.SetLocation(ptHome.X, ptHome.Y);
            }
        }

        private void CreateEnemy()
        {
            do
            {
                try
                {
                    if (this.m_enemy != null)
                    {
                        if (this.m_enemyLoad > 0)
                        {
                            this.m_enemyLoad--;
                        }
                        else if (this.m_enemyCount < 20)
                        {
                            int index = -1;
                            for (int i = 0; i < this.m_enemy.Length; i++)
                            {
                                if ((this.m_enemy[i] != null) && (this.m_enemy[i].Life < 0))
                                {
                                    this.ObjectDispose(this.m_enemy[i]);
                                    index = i;
                                    break;
                                }

                                if (this.m_enemy[i] == null)
                                {
                                    index = i;
                                    break;
                                }
                            }

                            if (index > -1)
                            {
                                int type = 0, level = 0;
                                bool withBonus = false;
                                if (this.m_state.Enemys.Count > this.m_enemyCount)
                                {

                                    level = this.m_state.Enemys[this.m_enemyCount].Tank - 1;
                                    withBonus = (this.m_state.Enemys[this.m_enemyCount].Bonus == 1);
                                    type = this.m_state.Enemys[this.m_enemyCount].TankType;
                                }

                                Enemies enemies = new Enemies(this, level, type, withBonus, this.m_enemySpwanLoc);
                                this.m_enemy[index] = enemies;
                                this.m_enemySpwanLoc++;
                                this.m_enemyCount++;

                                this.m_infoPane.DecreaseEnemy();

                                TRACE("Enemy Created");

                                if (this.m_enemySpwanLoc == 3) this.m_enemySpwanLoc = 0;
                            }

                            this.m_enemyLoad = 10;
                        }
                        else
                        {
                            if ((this.m_createEnemyThread != null) && ((this.m_createEnemyThread.ThreadState & ThreadState.Running) == ThreadState.Running))
                            {
                                this.m_createEnemyThread.Abort();
                                this.m_createEnemyThread.Join();
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    TRACE(ex.Message);
                }

                Thread.Sleep(800);
            } while (this.m_createEnemyThread.IsAlive);
        }

        #region Bonus
        private void bonusUpRank(Player player)
        {
            if (player != null)
            {
                player.Level = player.Level + 1;
            }
        }

        private void bonusDestroyAllEnemy(Player player)
        {
            if (this.m_enemy != null)
            {
                for (int i = 0; i < this.m_enemy.Length; i++)
                {
                    if ((this.m_enemy[i] != null) && (this.m_enemy[i].Life > 0))
                    {
                        this.m_enemy[i].WithBonus = false;
                        this.m_enemy[i].Life = 0;
                    }
                }
            }
        }

        private void bonusLifeAdd(Player player)
        {
            if ((player != null) && (player == this.m_player1))
            {
                this.m_infoPane.IncreaseLife(false);
            }
        }

        private void bonusSetArmor(Player player)
        {
            if (player != null)
            {
                player.SetArmor();
            }
        }

        private void bonusStopTime(Player player)
        {
            // 500

            if (this.m_enemy != null)
            {
                for (int i = 0; i < this.m_enemy.Length; i++)
                {
                    if ((this.m_enemy[i] != null) && (this.m_enemy[i].Life > 0))
                    {
                        this.m_enemy[i].DoPause(true);
                    }
                }
            }

            this.m_bonusStopTime = 500;
        }

        private void bonusBlockStuff(Player player)
        {
            // 500
            this.m_bonusBlockStuff = 500;
        }

        private void bonusFullRank(Player player)
        {
            if (player != null)
            {
                player.Level = 3;
            }
        }
        #endregion

        private void home_LifeChange(object sender, gLifeChangeEventArgs e)
        {
            if (e.Value == 1)
            {
                this.DoGameOver();   
            }
        }
        #endregion

        #region Public Methods
        public void Create(Map.StateData state)
        {
            if (state == null) return;

            if ((this.m_state != null) && (this.m_state.ID == state.ID))
            {
                return;
            }

            this.m_life = 0;
            this.m_state = state;

            if ((state != null) && (!state.IsEmpty))
            {
                this.m_height = this.m_state.Canvas.RowCount * 16;
                this.m_width = this.m_state.Canvas.GetRow(0).Length * 16;

                if (this.m_infoPane != null)
                {
                    this.m_infoPane.SetLocation((this.m_x + this.m_width + 12), (this.m_y + 12));
                    this.m_infoPane.StateID = state.ID;
                }

                this.CreateMapItems();

                if (this.m_player1 == null)
                {
                    this.m_player1 = new Player();
                }

                TRACE("Canvas Created!");
            }
        }

        public void Show(bool show)
        {
            this.m_life = (show) ? 1 : 0;

            if (this.m_processThread != null)
            {
                if ((this.m_processThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                {
                    this.m_processThread.Abort();
                    this.m_processThread.Join();
                }
                this.m_processThread = null;
            }

            if (this.m_createEnemyThread != null)
            {
                if ((this.m_createEnemyThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                {
                    this.m_createEnemyThread.Abort();
                    this.m_createEnemyThread.Join();
                }
                this.m_createEnemyThread = null;
            }

            if (show)
            {
                this.m_player1.Start(this, false, 0);

                this.m_processThread = new Thread(new ThreadStart(this.DoProcess));
                this.m_processThread.Start();

                this.m_createEnemyThread = new Thread(new ThreadStart(this.CreateEnemy));
                this.m_createEnemyThread.Start();
            }
        }

        public void OnEnemyDead(Enemies sender)
        {
            if (sender.WithBonus)
            {
                this.CreateBonus();
            }

            this.m_score += sender.Type * 100;

            sender.Dispose();

            if (this.IsEnemyClear)
            {
                this.m_stateWait = 100;
            }
        }

        public void OnPlayerDead(Player sender)
        {
            TRACE("OnPlayerDead");
            this.m_playerSwpan = 50;
        }

        public void CreateBonus()
        {
            //Rectangle rectBonus = new Rectangle(this.m_x + 48, this.m_y + 48, this.m_width - 96, this.m_height - 96);

            System.Random ranBonus = new Random();
            int ranX = ranBonus.Next(10) * 32;
            int ranY = ranBonus.Next(10) * 32;
            int type = ranBonus.Next(7);

            ranX += this.m_x + 48;
            ranY += this.m_y + 48;

            BonusItem item = new BonusItem(this, (BonusTypes)type);
            //BonusItem item = new BonusItem(this, BonusTypes.spade);
            item.SetLocation(ranX, ranY);
            this.m_bonusItem.Add(item);

            ranBonus = null;

            TRACE("CreateBonus - Type : " + ((BonusTypes)type).ToString());
        }

        public void HitBonus(Player player, BonusItem item)
        {
            if (item != null)
            {
                TRACE("HitBonus : " + item.Type.ToString());

                this.m_score += 500;

                item.Dispose();
                this.m_bonusItem.Remove(item);

                switch (item.Type)
                {
                    case BonusTypes.star:
                        this.bonusUpRank(player);
                        break;
                    case BonusTypes.spade:
                        this.bonusBlockStuff(player);
                        break;
                    case BonusTypes.grenade:
                        this.bonusDestroyAllEnemy(player);
                        break;
                    case BonusTypes.clock:
                        this.bonusStopTime(player);
                        break;
                    case BonusTypes.lifeAdd:
                        this.bonusLifeAdd(player);
                        break;
                    case BonusTypes.helmet:
                        this.bonusSetArmor(player);
                        break;
                    case BonusTypes.rocket:
                        this.bonusFullRank(player);
                        break;
                }
            }
        }
        #endregion

        #region Properties
        public Map.StateData State
        {
            get { return this.m_state; }
        }

        public Player Player1
        {
            get { return this.m_player1; }
        }

        public HomeEagle Home
        {
            get { return this.m_home; }
        }

        public MapItem[] MapItemsHomeCover
        {
            get { return this.m_homeCover; }
        }

        public ItemList<MapItem> MapItemsBrick
        {
            get { return this.m_mapItemsBrick; }
        }

        public ItemList<MapItem> MapItemsFerum
        {
            get { return this.m_mapItemsFerum; }
        }

        public ItemList<MapItem> MapItemsGarden
        {
            get { return this.m_mapItemsGarden; }
        }

        public ItemList<MapItem> MapItemsWater
        {
            get { return this.m_mapItemsWater; }
        }

        public ItemList<MapItem> MapItemsIce
        {
            get { return this.m_mapItemsIce; }
        }

        public ItemList<BonusItem> BonusItems
        {
            get { return this.m_bonusItem; }
        }

        public Enemies[] Enemy
        {
            get { return this.m_enemy; }
        }

        public bool IsEnemyClear
        {
            get 
            {
                if (this.m_enemy != null)
                {
                    if (this.m_enemyCount < 20) return false;

                    int count = 0;
                    for (int i = 0; i < this.m_enemy.Length; i++)
                    {
                        if (this.m_enemy[i] == null)
                        {
                            count++;
                        }
                        else if (this.m_enemy[i].Life < 1)
                        {
                            count++;
                        }
                    }

                    return (count == this.m_enemy.Length);
                }
                return false;
            }
        }
        #endregion
    }

    internal class StateChangeEventArgs : gEventArgs<string>
    {
        #region Constructor
        public StateChangeEventArgs(string stateId)
            : base(stateId)
        {
        }
        #endregion
    }
    #endregion
}

