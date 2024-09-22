using System;
using System.Diagnostics;

namespace Life_Simulation
{
    class SavingTile : Tile
    {
        private const int energy_saves = 20;

        public SavingTile(Root root) 
        { 
            this.root = root;
            root.EnergyLimit += energy_saves;

            Construct('%', ConsoleColor.Yellow, 2, 0, root, 40);
        }

        public SavingTile(Root root, Vector2 position)
        {
            this.root = root;
            root.EnergyLimit += energy_saves;

            Position = position;

            Construct('%', ConsoleColor.Yellow, 2, 0, root, 40);
        }

        public override void Die()
        {
            base.Die();
            root.Energy -= energy_saves;
        }
    }
}
