using System;

namespace Life_Simulation
{
    class DeathTile : Tile
    {
        public static float EnergySaving = 4.5f;
        public DeathTile(Vector2 position)
        {
            Position = position;
            Construct('#', ConsoleColor.Gray);
            ClanColor = ConsoleColor.Gray;
        }
    }
}