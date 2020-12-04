using SharpDX;

namespace GameObjects
{
    /// <summary>
    /// Class bullet
    /// </summary>
    public class Bullet
    {
        public Vector2 position;
        public int direction;

        /// <summary>
        /// Constroctor for bullet
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public Bullet(Vector2 position, int direction)
        {
            this.position = position;
            this.direction = direction;
        }
    }
}
