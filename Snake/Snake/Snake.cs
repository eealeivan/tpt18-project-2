using System.Windows.Controls;
using System.Windows.Shapes;

namespace SnakeGame
{
    class Snake
    {
        Shape snakeShape;
        double cellSize;
        int cellCount;

        Direction direction;

        public Snake(
            Shape snakeShape,
            double cellSize,
            int cellCount)
        {
            this.snakeShape = snakeShape;
            this.cellSize = cellSize;
            this.cellCount = cellCount;
        }

        public void Init()
        {
            snakeShape.Height = cellSize;
            snakeShape.Width = cellSize;
            double coord = cellCount * cellSize / 2;
            Canvas.SetTop(snakeShape, coord);
            Canvas.SetLeft(snakeShape, coord);

            ChangeDirection(Direction.Up);
        }

        public void ChangeDirection(Direction newDirection)
        {
            direction = newDirection;
        }

        public void Move()
        {
            if (direction == Direction.Up ||
               direction == Direction.Down)
            {
                double currentTop = Canvas.GetTop(snakeShape);
                double newTop = direction == Direction.Up
                    ? currentTop - cellSize
                    : currentTop + cellSize;
                Canvas.SetTop(snakeShape, newTop);
            }

            if (direction == Direction.Left ||
                direction == Direction.Right)
            {
                double currentLeft = Canvas.GetLeft(snakeShape);
                double newLeft = direction == Direction.Left
                    ? currentLeft - cellSize
                    : currentLeft + cellSize;
                Canvas.SetLeft(snakeShape, newLeft);
            }
        }
    }
}
