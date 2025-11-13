using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;


// Youtube: Louis Yang, WPF 2D Graphics
// https://www.youtube.com/watch?v=wTxktpA5GUM


namespace PacMan
{
    class Constants
    {
        public static int CORRIDOR_WIDTH = 60;
        public static int CORRIDOR_WIDTH_HALF = 30;
        public static int COIN_SIZE = 4;
    }

    class IntegerPoint
    {
        public int x;
        public int y;
        public IntegerPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int distanceInX(int x)
        {
            return Math.Abs(this.x - x);
        }
        public int distanceInY(int y)
        {
            return Math.Abs(this.y - y);
        }
        public int distance(IntegerPoint p)
        {
            return (int)Math.Sqrt(Math.Pow(x - p.x, 2) + Math.Pow(y - p.y, 2));
        }
    }

    class Character
    {
        protected IntegerPoint position = new IntegerPoint(Constants.CORRIDOR_WIDTH + Constants.CORRIDOR_WIDTH_HALF, Constants.CORRIDOR_WIDTH + Constants.CORRIDOR_WIDTH_HALF);
        protected IntegerPoint velocity = new IntegerPoint(0, 0);
        
        public Character()
        {
        }

        public void update() {
            position.x += this.velocity.x;
            position.y += this.velocity.y;
        }
        
        public Point getPosition()
        {
            return new Point(position.x, position.y);
        }

        public IntegerPoint getIntPosition()
        {
            return position;
        }
    }

    class Pacman : Character
    {
        int pendingNewDirAge = 0;
        IntegerPoint pendingNewDir = new IntegerPoint(0, 0);
        public bool gameOver = false;
        public double mouthMoveTime = 0;

        
        public void left()
        {
            pendingNewDir.x = -3;
            pendingNewDir.y = 0;
            pendingNewDirAge = 20;
        }

        public void right()
        {
            pendingNewDir.x = 3;
            pendingNewDir.y = 0;
            pendingNewDirAge = 20;
        }

        public void up()
        {
            pendingNewDir.x = 0;
            pendingNewDir.y = -3;
            pendingNewDirAge = 20;
        }

        public void down()
        {
            pendingNewDir.x = 0;
            pendingNewDir.y = 3;
            pendingNewDirAge = 20;
        }

        /// <summary>
        /// Draw the pacman to the given drawing context.
        /// </summary>
        /// <param name="dc">The drawing context to which pacman is drawn.</param>
        public void draw(DrawingContext dc)
        {
            bool eyeUp = false;
            double angle = 0;
            Point p = getPosition();
            if ((velocity.x > 0) && (velocity.y == 0))
            {
                angle = 0.0;
            }
            else if ((velocity.x < 0) && (velocity.y == 0))
            {
                angle = Math.PI;
                eyeUp = true;
            }
            else if ((velocity.x == 0) && (velocity.y > 0))
            {
                angle = Math.PI / 2.0;
            }
            else
            {
                angle = 3 * Math.PI / 2.0;
            }
            int r = Constants.CORRIDOR_WIDTH_HALF * 2 / 3
            double a = Math.Abs(Math.Sin(mouthMoveTime) * Math.PI / 180 * 25);
            Point ofs1 = new Point(Math.Cos(angle + a) * r, Math.Sin(angle + a) * r);
            Point ofs2 = new Point(Math.Cos(angle - a) * r, Math.Sin(angle - a) * r);
            dc.DrawEllipse(Brushes.Blue, new Pen(Brushes.Black, 0), p, r, r);
            double stepSize = 0.05;
            for (double aa = a; aa <= Math.PI*2-a; aa += stepSize)
            {
                Point pt1 = new Point(p.X + Math.Cos(angle + aa) * r, p.Y + Math.Sin(angle + aa) * r);
                Point pt2 = new Point(p.X + Math.Cos(angle + aa + stepSize) * r, p.Y + Math.Sin(angle + aa + stepSize) * r);
                dc.DrawLine(new Pen(Brushes.Black, 3), pt1, pt2);
            }
            dc.DrawLine(new Pen(Brushes.Black, 3), p, new Point(p.X + ofs1.X, p.Y + ofs1.Y));
            dc.DrawLine(new Pen(Brushes.Black, 3), p, new Point(p.X + ofs2.X, p.Y + ofs2.Y));
            angle -= Math.PI / 180.0 * 60;
            double eyeX = p.X + Math.Cos(angle) * 0.65 * r;
            double eyeY = p.Y + Math.Sin(angle) * 0.65 * r;
            if (eyeUp)
            {
                eyeY = p.Y - Math.Sin(angle) * 0.65 * r;
            }
            dc.DrawEllipse(Brushes.Black, new Pen(Brushes.Black, 0), new Point(eyeX, eyeY), 3, 3);
            mouthMoveTime += 0.2;
        }

