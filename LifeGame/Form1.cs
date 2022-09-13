using System;
using System.Drawing;
using System.Windows.Forms;

namespace LifeGame
{
    public partial class Form1 : Form
    {
        public Form1(GameModel game)
        {
            DoubleBuffered = true;
            InitializeComponent();
            Size = SystemInformation.PrimaryMonitorSize;
            Text = "Игра \"Жизнь\"";
            var mapColor = Color.Black;
            var marginColor = Color.BurlyWood;
            var microbColor = Color.BurlyWood;
            BackColor = marginColor;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            MouseClick += (sender, args) => { game.FlipMicrobeState(MousePosition.X, MousePosition.Y, Size.Width, Size.Height); Invalidate(); };
            var timer = new Timer { Interval = 2000 };
            timer.Tick += (sender, args) =>
            {
                game.ActualizeMap();
                Invalidate();
            };
            timer.Start();
            Paint += (sender, args) =>
            {
                var marginHor = (Size.Width - game.MapWidth) / 2;
                var marginVert = (Size.Height - game.MapHeight) / 2;
                args.Graphics.FillRectangle(new SolidBrush(mapColor), marginHor, marginVert, game.MapWidth, game.MapHeight);
                for (int i = marginHor; i < Size.Width - marginHor; i += game.MicrobeSize)
                    for (int j = marginVert; j < Size.Height - marginVert; j += game.MicrobeSize)
                        if (game.MicrobeMap[(i - marginHor) / game.MicrobeSize, (j - marginVert) / game.MicrobeSize])
                            args.Graphics.FillRectangle(new SolidBrush(microbColor), i, j, game.MicrobeSize, game.MicrobeSize);
            };

            var decreaseIntervalBtn = new Button
            {
                Location = new Point(Size.Width / 2 + 20, Size.Height - (Size.Height - game.MapHeight) / 4),
                Size = new Size(40, 40),
                Text = "+SPD"
            };
            decreaseIntervalBtn.Click += (sender, args) => { if (timer.Interval / 2 > 0) timer.Interval /= 2; };

            var increaseIntervalBtn = new Button
            {
                Location = new Point(Size.Width / 2 - 60, Size.Height - (Size.Height - game.MapHeight) / 4),
                Size = new Size(40, 40),
                Text = "-SPD"
            };
            increaseIntervalBtn.Click += (sender, args) => { timer.Interval *= 2; };

            var randomMapBtn = new Button
            {
                Location = new Point(Size.Width / 2 + 60, Size.Height - (Size.Height - game.MapHeight) / 4),
                Size = new Size(40, 40),
                Text = "RANDOM"
            };
            randomMapBtn.Click += (sender, args) =>
            {
                game.RandomizeMap();
                Invalidate();
            };

            var increaseMapBtn = new Button
            {
                Location = new Point(Size.Width / 2 + 140, Size.Height - (Size.Height - game.MapHeight) / 4),
                Size = new Size(40, 40),
                Text = "+Map"
            };
            increaseMapBtn.Click += (sender, args) =>
            {
                if (game.MapHeight < Size.Height)
                {
                    game = new GameModel(game.MicrobeSize, game.MapWidth + 64, game.MapHeight + 64);
                    Invalidate();
                }
            };

            var decreaseMapBtn = new Button
            {
                Location = new Point(Size.Width / 2 + 100, Size.Height - (Size.Height - game.MapHeight) / 4),
                Size = new Size(40, 40),
                Text = "-Map"
            };
            decreaseMapBtn.Click += (sender, args) =>
            {
                if (game.MapWidth > 64)
                {
                    game = new GameModel(game.MicrobeSize, game.MapWidth - 64, game.MapHeight - 64);
                    Invalidate();
                }
            };

            var increaseMicrobBtn = new Button
            {
                Location = new Point(Size.Width / 2 - 140, Size.Height - (Size.Height - game.MapHeight) / 4),
                Size = new Size(40, 40),
                Text = "+Microb"
            };
            increaseMicrobBtn.Click += (sender, args) =>
            {
                if (game.MicrobeSize < 64)
                {
                    game = new GameModel(game.MicrobeSize * 2, game.MapWidth, game.MapHeight);
                    Invalidate();
                }
            };

            var decreaseMicrobBtn = new Button
            {
                Location = new Point(Size.Width / 2 - 180, Size.Height - (Size.Height - game.MapHeight) / 4),
                Size = new Size(40, 40),
                Text = "-Microb"
            };
            decreaseMicrobBtn.Click += (sender, args) =>
            {
                if (game.MicrobeSize / 2 != 0)
                {
                    game = new GameModel(game.MicrobeSize / 2, game.MapWidth, game.MapHeight);
                    Invalidate();
                }
            };

            var pauseBtn = new Button
            {
                Location = new Point(Size.Width / 2 - 20, Size.Height - (Size.Height - game.MapHeight) / 4),
                Size = new Size(40, 40),
                Text = "| |"
            };
            pauseBtn.Click += (sender, args) =>
            {
                if (timer.Enabled)
                {
                    timer.Stop();
                    pauseBtn.Text = ">";
                }
                else
                {
                    timer.Start();
                    pauseBtn.Text = "| |";
                }
            };

            var changeColorBtn = new Button
            {
                Location = new Point(Size.Width / 2 - 100, Size.Height - (Size.Height - game.MapHeight) / 4),
                Size = new Size(40, 40),
                Text = "Recolor"
            };
            changeColorBtn.Click += (sender, args) =>
            {
                var rnd = new Random();
                BackColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                microbColor = BackColor;
                mapColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                Invalidate();
            };

            var label = new Label
            {
                Size = new Size(ClientSize.Width, 30),
                Location = new Point(0, (Size.Height - game.MapHeight) / 4 - 16),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter
            };
            Paint += (sender, args) => label.Text = $"Cycle duration: {timer.Interval / 1000.0}s;  " +
                $"microb syze: {game.MicrobeSize};  map size: {game.MapWidth}x{game.MapHeight}";

            Controls.Add(decreaseIntervalBtn);
            Controls.Add(increaseIntervalBtn);
            Controls.Add(randomMapBtn);
            Controls.Add(increaseMapBtn);
            Controls.Add(decreaseMapBtn);
            Controls.Add(increaseMicrobBtn);
            Controls.Add(decreaseMicrobBtn);
            Controls.Add(pauseBtn);
            Controls.Add(label);
            Controls.Add(changeColorBtn);
        }
    }

