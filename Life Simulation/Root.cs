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

        public List<Tile> tiles = new List<Tile>();

        private float _energy = 35;

        private float _start_energy = 0;

        private float _maxEnergy = 35;

        private float _energyConsuming = 1;

        public bool IsAlive = true;

        public SeedTile seed;

        public float Energy 
        { 
            get { return _energy; } 

            set 
            {
                if (value <= EnergyLimit) 
                {
                    _energy = value; 
                }
                else
                {
                    _energy = EnergyLimit;
                }
            } 
        }

        public float EnergyLimit { get { return _maxEnergy; } set { _maxEnergy = value; } }

        public float EnergyConsuming {get { return _energyConsuming; } set { _energyConsuming = value; }} 

        public float StarterEnergy { set { _start_energy = value; }}

        public void Die()
        {   
            foreach (Tile tile in tiles)
            {
                if (tile.GetType() != typeof(SeedTile)) { tile.Die(); }
            }
            IsAlive = false;
        }

        public void NewTurn()
        {
            Energy -= EnergyConsuming;
            if (_start_energy > 0 && Energy < EnergyLimit) 
            {
                _start_energy -= EnergyLimit - Energy; 
                Energy = EnergyLimit;
            }
            if (IsAlive) 
            {   
                if (Energy <= 0) {Die(); return;}
            }
            if (!seed.IsPlased)
            {
                Die();
            }
        }

    }
}
