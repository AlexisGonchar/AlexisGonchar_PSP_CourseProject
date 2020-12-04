using SharpDX;

namespace GameObjects
{
    /// <summary>
    /// Class Rock
    /// </summary>
    public class Rock
    {
        public int scale;
        public Vector2 position;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"></param>
        public Rock(Vector2 position)
        {
            this.position = position;
            scale = 70;
        }
    }
}
