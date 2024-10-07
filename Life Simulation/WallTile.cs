using System;

namespace Life_Simulation
{
    class  WallTile : Tile
    {
        public WallTile(Vector2 position)
        {
            Construct('█', ConsoleColor.White);
            Position = position;
        }
    }
}