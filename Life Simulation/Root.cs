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

        private float _energy = 100;

        private int _maxEnergy = 100;

        private int _energyConsuming = 1;

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
            } 
        }

        public int EnergyLimit { get { return _maxEnergy; } set { _maxEnergy = value; } }

        public int EnergyConsuming {get { return _energyConsuming; } set { _energyConsuming = value; }} 

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
