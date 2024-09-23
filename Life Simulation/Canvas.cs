
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
            if (tile == null) return;

            Console.ForegroundColor = tile.Color;

            Console.Write(tile.Symbol + " ");

            //if (tile.Position.X == width - 1) {Console.Write("|\n");} 
        }

        public void DrawAllTiles (Tile[] tiles)
        {
            Console.Clear();

            foreach (Tile tile in tiles)
            {
                DrawTile(tile, _field_width);
            }
            for (int i = 0; i < _field_width; i++)
            {
                Console.Write("--");
            }
            Console.Write('\n');
            System.Console.WriteLine("SIMULATION  "+gen);
            System.Console.Write("TURN  "+turn+"  MAX TURN  "+max_turn);
        }
    }
}
