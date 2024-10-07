using System;

namespace Life_Simulation
{
    class InvestingTile : Tile
    {
        public InvestingTile (Root root)
        {
            Construct('o', ConsoleColor.Cyan, 5, 0.2f, root, 80);
        }

        float leafs_around = 0;
    }
}