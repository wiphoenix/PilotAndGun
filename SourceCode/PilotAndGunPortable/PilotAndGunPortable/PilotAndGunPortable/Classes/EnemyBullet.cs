using CocosSharp;

namespace PilotAndGunPortable.Classes
{
    class EnemyBullet : CCSprite
    {
        public int Damage { get; } = 1;

        public EnemyBullet() : base("enemyBullet.png")
        {

        }
    }
}