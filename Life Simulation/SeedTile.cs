﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Life_Simulation
{
    class SeedTile : Tile
    {
        public SeedTile(Vector2 position)
        {
            Position = position;

            Construct('&', ConsoleColor.Cyan, 0, 3, new Root(), 240);

            root.seed = this;

            root.StarterEnergy = 101;

            defaultGen = (byte) new Random().Next(gens_amnd);
            currentGen = defaultGen;

            flying_dir = GetPositionFromInd((byte) new Random().Next(3));

            flying_dist = 10;
            left_fly = 10;
        }

        public SeedTile(Vector2 position, Root previous_root)
        {
            Position = position;

            Construct('&', ConsoleColor.Cyan, 0, 3, new Root(), 240);

            root.seed = this;

            base_gen = previous_root.seed.gen;

            root_base_gen = previous_root.seed.root_gen;
            root_base_second_gen = previous_root.seed.root_sec_gen;

            is_flying = true;

            flying_dir = previous_root.seed.flying_dir;

            flying_dist = previous_root.seed.flying_dist;

            if (new Random().Next(100) < 10) { flying_dist += new Random().Next(2)-1; }

            left_fly = flying_dist;

            root.StarterEnergy = 50;//25;

            previous_root.NextGeneration.Add(this);

            this.previous_root = previous_root;

            defaultGen = previous_root.seed.defaultGen;

            if (new Random().Next(100) < 12) {defaultGen = (byte) new Random().Next(gens_amnd);}

            currentGen = defaultGen;

            ClanColor = previous_root.seed.ClanColor;
            GenColor = previous_root.seed.GenColor;

            LifeSpeed = previous_root.seed.LifeSpeed + (float)(new Random().NextDouble()*0.1-0.05);
        }

        public float LifeSpeed = 1f;

        private bool is_flying = false;
        private Vector2 flying_dir;
        public int flying_dist;
        private int left_fly;

        private bool is_sleeping = false;
        private byte sleep_for = 0;

        private bool is_rebirth = false;

        private byte[][] base_gen;
        public byte[][] root_gen = new byte[2][];
        private byte[][] root_base_gen;

        public byte[][] root_sec_gen = new byte[2][];
        private byte[][] root_base_second_gen;

        public int root_gen_pair = 0;

        private static byte gen_size = 12;
        private static byte gens_amnd = 10;

        public byte[][] gen = new byte[gens_amnd][];

        public byte currentGen;
        public byte defaultGen;
        private byte currentStep = 0;
        /*
         * GEN:
         * 0 - 3 -> movement ( watch Move method ) - [0]
         * 0 - 3 -> plant - [1]
         * 0 - 3 -> priority planting dir - [2]
         * 0 - 3 -> if ( watch ReadIf method ) - [3]
         * 0 - 3 -> if param - [4]
         * 0 - 3 -> switch current gen to N if [3] is true - [5]
         * 0 - 3 -> extra command - [6]
         * 0 - 3 -> command param - [7]
         * 
         */
        private Root previous_root;

        public override void NextTurn(Game game)
        {
            LifeSpeedProcess();

            for (int i = 0; i < root.seed.LifeSpeedCount(); i++)
            {
                if (!Sleep()) 
                {
                    if (is_flying) { Fly(); return; }

                    base.NextTurn(game);

                    root.NewTurn();
                    
                    ReadGen();
                }
            }
        }

        public override void Start()
        {
            base.Start();

            if (previous_root != null)            
            {
                for (byte i = 0; i < 4; i++)
                {
                    if (game.GetTile(Position+GetPositionFromInd(i)).GetType() == typeof(InvestingTile))
                    {
                        root.tiles.Add(game.GetTile(Position+GetPositionFromInd(i)));
                        previous_root.tiles.Remove(game.GetTile(Position+GetPositionFromInd(i)));
                        root.StarterEnergy += 10;
                    }    
                }
            }

            NewGen(1.2f);

            NewRootGen(1.2f);
        }

        public override void Die()
        {
            base.Die();
            root.Die();
            if (previous_root != null)
            {
                previous_root.NextGeneration.Remove(this);
            }
        }

        private void Fly()
        {
            if (left_fly > 0 && game.IsTileFree(Position + flying_dir)) { game.MoveTile(this, new FreeTile(Position), Position+flying_dir); left_fly--; }
            else {is_flying = false; }
        }

        private bool Sleep()
        {
            if (is_sleeping)
            {
                Color = ConsoleColor.Yellow;
                if (sleep_for <= 0)
                {
                    is_sleeping = false;
                    root.EnergyConsuming += 3;
                    return false;
                }
                sleep_for--;
                return true;
            }
            else
            {
                Color = ConsoleColor.Cyan;
                return false;
            }
        }
        private bool Sleep(byte length)
        {
            is_sleeping = true;
            sleep_for = length;
            root.EnergyConsuming -= 3;
            return true;
        }

        private void NewGen(float mutatability)
        {
            Random r = new Random ();

            for (int i = 0; i < gens_amnd; i++)
            {
                if (base_gen != null) { gen[i] = base_gen[i]; }
                else { gen[i] = new byte[gen_size]; }

                for (int j = 0; j < gen_size; j++)
                {
                    if (base_gen == null || r.Next(100) < mutatability)
                    {
                        gen[i][j] = (byte)r.Next(255);
                    }
                    else
                    {
                        gen[i][j] = base_gen[i][j];
                    }
                }
            }
        }
        private void NewRootGen(float mutatability)
        {
            Random r = new Random ();

            root_gen[0] = new byte[4];
            root_gen[1] = new byte[4];

            root_sec_gen[0] = new byte[4];
            root_sec_gen[1] = new byte[4];

            if (root_base_gen != null)
            {
                root_gen[0] = root_base_gen[0];
                root_sec_gen[0] = root_base_second_gen[0];

                root_gen[1] = root_base_gen[1];
                root_sec_gen[1] = root_base_second_gen[1];
            }

            for (int i = 0; i < 4; i++)
            {
                if (root_base_gen == null || r.Next(100) < mutatability) 
                {
                    root_gen[0][i] = (byte)r.Next(7);
                    root_sec_gen[0][i] = (byte)r.Next(7);

                    root_gen[1][i] = (byte)r.Next(7);
                    root_sec_gen[1][i] = (byte)r.Next(7);
                }
            }
        }

        private void Move(byte dir)
        {
            Vector2 move = new Vector2();
            
            move = GetPositionFromInd(dir);
            
            game.MoveTile(this,new RootTile(root, Position, root_gen, root_sec_gen), Position + move);
        }

        private void GrowTile(int tile, byte priority)
        {
            Tile new_tile = GetTlileFromInd((byte)tile);

            if (root.Energy < new_tile.min_energy_level*1.5f) {return;}

            if (game.IsTileFree(GetPositionFromInd(priority)))
            {
                new_tile.Position = Position + GetPositionFromInd(priority);
                game.TryAddTile(Position + GetPositionFromInd(priority), new_tile);
            }
            else
            {
                for (byte i = 0; i < 4; i++)
                {
                    if (game.IsTileFree(GetPositionFromInd(i)))
                    {
                        new_tile.Position = Position + GetPositionFromInd(i);
                        game.TryAddTile(Position + GetPositionFromInd(i), new_tile);
                    }
                }
            }
        }

        private bool ReadIf(byte condition_byte, byte param)
        {
            byte condition = (byte)(condition_byte % 12);
            if (condition <= 3)
            {
                return game.GetTile(GetPositionFromInd(condition)).GetType() == GetTlileFromInd((byte)(param % 4)).GetType();
            }
            else if (condition <= 7)
            {
                return game.IsTileFree(GetPositionFromInd((byte)(condition-4)));
            }
            else if (condition <= 8)
            {
                return root.Energy > param / 2.55;
            }
            else if (condition <= 9)
            {
                return root.EnergyConsuming > param / 2.55;
            }
            else if (condition <= 11)
            {
                return root.NextGeneration.Count > param / 2.55 / 10;
            }
            else if (condition <= 12)
            {
                return Age > param / 2.55 / 10;
            }
            else
            {
                return true;
            }
        }

        private void Command(byte task, byte param)
        {
            task %= 13; // tasks count ++

            if (task == 0) { Move((byte)new Random().Next(4)); }

            else if (task == 1 && !is_rebirth) 
            { 
                root.Die(); Age /= 2;  
                float enrg = root.Energy + root.StarterEnergy / 1.5f;
                root = new Root();
                root.seed = this;
                root.StarterEnergy = enrg;
                is_rebirth = true;
            }
            else if (task == 2) { Sleep(10); }

            else if (task == 3) { Die(); }

            else if (task == 4) { game.TryAddTile(Position+GetPositionFromInd((byte)(param%7)), new KillerTile (root)); }

            else if (task == 5) 
            { 
                foreach (Tile tile in root.NextGeneration)
                {
                    tile.root.StarterEnergy += param / 2.55f / root.NextGeneration.Count; 
                }
                root.Energy -= param / 2.55f;
            }
            else if (task == 6)
            {
                Tile tile_buffer = game.GetTile(GetPositionFromInd((byte)(param%4)));
                if (game.TryDeleteTile(GetPositionFromInd((byte)(param%4))))
                {
                    root.Energy += tile_buffer.min_energy_level;
                    Move((byte)(param%4));
                }
            }
            else if (task == 7)
            {
                currentGen = (byte)(param % gens_amnd);
            }
            else if (task == 8)
            {
                GrowTile(param%7, (byte)(param%4));
            }
            else if (task == 9)
            {
                Move((byte)(param%4));
            }
            else if (task == 10)
            {
                root.Energy += root.Minerals * game.OverallEnergy * Game.EnergyPerMineral;
                root.Minerals = 0;
            }
            else if (task == 11)
            {
                if (root_gen_pair == 1) { root_gen_pair = 0; }
                else { root_gen_pair = 1; }
            }
            else if (task == 12)
            {
                if (game.OxigenLevel <= 0) {return;}
                game.OxigenLevel -= 0.03f;
                game.CarbonDioxideLivel += 0.03f;
                root.Energy += 5;
            }
        }

        private void ReadGen()
        {
            if (currentStep == 0)
            {
                Move((byte)(gen[currentGen][0]%4));
            }
            else if (currentStep == 1)
            {
                GrowTile(gen[currentGen][1]%7, (byte)(gen[currentGen][2]%4));
            }
            else if (currentStep <= 6)
            {
                if (currentStep % 2 == 1) {currentStep++;}

                Command(gen[currentGen][currentStep], gen[currentGen][currentStep+1]);
            }
            else if (currentStep == 7)
            {
                if (ReadIf(gen[currentGen][7], gen[currentGen][8]))
                {
                    Command(gen[currentGen][9], gen[currentGen][10]);
                    currentGen = gen[currentGen][11];
                }
            }
        }

        public Vector2 GetPositionFromInd(byte ind)
        {
            switch (ind)
            {
                case 0:
                    return new Vector2(1, 0);

                case 1:
                    return new Vector2(-1, 0); 

                case 2:
                    return new Vector2(0, 1);

                case 3:
                    return new Vector2(0, -1);

                default:
                    return new Vector2();
            }
        }
        public Tile GetTlileFromInd(byte ind)
        {
            switch (ind)
            {
                case 0:
                    return new LeafTile (root, Position);

                case 1:
                    return new SavingTile (root);

                case 2:
                    return new FlowerTile (root);
                
                case 3:
                    return new MineralTile (root);

                case 4:
                    return new KillerTile (root);

                case 5:
                    return new LeafTile (root, Position);
                    //return new InvestingTile (root);
                
                default:
                    return new FreeTile (Position);
            }
        }

        private float turn_counter = 0;
        public int LifeSpeedCount()
        {
            if (LifeSpeed >= 1)
            {
                if (turn_counter >= 1)
                {
                    return (int)LifeSpeed + 1;
                }
                else
                {
                    return (int)LifeSpeed;
                }
                
            }
            else // LifeSpeed < 1
            {
                if (turn_counter < 1) { return 0; }
                else { return 1; }
            }
        }
        public void LifeSpeedProcess()
        {
            if (turn_counter >= 1) { turn_counter = 0; }

            if (turn_counter <= 0)
            {
                turn_counter = LifeSpeed;
            }

            if (LifeSpeed < 1)
            {
                turn_counter += LifeSpeed;
            }
            if (LifeSpeed > 1)
            {
                turn_counter += LifeSpeed-(int)Math.Floor(LifeSpeed);
            }
        }
    }
}
