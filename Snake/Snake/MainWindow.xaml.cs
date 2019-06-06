using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double CellSize = 30D;
        const int CellCount = 16;

        DispatcherTimer timer;
        Direction snakeDirection;

        public MainWindow()
        {
            InitializeComponent();
            DrawBoardBackground();
            InitSnake();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }        

        private void DrawBoardBackground()
        {
            SolidColorBrush color1 = Brushes.LightGreen;
            SolidColorBrush color2 = Brushes.LimeGreen;

            for (int row = 0; row < CellCount; row++)
            {
                SolidColorBrush color =
                    row % 2 == 0 ? color1 : color2;

                for (int col = 0; col < CellCount; col++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = CellSize;
                    r.Height = CellSize;
                    r.Fill = color;
                    Canvas.SetTop(r, row * CellSize);
                    Canvas.SetLeft(r, col * CellSize);
                    board.Children.Add(r);

                    color = color == color1 ? color2 : color1;
                }
            }
        }

        private void InitSnake()
        {
            snakeShape.Height = CellSize;
            snakeShape.Width = CellSize;
            double coord = CellCount * CellSize / 2;
            Canvas.SetTop(snakeShape, coord);
            Canvas.SetLeft(snakeShape, coord);

            ChangeSnakeDirection(Direction.Up);
        }

        private void ChangeSnakeDirection(Direction direction)
        {
            snakeDirection = direction;
            lblSnakeDirection.Content =
                $"Direction: {direction}";
        }

        private void MoveSnake()
        {
            if (snakeDirection == Direction.Up ||
               snakeDirection == Direction.Down)
            {
                double currentTop = Canvas.GetTop(snakeShape);
                double newTop = snakeDirection == Direction.Up
                    ? currentTop - CellSize
                    : currentTop + CellSize;
                Canvas.SetTop(snakeShape, newTop);
            }

            if (snakeDirection == Direction.Left ||
                snakeDirection == Direction.Right)
            {
                double currentLeft = Canvas.GetLeft(snakeShape);
                double newLeft = snakeDirection == Direction.Left
                    ? currentLeft - CellSize
                    : currentLeft + CellSize;
                Canvas.SetLeft(snakeShape, newLeft);
            }
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        private void Window_KeyDown(
            object sender, KeyEventArgs e)
        {
            Direction direction;
            switch (e.Key)
            {
                case Key.Up:
                    direction = Direction.Up;
                    break;
                case Key.Down:
                    direction = Direction.Down;
                    break;
                case Key.Left:
                    direction = Direction.Left;
                    break;
                case Key.Right:
                    direction = Direction.Right;
                    break;
                default:
                    return;
            }

            ChangeSnakeDirection(direction);           
        }        
    }
}