        private void checkForMonsterCollision(Monster[] monster)
        {
            for (int i = 0; i < monster.Length; i++)
            {
                if (monster[i].getIntPosition().distance(position) < Constants.CORRIDOR_WIDTH_HALF)
                {
                    gameOver = true;
                }
            }
        }

        public void update(World w, Monster[] monster)
        {
            checkForMonsterCollision(monster);

            if (pendingNewDirAge > 0)
            {
                if (w.checkIntersection(new IntegerPoint(position.x + pendingNewDir.x, position.y + pendingNewDir.y)))
                {
                    // move is not possible: just reduce value of pending new pt age.
                    pendingNewDirAge--;
                } 
                else
                {
                    // move is possible: take over the new values for velocity, new pt age is zero.
                    pendingNewDirAge = 0;
                    this.velocity.x = pendingNewDir.x;
                    this.velocity.y = pendingNewDir.y;
                }
            }
            position.x += this.velocity.x;
            position.y += this.velocity.y;
            if (w.checkIntersection(position))
            {
                position.x -= this.velocity.x;
                position.y -= this.velocity.y;
                this.velocity.x = 0;
                this.velocity.y = 0;
            }
        }
    }

    class Monster : Character
    {
        static Random rand = new Random();

        /// <summary>
        /// Constructor: Create a new monster at random position
        /// </summary>
        public Monster()
        {
            // calculate random init position until position is not in top left corner (i.e. far from Pacman)
            do {
                position = new IntegerPoint(
                    Constants.CORRIDOR_WIDTH_HALF + Constants.CORRIDOR_WIDTH * (int)(rand.NextDouble() * 11 + 1),
                    Constants.CORRIDOR_WIDTH_HALF + Constants.CORRIDOR_WIDTH * (int)(rand.NextDouble() * 7 + 1));
            } while ((position.x < 5 * Constants.CORRIDOR_WIDTH) && (position.y < 5 * Constants.CORRIDOR_WIDTH));
            velocity.x = -3;
            velocity.y = 0;
        }

        
        /// <summary>
        /// Draw the monster
        /// </summary>
        /// <param name="dc">The drawing context to which the monster is drawn.</param>
        public void draw(DrawingContext dc)
        {
            int size = Constants.CORRIDOR_WIDTH_HALF;
            int x = position.x - Constants.CORRIDOR_WIDTH_HALF / 2;
            int y = position.y - Constants.CORRIDOR_WIDTH_HALF / 2;
            dc.DrawRectangle(Brushes.Green, new Pen(Brushes.Black, 2), new Rect(new Point(x, y), new Point(x+size, y+size)));

        }

        public int getBwDirectionIndex()
        {
            if (velocity.x == 3) return 1;
            if (velocity.x == -3) return 0;
            if (velocity.y == 3) return 3;
            if (velocity.y == -3) return 2;
            return -1;
        }

