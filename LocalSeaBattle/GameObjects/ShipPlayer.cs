using SharpDX;

namespace GameObjects
{
    /// <summary>
    /// Class ShipPlayer
    /// </summary>
    public class ShipPlayer : Ship
    {
        public ShipPlayer(Vector2 position) : base(position)
        {
        }

        public override int GetDamage()
        {
            return damage;
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public override float GetDamageAbsorption()
        {
            return damageAbsorption;
        }

        public override int GetSpeed()
        {
            return speed;
        }

        public override int GetSpeedBullet()
        {
            return speedBullet;
        }

        public override int GetWeaponsReload()
        {
            return weaponsReload;
        }
    }
}
