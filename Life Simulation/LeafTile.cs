using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_Simulation
{
    class LeafTile : ProdusingTile
    {
        public LeafTile(Root root, Vector2 positon)
        {
            Construct('@', ConsoleColor.Green, 4, 0, root, 30);

            Position = positon;

            EnergyProdusing = 4;
        }

        public LeafTile(Root root) 
        {
            Construct('@', ConsoleColor.Green, 4, 0, root, 30);

            EnergyProdusing = 4;
        }

        public override void ProduseEnergy()
        {
            root.Energy += EnergyProdusing * game.SunLevel;
        }
    }
}
