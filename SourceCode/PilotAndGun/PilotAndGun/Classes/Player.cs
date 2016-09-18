using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CocosSharp;

namespace PilotAndGun.Classes
{
    public class Player : CCSprite
    {
        private CCSprite sprite;
        public Player()
        {
            var sprSheet = new CCSpriteSheet("Animations/Player.plist");
            var flyingAnimation = new CCAnimation(sprSheet.Frames, 0.1f);
            var flyingRepeatAnimation = new CCRepeatForever(new CCAnimate(flyingAnimation));

            sprite = new CCSprite(sprSheet.Frames[0]) { Name = "Player" };
            sprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
            sprite.RunAction(flyingRepeatAnimation);
            sprite.Position = new CCPoint(0, 0);
            AddChild(sprite);
            ContentSize = sprite.ContentSize;
        }
    }
}