        public bool updateVelocityByDir(World w, int direction)
        {
            int vx = 0;
            int vy = 0;
            bool ok = false;
            switch(direction)
            {
                case 0: { vx = 3; break; }
                case 1: { vx = -3; break; }
                case 2: { vy = 3; break; }
                case 3: { vy = -3; break; }
            }
            if (w.checkIntersection(new IntegerPoint(position.x + vx, position.y + vy)))
            {
                velocity.x = 0;
                velocity.y = 0;
            }
            else
            {
                velocity.x = vx;
                velocity.y = vy;
                ok = true;
            }
            return ok;
        }

        public void update(World w)
        {
            int[] direction = { 0, 1, 2, 3 };
            int j = 0;
            bool ok = false;

            int currBwDir = getBwDirectionIndex();

            double r = rand.NextDouble();
            
            for (int i = 0; i < 4; i++)
            {
                int i0 = (int)(rand.NextDouble() * 3.99);
                int i1 = (int)(rand.NextDouble() * 3.99);
                int h = direction[i0];
                direction[i0] = direction[i1];
                direction[i1] = h;
            }
            while((ok == false) && (j < 4))
            {
                if (direction[j] != currBwDir)
                ok = updateVelocityByDir(w, direction[j]);
                j++;
            }
            position.x += velocity.x;
            position.y += velocity.y;
        }
    }

    class Coins
    {
        bool[] coinsStates;
        int width;
        int height;
        int coinsEaten = 0;

        public Coins(int w = 11, int h = 8)
        {
            width = w;
            height = h;
            coinsStates = new bool[width * height];
            for (int i = 0; i < coinsStates.Length; i++)
            {
                coinsStates[i] = true;
            }
        }
        public bool eatCoin(int x, int y)
        {
            if (coinsStates[y * width + x])
            {
                coinsStates[y * width + x] = false;
                coinsEaten++;
                return true;
            }
            return false;
        }

        public int getNumCoinsEaten()
        {
            return coinsEaten;
        }

        public bool allCoinsEaten()
        {
            return coinsEaten == coinsStates.Length;
        }

        public void update(IntegerPoint pacmanPosition)
        {
            int x = (int)((pacmanPosition.x - Constants.CORRIDOR_WIDTH) / Constants.CORRIDOR_WIDTH);
            int y = (int)((pacmanPosition.y - Constants.CORRIDOR_WIDTH) / Constants.CORRIDOR_WIDTH);
            eatCoin(x, y);
        }

        public void draw(DrawingContext dc)
        {
            for (int i = 0; i < coinsStates.Length; i++)
            {
                if (coinsStates[i])
                {
                    int size = Constants.COIN_SIZE;
                    int x = (i % width + 1) * Constants.CORRIDOR_WIDTH + Constants.CORRIDOR_WIDTH_HALF;
                    int y = (i / width + 1) * Constants.CORRIDOR_WIDTH + Constants.CORRIDOR_WIDTH_HALF;
                    dc.DrawRectangle(Brushes.Red, new Pen(Brushes.Black, 0), new Rect(new Point(x - size, y - size), new Point(x + size, y + size)));
                }
            }
        }
    }

