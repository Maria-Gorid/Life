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
        private GameLife _gameLife;
        private int width = 25;
        private int height = 25;
        public MainWindow()
        {
            InitializeComponent();
            _gameLife = new GameLife(width, height);
            CreateMatrix(width, height);
        }

        private void ShowMatrix()
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    ((Rectangle)GameMatrix.Children[height * i + j]).Fill = _gameLife[i, j] ? Brushes.Black : Brushes.White;
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
                _gameLife[coords[0], coords[1]] = rectangle.Fill == Brushes.Black;
            }
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            _gameLife.NextGeneration();
            ShowMatrix();
        }
    }
}