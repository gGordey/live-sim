
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
        private static void DrawTile(Tile tile, Game.DrawMode mode)
        {
            // if (tile == null) return;

            int pos = 0;
            if (tile.Position.X != 0) { pos = tile.Position.X*2-1;}

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

            foreach (Tile tile in tiles)
            {
                DrawTile(tile, mode);
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
