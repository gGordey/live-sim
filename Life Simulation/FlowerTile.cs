using System;

namespace Life_Simulation
{
    class FlowerTile : Tile
    {
        private byte grown_level = 0;
        private const byte needed_level = 6;
        private const float energy_for_growing = 3.1f;

        private int life_length = 32;
        public FlowerTile (Root root)
        {
            Construct('@', ConsoleColor.Magenta, 5, 1, root, life_length);
        }
        public FlowerTile (Root root, Vector2 pos)
        {
            Construct('@', ConsoleColor.Magenta, 5, 1, root, life_length);

            Position = pos;
        }

        public override void NextTurn(Game game)
        {
            base.NextTurn(game);

            if (root.Energy >= energy_for_growing*2) { grown_level++; root.Energy -= energy_for_growing; }

            if (grown_level >= needed_level) { Grow();}
        }

        public void Grow()
        {
            game.ReplaceTile
            (
                Position, 

                new SeedTile
                (
                    Position,
                    root.seed.gen,
                    root.seed.root_gen, 
                    root.seed.root_sec_gen, 
                    root.seed.GetPositionFromInd(root.seed.gen[root.seed.currentGen][7]),
                    root
                )
            );   
            root.EnergyConsuming += 1;
        }
    }
}