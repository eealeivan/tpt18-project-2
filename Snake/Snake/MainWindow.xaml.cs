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
            timer.Interval = TimeSpan.FromSeconds(0.25);
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
                    InitRectangle(
                        CellSize, row, col, color, 0);

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

                Rectangle r = InitRectangle(
                    CellSize, row, col, Brushes.MediumBlue, 10);

                snakeParts.AddLast(r);
            }

            ChangeSnakeDirection(Direction.Left);
        }

        private void ChangeSnakeDirection(Direction direction)
        {
            if (snakeDirection == Direction.Left &&
               direction == Direction.Right)
            {
                return;
            }

            if (snakeDirection == Direction.Right &&
                direction == Direction.Left)
            {
                return;
            }

            if (snakeDirection == Direction.Up &&
                direction == Direction.Down)
            {
                return;
            }

            if (snakeDirection == Direction.Down &&
                direction == Direction.Up)
            {
                return;
            }

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

            bool outOfBoundaries =
                newHeadRow < 0 || newHeadRow >= CellCount ||
                newHeadCol < 0 || newHeadCol >= CellCount;
            if (outOfBoundaries)
            {
                ChangeGameStatus(GameStatus.GameOver);
                return;
            }

            bool food =
               newHeadRow == foodRow &&
               newHeadCol == foodCol;

            foreach (Rectangle r in snakeParts)
            {
                if (!food && snakeParts.Last.Value == r)
                {
                    continue;
                }

                Location location = (Location)r.Tag;
                if (location.Row == newHeadRow &&
                   location.Col == newHeadCol)
                {
                    ChangeGameStatus(GameStatus.GameOver);
                    return;
                }
            }

            if (food)
            {
                ChangePoints(points + 1);
                InitFood();

                Rectangle r = InitRectangle(
                    CellSize,
                    newHeadRow,
                    newHeadCol,
                    Brushes.MediumBlue,
                    10);
                snakeParts.AddFirst(r);
            }
            else
            {
                Rectangle newHead = snakeParts.Last.Value;
                newHead.Tag = new Location(newHeadRow, newHeadCol);

                SetShape(newHead, newHeadRow, newHeadCol);
                snakeParts.RemoveLast();
                snakeParts.AddFirst(newHead);
            }
        }

        private Rectangle InitRectangle(
            double size,
            int row,
            int col,
            Brush fill,
            int zIndex)
        {
            Rectangle r = new Rectangle();
            r.Height = size;
            r.Width = size;
            r.Fill = fill;
            Panel.SetZIndex(r, zIndex);
            r.Tag = new Location(row, col);

            SetShape(r, row, col);
            board.Children.Add(r);

            return r;
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
