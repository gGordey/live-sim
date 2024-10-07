
using System.Reflection.Emit;

namespace Life_Simulation
{
    class Canvas
    {
        private int _field_width;

        private int _field_height;

        public int fieldWidth {  set { _field_width = value; }  }

        public int fieldHeight { set { _field_height = value; } }

        public int gen;
        public int turn;
        public int max_turn;
        public float o2, co2;
        public static float[] org = new float[Game.field_size.X*Game.field_size.Y];
        private static void DrawTile(Tile tile, Game.DrawMode mode)
        {
            // if (tile == null) return;

            int pos = 0;
            if (tile.Position.X != 0) { pos = tile.Position.X*2-1;} // *2-1

            Console.SetCursorPosition(pos, tile.Position.Y);
            
            if (mode == Game.DrawMode.Default) {Console.ForegroundColor = tile.Color;}

            if (mode == Game.DrawMode.Clan) {Console.ForegroundColor = tile.ClanColor;}

            if (mode == Game.DrawMode.FirstGen) {Console.ForegroundColor = tile.GenColor;}

            Console.Write(tile.Symbol + " ");

            //if (tile.Position.X == width - 1) {Console.Write("|\n");} 
        }

        public void DrawAllTiles (List<Tile> tiles, Game.DrawMode mode)
        {
            //Console.Clear();
            if (mode != Game.DrawMode.Organic)
            { 
                foreach (Tile tile in tiles)
                {
                    DrawTile(tile, mode);
                }
            }
            else
            {
                for (int i = 0; i < org.Length; i++)
                {   
                    float val = org[i];
                    if (val < 0.4f) { Console.ForegroundColor = ConsoleColor.White; }
                    else if (val < 0.9f) { Console.ForegroundColor = ConsoleColor.Gray; }
                    else if (val < 1.4f) { Console.ForegroundColor = ConsoleColor.DarkGray; }
                    else if (val < 1.5f) { Console.ForegroundColor = ConsoleColor.Black; }
                    else { Console.ForegroundColor = ConsoleColor.Red; }

                    int y = i / Game.field_size.X;
                    int x = i - y * Game.field_size.X;

                    Console.SetCursorPosition(x*2, y);
                    Console.Write("██");
                }
            }
            // for (int i = 0; i < _field_width; i++)
            // {
            //     Console.Write("--");
            // }
            Console.SetCursorPosition(0, _field_height+1);
            System.Console.WriteLine("SIMULATION  "+gen);
            System.Console.WriteLine("TURN  "+turn+"  MAX TURN  "+max_turn);
            //System.Console.Write("OXIGEN  "+o2+"  CARBON DIOXIDE  "+co2);
        }
    }
}
