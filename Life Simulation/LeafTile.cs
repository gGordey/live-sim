using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_Simulation
{
    class LeafTile : ProdusingTile
    {
        public LeafTile(Root root, Vector2 positon)
        {
            Construct('@', ConsoleColor.Green, 4, 1, root);

            Position = positon;
        }

        public LeafTile(Root root) 
        {
            Construct('@', ConsoleColor.Green, 4, 1, root);
        }
    }
}
