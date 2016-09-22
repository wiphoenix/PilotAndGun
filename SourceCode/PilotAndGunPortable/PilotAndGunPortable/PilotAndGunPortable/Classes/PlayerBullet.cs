using CocosSharp;

namespace PilotAndGunPortable.Classes
{
    public class PlayerBullet : CCSprite
    {
        public int Damage { get; } = 1;

        public PlayerBullet() : base("bullet.png")
        {

        }
    }
}