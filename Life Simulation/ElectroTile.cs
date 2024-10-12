using System;

namespace Life_Simulation
{
    class MineralTile : ProdusingTile
    {
        public MineralTile (Root root)
        {
            Construct('$', ConsoleColor.DarkBlue, 7, 1.5f, root, 104);
        }

        public override void ProduseEnergy()
        {
            root.Minerals += 2.3f;
            if (root.Minerals > 7 && !root.seed.is_sleeping)
            {
                int min_take = new Random ().Next(1);
                root.Minerals -= min_take;
                root.Energy += min_take * game.OverallEnergy * Game.EnergyPerMineral;
            }
        }
    }
}