    class Wall
    {
        private IntegerPoint pt1;
        private IntegerPoint pt2;
        private bool horizontal;
        public Wall(IntegerPoint p1, IntegerPoint p2)
        {
            int corridorSize = Constants.CORRIDOR_WIDTH;
            pt1 = new IntegerPoint(p1.x * corridorSize, p1.y * corridorSize);
            pt2 = new IntegerPoint(p2.x * corridorSize, p2.y * corridorSize);
            int dx = p1.distanceInX(p2.x);
            int dy = p1.distanceInY(p2.y);
            if (dx == 0)
            {
                horizontal = false;
            } else if (dy == 0)
            {
                horizontal = true;
            } else {
                throw new Exception("Error! Invalid Wall definition. Must be zero length in either X or Y.");
            }
        }
        public bool checkIntersection(IntegerPoint position)
        {
            int intersectionSpacing = Constants.CORRIDOR_WIDTH_HALF-1;
            if (this.horizontal)
            {
                if ((position.x + intersectionSpacing > pt1.x) && (position.x - intersectionSpacing < pt2.x) && (position.distanceInY(pt1.y) < intersectionSpacing)) {
                    return true; // collision!
                }
            }
            else
            {
                if ((position.y + intersectionSpacing > pt1.y) && (position.y - intersectionSpacing < pt2.y) && (position.distanceInX(pt1.x) < intersectionSpacing))
                {
                    return true; // collision!
                }
            }
            return false;
        }
        public void draw(DrawingContext dc)
        {
            int s = Constants.CORRIDOR_WIDTH_HALF / 6;
            int x1 = pt1.x - s;
            int y1 = pt1.y - s;
            int x2 = pt2.x + s;
            int y2 = pt2.y + s;
            dc.DrawRectangle(Brushes.Black, new Pen(Brushes.Black, s), new Rect(new Point(x1, y1), new Point(x2, y2)));
        }
    }

    class World
    {
        Wall[] walls = new Wall[33];
        public World()
        {
            // Outside border
            walls[0] = new Wall(new IntegerPoint(1, 1), new IntegerPoint(1, 9));
            walls[1] = new Wall(new IntegerPoint(1, 1), new IntegerPoint(12, 1));
            walls[2] = new Wall(new IntegerPoint(1, 9), new IntegerPoint(12, 9));
            walls[3] = new Wall(new IntegerPoint(12, 1), new IntegerPoint(12, 9));
 
            walls[4] = new Wall(new IntegerPoint(2, 2), new IntegerPoint(4, 2));
            walls[5] = new Wall(new IntegerPoint(2, 3), new IntegerPoint(2, 4));
            walls[6] = new Wall(new IntegerPoint(5, 2), new IntegerPoint(7, 2));
            walls[7] = new Wall(new IntegerPoint(8, 2), new IntegerPoint(11, 2));
            walls[8] = new Wall(new IntegerPoint(2, 2), new IntegerPoint(4, 2));
            walls[9] = new Wall(new IntegerPoint(3, 2), new IntegerPoint(3, 4));
            walls[10] = new Wall(new IntegerPoint(4, 3), new IntegerPoint(7, 3));
            walls[11] = new Wall(new IntegerPoint(2, 5), new IntegerPoint(6, 5));
            walls[12] = new Wall(new IntegerPoint(7, 5), new IntegerPoint(10, 5));
            walls[13] = new Wall(new IntegerPoint(4, 4), new IntegerPoint(4, 5));
            walls[14] = new Wall(new IntegerPoint(11, 3), new IntegerPoint(11, 5));
            walls[15] = new Wall(new IntegerPoint(5, 4), new IntegerPoint(7, 4));
            walls[16] = new Wall(new IntegerPoint(8, 3), new IntegerPoint(8, 4));
            walls[17] = new Wall(new IntegerPoint(9, 3), new IntegerPoint(9, 4));
            walls[18] = new Wall(new IntegerPoint(10, 3), new IntegerPoint(10, 4));
            walls[19] = new Wall(new IntegerPoint(4, 7), new IntegerPoint(4, 8));
            walls[20] = new Wall(new IntegerPoint(6, 7), new IntegerPoint(6, 8));
            walls[21] = new Wall(new IntegerPoint(8, 6), new IntegerPoint(8, 7));
            walls[22] = new Wall(new IntegerPoint(9, 6), new IntegerPoint(9, 8));
            walls[23] = new Wall(new IntegerPoint(9, 6), new IntegerPoint(11, 6));
            walls[24] = new Wall(new IntegerPoint(10, 7), new IntegerPoint(11, 7));
            walls[25] = new Wall(new IntegerPoint(10, 8), new IntegerPoint(11, 8));

            // the H in HTL (left bottom region)
            walls[26] = new Wall(new IntegerPoint(2, 6), new IntegerPoint(2, 8));
            walls[27] = new Wall(new IntegerPoint(2, 7), new IntegerPoint(3, 7));
            walls[28] = new Wall(new IntegerPoint(3, 6), new IntegerPoint(3, 8));

            // the T in HTL (left bottom region)
            walls[29] = new Wall(new IntegerPoint(4, 6), new IntegerPoint(6, 6));
            walls[30] = new Wall(new IntegerPoint(5, 6), new IntegerPoint(5, 8));

            // the L in HTL (left bottom region)
            walls[31] = new Wall(new IntegerPoint(7, 6), new IntegerPoint(7, 8));
            walls[32] = new Wall(new IntegerPoint(7, 8), new IntegerPoint(8, 8));
        }
        
