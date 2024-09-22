using System;
using System.Threading;

namespace Life_Simulation
{
    class Game
    {
        public float SunLevel = 1.0f;

        private static Vector2 field_size = new Vector2 (80, 50);

        private static byte spaceBetweenTiles = 5;

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
                
                // for (int i = 0; i < 1000; i++)
                // {
                //     NewTurn();
                // } 
                // Thread.Sleep(10000);

                while (is_there_any_life)
                {
                    NewTurn();

                    DrawTiles();

                    Thread.Sleep(600);
                }
            }
        }

        private void UpdateTiles()
        {
            int life_counter = 0;

            Tile[] tiles_cp = (Tile[])tiles.Clone();

            for (int i = 0; i < tiles_cp.Length; i++)
            {
                if (IsTileFree(tiles_cp[i].Position) || !tiles_cp[i].IsAlive || (tiles_cp[i].root != null && !tiles_cp[i].root.IsAlive))//((tiles_cp[i].root != null && !tiles_cp[i].root.IsAlive) || IsTileFree(tiles_cp[i].Position))
                {
                    tiles[i] = new FreeTile(tiles_cp[i].Position);
                    continue;
                }
                tiles_cp[i].NextTurn(this);
            }
            for (int i = 0; i < tiles_cp.Length; i++)
            {
                //tiles_cp[i].NextTurn(this);
                if (!IsTileFree(tiles[i].Position)) { life_counter++; }
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
            if (!IsTileInField(position)) { return false; }

            return tiles[GetTileIndFromPosition(position)].GetType() == typeof(FreeTile);
        }

        private void FillMap()
        {
            for (int x = 0, y = 0; ; x++)
            {
                if ( x == field_size.X )
                {
                    x = 0;
                    ++y;
                }

                if (y == field_size.Y) { break; }

                tiles[GetTileIndFromPosition(new Vector2(x, y))] = new FreeTile(x, y);   
            }
        }

        private void NewTile(Vector2 position, Tile tile)
        {
            tile.Start();

            tiles[GetTileIndFromPosition(position)].IsPlased = false;

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
            if (!IsTileInField(position)) {return;}
            if (tiles[GetTileIndFromPosition(position)].GetType() == typeof(SeedTile)) { return; }

            tiles[GetTileIndFromPosition(position)].IsPlased = false;

            tiles[GetTileIndFromPosition(position)] = tile;

            tile.Position = position;

            tile.Start();
        }

        public void MoveTile(Tile tile, Vector2 position)
        {
            if (IsTileFree(position) && IsTileInField(position))
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

        private bool IsTileInField(Vector2 position)
        {
            return position.X >= 0 && position.X < field_size.X && position.Y >= 0 && position.Y < field_size.Y;
        }
    }
}
