using System;
using System.Collections.Generic;
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
        Random rnd = new Random();

        GameStatus gameStatus;

        int foodRow;
        int foodCol;

        Direction snakeDirection;
        LinkedList<Rectangle> snakeParts =
            new LinkedList<Rectangle>();

        int points;

        public MainWindow()
        {
            InitializeComponent();
            DrawBoardBackground();
            InitFood();
            InitSnake();
            ChangePoints(0);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();

            ChangeGameStatus(GameStatus.Ongoing);
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
                    SetShape(r, row, col);
                    board.Children.Add(r);

                    color = color == color1 ? color2 : color1;
                }
            }
        }

        private void ChangeGameStatus(GameStatus newGameStatus)
        {
            gameStatus = newGameStatus;
            lblGameStatus.Content =
                $"Status: {gameStatus}";
        }

        private void ChangePoints(int newPoints)
        {
            points = newPoints;
            lblPoints.Content =
                $"Points: {points}";
        }

        private void InitFood()
        {
            foodShape.Height = CellSize;
            foodShape.Width = CellSize;
            foodRow = rnd.Next(0, CellCount);
            foodCol = rnd.Next(0, CellCount);
            SetShape(foodShape, foodRow, foodCol);
        }

        private void InitSnake()
        {
            int index = CellCount / 2;
            for (int i = 0; i < 3; i++)
            {
                int row = index;
                int col = index + i;

                Location location = new Location(row, col);

                Rectangle r = new Rectangle();
                r.Height = CellSize;
                r.Width = CellSize;
                r.Fill = Brushes.MediumBlue;
                Panel.SetZIndex(r, 10);
                r.Tag = location;

                SetShape(r, row, col);
                board.Children.Add(r);
                snakeParts.AddLast(r);
            }

            ChangeSnakeDirection(Direction.Left);
        }

        private void ChangeSnakeDirection(Direction direction)
        {
            snakeDirection = direction;
            lblSnakeDirection.Content =
                $"Direction: {direction}";
        }

        private void MoveSnake()
        {
            Rectangle currentHead = snakeParts.First.Value;
            Location currentHeadLocation =
                (Location)currentHead.Tag;

            int newHeadRow = currentHeadLocation.Row;
            int newHeadCol = currentHeadLocation.Col;

            switch (snakeDirection)
            {
                case Direction.Up:
                    newHeadRow--;
                    break;
                case Direction.Down:
                    newHeadRow++;
                    break;
                case Direction.Left:
                    newHeadCol--;
                    break;
                case Direction.Right:
                    newHeadCol++;
                    break;
            }

            Location newHeadLocation =
               new Location(newHeadRow, newHeadCol);
            Rectangle newHead = snakeParts.Last.Value;
            newHead.Tag = newHeadLocation;

            SetShape(newHead, newHeadRow, newHeadCol);
            snakeParts.RemoveLast();
            snakeParts.AddFirst(newHead);



            //bool outOfBoundaries =
            //    snakePart.Row < 0 || snakePart.Row >= CellCount ||
            //    snakePart.Col < 0 || snakePart.Col >= CellCount;
            //if (outOfBoundaries)
            //{
            //    ChangeGameStatus(GameStatus.GameOver);
            //    return;
            //}

            //bool food =
            //    snakePart.Row == foodRow &&
            //    snakePart.Col == foodCol;
            //if (food)
            //{
            //    ChangePoints(points + 1);
            //    InitFood();
            //}

            //SetShape(snakeShape, snakePart.Row, snakePart.Col);
        }

        private void SetShape(
            Shape shape, int row, int col)
        {
            double top = row * CellSize;
            double left = col * CellSize;

            Canvas.SetTop(shape, top);
            Canvas.SetLeft(shape, left);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (gameStatus != GameStatus.Ongoing)
            {
                return;
            }

            MoveSnake();
        }

        private void Window_KeyDown(
            object sender, KeyEventArgs e)
        {
            if (gameStatus != GameStatus.Ongoing)
            {
                return;
            }

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
