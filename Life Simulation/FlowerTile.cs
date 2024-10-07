using System;

namespace Life_Simulation
{
    class FlowerTile : Tile
    {
        private byte grown_level = 0;
        private const byte needed_level = 6;
        private const float energy_for_growing = 3.4f;

        private byte base_starter_gen_ind;

        private int life_length = 32;
        public FlowerTile (Root root)
        {
            Construct('@', ConsoleColor.Magenta, 5, 0.2f, root, life_length);
        }
        public FlowerTile (Root root, Vector2 pos)
        {
            Construct('@', ConsoleColor.Magenta, 5, 0.2f, root, life_length);

            Position = pos;
        }

        public override void NextTurn(Game game)
        {
            for (int i = 0; i < root.seed.LifeSpeedCount(); i++)
            {
                base.NextTurn(game);

                if (root.Energy >= energy_for_growing*2) { grown_level++; root.Energy -= energy_for_growing; }

                if (grown_level >= needed_level) { Grow();}
            }
        }

        public void Grow()
        {
            game.ReplaceTile
            (
                Position, 

                new SeedTile(Position, root)
            );   
            root.EnergyConsuming += 1;
        }
    }
}