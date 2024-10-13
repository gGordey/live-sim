using System;
using System.Numerics;

namespace Life_Simulation
{
    class OrganicTile : ProdusingTile
    {
        public OrganicTile (Root root)
        {
            Construct('Q', ConsoleColor.DarkRed, 4f, 1.3f, root, 80);
        }

        private float organic_taking = 0.002f;
        private int range = 2;
        private bool is_using_range = true;

        public override void ProduseEnergy()
        {
            base.ProduseEnergy();

            // if (Position.X >= 120) { return; }

            //root.Energy += game.CountEnergy(this);

            if (is_using_range)
            {
                for (int x = -range, y = -range; y <= range; x++)
                {
                    if (game.IsTileInField(Position + new Vector2(x, y)) && game.Organic[game.GetTileIndFromPosition(Position + new Vector2(x, y))] > 0)
                    {
                        root.Energy += organic_taking * 4.5f * game.OverallEnergy;
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
            else
            {
                for (byte i = 0; i < 5; i++)
                {
                    if (game.IsTileInField(Position + SeedTile.GetPositionFromInd(i)) && game.Organic[game.GetTileIndFromPosition(Position + SeedTile.GetPositionFromInd(i))] > 0)
                    {
                        root.Energy += organic_taking * 4.5f * game.OverallEnergy;
                        game.Organic[game.GetTileIndFromPosition(Position + SeedTile.GetPositionFromInd(i))] -= organic_taking;
                    }
                    if (game.GetTile(Position+SeedTile.GetPositionFromInd(i)).GetType() == typeof(DeathTile))
                    {
                        game.ReplaceTile(Position+SeedTile.GetPositionFromInd(i), new FreeTile(Position + SeedTile.GetPositionFromInd(i)));
                        root.Energy += DeathTile.EnergySaving;
                    }
                }
            }
        }
    }
}