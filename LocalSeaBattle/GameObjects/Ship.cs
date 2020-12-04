using System.Collections.Generic;
using SharpDX;

namespace GameObjects
{
    /// <summary>
    /// Abstract class Ship
    /// </summary>
    public abstract class Ship
    {
        public bool isAlive;
        public int Health { get { return health; }
            set
            {
                if (value < 0) health = 0;
                else health = value;
            }
        }
        private int health;
        public int direction;
        public int IsReload;
        public Vector2 position;
        public int height;
        public int width;
        public List<Bullet> bullets = new List<Bullet>();
        public int speedBullet;
        public bool weaponsMode;
        public int speed;
        public int weaponsReload;
        public int damage;
        public float damageAbsorption;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"></param>
        public Ship(Vector2 position)
        {
            weaponsMode = true;
            isAlive = true;
            Health = 1000;
            IsReload = 0;
            this.position = position;
            height = 55;
            width = 110;
            direction = 1;
            damageAbsorption = 0;
        }
        /// <summary>
        /// Method for get players speed
        /// </summary>
        /// <returns></returns>
        public abstract int GetSpeed();
        /// <summary>
        /// Mothod for get bullets speed
        /// </summary>
        /// <returns></returns>
        public abstract int GetSpeedBullet();
        /// <summary>
        /// method for get players damage
        /// </summary>
        /// <returns></returns>
        public abstract int GetDamage();
        /// <summary>
        /// Method for get players weapons reload
        /// </summary>
        /// <returns></returns>
        public abstract int GetWeaponsReload();
        /// <summary>
        /// Method for get players damage absorption
        /// </summary>
        /// <returns></returns>
        public abstract float GetDamageAbsorption();
        /// <summary>
        /// Method for take damage for player
        /// </summary>
        /// <param name="damage"></param>
        public abstract void TakeDamage(int damage);
    }
}
