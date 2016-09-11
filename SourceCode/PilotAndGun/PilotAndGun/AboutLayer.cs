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

namespace PilotAndGun
{
    public class AboutLayer : CCLayerColor
    {
        // Define a label variable
        CCLabel label;

        public AboutLayer() : base(CCColor4B.Blue)
        {
            // create and initialize a Label
            label = new CCLabel("About layer", "Fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);

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

        public static CCScene AboutScene(CCGameView gameView)
        {
            var scene = new CCScene(gameView);
            var layer = new AboutLayer();

            scene.AddChild(layer);

            return scene;
        }
    }
}