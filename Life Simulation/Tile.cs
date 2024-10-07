using System;
using System.Diagnostics;

namespace Life_Simulation
{
    class Tile
    { 
        private Vector2 _position = new Vector2();
        
        private float _min_energy_level = 0;

        private float _energy_consuming = 0;

        private int _age = 0;

        private int _life_length = 0;

        public float min_energy_level { get { return _min_energy_level; } set { _min_energy_level = value; } }

        public Vector2 Position { get { return _position; } set { _position = value; } }

        public int Age { get { return _age; } set { _age = value; }}

        public bool IsAlive = true;

        public char Symbol = ' ';

        public Root root;

        public Game game;

        public ConsoleColor Color = ConsoleColor.White;

        private ConsoleColor _clan_color = ConsoleColor.Gray;
        public ConsoleColor ClanColor
        { 
            set {_clan_color = value;}

            get
            {
                if (GetType() == typeof(SeedTile) || root == null) { return _clan_color; }
                else {return root.seed.ClanColor;}
            }
        
        }

        private ConsoleColor _gen_color = ConsoleColor.Gray;
        public ConsoleColor GenColor 
        { 
            set {_gen_color = value;}

            get
            {
                if (GetType() == typeof(SeedTile) || root == null) { return _gen_color; }
                else {return root.seed.GenColor;}
            }
        
        }

        public bool IsPlased = true;

        public Tile(Vector2 position) { _position = position; }
        public Tile() { _position = new Vector2(); }

        public virtual void NextTurn(Game game)
        {
            Update();

            this.game = game;
        }

        public virtual void Start()
        {
            energy_construct();
        }

        private void Update()
        {
            if (_life_length <= 1) { return; }

            Age++;

            if (Age >= _life_length) {Die();}

            if (root != null && !root.IsAlive) {Die();}
        }

        public virtual void Die()
        {
            IsAlive = false;
            
            if (root == null) { return; }

            root.EnergyConsuming -= _energy_consuming;

            if (game == null) { return; }

            //if (new Random().Next(100) < 2) { game.ReplaceTile(Position, new DeathTile(Position)); }

            game.SpreadOrganic(0.07f, Position);
        }

        private void energy_construct()
        {
            if (root == null) {return;}
            root.EnergyConsuming += _energy_consuming;
            root.Energy -= min_energy_level;
        }

        public void Construct(char symb, ConsoleColor symb_col, float energy_needed, float energy_consuming, Root rt, int life_length)
        {
            Symbol = symb;
            Color = symb_col;
            _energy_consuming = energy_consuming;
            _life_length = life_length;
            root = rt;
            min_energy_level = energy_needed;
            root.tiles.Add(this);
        }
        public void Construct(char symb, ConsoleColor symb_col)
        {
            Symbol = symb;
            Color = symb_col;
        }
    }
}
