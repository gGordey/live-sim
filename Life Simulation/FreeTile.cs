using System;

namespace Life_Simulation
{
    class FreeTile : Tile
    {
        private char s = ' ';
        public FreeTile(int x, int y)
        {
            Construct(s, ConsoleColor.White);
            Position = new Vector2(x, y);
        }
        public FreeTile(Vector2 pos)
        {
            Construct(s, ConsoleColor.White);
            Position = pos;
        }
        public FreeTile()
        {
            Construct(s, ConsoleColor.White);
        }
    }
}
