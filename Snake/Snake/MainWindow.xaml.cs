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

        GameStatus gameStatus;

        Direction snakeDirection;
        int snakeRow;
        int snakeCol;

        public MainWindow()
        {
            InitializeComponent();
            DrawBoardBackground();
            InitSnake();

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
                    Canvas.SetTop(r, row * CellSize);
                    Canvas.SetLeft(r, col * CellSize);
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

        private void InitSnake()
        {
            snakeShape.Height = CellSize;
            snakeShape.Width = CellSize;
            int index = CellCount / 2;
            snakeRow = index;
            snakeCol = index;
            SetShape(snakeShape, snakeRow, snakeCol);

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
            switch (snakeDirection)
            {
                case Direction.Up:
                    snakeRow--;
                    break;
                case Direction.Down:
                    snakeRow++;
                    break;
                case Direction.Left:
                    snakeCol--;
                    break;
                case Direction.Right:
                    snakeCol++;
                    break;
            }

            if(snakeRow < 0 || snakeRow >= CellCount ||
                snakeCol < 0 || snakeCol >= CellCount)
            {
                ChangeGameStatus(GameStatus.GameOver);
                return;
            }

            SetShape(snakeShape, snakeRow, snakeCol);
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
