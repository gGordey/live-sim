using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_Simulation
{
    class LeafTile : ProdusingTile
    {
        private float naberhood = 1f;
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
            root.Energy += EnergyProdusing * game.SunLevel * naberhood;

            if (Age % 3 == 0)
            {
                naberhood = 1f;
                for (int i = 0; i < 4; i++)
                {
                    if (game.GetTile(Position+root.seed.GetPositionFromInd((byte)i)).GetType() == typeof(LeafTile)) {naberhood = 0.4f;}
                }
            }
        }
    }
}
