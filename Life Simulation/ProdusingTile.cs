using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Life_Simulation
{
    class ProdusingTile : Tile
    {
        private int energy_produsing;

        public int EnergyProdusing { get { return energy_produsing; } set { energy_produsing = value; } }

        public override void NextTurn(Game game)
        {
            base.NextTurn(game);
            
            ProduseEnergy();
        }

        private void ProduseEnergy()
        {
            root.Energy += energy_produsing;
        }
    }
}
