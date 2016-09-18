using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using PilotAndGun.Classes;

namespace PilotAndGun
{
    public class GameLayer : CCLayerColor
    {
        private const float MOVE_SPEED = 350.0f;
        private const string SCORE_CONTENT = "Score: ";

        long score = 0;
        CCLabel lblScore;
        Player player;

        List<CCSprite> visibleEnemies;
        List<CCSprite> visileEnemyBullets;
        List<CCSprite> visiblePlayerBullets;

        private float rightBoundX;
        private float leftBoundX;

        public GameLayer() : base(CCColor4B.Black)
        {
            player = new Player();
            AddChild(player);

            lblScore = new CCLabel(SCORE_CONTENT + score, "Fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            lblScore.AnchorPoint = new CCPoint(0f, 1f);
            lblScore.Schedule(s => { lblScore.Text = SCORE_CONTENT + score; });
            AddChild(lblScore);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            player.Position = new CCPoint(VisibleBoundsWorldspace.Center.X, player.ContentSize.Height / 2);
            lblScore.Position = new CCPoint(0 + 10, VisibleBoundsWorldspace.MaxY - 10);

            leftBoundX = player.ContentSize.Width / 2;
            rightBoundX = VisibleBoundsWorldspace.MaxX - player.ContentSize.Width / 2;

            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = OnTouchesBegan;
            touchListener.OnTouchesMoved = OnTouchesMoved;
            AddEventListener(touchListener, this);
        }
        private void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            var location = touches[0].LocationOnScreen;
            location = WorldToScreenspace(location);
        }

        private void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            player.StopAllActions();
            var location = touches[0].LocationOnScreen;
            location = WorldToScreenspace(location);

            float x = location.X;
            if (x > rightBoundX)
                x = rightBoundX;
            if (x < leftBoundX)
                x = leftBoundX;
            player.Position = new CCPoint(x, player.Position.Y);
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

