using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity.Engine
{
    interface IGameObject
    {
        void Update(float ticksPerSecond);
        bool Render();
    }
}
