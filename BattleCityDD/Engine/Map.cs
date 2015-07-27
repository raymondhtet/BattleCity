using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;

namespace BattleCity.Engine
{
    internal class Map : IDisposable
    {
        #region Variables
        internal const string c_file = "BattleCity.Resources.states.xml";
        protected XmlDocument m_xDoc = null;
        #endregion

        #region Constructor
        public Map()
        {
            this.m_xDoc = new XmlDocument();
            this.Load();
        }
        #endregion

        #region Override Methods
        #endregion

        #region Protected Methods
        protected virtual void Load()
        {
            try
            {
                using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(c_file))
                {
                    this.m_xDoc.Load(manifestResourceStream);
                }
            }
            catch
            {
            }
        }
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        public void Dispose()
        {
        }
        #endregion

        #region Properties
        public StateData this[string id]
        {
            get
            {
                if (this.m_xDoc == null) return null;
                if (!this.m_xDoc.HasChildNodes) return null;

                XmlNode xNode = this.m_xDoc.FirstChild;
                while ((xNode != null) && (xNode.Name != "battleCity"))
                {
                    xNode = xNode.NextSibling;
                }

                if ((xNode != null) && (xNode.Name == "battleCity") && (xNode.HasChildNodes))
                {
                    XmlNode xNode1 = xNode.FirstChild;
                    while ((xNode1 != null) && (xNode1.Name != "states"))
                    {
                        xNode1 = xNode1.NextSibling;
                    }

                    if ((xNode1 != null) && (xNode1.Name == "states") && (xNode1.NodeType == XmlNodeType.Element))
                    {
                        return new StateData((XmlElement)xNode1, id);
                    }
                }

                return null;
            }
        }
        #endregion

        #region Nested Types
        public class StateData
        {
            #region Variables
            private const string c_parent = "states";
            private const string c_root = "map";

            private string m_id = "";

            private List<EnemyData> m_enemys = null;
            private CanvasData m_canvas = null;
            #endregion

            #region Constructor
            public StateData()
            {
            }

            public StateData(XmlElement xEleParent, string id)
            {
                this.m_id = id;

                if ((xEleParent != null) && (xEleParent.Name == StateData.c_parent))
                {
                    XmlElement xElement = null;

                    foreach (XmlElement child in xEleParent.ChildNodes)
                    {
                        if ((child.Name == StateData.c_root) && (child.GetAttribute("id") == id))
                        {
                            xElement = child;
                            break;
                        }
                    }

                    this.Load(xElement);
                }
            }
            #endregion

            #region Override Methods
            #endregion

            #region Protected Methods
            #endregion

            #region Private Methods
            private void Load(XmlElement xElement)
            {
                if (xElement == null) return;
                if (xElement.Name != StateData.c_root) return;
                if (!xElement.HasChildNodes) return;
                if (this.m_enemys == null) this.m_enemys = new List<EnemyData>();

                XmlElement canvas = null;
                XmlElement enemys = null;

                foreach (XmlNode child in xElement.ChildNodes)
                {
                    if (child.Name == "canvas")
                    {
                        if (canvas == null) canvas = (XmlElement)child;
                    }
                    else if (child.Name == "enemy")
                    {
                        if (enemys == null) enemys = (XmlElement)child;
                    }
                }

                if (canvas != null) this.m_canvas = new CanvasData(canvas);

                if (enemys != null)
                {
                    string sTank = "";
                    string sBonus = "";
                    string sType = "";

                    foreach (XmlNode enemyChild in enemys.ChildNodes)
                    {
                        if (enemyChild.Name == "tank")
                        {
                            sTank = enemyChild.InnerText;
                        }
                        else if (enemyChild.Name == "bonus")
                        {
                            sBonus = enemyChild.InnerText;
                        }
                        else if (enemyChild.Name == "type")
                        {
                            sType = enemyChild.InnerText;
                        }
                    }

                    short[] iTank = this.GetValueList(sTank);
                    short[] iBonus = this.GetValueList(sBonus);
                    short[] iType = this.GetValueList(sType);

                    if (iTank != null)
                    {
                        for (int i = 0; i < iTank.Length; i++)
                        {
                            short num1 = 0;
                            short num2 = 0;
                            if ((iBonus != null) && (iBonus.Length > i))
                            {
                                num1 = iBonus[i];
                            }

                            if ((iType != null) && (iType.Length > i))
                            {
                                num2 = iType[i];
                            }

                            EnemyData data = new EnemyData(iTank[i], num1, num2);
                            this.m_enemys.Add(data);
                        }
                    }
                }
            }

