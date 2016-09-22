using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotAndGunPortable.Classes
{
    public class PlayerHealthBar : CCSprite
    {
        CCSpriteSheet sprSheet;
        public int Health { get; private set; } = 6;

        public PlayerHealthBar()
        {
            sprSheet = new CCSpriteSheet("playerHP.plist");

            SpriteFrame = sprSheet.Frames[Health];
            Name = "HealthBar";

            AnchorPoint = CCPoint.AnchorLowerLeft;
        }

        public void Decrease(int damage)
        {
            if (damage > Health)
                Health = 0;
            Health -= damage;
            SpriteFrame = sprSheet.Frames[Health];
        }

        public bool IsEmpty
        {
            get
            {
                return Health == 0;
            }
        }
    }
}
