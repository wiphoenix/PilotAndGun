using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotAndGunPortable.Classes
{
    public class Boss : CCSprite
    {
        private BossHealthBar healthBar;

        public int Score { get; } = 7;

        public CCSprite HealthBar
        {
            get
            {
                return healthBar;
            }
        }

        public Boss() : base("boss01.png")
        {
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            FlipY = true;

            healthBar = new BossHealthBar();
            healthBar.AnchorPoint = CCPoint.AnchorMiddleBottom;
            healthBar.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height + 10);
            AddChild(healthBar);
        }
    }
}
