using System;
using System.Collections.Generic;
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
        }

        public SeedTile(Vector2 position, byte[][] base_gen, byte[] rfg, byte[] rsg)
        {
            Position = position;

            Construct('&', ConsoleColor.Cyan, 0, 3, new Root(), 120);

            root.seed = this;

            this.base_gen = base_gen;

            root_base_gen = rfg;
            root_base_second_gen = rsg;
        }

        private byte[][] base_gen;
        public byte[] root_gen = new byte[4];
        private byte[] root_base_gen;

        public byte[] root_sec_gen = new byte[4];
        private byte[] root_base_second_gen;


        private static byte gen_size = 10;
        private static byte gens_amnd = 3;

        public byte[][] gen = new byte[gens_amnd][];

        private byte currentGen = 0;
        private byte currentStep = 0;
        /*
         * GEN:
         * 0 - 3 -> movement ( watch Move method ) - [0]
         * 0 - 3 -> plant - [1]
         * 0 - 3 -> priority planting dir - [2]
         * 0 - 3 -> if ( watch ReadIf method ) - [3]
         * 0 - 3 -> switch current gen to N if [3] is true - [4]
         */

        public override void NextTurn(Game game)
        {
            base.NextTurn(game);

            root.NewTurn();
            
            ReadGen();
        }

        public override void Start()
        {
            base.Start();

            NewGen();

            NewRootGen();
        }

        public override void Die()
        {
            base.Die();
            root.Die();
        }

        private void NewGen()
        {
            Random r = new Random ();

            for (int i = 0; i < gens_amnd; i++)
            {
                if (base_gen != null) { gen[i] = base_gen[i]; }
                else { gen[i] = new byte[10]; }

                for (int j = 0; j < 10; j++)
                {
                    if (base_gen == null || r.Next(100) < 10)
                    {
                        gen[i][j] = (byte)r.Next(4);
                        
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
                if (root_base_gen == null || r.Next(100) < 10) 
                {
                    root_gen[i] = (byte)r.Next(5);
                    root_sec_gen[i] = (byte)r.Next(5);
                }
            }
        }

        private void Move(byte dir)
        {
            Vector2 move = new Vector2();
            
            move = GetPositionFromInd(dir);

            game.MoveTile(this, Position + move);
        }

        private void GrowTile(int tile, byte priority)
        {
            Tile new_tile;


            switch (tile)
            {
                case 0:
                    new_tile = new LeafTile (root, Position);
                    break;

                case 1:
                    new_tile = new SavingTile (root);
                    break;

                case 2:
                    new_tile = new FlowerTile (root);
                    break;
                
                case 3:
                    new_tile = new ElectroTile (root);
                    break;
                
                default:
                    new_tile = new FreeTile ();
                    break;
            }

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

        private bool ReadIf(byte ind)
        {
            switch (ind)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return !game.IsTileFree(Position+GetPositionFromInd(ind));
                
                default:
                    return false;
            }
        }

        private void ReadGen()
        {
            if  (currentStep == 0) { Move(gen[currentGen][0]); }

            else if (currentStep == 1) { GrowTile(gen[currentGen][1], gen[currentGen][2]); }

            else if (currentStep == 3) 
            {
                if (ReadIf(gen[currentGen][3]))
                {
                    if (gen[currentGen][4] < gen.Length) {currentGen = gen[currentGen][4];}
                }
            }

            if (currentStep < 4) {currentStep++;}
            else {currentStep = 0;}
        }

        private Vector2 GetPositionFromInd(byte ind)
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
    }
}
