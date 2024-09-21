using System;

namespace Life_Simulation
{
    class FlowerTile : Tile
    {
        private byte _happyness = 12;
        public FlowerTile (Root root)
        {
            Construct('@', ConsoleColor.Magenta, 19, 5, root);
            
            root.Happyness += _happyness;
        }
    }
}