using System;
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

            Construct('&', ConsoleColor.Cyan, 0, 3, new Root(), 120);

            root.seed = this;

            root.StarterEnergy = 101;

            defaultGen = (byte) new Random().Next(gens_amnd);
            currentGen = defaultGen;
        }

        public SeedTile(Vector2 position, byte[][] base_gen, byte[] rfg, byte[] rsg, Vector2 dir, Root previous_root, byte cg)
        {
            Position = position;

            Construct('&', ConsoleColor.Cyan, 0, 3, new Root(), 120);

            root.seed = this;

            this.base_gen = base_gen;

            root_base_gen = rfg;
            root_base_second_gen = rsg;

            is_flying = true;

            flying_dir = dir;

            root.StarterEnergy = 25;

            previous_root.NextGeneration.Add(this);

            this.previous_root = previous_root;

            defaultGen = cg;

            if (new Random().Next(100) < 12) {defaultGen = (byte) new Random().Next(gens_amnd);}

            currentGen = defaultGen;
        }

        private bool is_flying = false;
        private Vector2 flying_dir;

        private bool is_sleeping = false;
        private byte sleep_for = 0;

        private bool is_redirth = false;

        private byte[][] base_gen;
        public byte[] root_gen = new byte[4];
        private byte[] root_base_gen;

        public byte[] root_sec_gen = new byte[4];
        private byte[] root_base_second_gen;


        private static byte gen_size = 10;
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
         * 0 - 3 -> shoot seed dir - [8]
         */

        private float mut = 12f;

        private Root previous_root;

        public override void NextTurn(Game game)
        {
            if (Sleep()) { return; }

            base.NextTurn(game);

            if (is_flying) { Fly(); return; }

            root.NewTurn();
            
            ReadGen();
        }

        public override void Start()
        {
            base.Start();
            for (int i = 0; i < game.turn; i += 100)
            {
                mut -= mut / 10;
            }

            NewGen(mut);

            NewRootGen();
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
            if (game.IsTileFree(Position + flying_dir)) { game.MoveTile(this, new FreeTile(Position), Position+flying_dir); }
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
                else { gen[i] = new byte[10]; }

                for (int j = 0; j < 10; j++)
                {
                    if (base_gen == null || r.Next(100) < mutatability)
                    {

                        if (j == 5) { gen[i][j] = (byte)r.Next(9); }
                        else if (j == 4 || j == 3) { gen[i][j] = (byte)r.Next(255); }
                        else if (j == 1) { gen[i][j] = (byte)r.Next(6); }
                        else {gen[i][j] = (byte)r.Next(5);}
                    }
                    else
                    {
                        gen[i][j] = base_gen[i][j];
                    }
                }
            }
        }
        private void NewRootGen()
        {
            Random r = new Random ();

            for (int i = 0; i < 4; i++)
            {
                if (root_base_gen != null)
                {
                    root_gen[i] = root_base_gen[i];
                    root_sec_gen[i] = root_base_second_gen[i];
                }
                if (root_base_gen == null || r.Next(100) < 3.5f) 
                {
                    root_gen[i] = (byte)r.Next(7);
                    root_sec_gen[i] = (byte)r.Next(7);
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
            float condition = condition_byte / 2.55f / 6.5f;
            if (condition <= 3)
            {
                return game.GetTile(GetPositionFromInd((byte)condition)).GetType() == GetTlileFromInd((byte)(param % 4)).GetType();
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
                return root.Energy < param / 2.55;
            }
            else if (condition <= 10)
            {
                return root.EnergyConsuming > param / 2.55;
            }
            else if (condition <= 11)
            {
                return root.EnergyConsuming < param / 2.55;
            }
            else if (condition <= 12)
            {
                return root.NextGeneration.Count < param / 2.55 / 10;
            }
            else if (condition <= 13)
            {
                return root.NextGeneration.Count > param / 2.55 / 10;
            }
            else if (condition <= 14)
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
            if (task == 0) { root.Energy -= 6.5f; Age -= 5; }
            else if (task == 1 && !is_redirth) 
            { 
                root.Die(); Age /= 2;  
                float enrg = root.Energy + root.StarterEnergy / 1.5f;
                root = new Root();
                root.seed = this;
                root.StarterEnergy = enrg;
                is_redirth = true;
            }
            else if (task == 2) { Sleep(10); }
            else if (task == 3) { Die(); }
            else if (task == 4) { game.TryAddTile(Position+GetPositionFromInd(param), new KillerTile (root)); }
            else if (task == 5) { base_gen = gen; NewGen(2f);}
        }

        private void ReadGen()
        {
            if  (currentStep == 0) { Move(gen[currentGen][0]); }

            else if (currentStep == 1) { GrowTile(gen[currentGen][1], gen[currentGen][2]); }

            else if (currentStep == 3) 
            {
                if (ReadIf(gen[currentGen][3], gen[currentGen][4]))
                {
                    if (gen[currentGen][5] < gen.Length) {currentGen = gen[currentGen][5]; currentStep = 0;}
                }
            }

            else if (currentStep == 6) { Command(gen[currentGen][6], gen[currentGen][7]); }

            if (currentStep < 7) {currentStep++;}
            else {currentStep = 0;}
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
                    return new ElectroTile (root);

                case 4:
                    return new KillerTile (root);

                case 5:
                    return new InvestingTile (root);
                
                default:
                    return new FreeTile (Position);
            }
        }
    }
}
