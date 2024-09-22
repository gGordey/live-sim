using System;

namespace Life_Simulation
{
    class ElectroTile : ProdusingTile
    {
        public ElectroTile (Root root)
        {
            Construct('$', ConsoleColor.DarkBlue, 5, 2, root, 120);
        }

        public override void ProduseEnergy()
        {
            root.Energy += (120-Age) * 0.08f;
        }
    }
}