using System;
using System.Threading;

namespace Life_Simulation
{
    class Game
    {
        public float SunLevel = 1.0f;

        private static Vector2 field_size = new Vector2 (80, 50);

        private static byte spaceBetweenTiles = 10;

        private Tile[] tiles = new Tile[field_size.X * field_size.Y];

        private Canvas canvas = new Canvas();

        private bool is_there_any_life = true;

        public void NewTurn()
        {
            UpdateTiles();
        }

        public void Start()
        {
            canvas.fieldWidth = field_size.X;
            canvas.fieldHeight = field_size.Y;
            while(true)
            {   
                is_there_any_life = true;
                FillMap();
                for (byte x = 1, y = 1; ; x += spaceBetweenTiles)
                {
                    NewTile(new Vector2 (x, y), new SeedTile (new Vector2(x,y)));
                    if (x >= field_size.X)
                    {
                        y += spaceBetweenTiles;
                        x = 0;
                        if (y >= field_size.Y) { break; }
                    }
                }

                // NewTile(new Vector2 (10, 10), new SeedTile (new Vector2(10, 10)));
                
                // for (int i = 0; i < 100; i++)
                // {
                //     NewTurn();
                // } 
                // Thread.Sleep(10000);

                while (is_there_any_life)
                {
                    NewTurn();

                    DrawTiles();

                    Thread.Sleep(500);
                }
            }
        }

        private void UpdateTiles()
        {
            int life_counter = 0;
            Tile[] tiles_cp = (Tile[])tiles.Clone();
            for (int i = 0; i < tiles_cp.Length; i++)
            {
                if (tiles_cp[i].root != null && !tiles_cp[i].root.IsAlive && !IsTileFree(tiles_cp[i].Position)) 
                {
                    tiles[i] = new FreeTile(tiles_cp[i].Position);
                    continue;
                }
                tiles[i].NextTurn(this);
                life_counter++;
            }
            if (life_counter == 0)
            {
                is_there_any_life = false;
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
            tile.Start();

            tiles[GetTileIndFromPosition(position)] = tile;

            tile.Position = position;

            tile.game = this;
        }

        public void TryAddTile(Vector2 position, Tile tile)
        {
            if (IsTileFree(position))
            {
                NewTile(position, tile);
            }
        }

        public void ReplaceTile(Vector2 position, Tile tile)
        {
            tiles[GetTileIndFromPosition(position)] = tile;

            tile.Position = position;

            tile.Start();
        }

        public void MoveTile(Tile tile, Vector2 position)
        {
            if (IsTileFree(position) && position.X < field_size.X && position.Y < field_size.Y)
            {
                SeedTile _tile = (SeedTile)tile;

                tiles[GetTileIndFromPosition(_tile.Position)] = new RootTile(tile.root, tile.Position, _tile.root_gen, _tile.root_sec_gen);

                _tile.Position = position;

                tiles[GetTileIndFromPosition(position)] = _tile;
            }
        }

        private int GetTileIndFromPosition(Vector2 position)
        {
            return position.Y * field_size.X + position.X;
        }
    }
}
