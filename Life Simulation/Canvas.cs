
namespace Life_Simulation
{
    class Canvas
    {
        private int _field_width;

        private int _field_height;

        public int fieldWidth {  set { _field_width = value; }  }

        public int fieldHeight { set { _field_height = value; } }

        private static void DrawTile(Tile tile, int width)
        {
            if (tile == null) return;

            Console.ForegroundColor = tile.Color;

            Console.Write(tile.Symbol + " ");

            if (tile.Position.X == width - 1) Console.Write("\n"); 
        }

        public void DrawAllTiles (Tile[] tiles)
        {
            Console.Clear();
            
            foreach (Tile tile in tiles)
            {
                DrawTile(tile, _field_width);
            }
        }
    }
}
