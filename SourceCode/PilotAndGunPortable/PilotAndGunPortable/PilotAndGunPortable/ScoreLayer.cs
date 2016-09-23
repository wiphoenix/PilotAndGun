using CocosSharp;

namespace PilotAndGunPortable
{
    public class ScoreLayer : CCLayerColor
    {
        // Define a label variable
        CCSprite background;


        CCMenu mnuBack;
        CCMenuItemImage mniBack;

        CCSprite sprCoin;
        CCLabel label;

        public ScoreLayer() : base(CCColor4B.Blue)
        {
            background = new CCSprite("aboutBg.jpg");
            background.AnchorPoint = CCPoint.AnchorLowerLeft;
            background.Position = CCPoint.Zero;
            AddChild(background);

            mniBack = new CCMenuItemImage(new CCSprite("btnBack.png"), new CCSprite("btnBack.png"), delegate (object obj)
            {
                GameView.Director.PushScene(HomeLayer.HomeScene(GameView));
            });
            mnuBack = new CCMenu(new CCMenuItem[] { mniBack });
            mnuBack.AlignItemsVertically();

            AddChild(mnuBack);

            var sprSheet = new CCSpriteSheet("coin.plist");
            sprCoin = new CCSprite(sprSheet.Frames[0]);
            var spinningAnimation = new CCAnimation(sprSheet.Frames, 0.05f);
            var spinningRepeatAnimation = new CCRepeatForever(new CCAnimate(spinningAnimation));

            sprCoin.RunAction(spinningRepeatAnimation);

            AddChild(sprCoin);

            label = new CCLabel("High scores", "Fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            AddChild(label);
        }

        public void AddScore(long score)
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            mnuBack.Position = new CCPoint(VisibleBoundsWorldspace.MaxX - mnuBack.ContentSize.Width / 2 - 10, VisibleBoundsWorldspace.MaxY - mnuBack.ContentSize.Height / 2 - 10);

            sprCoin.Position = new CCPoint(VisibleBoundsWorldspace.Center.X, VisibleBoundsWorldspace.MaxY - sprCoin.ContentSize.Height / 2 - 10);

            label.Position = new CCPoint(sprCoin.Position.X, sprCoin.Position.Y - sprCoin.ContentSize.Height);
        }

        public static CCScene ScoreScene(CCGameView gameView)
        {
            var scene = new CCScene(gameView);
            var layer = new ScoreLayer();

            scene.AddChild(layer);

            return scene;
        }
    }
}