            private short[] GetValueList(string value)
            {
                if (string.IsNullOrEmpty(value)) return null;
                string[] data = value.Split(',');
                short[] iData = new short[data.Length];

                for (int i = 0; i < data.Length; i++)
                {
                    try
                    {
                        iData[i] = Convert.ToInt16(data[i].TrimEnd(' ').TrimStart(' '), 10);
                    }
                    catch
                    {
                        iData[i] = 0;
                    }
                }

                return iData;
            }
            #endregion

            #region Public Methods
            #endregion

            #region Properties
            public bool IsEmpty
            {
                get { return ((this.m_canvas == null) || (this.m_enemys == null) || (this.m_enemys.Count == 0)); }
            }

            public string ID
            {
                get { return this.m_id; }
                set { this.m_id = value; }
            }

            public List<EnemyData> Enemys
            {
                get { return this.m_enemys; }
            }

            public CanvasData Canvas
            {
                get { return this.m_canvas; }
            }
            #endregion
        }

        public class EnemyData
        {
            #region Variables
            private int m_tank = 0;
            private int m_bonus = 0;
            private int m_type = 0;
            #endregion

            #region Constructor
            public EnemyData()
            {
            }

            public EnemyData(int tank, int bonus, int type)
            {
                this.m_tank = tank;
                this.m_bonus = bonus;
                this.m_type = type;
            }
            #endregion

            #region Public Methods
            #endregion

            #region Properties
            public int Tank
            {
                get { return this.m_tank; }
                set { this.m_tank = value; }
            }

            public int Bonus
            {
                get { return this.m_bonus; }
                set { this.m_bonus = value; }
            }

            public int TankType
            {
                get { return this.m_type; }
                set { this.m_type = value; }
            }
            #endregion
        }

        public class CanvasData
        {
            #region Variables
            private const string c_root = "canvas";
            private const string c_child = "row";

            private List<short[]> m_rows = null;
            #endregion

            #region Constructor
            public CanvasData()
            {
            }

            public CanvasData(XmlElement xElement)
            {
                this.m_rows = new List<short[]>();
                this.Load(xElement);
            }
            #endregion

            #region Methods
            private void Load(XmlElement xElement)
            {
                if (xElement == null) return;
                if (xElement.Name != CanvasData.c_root) return;
                if (!xElement.HasChildNodes) return;

                foreach (XmlNode child in xElement.ChildNodes)
                {
                    if (child.Name != CanvasData.c_child) continue;
                    short[] arr = this.GetValueList(child.InnerText);
                    if (arr != null) this.m_rows.Add(arr);
                }
            }

            private short[] GetValueList(string value)
            {
                if (string.IsNullOrEmpty(value)) return null;

                string[] data = value.Split(',');
                short[] iData = new short[data.Length];

                for (int i = 0; i < data.Length; i++)
                {
                    try
                    {
                        iData[i] = Convert.ToInt16(data[i].TrimEnd(' ').TrimStart(' '), 10);
                    }
                    catch
                    {
                        iData[i] = 0;
                    }
                }

                return iData;
            }

            public short[] GetRow(int index)
            {
                if (this.m_rows == null) return null;
                if (this.m_rows.Count < index) return null;
                return this.m_rows[index];
            }
            #endregion

            #region Properties
            public int this[int x, int y]
            {
                get
                {
                    if (this.m_rows.Count < x) return 0;
                    if (this.m_rows[x].Length < y) return 0;
                    return this.m_rows[x][y];
                }
            }

            public int RowCount
            {
                get { return this.m_rows.Count; }
            }
            #endregion
        }
        #endregion
    }
}
