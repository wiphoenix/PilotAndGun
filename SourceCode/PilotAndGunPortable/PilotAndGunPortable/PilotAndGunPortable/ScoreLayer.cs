using CocosSharp;

namespace PilotAndGunPortable
{
    public class ScoreLayer : CCLayerColor
    {
        // Define a label variable
        CCLabel label;

        public ScoreLayer() : base(CCColor4B.Blue)
        {
            // create and initialize a Label
            label = new CCLabel("Score layer", "Fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);

            // add the label as a child to this Layer
            AddChild(label);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // position the label on the center of the screen
            label.Position = bounds.Center;
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