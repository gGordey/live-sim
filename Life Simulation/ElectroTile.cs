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
            root.Minerals += 2f;
            if (root.Minerals > 5f)
            {
                int min_take = new Random ().Next(2);
                root.Minerals -= min_take;
                root.Energy += min_take * game.OverallEnergy * Game.EnergyPerMineral;
            }
        }
    }
}