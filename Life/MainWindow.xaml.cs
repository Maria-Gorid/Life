using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Life
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool[,] matrix;
        private int width = 25;
        private int height = 25;

        public MainWindow()
        {
            InitializeComponent();
            CreateMatrix(width, height);
            matrix = new bool[height, width];
        }

        private int GetNeighbors(int r, int c)
        {
            return Convert.ToInt32(matrix[(r - 1) % height, (c - 1) % width]) +
                   Convert.ToInt32(matrix[(r + 1) % height, (c - 1) % width]) +
                   Convert.ToInt32(matrix[(r + 1) % height, (c + 1) % width]) +
                   Convert.ToInt32(matrix[(r - 1) % height, (c + 1) % width]) +
                   Convert.ToInt32(matrix[(r - 1) % height, c]) +
                   Convert.ToInt32(matrix[(r + 1) % height, c]) +
                   Convert.ToInt32(matrix[r, (c - 1) % width]) +
                   Convert.ToInt32(matrix[r, (c + 1) % width]);
        }

        private void NextGeneration()
        {
            bool[,] tmpMatrix = new bool[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int neighborsCount = GetNeighbors(i, j);
                    bool currentState = matrix[i, j];
                    // currentState && neighborsCount <= 1 => False
                    // currentState && neighborsCount > 3  => False
                    // !currentState && neighborsCount == 3 => True
                    // currentState && (neighborsCount >= 2 && neighborsCount <= 3)

                    tmpMatrix[i, j] = (currentState && neighborsCount is >= 2 and <= 3) ||
                                      (!currentState && neighborsCount == 3);
                }
            }
            // TODO: Проверить, работает ЛИ
            matrix = tmpMatrix;
        }

        void CreateMatrix(int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = GameMatrix.Width / width;
                    rectangle.Height = GameMatrix.Height / height;
                    rectangle.Fill = Brushes.White;
                    rectangle.Stroke = Brushes.Gray;
                    rectangle.StrokeThickness = 1;
                    rectangle.Tag = new int[]{i, j};

                    rectangle.PreviewMouseDown += FillCell;

                    GameMatrix.Children.Add(rectangle);

                    Canvas.SetLeft(rectangle, rectangle.Width * j);
                    Canvas.SetTop(rectangle, rectangle.Height * i);
                }
            }
        }

        void FillCell(object sender, MouseEventArgs e)
        {
            Rectangle? rectangle = sender as Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = rectangle.Fill == Brushes.Black ? Brushes.White : Brushes.Black;
                int[] coords = (int[])rectangle.Tag;
                matrix[coords[0], coords[1]] = rectangle.Fill == Brushes.Black;
            }
        }
    }
}