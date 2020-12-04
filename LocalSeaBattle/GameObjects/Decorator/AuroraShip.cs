namespace GameObjects
{
    /// <summary>
    /// Decorator class for ship Aurora
    /// </summary>
    public class AuroraShip : ShipDecorator
    {
        int[] parameters;
        public AuroraShip(Ship player) : base(player.position, player)
        {
            parameters = Features.getShipParameters(0);
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public override int GetDamage()
        {
            return player.GetDamage() + parameters[3];
        }

        public override float GetDamageAbsorption()
        {
            return player.GetDamageAbsorption() + parameters[4]/100.0f;
        }

        public override int GetSpeed()
        {
            return player.GetSpeed() + parameters[0];
        }

        public override int GetSpeedBullet()
        {
            return player.GetSpeedBullet() + parameters[1];
        }

        public override int GetWeaponsReload()
        {
            return player.GetWeaponsReload() + parameters[2];
        }
    }
}
