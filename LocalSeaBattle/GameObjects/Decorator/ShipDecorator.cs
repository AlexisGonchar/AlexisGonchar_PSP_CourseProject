using SharpDX;

namespace GameObjects
{
    /// <summary>
    /// Abstaract decorator class
    /// </summary>
    public abstract class ShipDecorator : Ship
    {
        protected Ship player;
        public ShipDecorator(Vector2 position, Ship player) : base(position)
        {
            this.player = player;
        }
    }
}
