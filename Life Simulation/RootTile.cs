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

        private byte[][] gen = new byte[2][];

        private byte[][] second_gen = new byte[2][];

        private int life_length = 120;

        public RootTile(Root root)
        {
            Construct('*', ConsoleColor.DarkYellow, 0.2f, 0, root, life_length);
        }

        public RootTile(Root root, Vector2 pos, byte[][] gen, byte[][] second_gen)
        {
            this.gen = gen;
            this.second_gen = second_gen;
            this.Position = pos;

            Construct('*', ConsoleColor.DarkYellow, 0.2f, 0, root, life_length);
        }

        private Vector2[] directions = { new Vector2(0, -1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0) };

        private byte currentStep = 0;

        public override void NextTurn(Game game)
        {
            for (int i = 0; i < root.seed.LifeSpeedCount(); i++)
            {
                base.NextTurn(game);

                Grow();

                currentStep++;

                if (currentStep > 3)
                {
                    currentStep = 0;
                }
            }
        }
        private void Grow()
        {
            Tile new_tile = ReadGen(currentStep);

            if (root.Energy > new_tile.min_energy_level)
            {
                new_tile.Position = Position + directions[currentStep];

                game.TryAddTile(Position + directions[currentStep], new_tile);
            }
        }
        
        private Tile ReadGen(int ind) 
        {

            switch (gen[root.seed.root_gen_pair][ind])
            { 
                case 0: return new RootTile(root, Position + directions[ind], second_gen, gen);

                case 1: return new LeafTile(root);

                case 2: return new SavingTile(root);

                case 3: return new FlowerTile(root);

                case 4: return new MineralTile(root);

                case 5: return new KillerTile(root);

                case 6: return new LeafTile(root);// return new InvestingTile (root);

                case 7: return new OrganicTile(root);

                default: return new FreeTile(Position);
            }
        }
    }
}
