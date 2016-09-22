using CocosSharp;

namespace PilotAndGunPortable
{
    public class HomeLayer : CCLayerColor
    {
        private const float MENU_SPACING = 20f;

        CCSprite sprLogo;

        CCMenu mnuMain;
        CCMenuItemImage mniStart;
        CCMenuItemImage mniScore;
        CCMenuItemImage mniAbout;

        public HomeLayer() : base(CCColor4B.White)
        {
            sprLogo = new CCSprite("logo.png");
            AddChild(sprLogo);

            mniAbout = new CCMenuItemImage(new CCSprite("btnAbout.png"), new CCSprite("btnAboutSelected.png"), delegate (object obj)
            {
                GameView.Director.PushScene(AboutLayer.AboutScene(GameView));
            });

            mniScore = new CCMenuItemImage(new CCSprite("btnScore.png"), new CCSprite("btnScoreSelected.png"), delegate (object obj)
            {
                GameView.Director.PushScene(ScoreLayer.ScoreScene(GameView));
            });

            mniStart = new CCMenuItemImage(new CCSprite("btnStart.png"), new CCSprite("btnStartSelected.png"), delegate (object obj)
            {
                GameView.Director.PushScene(GameLayer.GameScene(GameView));
            });


            mnuMain = new CCMenu(new CCMenuItem[] { mniStart, mniScore, mniAbout });
            mnuMain.AlignItemsVertically(MENU_SPACING);

            AddChild(mnuMain);

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            //set anchor point at center-top
            sprLogo.AnchorPoint = new CCPoint(0.5f, 1f);
            // position the label on the center-top of the screen
            sprLogo.Position = new CCPoint(VisibleBoundsWorldspace.Size.Width / 2, VisibleBoundsWorldspace.Size.Height);

            //set anchor point at center-top
            mnuMain.AnchorPoint = new CCPoint(0.5f, 0f);
            mnuMain.Position = new CCPoint(VisibleBoundsWorldspace.Center.X, VisibleBoundsWorldspace.Center.Y - mnuMain.ContentSize.Height / 2);

        }

        public static CCScene HomeScene(CCGameView gameView)
        {
            var scene = new CCScene(gameView);
            var layer = new HomeLayer();

            scene.AddChild(layer);

            return scene;
        }
    }
}