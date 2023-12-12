using System;

namespace Life;

public class GameLife
{
    private bool[,] matrix;
    private int width;
    private int height;

    public GameLife(int width, int height)
    {
        this.width = width;
        this.height = height;
        matrix = new bool[height, width];
    }

    public bool this[int i, int j]
    {
        get => matrix[i, j];
        set => matrix[i, j] = value;
    }

    private int GetNeighbors(int r, int c)
    {
        return Convert.ToInt32(matrix[(height + r - 1) % height, (width + c - 1) % width]) +
               Convert.ToInt32(matrix[(height + r + 1) % height, (width + c - 1) % width]) +
               Convert.ToInt32(matrix[(height + r + 1) % height, (width + c + 1) % width]) +
               Convert.ToInt32(matrix[(height + r - 1) % height, (width + c + 1) % width]) +
               Convert.ToInt32(matrix[(height + r - 1) % height, c]) +
               Convert.ToInt32(matrix[(height + r + 1) % height, c]) +
               Convert.ToInt32(matrix[r, (width + c - 1) % width]) +
               Convert.ToInt32(matrix[r, (width + c + 1) % width]);
    }

    public void NextGeneration()
    {
        bool[,] tmpMatrix = new bool[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int neighborsCount = GetNeighbors(i, j);
                bool currentState = matrix[i, j];

                tmpMatrix[i, j] = (currentState && neighborsCount is >= 2 and <= 3) ||
                                  (!currentState && neighborsCount == 3);
            }
        }

        matrix = tmpMatrix;
    }

    // TODO: Генерация с вероятностью
    public void RandomFill(int LifePercent)
    {
        Random rnd = new Random();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                matrix[i, j] = rnd.Next(1) == 1;
            }
        }
    }
}