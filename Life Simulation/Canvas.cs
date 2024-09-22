
namespace Life_Simulation
{
    class Canvas
    {
        private int _field_width;

        private int _field_height;

        public int fieldWidth {  set { _field_width = value; }  }

        public int fieldHeight { set { _field_height = value; } }

        private static int x_count;

        private static void DrawTile(Tile tile, int width)
        {
            if (tile == null) return;

            Console.ForegroundColor = tile.Color;

            Console.Write(tile.Symbol + " ");

            if (tile.Position.X == width - 1) {Console.Write("|\n"); x_count = 0;} 
        }

        public void DrawAllTiles (Tile[] tiles)
        {
            Console.Clear();
            
            x_count = 0;

            foreach (Tile tile in tiles)
            {
                DrawTile(tile, _field_width);
            }
            for (int i = 0; i < _field_width; i++)
            {
                Console.Write("--");
            }
            Console.Write('\n');
        }
    }
}
