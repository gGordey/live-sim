using System;

namespace Life_Simulation
{
    public enum MapTiles
    {
        Ground,
        Sand,
        Ocean,
        Rock
    }
    public static class MapGenerator
    {
        public static MapTiles[] Map = new MapTiles[Game.field_size.X * Game.field_size.Y];

        
    }
}