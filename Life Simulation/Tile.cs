using System;
using System.Diagnostics;

namespace Life_Simulation
{
    class Tile
    { 
        private Vector2 _position = new Vector2();
        
        private int _min_energy_level = 0;

        private int _energy_consuming = 0;

        public int min_energy_level { get { return _min_energy_level; } set { _min_energy_level = value; } }

        public Vector2 Position { get { return _position; } set { _position = value; } }

        public bool IsAlive = true;

        public char Symbol = ' ';

        public Root root;

        public Game game;

        public ConsoleColor Color = ConsoleColor.White;

        public Tile(Vector2 position) { _position = position; }
        public Tile() { _position = new Vector2(); }

        public virtual void NextTurn(Game game)
        {
            Update();
            this.game = game;
        }

        public virtual void Start()
        {
            
        }

        private void Update()
        {

        }

        private void Die()
        {
            //IsAlive = false;
        }

        public void Construct(char symb, ConsoleColor symb_col, int energy_needed, int energy_consuming, Root rt)
        {
            Symbol = symb;
            Color = symb_col;
            min_energy_level = energy_needed;
            _energy_consuming = energy_consuming;
            root = rt;
            root.EnergyConsuming = _energy_consuming;
        }
        public void Construct(char symb, ConsoleColor symb_col)
        {
            Symbol = symb;
            Color = symb_col;
        }
    }
}
