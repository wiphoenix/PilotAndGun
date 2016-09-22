using CocosSharp;
using System.Collections.Generic;

namespace PilotAndGunPortable.Classes
{
    public class Player : CCSprite
    {
        private int health = 100;

        public Player()
        {
            var sprSheet = new CCSpriteSheet("Animations/Player.plist");
            var flyingAnimation = new CCAnimation(sprSheet.Frames, 0.1f);
            var flyingRepeatAnimation = new CCRepeatForever(new CCAnimate(flyingAnimation));

            SpriteFrame = sprSheet.Frames[0];
            Name = "Player";

            AnchorPoint = new CCPoint(0.5f, 0.5f);
            RunAction(flyingRepeatAnimation);
        }

        public int Health
        {
            get
            {
                return health;
            }
        }
    }
}