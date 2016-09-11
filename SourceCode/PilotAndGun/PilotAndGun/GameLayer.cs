using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace PilotAndGun
{
    public class GameLayer : CCLayerColor
    {

        // Define a label variable
        CCSprite player;

        public GameLayer() : base(CCColor4B.Black)
        {

            //// create and initialize a Label
            //label = new CCLabel("Hello CocosSharp", "Fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);

            //// add the label as a child to this Layer
            //AddChild(label);

            

        }

        private void AddPlayer()
        {
            var sprSheet = new CCSpriteSheet("Animations/Player.plist");
            var flyingAnimation = new CCAnimation(sprSheet.Frames, 0.1f);
            var flyingRepeatAnimation = new CCRepeatForever(new CCAnimate(flyingAnimation));

            player = new CCSprite(sprSheet.Frames[0]) { Name = "Player" };
            player.AnchorPoint = new CCPoint(0.5f, 0.5f);
            player.RunAction(flyingRepeatAnimation);

            player.Position = new CCPoint(VisibleBoundsWorldspace.Center.X, player.ContentSize.Height / 2);
            AddChild(player);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            AddPlayer();

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }

        public static CCScene GameScene(CCGameView gameView)
        {
            var scene = new CCScene(gameView);
            var layer = new GameLayer();

            scene.AddChild(layer);

            return scene;
        }
    }
}

