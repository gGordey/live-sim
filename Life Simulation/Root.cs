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
            //System.Console.WriteLine("DIIIIIEEED");
            IsAlive = false;
        }

        public void NewTurn()
        {
            if (IsAlive) 
            {
                Energy -= EnergyConsuming;
                if (Energy <= 0) {Die(); return;}
                //System.Console.WriteLine("e "+Energy+"\nec "+EnergyConsuming);
            }
        }

    }
}
