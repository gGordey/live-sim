using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_Simulation
{
    class Root
    {
        public Root() { }

        private List<Tile> tiles = new List<Tile>();

        private int _energy = 0;

        private int _maxEnergy = 15;

        private int _energyConsuming;

        public int Energy { get { return _energy; } set 
            {
                if (value < EnergyLimit) 
                {
                    _energy = value; 
                }
                if (value <= 0) { Die(); }; 
            } }

        public int EnergyLimit { get { return _maxEnergy; } set { _maxEnergy = value; } } 

        public int EnergyConsuming { get { return _energyConsuming; } set { _energyConsuming = value; } }

        private void Die()
        {

        }

        private void NewTurn()
        {
            Enery -= EnergyConsuming;
        }
    }
}
