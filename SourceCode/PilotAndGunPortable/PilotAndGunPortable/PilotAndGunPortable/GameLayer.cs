using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using PilotAndGunPortable.Classes;

namespace PilotAndGunPortable
{
    public class GameLayer : CCLayerColor
    {
        private const string SCORE_CONTENT = "Score: ";

        private const float SHOOTING_SPEED = 0.5f;

        private const int NO_OF_ENEMIES_IN_A_BATCH = 5;

        private const int ENEMY_INDEX = 10;
        private const int PLAYER_INDEX = 10;
        private const int BULLET_INDEX = 1;

        int noOfBatch;

        long score = 0;
        CCLabel lblScore;
        Player player;

        List<CCSprite> visibleEnemies = new List<CCSprite>();
        List<CCSprite> visileEnemyBullets = new List<CCSprite>();
        List<CCSprite> visiblePlayerBullets = new List<CCSprite>();

        private float rightBoundX;
        private float leftBoundX;

        CCCallFuncN moveOutOfView = new CCCallFuncN(node => node.RemoveFromParent());

        Random random = new Random();

        public GameLayer() : base(CCColor4B.Black)
        {
            player = new Player();
            AddChild(player, PLAYER_INDEX);

            lblScore = new CCLabel(SCORE_CONTENT + score, "Fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            lblScore.AnchorPoint = new CCPoint(0f, 1f);
            lblScore.Schedule(s => { lblScore.Text = SCORE_CONTENT + score; });
            AddChild(lblScore);

            //player shoots bullets
            Schedule(s => visiblePlayerBullets.Add(AddPlayerBullet()), SHOOTING_SPEED);

            //check collision
            Schedule(s => CheckCollision());

            //spawn enemies
            Schedule(s => SpawnEnemies(), 2f);

            //ScheduleOnce(s => visibleEnemies.Add(AddEnemy(true)), 5f);
        }

        private void CheckCollision()
        {
            visiblePlayerBullets.RemoveAll(spr => spr.Parent == null);
            visibleEnemies.RemoveAll(spr => spr.Parent == null);
        }

        private void SpawnEnemies()
        {
            if (visibleEnemies.Count != 0)
                return;

            noOfBatch++;

            bool direction = random.Next(2) == 0;
            Schedule(s => visibleEnemies.Add(AddEnemy(direction)), 1.0f, NO_OF_ENEMIES_IN_A_BATCH - 1, 0);
        }

        private CCSprite AddEnemy(bool leftToRight)
        {
            Enemy enemy = new Enemy();

            float enemyMoveDuration = 5.0f;
            CCMoveTo moveEnemy;
            if (leftToRight)
            {
                enemy.Position = new CCPoint(0, VisibleBoundsWorldspace.MaxY);
                moveEnemy = new CCMoveTo(enemyMoveDuration, new CCPoint(VisibleBoundsWorldspace.MaxX, 0));
            }
            else
            {
                enemy.Position = new CCPoint(VisibleBoundsWorldspace.MaxX, VisibleBoundsWorldspace.MaxY);
                moveEnemy = new CCMoveTo(enemyMoveDuration, new CCPoint(0, 0));
            }
            AddChild(enemy, ENEMY_INDEX);

            enemy.RunActions(moveEnemy, moveOutOfView);
            return enemy;
        }

        private Bullet AddPlayerBullet()
        {
            Bullet bullet = new Bullet();
            bullet.Position = new CCPoint(player.Position.X, player.Position.Y + player.ContentSize.Height / 2);
            AddChild(bullet, BULLET_INDEX);

            var moveBullet = new CCMoveTo(5.0f, new CCPoint(bullet.Position.X, VisibleBoundsWorldspace.MaxY));
            bullet.RunActions(moveBullet, moveOutOfView);
            return bullet;
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

