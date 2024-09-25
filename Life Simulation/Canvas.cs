
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
        private static void DrawTile(Tile tile, int width)
        {
            // if (tile == null) return;

            int pos = 0;
            if (tile.Position.X != 0) { pos = tile.Position.X*2-1;}

            Console.SetCursorPosition(pos, tile.Position.Y);

            Console.ForegroundColor = tile.Color;

            Console.Write(tile.Symbol + " ");

            //if (tile.Position.X == width - 1) {Console.Write("|\n");} 
        }

        public void DrawAllTiles (List<Tile> tiles)
        {
            //Console.Clear();

            foreach (Tile tile in tiles)
            {
                DrawTile(tile, _field_width);
            }
            // for (int i = 0; i < _field_width; i++)
            // {
            //     Console.Write("--");
            // }
            Console.SetCursorPosition(0, _field_height+1);
            System.Console.WriteLine("SIMULATION  "+gen);
            System.Console.Write("TURN  "+turn+"  MAX TURN  "+max_turn);
        }
    }
}
