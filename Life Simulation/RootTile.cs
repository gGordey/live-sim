using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_Simulation
{
    class RootTile : Tile
    {

        private byte[] gen = new byte[4];

        private byte[] second_gen = new byte[4];

        public RootTile(Root root)
        {
            this.root = root;

            Construct('*', ConsoleColor.DarkYellow);
        }

        public RootTile(Root root, Vector2 pos, byte[] gen, byte[] second_gen)
        {
            this.gen = gen;
            this.root = root;
            this.second_gen = second_gen;
            this.Position = pos;

            Construct('*', ConsoleColor.DarkYellow);
        }

        private Vector2[] directions = { new Vector2(0, -1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0) };

        /*
         * 
         * GEN:
         * 0 - root
         * 1 - leaf
         * 2 - saving tile
         */

        private byte currentStep = 0;

        public override void NextTurn(Game game)
        {
            base.NextTurn(game);

            for (byte i = 0; i < 4; i++)
            {
                if (game.IsTileFree(Position+directions[i]))
                {
                    currentStep = i;
                    break;
                }
            }

            if (currentStep <= 3)
            {
                Grow(currentStep);
            }
        }

        private void Grow(int ind)
        {
            game.TryAddTile(Position + directions[currentStep], ReadGen(currentStep));
        }
        
        private Tile ReadGen(int ind) 
        {

            switch (gen[ind])
            { 
                case 0: return new RootTile(root, Position + directions[ind], second_gen, gen);

                case 1: return new LeafTile(root);

                case 2: return new SavingTile(root);

                default: return new FreeTile();
            }
        }
    }
}
