using SharpDX;
using SharpDX.Direct2D1;
using GameControl;
using System.Windows.Input;
using System.Collections.Generic;
using GameObjects;

namespace SeaBattleGame
{
    /// <summary>
    /// Class for render game field
    /// </summary>
    class Renderer : Direct2DComponent
    {
        GameField field = new GameField();
        private SolidColorBrush color1;
        private SolidColorBrush color2;
        private SolidColorBrush colorHealth;
        private SolidColorBrush colorHealthBg;
        private SolidColorBrush colorReload;
        private SolidColorBrush colorReloadBg;
        SharpDX.DirectWrite.Factory factory;
        
        ShipFactory shipFactory = new ShipFactory();

        Bitmap shipLeft, shipRight, shipUp, shipDown, bulletBmp, seaBmp, flagpl1, flagpl2, rockBmp;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dc"></param>
        public Renderer(Dictionary<string, int> dc)
        {
            field.Initialize(dc);
        }
        
        /// <summary>
        /// Initialize render
        /// </summary>
        protected override void InternalInitialize()
        {
            base.InternalInitialize();

            shipLeft = BitmapWorker.LoadFromFile(RenderTarget2D, @"resources\shipLeft.png");
            shipRight = BitmapWorker.LoadFromFile(RenderTarget2D, @"resources\shipRight.png");
            shipUp = BitmapWorker.LoadFromFile(RenderTarget2D, @"resources\shipUp.png");
            shipDown = BitmapWorker.LoadFromFile(RenderTarget2D, @"resources\shipDown.png");
            bulletBmp = BitmapWorker.LoadFromFile(RenderTarget2D, @"resources\bullet.png");
            seaBmp = BitmapWorker.LoadFromFile(RenderTarget2D, @"resources\sea.jpg");
            flagpl1 = BitmapWorker.LoadFromFile(RenderTarget2D, @"resources\player1.png");
            flagpl2 = BitmapWorker.LoadFromFile(RenderTarget2D, @"resources\player2.png");
            rockBmp = BitmapWorker.LoadFromFile(RenderTarget2D, @"resources\rock.png");

            color1 = new SolidColorBrush(RenderTarget2D, new Color(0, 0, 0));
            color2 = new SolidColorBrush(RenderTarget2D, new Color(146, 78, 0));
            colorHealth = new SolidColorBrush(RenderTarget2D, new Color(0, 255, 0));
            colorHealthBg = new SolidColorBrush(RenderTarget2D, new Color(255, 0, 0));
            colorReload = new SolidColorBrush(RenderTarget2D, new Color(0, 0, 255));

            factory = new SharpDX.DirectWrite.Factory();
            colorReloadBg = new SolidColorBrush(RenderTarget2D, new Color(255, 255, 255));
            
        }

        protected override void InternalUninitialize()
        {
            Utilities.Dispose(ref color1);
            Utilities.Dispose(ref color2);

            base.InternalUninitialize();
        }


        /// <summary>
        /// Drawing of the playing field
        /// </summary>
        protected override void Render()
        {
            field.UpdatePosition();
            //Фон
            RenderTarget2D.Clear(new Color(255, 255, 255));
            RenderTarget2D.DrawBitmap(seaBmp, new RectangleF(0, 0, 733, 703), 1.0f, BitmapInterpolationMode.Linear);

            //Полет пули и проверка на удар
            field.BulletsMove(field.player1, field.player2);

            field.BulletsMove(field.player2, field.player1);

            //Отрисовка игроков
            Bitmap ship = chooseDirectionShip(field.player1.direction);
            RenderTarget2D.DrawBitmap(ship, new RectangleF(field.player1.position.X - field.player1.width / 2, field.player1.position.Y - field.player1.height / 2, field.player1.width, field.player1.height), 1.0f, BitmapInterpolationMode.Linear);
            RenderTarget2D.DrawBitmap(flagpl1, new RectangleF(field.player1.position.X - 14, field.player1.position.Y - 14, 28, 28), 1.0f, BitmapInterpolationMode.Linear);
            ship = chooseDirectionShip(field.player2.direction);
            RenderTarget2D.DrawBitmap(ship, new RectangleF(field.player2.position.X - field.player2.width / 2, field.player2.position.Y - field.player2.height / 2, field.player2.width, field.player2.height), 1.0f, BitmapInterpolationMode.Linear);
            RenderTarget2D.DrawBitmap(flagpl2, new RectangleF(field.player2.position.X - 14, field.player2.position.Y - 14, 28, 28), 1.0f, BitmapInterpolationMode.Linear);

            //Полоса здоровья первого игрока
            RenderTarget2D.FillRectangle(new RectangleF(40, 20, 100, 10), colorHealthBg);
            RenderTarget2D.FillRectangle(new RectangleF(40, 20, (int)(field.player1.Health * (100.0 / 1000)), 10), colorHealth);

            //Полоса здоровья второго игрока
            RenderTarget2D.FillRectangle(new RectangleF(590, 20, 100, 10), colorHealthBg);
            RenderTarget2D.FillRectangle(new RectangleF(590, 20, (int)(field.player2.Health * (100.0 / 1000)), 10), colorHealth);

            //Полоса перезарядки первого игрока
            RenderTarget2D.FillRectangle(new RectangleF(40, 40, 100, 10), colorReloadBg);
            RenderTarget2D.FillRectangle(new RectangleF(40, 40, (int)((field.player1.GetWeaponsReload() - field.player1.IsReload) * (100.0 / field.player1.GetWeaponsReload())), 10), colorReload);

            //Полоса перезарядки второго игрока
            RenderTarget2D.FillRectangle(new RectangleF(590, 40, 100, 10), colorReloadBg);
            RenderTarget2D.FillRectangle(new RectangleF(590, 40, (int)((field.player2.GetWeaponsReload() - field.player2.IsReload) * (100.0 / field.player2.GetWeaponsReload())), 10), colorReload);

            drawRocks();

            drawBullets(field.player1);
            drawBullets(field.player2);

            //GameOver
            if (!field.player1.isAlive || !field.player2.isAlive)
            {
                var textFormat = new SharpDX.DirectWrite.TextFormat(factory, "Algerian", 70);
                RenderTarget2D.DrawText("GAME OVER", textFormat, new RectangleF(170, 220, 600, 400), color1);
                //GameOver
                if (!field.player1.isAlive)
                {
                    RenderTarget2D.DrawText("Player 2 WIN!", textFormat, new RectangleF(140, 360, 600, 400), color1);
                }else
                {
                    RenderTarget2D.DrawText("Player 1 WIN!", textFormat, new RectangleF(140, 360, 600, 400), color1);
                }
            }
        }

        private void drawBullets(Ship pl)
        {
            foreach(Bullet bullet in pl.bullets)
            {
                RenderTarget2D.DrawBitmap(bulletBmp, new RectangleF(bullet.position.X, bullet.position.Y, 10, 10), 1.0f, BitmapInterpolationMode.Linear);
            }
        }

        /// <summary>
        /// Drawing rocks
        /// </summary>
        private void drawRocks()
        {
            foreach(Rock rock in field.rocks)
            {
                RenderTarget2D.DrawBitmap(rockBmp, new RectangleF(rock.position.X, rock.position.Y, rock.scale, rock.scale), 1.0f, BitmapInterpolationMode.Linear);
            }
        }

        private Bitmap chooseDirectionShip(int direction)
        {
            switch (direction)
            {
                case 1: return shipLeft;
                case 2: return shipUp;
                case 3: return shipRight;
                case 4: return shipDown;
            }
            return shipLeft;
        }
    }
}
