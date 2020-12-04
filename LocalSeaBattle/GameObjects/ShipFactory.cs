using SharpDX;

namespace GameObjects
{
    /// <summary>
    /// Class ShipFactory
    /// </summary>
    public class ShipFactory
    {
        /// <summary>
        /// Method for create ship
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Ship GetShip(int index)
        {
            Ship player = new ShipPlayer(new Vector2());
            switch (index)
            {
                case 0:
                    player = new AuroraShip(player);
                    break;
                case 1:
                    player = new FlyingDutchmanShip(player);
                    break;
                case 2:
                    player = new ForwardShip(player);
                    break;
                case 3:
                    player = new GhostShip(player);
                    break;
                case 4:
                    player = new PiligrimShip(player);
                    break;
            }
            return player;
        }
    }
}
