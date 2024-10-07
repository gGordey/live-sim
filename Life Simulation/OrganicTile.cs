using System;
using System.Numerics;

namespace Life_Simulation
{
    class OrganicTile : ProdusingTile
    {
        public OrganicTile (Root root)
        {
            Construct('Q', ConsoleColor.DarkRed, 4f, 1.8f, root, 80);
        }

        private float organic_taking = 0.04f;
        private int range = 2;

        public override void ProduseEnergy()
        {
            base.ProduseEnergy();

            //root.Energy += game.CountEnergy(this);

            for (int x = -range, y = -range; y <= range; x++)
            {
                if (game.IsTileInField(Position + new Vector2(x, y)) && game.Organic[game.GetTileIndFromPosition(Position + new Vector2(x, y))] > 0)
                {
                    root.Energy += organic_taking * 2.5f;
                    game.Organic[game.GetTileIndFromPosition(Position + new Vector2(x, y))] -= organic_taking;
                }
                if (game.GetTile(Position+new Vector2(x,y)).GetType() == typeof(DeathTile))
                {
                    game.ReplaceTile(Position+new Vector2(x, y), new FreeTile(Position+new Vector2(x, y)));
                    root.Energy += DeathTile.EnergySaving;
                }
                if (x == range) { x = -range; y++; }
            }
        }
    }
}