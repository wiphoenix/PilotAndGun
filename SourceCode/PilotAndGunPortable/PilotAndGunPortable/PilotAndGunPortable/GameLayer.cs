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

        private const float PLAYER_SHOOTING_INTEVAL = 0.5f;
        private const float ENEMY_SHOOTING_INTERVAL = 2f;
        private const float ENEMY_BULLET_SPEED = 300f;

        private const int NO_OF_ENEMIES_IN_A_BATCH = 5;

        private const int BACKGROUND_INDEX = 0;
        private const int SCORE_INDEX = 1;
        private const int HEALTH_BAR_INDEX = 1;
        private const int PAUSE_BUTTON_INDEX = 50;
        private const int ENEMY_INDEX = 10;
        private const int PLAYER_INDEX = 10;
        private const int PLAYER_BULLET_INDEX = 2;
        private const int ENEMY_BULLET_INDEX = 2;

        int noOfBatch;

        long score = 0;
        CCLabel lblScore;
        Player player;
        PlayerHealthBar healthBar;

        List<CCSprite> visibleEnemies = new List<CCSprite>();
        List<EnemyBullet> visileEnemyBullets = new List<EnemyBullet>();
        List<PlayerBullet> visiblePlayerBullets = new List<PlayerBullet>();

        private float rightBoundX;
        private float leftBoundX;

        CCCallFuncN moveOutOfView = new CCCallFuncN(node => node.RemoveFromParent());

        Random random = new Random();

        CCMenu mnuPause;
        CCMenuItemImage mniPause;

        CCParallaxNode parallaxBackground;

        public GameLayer() : base(CCColor4B.Black)
        {
            player = new Player();
            AddChild(player, PLAYER_INDEX);
            healthBar = new PlayerHealthBar();
            AddChild(healthBar, HEALTH_BAR_INDEX);


            lblScore = new CCLabel(SCORE_CONTENT + score, "Fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            lblScore.AnchorPoint = new CCPoint(0f, 1f);
            lblScore.Schedule(s => { lblScore.Text = SCORE_CONTENT + score; });
            AddChild(lblScore, SCORE_INDEX);

            mniPause = new CCMenuItemImage(new CCSprite("btnPause.png"), new CCSprite("btnPauseSelected.png"), delegate (object obj)
            {
                //GameView.Paused = !GameView.Paused;
            });

            mnuPause = new CCMenu(new CCMenuItem[] { mniPause });
            mnuPause.AlignItemsVertically();
            mnuPause.AnchorPoint = CCPoint.AnchorUpperRight;
            AddChild(mnuPause, PAUSE_BUTTON_INDEX);

            //check collision
            Schedule(s => CheckCollision());

            //player shoots bullets
            Schedule(s => visiblePlayerBullets.Add(AddPlayerBullet()), PLAYER_SHOOTING_INTEVAL);

            //spawn enemies
            Schedule(s => SpawnEnemies(), 2f);
        }

        private void AddBackground()
        {
            float h = VisibleBoundsWorldspace.Size.Height;
            parallaxBackground = new CCParallaxNode { Position = new CCPoint(0, h) };
            AddChild(parallaxBackground, BACKGROUND_INDEX);

            var bg = new CCSprite("gameBg.png");

            parallaxBackground.AddChild(bg, 0, new CCPoint(1f, 0), new CCPoint(bg.ContentSize.Width / 2, bg.ContentSize.Height / 2));
        }

        private void RemoveNoParentNodes()
        {
            //remove invisible nodes
            visiblePlayerBullets.RemoveAll(spr => spr.Parent == null);
            visibleEnemies.RemoveAll(spr => spr.Parent == null);
            visileEnemyBullets.RemoveAll(spr => spr.Parent == null);
        }

        private void Explode(CCPoint position)
        {
            var explosion = new CCParticleExplosion(position);
            explosion.TotalParticles = 10;
            explosion.AutoRemoveOnFinish = true;
            AddChild(explosion);
        }

        private void CheckCollision()
        {
            RemoveNoParentNodes();

            //check if player intersects with enemy bullets
            foreach (EnemyBullet eb in visileEnemyBullets)
            {
                bool hit = eb.BoundingBoxTransformedToParent.IntersectsRect(player.BoundingBoxTransformedToParent);
                if (hit)
                {
                    Explode(eb.Position);
                    eb.RemoveFromParent();
                    healthBar.Decrease(eb.Damage);
                }
            }
            //check if player intersects with enemies
            foreach (CCSprite enemy in visibleEnemies)
            {
                bool hit = enemy.BoundingBoxTransformedToParent.IntersectsRect(player.BoundingBoxTransformedToParent);
                if (hit)
                {
                    Explode(enemy.Position);
                    Explode(player.Position);
                    enemy.RemoveFromParent();
                    healthBar.Decrease(1);
                }
            }

            //check if enemies intersects with player bullets
            foreach (PlayerBullet pb in visiblePlayerBullets)
            {
                foreach (CCSprite enemy in visibleEnemies)
                {
                    bool hit = pb.BoundingBoxTransformedToParent.IntersectsRect(enemy.BoundingBoxTransformedToParent);
                    if (hit)
                    {
                        Explode(pb.Position);
                        pb.RemoveFromParent();
                        if (enemy is Enemy)
                        {
                            enemy.RemoveFromParent();
                            score += (enemy as Enemy).Score;
                        }
                        else
                        {
                            Boss b = enemy as Boss;
                            b.HealthBar.Decrease(1);
                            if (b.HealthBar.Health == 0)
                            {
                                b.RemoveFromParent();
                                score += b.Score;
                            }
                        }
                    }
                }
            }

            //check GAME END
            if (healthBar.Health == 0)
                GameView.Director.PushScene(ScoreLayer.ScoreScene(GameView));

            RemoveNoParentNodes();

        }

        private void SpawnEnemies()
        {
            if (visibleEnemies.Count != 0)
                return;

            noOfBatch++;

            //boss appears each 10 batchs
            if (noOfBatch % 10 == 0)
            {
                //spawn a boss
                ScheduleOnce(s => visibleEnemies.Add(AddBoss()), 0);
            }
            else
            {
                //spawn a batch
                bool direction = random.Next(2) == 0;
                Schedule(s => visibleEnemies.Add(AddEnemy(direction)), 1.0f, NO_OF_ENEMIES_IN_A_BATCH - 1, 0);
            }
        }

        private CCSprite AddBoss()
        {
            Boss boss = new Boss();
            float y = VisibleBoundsWorldspace.MaxY - boss.ContentSize.Height - 10;
            CCPoint left = new CCPoint(boss.ContentSize.Width / 2, y);
            CCPoint right = new CCPoint(VisibleBoundsWorldspace.MaxX - boss.ContentSize.Width / 2, y);
            boss.Position = left;

            var moveRight = new CCMoveTo(3f, right);
            var moveLeft = new CCMoveTo(3f, left);
            var forever = new CCRepeatForever(new CCFiniteTimeAction[] { moveRight, moveLeft });

            AddChild(boss, ENEMY_INDEX);

            boss.RunAction(forever);
            boss.Schedule(s => visileEnemyBullets.Add(AddEnemyBullet(boss)), 1f);
            return boss;
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
            enemy.ScheduleOnce(s => visileEnemyBullets.Add(AddEnemyBullet(enemy)), random.Next(1, 3));
            return enemy;
        }

        private EnemyBullet AddEnemyBullet(CCSprite sender)
        {
            EnemyBullet bullet = new EnemyBullet();
            bullet.Position = new CCPoint(sender.Position.X, sender.Position.Y - sender.ContentSize.Height / 2);
            AddChild(bullet, ENEMY_BULLET_INDEX);

            CCPoint target = CCPoint.IntersectPoint(bullet.Position, player.Position, CCPoint.Zero, new CCPoint(VisibleBoundsWorldspace.MaxX, 0));
            float distance = CCPoint.Distance(bullet.Position, target);

            var moveBullet = new CCMoveTo(distance / ENEMY_BULLET_SPEED, target);
            bullet.RunActions(moveBullet, moveOutOfView);
            return bullet;
        }

        private PlayerBullet AddPlayerBullet()
        {
            PlayerBullet bullet = new PlayerBullet();
            bullet.Position = new CCPoint(player.Position.X, player.Position.Y + player.ContentSize.Height / 2);
            AddChild(bullet, PLAYER_BULLET_INDEX);

            var moveBullet = new CCMoveTo(5.0f, new CCPoint(bullet.Position.X, VisibleBoundsWorldspace.MaxY));
            bullet.RunActions(moveBullet, moveOutOfView);
            return bullet;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            AddBackground();

            player.Position = new CCPoint(VisibleBoundsWorldspace.Center.X, player.ContentSize.Height / 2);
            healthBar.Position = CCPoint.Zero;
            lblScore.Position = new CCPoint(0 + 10, VisibleBoundsWorldspace.MaxY - 10);
            mnuPause.Position = new CCPoint(VisibleBoundsWorldspace.MaxX - mnuPause.ContentSize.Width - 10, VisibleBoundsWorldspace.MaxY - mnuPause.ContentSize.Height - 10);

            leftBoundX = player.ContentSize.Width / 2;
            rightBoundX = VisibleBoundsWorldspace.MaxX - player.ContentSize.Width / 2;

            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = OnTouchesBegan;
            touchListener.OnTouchesMoved = OnTouchesMoved;
            AddEventListener(touchListener, this);
        }

        private void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            var location = touches[0].Location;
            CCNode currentTarget = touchEvent.CurrentTarget;
        }

        private void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            player.StopAllActions();
            var location = touches[0].Location;

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

