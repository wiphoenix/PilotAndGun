using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotAndGunPortable.Classes
{
    public class Boss : CCSprite
    {
        public int Health { get; } = 7;

        public int Score { get; } = 7;

        public Boss() : base("boss01.png")
        {
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            FlipY = true;
        }
    }
}
