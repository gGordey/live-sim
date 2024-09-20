using System;

namespace Life_Simulation
{
    class FreeTile : Tile
    {
        public FreeTile(int x, int y)
        {
            Construct('#', ConsoleColor.White);
            Position = new Vector2(x, y);
        }
        public FreeTile(Vector2 pos)
        {
            Construct('#', ConsoleColor.White);
            Position = pos;
        }
        public FreeTile()
        {
            Construct('#', ConsoleColor.White);
        }
    }
}
