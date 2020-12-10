using SharpDX;
using System.Windows.Input;
using System.Collections.Generic;
using UdpLib;

namespace GameObjects
{
    /// <summary>
    /// Class GameField
    /// </summary>
    public class GameField
    {

        public Ship player1;
        public Ship player2;

        public List<Rock> rocks;

        ShipFactory shipFactory = new ShipFactory();
        Vector4 borders;
        Client client;
        public GameField()
        {

        }


        public void Initialize(Dictionary<string, int> dc, int idShip, Client client)
        {
            GeneratePlayer(dc, idShip);
            GenerateRocks();
            borders = new Vector4(30, 0, 704, 570);
            this.client = client;
        }

        //Генерация скал
        private void GenerateRocks()
        {
            rocks = new List<Rock>();
            rocks.Add(new Rock(new Vector2(280, 180)));
            rocks.Add(new Rock(new Vector2(420, 380)));
            rocks.Add(new Rock(new Vector2(80, 320)));
        }

        //Создание игроков
        private void GeneratePlayer(Dictionary<string, int> dc, int idShip)
        {
            Vector2 position1, position2;
            if (idShip == 1)
            {
                position1 = new Vector2(200, 200);
                position2 = new Vector2(420, 200);
            }
            else
            {
                position2 = new Vector2(200, 200);
                position1 = new Vector2(420, 200);
            }
            int ship1, ship2;
            ship1 = dc["Ship1"];
            ship2 = dc["Ship2"];
            player1 = shipFactory.GetShip(ship1);
            player2 = shipFactory.GetShip(ship2);
            player1.position = position1;
            player2.position = position2;
        }

        private void MovePlayer(Key key, Ship pl, Vector2 vec, int direction)
        {
            if (Keyboard.IsKeyDown(key))
            {
                pl.position += vec;
                Features.SwapDirection(pl, direction);
                if (Features.CollisionCheck(player1, player2) || Features.CollisionRockPl(pl, rocks) || !BorderCheck(pl.position))
                    pl.position -= vec;
            }
        }

        public void UpdatePosition()
        {
            if (player1.isAlive && player2.isAlive)
            {
                //Движение первого игрока
                MovePlayer(Key.W, player1, new Vector2(0, -player1.GetSpeed()), 2);
                MovePlayer(Key.S, player1, new Vector2(0, player1.GetSpeed()), 4);
                MovePlayer(Key.A, player1, new Vector2(-player1.GetSpeed(), 0), 1);
                MovePlayer(Key.D, player1, new Vector2(player1.GetSpeed(), 0), 3);


                client.MyShip.x = (int)player1.position.X;
                client.MyShip.y = (int)player1.position.Y;
                client.MyShip.dircetion = player1.direction;
                //Движение второго игрока
                player2.position = new Vector2(client.EnemyShip.x, client.EnemyShip.y);
                Features.SwapDirection(player2, client.EnemyShip.dircetion);


                //Смена режима стрельбы
                if (Keyboard.IsKeyDown(Key.R) && player1.IsReload == 0)
                {
                    player1.weaponsMode = !player1.weaponsMode;
                    player1.IsReload = player1.GetWeaponsReload();
                }
                if (Keyboard.IsKeyDown(Key.RightShift) && player2.IsReload == 0)
                {
                    player2.weaponsMode = !player2.weaponsMode;
                    player2.IsReload = player2.GetWeaponsReload();
                }

                //Стрельба первого игрока
                if (Keyboard.IsKeyDown(Key.F) && player1.IsReload == 0)
                {
                    Shooting(player1);
                    client.MyShip.bullet = 1;
                }
                    

                if (player1.IsReload > 0)
                    player1.IsReload--;

                //Стрельа второго игрока
                if(client.EnemyShip.bullet == 1)
                {
                    Shooting(player2);
                    client.EnemyShip.bullet = 0;
                }

                if (player2.IsReload > 0)
                    player2.IsReload--;

                //Жив ли первый игрок
                if (player1.isAlive && player1.Health == 0)
                {
                    player1.isAlive = false;
                }

                //Жив ли второй игрок
                if (player2.isAlive && player2.Health == 0)
                {
                    player2.isAlive = false;
                }
            }
        }

        //Стрельба, создание пуль
        private void Shooting(Ship pl)
        {
            if (pl.weaponsMode)
            {
                int direction = pl.direction == 2 || pl.direction == 4 ? 1 : 2;
                if (direction == 1)
                {
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X, pl.position.Y - 5), 1));
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X, pl.position.Y - 25), 1));
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X, pl.position.Y + 15), 1));
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X, pl.position.Y - 5), 3));
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X, pl.position.Y - 25), 3));
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X, pl.position.Y + 15), 3));
                }
                else
                {
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X - 5, pl.position.Y), 2));
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X - 25, pl.position.Y), 2));
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X + 15, pl.position.Y), 2));
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X - 5, pl.position.Y), 4));
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X - 25, pl.position.Y), 4));
                    pl.bullets.Add(new Bullet(new Vector2(pl.position.X + 15, pl.position.Y), 4));
                }
            }
            else
                pl.bullets.Add(new Bullet(new Vector2(pl.position.X, pl.position.Y), pl.direction));
            pl.IsReload = pl.GetWeaponsReload();
        }

        public bool BorderCheck(Vector2 position)
        {
            return (borders.X < position.X && borders.Y < position.Y && borders.Z > position.X && borders.W > position.Y);
        }

        //Движение пуль
        public void BulletsMove(Ship pl, Ship plEnemy)
        {
            for (int i = 0; i < pl.bullets.Count; i++)
            {
                Bullet bullet = pl.bullets[i];
                switch (bullet.direction)
                {
                    case 1:
                        bullet.position.X -= pl.GetSpeedBullet();
                        break;
                    case 2:
                        bullet.position.Y -= pl.GetSpeedBullet();
                        break;
                    case 3:
                        bullet.position.X += pl.GetSpeedBullet();
                        break;
                    case 4:
                        bullet.position.Y += pl.GetSpeedBullet();
                        break;
                }

                if (!BorderCheck(bullet.position))
                {
                    pl.bullets.RemoveAt(i);
                    i--;
                }

                foreach (Rock rock in rocks)
                {
                    if (Features.CollisionRect((int)bullet.position.X, (int)bullet.position.Y, (int)rock.position.X, (int)rock.position.Y, 10, 10, rock.scale, rock.scale))
                    {
                        pl.bullets.RemoveAt(i);
                        i--;
                    }
                }

                if (Features.CollisionBullet(plEnemy, bullet))
                {
                    if (plEnemy.isAlive)
                        if (pl.weaponsMode)
                            plEnemy.TakeDamage((int)(pl.GetDamage() - plEnemy.GetDamageAbsorption() * pl.GetDamage()));
                        else
                            plEnemy.TakeDamage((int)((pl.GetDamage() - plEnemy.GetDamageAbsorption() * pl.GetDamage()) * 1.5f));
                    pl.bullets.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
