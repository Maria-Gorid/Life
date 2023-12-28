using System;

namespace Life;

[Serializable]
public class GameLife
{
    private int[,] matrix;
    private int width;
    private int height;

    public int Width => width;
    public int Height => height;

    public GameLife(int width, int height)
    {
        this.width = width;
        this.height = height;
        matrix = new int[height, width];
    }

    public int this[int i, int j]
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
        int[,] tmpMatrix = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int neighborsCount = GetNeighbors(i, j);
                int currentState = matrix[i, j];
                int value = Convert.ToInt32((currentState > 0 && neighborsCount is >= 2 and <= 3) ||
                                            (!(currentState > 0) && neighborsCount == 3));
                tmpMatrix[i, j] = value == 0 ? 0 : value;
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
                matrix[i, j] = rnd.Next(2);
            }
        }
    }
}