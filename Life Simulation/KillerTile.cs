using System;

namespace Life_Simulation
{
    class KillerTile : Tile
    {
        private static float energy_from_kill = 7.2f;

        public KillerTile(Root root)
        {
            Construct('#', ConsoleColor.Red, 0.4f, 0.2f, root, 18);
        }
        public void OnKill()
        {
            root.Energy += energy_from_kill;
        }
    }
}