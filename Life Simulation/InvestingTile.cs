using System;

namespace Life_Simulation
{
    class InvestingTile : ProdusingTile
    {
        public InvestingTile (Root root)
        {
            Construct('o', ConsoleColor.DarkGreen, 7, 3.5f, root, 80);
        }

        float leafs_around = 0;
        public override void ProduseEnergy()
        {
            if (Age % 2 == 0) 
            {
                leafs_around = 0.5f;
                for (int i = 0; i < 4; i++)
                {
                    if (game.GetTile(Position+root.seed.GetPositionFromInd((byte)i)).GetType() == typeof(LeafTile))
                    {
                        leafs_around += 0.15f;
                    }
                }
            }
            root.Energy += (Age-10) * leafs_around;
        }
    }
}