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
            Construct('@', ConsoleColor.Green, 4, 0.1f, root, 30);

            Position = positon;

            EnergyProdusing = 4;
        }

        public LeafTile(Root root) 
        {
            Construct('@', ConsoleColor.Green, 4, 0.1f, root, 30);

            EnergyProdusing = 4;
        }

        public override void ProduseEnergy()
        {
            //if (game.CarbonDioxideLivel <= 0) {return;}
            root.Energy += game.CountEnergy(this);
            // if (Age % 3 == 0)
            // {
            //     game.OxigenLevel += 0.002f;
            //     game.CarbonDioxideLivel -= 0.0002f;
            // }
        }
    }
}
