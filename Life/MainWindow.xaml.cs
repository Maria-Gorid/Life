using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
using Microsoft.Win32;

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
        }

        private void ShowMatrix()
        {
            for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
            {
                ((Rectangle) GameMatrix.Children[height * i + j]).Fill =
                    _gameLife[i, j] > 0 ? Brushes.Black : Brushes.White;
                ((Rectangle) GameMatrix.Children[height * i + j]).ToolTip = $"{_gameLife[i, j]}";
            }
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
                    rectangle.Tag = new int[] {i, j};

                    rectangle.PreviewMouseDown += FillCell;
                    rectangle.ToolTip = $"{_gameLife[i, j]}";

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
                int[] coords = (int[]) rectangle.Tag;
                _gameLife[coords[0], coords[1]] = Convert.ToInt32(rectangle.Fill == Brushes.Black);
                rectangle.ToolTip = $"{_gameLife[coords[0], coords[1]]}";
            }
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            _gameLife.NextGeneration();
            ShowMatrix();
        }

        [Obsolete("Obsolete")]
        void SaveToFile(string fileName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            bf.Serialize(fs, this._gameLife);
            fs.Close();
        }

        [Obsolete("Obsolete")]
        GameLife LoadFromFile(string fileName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            GameLife glife = (GameLife) bf.Deserialize(fs);
            fs.Close();
            return glife;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                SaveToFile(dialog.FileName);
            }
            // TODO: Сообщение об успешном выполнении
            // TODO: Безопасный способ сериализации
        }

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                _gameLife = LoadFromFile(dialog.FileName);
                this.height = _gameLife.Height;
                this.width = _gameLife.Width;
                ShowMatrix();
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _gameLife = LoadFromFile("state.dat");
            this.height = _gameLife.Height;
            this.width = _gameLife.Width;
            CreateMatrix(width, height);
            ShowMatrix();
        }

        private void MainWindow_OnClosing(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Сохранить текущие изменения?", "Сохранение изменений",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                SaveToFile("state.dat");
        }

        private void RandomGenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            _gameLife.RandomFill(50);
            ShowMatrix();
        }
    }
}