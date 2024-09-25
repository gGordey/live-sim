using System;
using System.Runtime.InteropServices;
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

        private List<Tile> UpdatedTiles = new List<Tile> ();

        

        private Canvas canvas = new Canvas();

        private bool is_there_any_life = true;

        public int generation = 1;
        public int turn = 1;
        public int max_turn = 0;

        public void NewTurn()
        {
            UpdateTiles();
        }

        private TerminalInput terminalInput;

        public bool is_simulated;
        public int amd_of_sim_turns, breakpoin_turn, turns_between_draw, draw_time, not_drawing_turn;
        public void Start()
        {
            terminalInput = new TerminalInput(this);

            canvas.fieldWidth = field_size.X;
            canvas.fieldHeight = field_size.Y;

            amd_of_sim_turns = 0;
            breakpoin_turn = 0;
            is_simulated = false;

            draw_time = 200;
            not_drawing_turn = 1;

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

                        if (i % 50 == 0) { Console.Clear(); Console.Write("turn: "+turn+"\nmax turn: "+max_turn+"\nprogress: "+((float)i / amd_of_sim_turns * 100)+" %\nsim "+generation);}

                        if (turn > breakpoin_turn) {break;}

                        if (Console.KeyAvailable) 
                        {
                            terminalInput.Start(); 
                            CloneTilesToUpdatedTiles();
                        }
                    } 
                    is_simulated = true;
                }

                while (is_there_any_life && is_simulated)
                {
                    for (int i = 0; i < not_drawing_turn; i++)
                    {
                        SimulateTurn();
                        if (Console.KeyAvailable) {terminalInput.Start(); CloneTilesToUpdatedTiles(); break;}
                    }
                    
                    DrawTiles();

                    Thread.Sleep(draw_time);
                }
                generation++;
                turn = 0;
            }
        }
        private void CloneTilesToUpdatedTiles()
        {
            foreach (Tile tile in tiles)
            {
                UpdatedTiles.Add(tile);
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
                if (!tiles_cp[i].IsAlive || (tiles_cp[i].root != null && !tiles_cp[i].root.IsAlive))//((tiles_cp[i].root != null && !tiles_cp[i].root.IsAlive) || IsTileFree(tiles_cp[i].Position))
                {
                    tiles[i] = new FreeTile(tiles_cp[i].Position);
                    UpdatedTiles.Add(tiles[i]);
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
            
            canvas.DrawAllTiles(UpdatedTiles);

            UpdatedTiles = new List<Tile>();
        }

        public bool IsTileFree(Vector2 position)
        {
            if (!IsTileInField(position)) { return false; }

            return tiles[GetTileIndFromPosition(position)].GetType() == typeof(FreeTile);
        }

        private void FillMap()
        {
            Console.Clear();
            for (int x = 0, y = 0; ; x++)
            {
                if ( x == field_size.X )
                {
                    x = 0;
                    ++y;
                }

                if (y == field_size.Y) { break; }

                tiles[GetTileIndFromPosition(new Vector2(x, y))] = new FreeTile(x, y);   
                UpdatedTiles.Add(tiles[GetTileIndFromPosition(new Vector2(x, y))]);
            }
            FillLife();
        }

        private void FillLife()
        {
            for (byte x = spaceBetweenTiles, y = spaceBetweenTiles; ; x += spaceBetweenTiles)
            {
                if (x >= field_size.X)
                {
                    y += spaceBetweenTiles;
                    x = spaceBetweenTiles;
                    if (y >= field_size.Y) { break; }
                }
                NewTile(new Vector2 (x, y), new SeedTile (new Vector2(x,y)));
            }
        }

        private void NewTile(Vector2 position, Tile tile)
        {
            tile.game = this;

            tile.Start();

            tiles[GetTileIndFromPosition(position)] = tile;

            tile.Position = position;

            UpdatedTiles.Add(tile);
        }

        public void TryAddTile(Vector2 position, Tile tile)
        {
            if (IsTileFree(position) && IsTileInField(position))
            {
                NewTile(position, tile);
            }
            else
            {
                if (IsTileInField(position) && tile.GetType() == typeof(KillerTile) && tiles[GetTileIndFromPosition(position)].GetType() != typeof(RootTile))
                {
                    tiles[GetTileIndFromPosition(position)].Die();

                    if (tiles[GetTileIndFromPosition(position)].GetType() == typeof(SeedTile))
                    {
                        tile.root.Energy += tiles[GetTileIndFromPosition(position)].root.Energy;
                    }

                    NewTile(position, tile);

                    ((KillerTile)tile).OnKill();
                }
            }
        }

        public void ReplaceTile(Vector2 position, Tile tile)
        {
            if (!IsTileInField(position)) {return;}
            if (tiles[GetTileIndFromPosition(position)].GetType() == typeof(SeedTile)) { return; }

            NewTile(position, tile);
        }

        public void MoveTile(Tile tile, Tile replace, Vector2 position)
        {
            if (IsTileFree(position) && IsTileInField(position))
            {
                //SeedTile _tile = (SeedTile)tile;
                NewTile(tile.Position, replace);
                NewTile(position, tile);
            }
        }

        private int GetTileIndFromPosition(Vector2 position)
        {
            return position.Y * field_size.X + position.X;
        }

        private bool IsTileInField(Vector2 position)
        {
            return position.X > 0 && position.X < field_size.X && position.Y > 0 && position.Y < field_size.Y;
        }

        public Tile GetTile(Vector2 position)
        {
            if (!IsTileInField(position)) { return new FreeTile(new Vector2 ()); }
            return tiles[GetTileIndFromPosition(position)];
        }
    }
}
