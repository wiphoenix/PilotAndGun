using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotAndGunPortable.Classes
{
    public class Enemy : CCSprite
    {
        public int Health { get; set; } = 2;

        public int Score { get; } = 1;

        public Enemy() : base("enemy.png")
        {
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            FlipY = true;
        }
    }
}
