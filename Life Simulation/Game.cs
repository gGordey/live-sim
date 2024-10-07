using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Life_Simulation
{
    class Game
    {
        private static float def_sun_level = 1.9f;
        public float SunLevel = def_sun_level;

        public float OverallEnergy = 4.8f;

        public static float oxigen_default = 5f; 
        public static float carbon_dioxide_default = 5f; 

        public float OxigenLevel;
        public float CarbonDioxideLivel;

        public static float EnergyPerMineral = 3f;

        private static Vector2 field_size = new Vector2 (190, 100);

        private static byte spaceBetweenTiles = 5;

        private Tile[] tiles = new Tile[field_size.X * field_size.Y];

        private List<Tile> UpdatedTiles = new List<Tile> ();

        public enum DrawMode
        {
            Default,
            Clan,
            FirstGen
        }

        public DrawMode OutputMode = DrawMode.Default;

        private Canvas canvas = new Canvas();

        public bool is_there_any_life = true;

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

            is_there_any_life = false;

            while(true)
            {   
                CarbonDioxideLivel = carbon_dioxide_default;
                OxigenLevel = oxigen_default;

                if (!is_there_any_life)
                {
                    is_there_any_life = true;
                    FillMap();
                }
                if (!is_simulated)
                {  
                    for (int i = 0; i < amd_of_sim_turns; i++)
                    {
                        UpdatedTiles.Clear();

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
                    CloneTilesToUpdatedTiles();
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
                if (!is_there_any_life)
                {
                    generation++;
                    turn = 0;
                }
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
                if (!tiles_cp[i].IsAlive)//((tiles_cp[i].root != null && !tiles_cp[i].root.IsAlive) || IsTileFree(tiles_cp[i].Position))
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

            if (life_counter <= 0)
            {
                is_there_any_life = false;
            }
        }

        private void DrawTiles()
        {
            canvas.gen = generation;
            canvas.turn = turn;
            canvas.max_turn = max_turn;
            canvas.co2 = CarbonDioxideLivel;
            canvas.o2 = OxigenLevel;
            
            canvas.DrawAllTiles(UpdatedTiles, OutputMode);

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

                // NewWall();

                UpdatedTiles.Add(tiles[GetTileIndFromPosition(new Vector2(x, y))]);
            }            

            FillLife();
        }

        private void FillLife()
        {
            for (byte x = 0, y = 0; ; x += spaceBetweenTiles)
            {
                if (x >= field_size.X)
                {
                    y += spaceBetweenTiles;
                    x = spaceBetweenTiles;
                    if (y >= field_size.Y) { break; }
                }
                if (x == 95) { continue; }
                NewTile(new Vector2 (x, y), new SeedTile (new Vector2(x,y)));

                if (x < 95) { tiles[GetTileIndFromPosition(new Vector2 (x, y))].ClanColor = ConsoleColor.Red; }

                else { tiles[GetTileIndFromPosition(new Vector2 (x, y))].ClanColor = ConsoleColor.Blue; }
            }
        }

        public void BreakWall()
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].GetType() == typeof(WallTile))
                {
                    tiles[i] = new FreeTile (tiles[i].Position);
                }
            }
        }
        public void NewWall()
        {
            for  (int i = 0; i < field_size.Y; i++)
            {
                tiles[GetTileIndFromPosition(new Vector2 (95, i))] = new WallTile (new Vector2 (95, i));
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

        public bool TryDeleteTile(Vector2 position)
        {
            if (IsTileInField(position) && tiles[GetTileIndFromPosition(position)].GetType() != typeof(WallTile))
            {
                tiles[GetTileIndFromPosition(position)].Die();

                tiles[GetTileIndFromPosition(position)] = new FreeTile (position);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void TryAddTile(Vector2 position, Tile tile)
        {
            if (IsTileFree(position) && IsTileInField(position))
            {
                NewTile(position, tile);
            }
            else
            {
                if (IsTileInField(position) && tile.GetType() == typeof(KillerTile) && tiles[GetTileIndFromPosition(position)].GetType() != typeof(WallTile))
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
            if (tiles[GetTileIndFromPosition(position)].GetType() == typeof(WallTile)) { return; }

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

        public float CountEnergy(Tile tile)
        {
            if (tile.GetType() == typeof(LeafTile))
            {
                return 3.2f * SunLevel * OverallEnergy;
            }
            // else if (tile.GetType() == typeof(ElectroTile))
            // {
            //     return (110-tile.Age) * 0.09f * OverallEnergy;
            // }
            else if (tile.GetType() == typeof(InvestingTile))
            {
                return tile.Age-5 * OverallEnergy;
            }
            return 1;
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
