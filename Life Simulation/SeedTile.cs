using System;
using System.Collections.Generic;
using System.Linq;
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

            this.game.root = root;
        }

        public SeedTile(Vector2 position, Game game)
        {
            Position = position;

            Construct('&', ConsoleColor.Cyan, 0, 2, new Root());

            this.game = game;

            this.game.root = root;
        }

        public byte[] root_gen = new byte[4];
        public byte[] root_sec_gen = new byte[4];

        private byte[] gen = new byte[10];
        private byte currentGen = 0;
        /*
         * GEN:
         * 0 - 3 -> movement ( watch Move method )
         * 3 - 
         */

        public override void NextTurn(Game game)
        {
            base.NextTurn(game);
            Move(0);
        }

        private void Move(int dir)
        {
            Vector2 move = new Vector2();
            switch (dir)
            {
                case 0:
                    move = new Vector2(1, 0); break;

                case 1:
                    move = new Vector2(-1, 0); break;

                case 2:
                    move = new Vector2(0, 1); break;

                case 3:
                    move = new Vector2(0, -1); break;
            }

            game.MoveTile(this, Position + move);
        }

        private void ReadGen()
        {
            if (gen[currentGen] <= 3) 
            {
                Move(gen[currentGen]);
            }

            else if (gen[currentGen] <= 4)
            {

            }
        }
    }
}
