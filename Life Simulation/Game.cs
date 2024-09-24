using System;
using System.Threading;

namespace Life_Simulation
{
    class Game
    {
        private static float def_sun_level = 1.9f;
        public float SunLevel = def_sun_level;

        private static Vector2 field_size = new Vector2 (95, 51);

        private static byte spaceBetweenTiles = 5;

        private Tile[] tiles = new Tile[field_size.X * field_size.Y];

        private Tile[] new_turn_tiles = new Tile[field_size.X * field_size.Y];

        private Canvas canvas = new Canvas();

        private bool is_there_any_life = true;

        public int generation = 1;
        public int turn = 1;
        public int max_turn = 0;

        public void NewTurn()
        {
            UpdateTiles();
        }

        public void Start()
        {
            canvas.fieldWidth = field_size.X;
            canvas.fieldHeight = field_size.Y;
            int amd_of_sim_turns = 0;
            bool is_simulated = false;
            while(true)
            {   
                is_there_any_life = true;
                FillMap();
                if (!is_simulated)
                {  
                    for (int i = 0; i < amd_of_sim_turns; i++)
                    {
                        SimulateTurn();

                        if (!is_there_any_life) { generation++; turn = 0; FillMap(); is_there_any_life = true; }

                        if (i % 50 == 0) { Console.Clear(); Console.Write("turn: "+turn+"\nprogress: "+((float)i / amd_of_sim_turns * 100)+" %\nsim "+generation);}

                        if (turn > 2200) {break;}
                    } 
                    is_simulated = true;
                }

                while (is_there_any_life)
                {
                    SimulateTurn();

                    DrawTiles();

                    Thread.Sleep(200);
                }
                generation++;
                turn = 0;
            }
        }

        private void SimulateTurn()
        {
            NewTurn();

            turn++;

            if (turn >= max_turn) { max_turn = turn; }
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
            }

            tiles_cp = (Tile[])tiles.Clone();
            for (int i = 0; i < tiles_cp.Length; i++)
            {
                tiles_cp[i].NextTurn(this);
                if (tiles_cp[i].GetType() == typeof(SeedTile)) { life_counter++; }
            }
            if (life_counter == 0)
            {
                is_there_any_life = false;
            }
        }

        private void DrawTiles()
        {
            canvas.gen = generation;
            canvas.turn = turn;
            canvas.max_turn = max_turn;
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
            FillLife();
        }

        private void FillLife()
        {
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
            else
            {
                if (IsTileInField(position) && tile.GetType() == typeof(KillerTile) && tiles[GetTileIndFromPosition(position)].GetType() != typeof(RootTile))
                {
                    tiles[GetTileIndFromPosition(position)].Die();

                    NewTile(position, tile);

                    ((KillerTile)tile).OnKill();
                }
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

        public void MoveTile(Tile tile, Tile replace, Vector2 position)
        {
            if (IsTileFree(position) && IsTileInField(position))
            {
                SeedTile _tile = (SeedTile)tile;

                tiles[GetTileIndFromPosition(_tile.Position)] = replace;//new RootTile(tile.root, tile.Position, _tile.root_gen, _tile.root_sec_gen);

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

        public Tile GetTile(Vector2 position)
        {
            if (!IsTileInField(position)) { return new FreeTile(new Vector2 ()); }
            return tiles[GetTileIndFromPosition(position)];
        }
    }
}
