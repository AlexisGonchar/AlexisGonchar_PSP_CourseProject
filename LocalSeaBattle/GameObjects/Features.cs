using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace GameObjects
{
    /// <summary>
    /// Static class for calculate
    /// </summary>
    public static class Features
    {
        public static void swap(ref int a, ref int b)
        {
            int c = a;
            a = b;
            b = c;
        }

        //Общая проверка на коллизию
        public static bool CollisionRect(int r1x, int r1y, int r2x, int r2y, int r1w, int r1h, int r2w, int r2h)
        {
            if (r1x + r1w >= r2x &&
                r1x <= r2x + r2w &&
                r1y + r1h >= r2y &&
                r1y <= r2y + r2h)
            {
                return true;
            }
            return false;
        }

        public static int[] getShipParameters(int index)
        {
            return File.ReadAllLines("config")[index].Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
        }

        //Проверка на коллизию между игроками
        public static bool CollisionCheck(Ship player1, Ship player2)
        {
            int r1x, r1y, r2x, r2y, r1w, r1h, r2w, r2h;
            r1x = (int)(player1.position.X - player1.width / 2);
            r1y = (int)(player1.position.Y - player1.height / 2);
            r2x = (int)(player2.position.X - player2.width / 2);
            r2y = (int)(player2.position.Y - player2.height / 2);
            r1w = player1.width;
            r1h = player1.height;
            r2w = player2.width;
            r2h = player2.height;
            return Features.CollisionRect(r1x, r1y, r2x, r2y, r1w, r1h, r2w, r2h);
        }

        //Проверка на коллизию с пулей
        public static bool CollisionBullet(Ship pl, Bullet bl)
        {
            int r1x, r1y, r2x, r2y, r1w, r1h, r2w, r2h;
            r1x = (int)(pl.position.X - pl.width / 2);
            r1y = (int)(pl.position.Y - pl.height / 2);
            r2x = (int)bl.position.X;
            r2y = (int)bl.position.Y;
            r1w = pl.width;
            r1h = pl.height;
            r2w = 10;
            r2h = 10;
            return Features.CollisionRect(r1x, r1y, r2x, r2y, r1w, r1h, r2w, r2h);
        }

        //Коллизия между игроком и скалой
        public static bool CollisionRockPl(Ship pl, List<Rock> rocks)
        {
            int r1x, r1y, r1w, r1h;
            r1x = (int)(pl.position.X - pl.width / 2);
            r1y = (int)(pl.position.Y - pl.height / 2);
            r1w = pl.width;
            r1h = pl.height;
            foreach (Rock rock in rocks)
            {
                if (Features.CollisionRect(r1x, r1y, (int)rock.position.X, (int)rock.position.Y, r1w, r1h, rock.scale, rock.scale))
                {
                    return true;
                }
            }
            return false;
        }

        //Смена направления корабля
        public static void SwapDirection(Ship pl, int direction)
        {
            if ((direction == 1 || direction == 3) && pl.height > pl.width)
            {
                Features.swap(ref pl.width, ref pl.height);
            }
            else if ((direction == 2 || direction == 4) && pl.height < pl.width)
            {
                Features.swap(ref pl.width, ref pl.height);
            }
            pl.direction = direction;
        }
    }
}