    public class GameModel
    {
        public bool[,] MicrobeMap;
        public int MicrobeSize;
        public int MapWidth;
        public int MapHeight;

        public GameModel(int microbeSize, int mapWidth, int mapHeight)
        {
            MicrobeSize = microbeSize;
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            MicrobeMap = new bool[mapWidth / microbeSize, mapHeight / microbeSize];
        }
        public void FlipMicrobeState(int x, int y, int screenWidth, int screenHeight)
        {
            if (x >= (screenWidth - MapWidth) / 2 && x < screenWidth - (screenWidth - MapWidth) / 2
                && y >= (screenHeight - MapHeight) / 2 && y < screenHeight - (screenHeight - MapHeight) / 2)
                MicrobeMap[(x - (screenWidth - MapWidth) / 2) / MicrobeSize, (y - (screenHeight - MapHeight) / 2) / MicrobeSize] ^= true;
        }

        public void RandomizeMap()
        {
            var rnd = new Random();
            for (int i = 0; i < MapWidth / MicrobeSize; i++)
                for (int j = 0; j < MapHeight / MicrobeSize; j++)
                    MicrobeMap[i, j] = rnd.Next(0, 2) == 0;
        }

        public void ActualizeMap()
        {
            var tempMap = (bool[,])MicrobeMap.Clone();
            for (int i = 0; i < MapWidth / MicrobeSize; i++)
                for (int j = 0; j < MapHeight / MicrobeSize; j++)
                    tempMap[i, j] = MicrobeMap[i, j];
            for (int i = 0; i < MapWidth/MicrobeSize; i++)
                for (int j = 0; j < MapHeight/MicrobeSize; j++)
                {
                    var neighboorsCount = 0;
                    for (int k = -1; k < 2; k++)
                        if (i + k >= 0 && i + k < MapWidth/MicrobeSize)
                            for (int m = -1; m < 2; m++)
                                if (j + m >= 0 && j + m < MapHeight/MicrobeSize)
                                    if (MicrobeMap[i + k, j + m] && !(k == 0 && m == 0))
                                        neighboorsCount++;
                    switch (neighboorsCount)
                    {
                        case 0:
                            {
                                tempMap[i, j] = false;
                                break;
                            }
                        case 1:
                            {
                                tempMap[i, j] = false;
                                break;
                            }
                        case 2:
                                break;
                        case 3:
                            {
                                tempMap[i, j] = true;
                                break;
                            }
                        default:
                            {
                                tempMap[i, j] = false;
                                break;
                            }
                    }
                }
            MicrobeMap = tempMap;
        }
    }
}
