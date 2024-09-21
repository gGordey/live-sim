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
        public SeedTile(Game game)
        {
            Construct('&', ConsoleColor.Cyan, 0, 2, new Root());

            this.game = game;
        }

        public SeedTile(Vector2 position, Game game)
        {
            Position = position;

            Construct('&', ConsoleColor.Cyan, 0, 2, new Root());

            this.game = game;
        }

        public byte[] root_gen = new byte[4];
        public byte[] root_sec_gen = new byte[4];

        private static byte gen_size = 10;

        private byte[] gen = new byte[gen_size];
        private byte currentGen = 0;
        /*
         * GEN:
         * 0 - 3 -> movement ( watch Move method ) [0]
         * 0 - 3 -> plant [1]
         * 0 - 3 -> priority planting dir [2]
         * 0 - 10 -> sleep for N moves [3]
         */

        public override void NextTurn(Game game)
        {
            base.NextTurn(game);
            
            ReadGen();
        }

        public override void Start()
        {
            base.Start();

            NewGen();

            NewRootGen();
        }

        private void NewGen()
        {
            Random r = new Random ();

            for (int i = 0; i < gen_size; i++)
            {
                gen[i] = (byte)r.Next(3);
            }
        }
        private void NewRootGen()
        {
            Random r = new Random ();

            for (int i = 0; i < 4; i++)
            {
                root_gen[i] = (byte)r.Next(3);
                root_sec_gen[i] = (byte)r.Next(3);
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
                
                default:
                    new_tile = new FreeTile ();
                    break;
            }

            if (game.IsTileFree(GetPositionFromInd(priority)))
            {
                game.TryAddTile(Position + GetPositionFromInd(priority), new_tile);
            }
            else
            {
                for (byte i = 0; i < 4; i++)
                {
                    if (game.IsTileFree(GetPositionFromInd(i)))
                    {
                        game.TryAddTile(Position + GetPositionFromInd(i), new_tile);
                    }
                }
            }


        }

        private void ReadGen()
        {
            if  (currentGen == 0) { Move(gen[0]); }

            else if (currentGen == 1) { GrowTile(gen[1], gen[2]); }

            else if (currentGen == 3) {}

            if (currentGen < 4) {currentGen++;}

            else {currentGen = 0;}
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
