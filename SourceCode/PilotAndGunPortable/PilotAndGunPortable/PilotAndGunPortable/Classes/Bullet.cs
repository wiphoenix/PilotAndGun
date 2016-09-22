using CocosSharp;

namespace PilotAndGunPortable.Classes
{
    public class Bullet : CCSprite
    {
        public int Damage { get; } = 1;

        public Bullet() : base("bullet.png")
        {

        }
    }
}