        public bool checkIntersection(IntegerPoint position)
        {
            for (int i = 0; i < walls.Length; i++)
            {
                if (walls[i].checkIntersection(position))
                {
                    return true;
                }
            }
            return false;
        }

        public void draw(DrawingContext dc)
        {
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].draw(dc);
            }
        }
    }

    /// <summary>
    /// Canvas object for drawing the pacman game.
    /// </summary>
    class MyCanvas : Canvas {

        public Pacman pacman;
        Monster[] monster;
        Coins coins;
        World world;
        Label bottomLabel;
        DispatcherTimer timer;

        /// <summary>
        /// Constructor: creates a new pacman game.
        /// </summary>
        public MyCanvas()
        {
            init();
        }

        /// <summary>
        /// This method is invoked when the user presses the start-button. A new
        /// pacman game is then initialized.
        /// </summary>
        public void reset()
        {
            if (timer != null)
            {
                timer.Stop();
            }
            init();
        }

        /// <summary>
        /// Initialize the pacman game. This private method is called initially by the constructor or when
        /// the user re-starts a game by clicking on the start-button.
        /// </summary>
        private void init()
        {
            pacman = new Pacman();
            monster = new Monster[4];
            coins = new Coins();
            world = new World();
            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromSeconds(0.02);
            timer.Tick += onTimer;
            timer.Start();

            for (int i = 0; i < monster.Length; i++)
            {
                monster[i] = new Monster();
            }
        }

        

        public void setBottomLabel(Label bottomLabel)
        {
            this.bottomLabel = bottomLabel;
        }

        void onTimer(object sender, EventArgs e)
        {
            this.coins.update(pacman.getIntPosition());
            for (int i = 0; i < monster.Length; i++)
            {
                this.monster[i].update(world);
            }
            this.pacman.update(world, monster);

            if (pacman.gameOver)
            {
                bottomLabel.Content = "Game Over!";
                timer.Stop();
            } 
            else if (coins.allCoinsEaten())
            {
                bottomLabel.Content = "Congratulations! You win!";
                timer.Stop();
            }
            else
            {
                bottomLabel.Content = "Points: " + coins.getNumCoinsEaten();
            }

            this.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            this.world.draw(dc);
            coins.draw(dc);
            this.pacman.draw(dc);
            for (int i = 0; i < monster.Length; i++)
            {
                this.monster[i].draw(dc);
            }
        }

    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Line> lines = new List<Line>();
        MyCanvas mc = new MyCanvas();

        public void Window_KeyDown(object sender, KeyEventArgs keyPressed)
        {
            if (keyPressed.Key == Key.Left)
            {
                mc.pacman.left();
            }
            else if (keyPressed.Key == Key.Right)
            {
                mc.pacman.right();
            }
            else if (keyPressed.Key == Key.Up)
            {
                mc.pacman.down();
            }
            else if (keyPressed.Key == Key.Down)
            {
                mc.pacman.up();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            MyStackPanel.Children.Add(mc);

            mc.setBottomLabel(bottomLabel);
        }

        private void Button_start_Click(object sender, RoutedEventArgs e)
        {
            mc.reset();
        }
    }
}
