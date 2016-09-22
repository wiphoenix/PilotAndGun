using CocosSharp;

namespace PilotAndGunPortable
{
    public class AboutLayer : CCLayerColor
    {
        // Define a label variable
        CCLabel label1;
        CCLabel label2;
        CCLabel label3;
        CCSprite background;

        CCMenu mnuBack;
        CCMenuItemImage mniBack;

        public AboutLayer() : base(CCColor4B.Blue)
        {
            background = new CCSprite("aboutBg.jpg");
            background.AnchorPoint = CCPoint.AnchorLowerLeft;
            background.Position = CCPoint.Zero;
            AddChild(background);

            // create and initialize a Label
            label1 = new CCLabel("Developed by:", "Fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            label2 = new CCLabel(" Lin LY", "Fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            label3 = new CCLabel(" Youke XIANG", "Fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);

            // add the label as a child to this Layer
            AddChild(label1);
            AddChild(label2);
            AddChild(label3);

            mniBack = new CCMenuItemImage(new CCSprite("btnBack.png"), new CCSprite("btnBack.png"), delegate (object obj)
            {
                GameView.Director.PushScene(HomeLayer.HomeScene(GameView));
            });
            mnuBack = new CCMenu(new CCMenuItem[] { mniBack });
            mnuBack.AlignItemsVertically();

            AddChild(mnuBack);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // position the label on the center of the screen
            label1.Position = bounds.Center;
            label2.Position = new CCPoint(label1.Position.X, label1.Position.Y - label2.ContentSize.Height - 10);
            label3.Position = new CCPoint(label1.Position.X, label2.Position.Y - label3.ContentSize.Height - 10);

            mnuBack.Position = new CCPoint(VisibleBoundsWorldspace.MaxX - mnuBack.ContentSize.Width / 2 - 10, VisibleBoundsWorldspace.MaxY - mnuBack.ContentSize.Height / 2 - 10);
        }

        public static CCScene AboutScene(CCGameView gameView)
        {
            var scene = new CCScene(gameView);
            var layer = new AboutLayer();

            scene.AddChild(layer);

            return scene;
        }
    }
}