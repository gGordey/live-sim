using System;

namespace Life_Simulation
{
    class ElectroTile : ProdusingTile
    {
        public ElectroTile (Root root)
        {
            Construct('$', ConsoleColor.DarkBlue, 7, 1.5f, root, 104);
        }

        public override void ProduseEnergy()
        {
            root.Energy += (120-Age) * 0.1f;
        }
    }
}