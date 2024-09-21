using System;
using System.Threading;

namespace Life_Simulation
{
    class Game
    {
        private const char backGrounSymbol = ' ';

        private float SunLevel = 1.0f;

        private static Vector2 field_size = new Vector2 (40, 20);

        private Tile[] tiles = new Tile[field_size.X * field_size.Y];

        private Canvas canvas = new Canvas();

        public void NewTurn()
        {
            UpdateTiles();
        }

        public void Start()
        {
            canvas.fieldWidth = field_size.X;
            canvas.fieldHeight = field_size.Y;

            FillMap();
            
            NewTile(new Vector2(20, 10), new SeedTile (new Vector2(5,5), this));
            
            DrawTiles();

            while (true)
            {
                NewTurn();

                DrawTiles();

                Thread.Sleep(500);
            }
        }

        private void UpdateTiles()
        {
            Tile[] tiles_cp = (Tile[])tiles.Clone();
            for (int i = 0; i < tiles_cp.Length; i++)
            {
                // if (!tiles_cp[i].IsAlive) 
                // {
                //     tiles_cp[i] = new FreeTile(tiles_cp[i].Position);
                //     continue;
                // }
                tiles_cp[i].NextTurn(this);
            }
        }

        private void DrawTiles()
        {
            canvas.DrawAllTiles(tiles);
        }

        public bool IsTileFree(Vector2 position)
        {
            if (position.X >= field_size.X || position.Y >= field_size.Y || position.Y < 0 || position.X < 0) { return false; }
            return tiles[GetTileIndFromPosition(position)].GetType() == typeof(FreeTile);
        }

        private void FillMap()
        {
            for (int x = 0, y = 0, c = 0; ; ++x, ++c)
            {
                if ( x == field_size.X )
                {
                    x = 0;
                    ++y;
                }

                if (y == field_size.Y) { break; }

                tiles[c] = new FreeTile(x, y);
    
                
            }
        }

        private void NewTile(Vector2 position, Tile tile)
        {
            tiles[GetTileIndFromPosition(position)] = tile;

            tile.Position = position;

            tile.Start();
        }

        public void TryAddTile(Vector2 position, Tile tile)
        {
            if (IsTileFree(position))
            {
                NewTile(position, tile);
            }
        }

        public void MoveTile(Tile tile, Vector2 position)
        {
            if (IsTileFree(position) && position.X < field_size.X && position.Y < field_size.Y)
            {
                SeedTile _tile = (SeedTile)tile;

                tiles[GetTileIndFromPosition(_tile.Position)] = new RootTile(tile.root, tile.Position, _tile.root_gen, _tile.root_sec_gen);

                _tile.Position = position;

                tiles[GetTileIndFromPosition(position)] = _tile;

                System.Console.WriteLine("IF not work");
            }
            else
            {
                System.Console.WriteLine("is free: "+ IsTileFree(position));
                System.Console.WriteLine("x: "+(position.X < field_size.X));
                System.Console.WriteLine("y: "+(position.Y < field_size.Y));
            }
        }

        private int GetTileIndFromPosition(Vector2 position)
        {
            return position.Y * field_size.X + position.X;
        }
    }
}
