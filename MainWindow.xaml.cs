using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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

namespace TestWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public bool IsIndexesInInterval(int i, int j, int start, int end)
        {
            if ((i <= end) && (j <= end) && (i >= start) && (j >= start)) return true;
            return false;
        }

        public void Tap(List<List<Button>> matrix, int i, int j)
        {
            ReverseButtonContent(matrix[i][j]);
            if (IsIndexesInInterval(i, j - 1, 0, matrix.Count - 1)) ReverseButtonContent(matrix[i][j - 1]); //left
            if (IsIndexesInInterval(i, j + 1, 0, matrix.Count - 1)) ReverseButtonContent(matrix[i][j + 1]); //right
            if (IsIndexesInInterval(i - 1, j, 0, matrix.Count - 1)) ReverseButtonContent(matrix[i - 1][j]); //down
            if (IsIndexesInInterval(i + 1, j, 0, matrix.Count - 1)) ReverseButtonContent(matrix[i + 1][j]); //up
        }

        public void ReverseButtonContent(Button button)
        {
            if((int)button.Content == 0)
            {
                button.Content = 1;
                button.Background = new SolidColorBrush(Constants.ActiveColor);
            } 
            else
            {
                button.Content = 0;
                button.Background = new SolidColorBrush(Constants.DisActiveColor);
            }
        }

        public void RandomTaps(List<List<Button>> buttons)
        {
            var random = new Random();
            var RandomSize = random.Next(0, buttons.Count);
            
            for (int i = 0; i < Constants.StartTapsCount; i++)
            {
                int indexI = random.Next(0, buttons.Count - 1);
                int indexJ = random.Next(0, buttons.Count - 1);

                Tap(buttons, indexI, indexJ);
            }
        }

        public bool IsMatrixZero(List<List<Button>> matrix)
        {
            for(int i = 0; i < matrix.Count; i++)
            {
                for(int j = 0; j < matrix.Count; j++)
                {
                    if ((int)matrix[i][j].Content == 1) return false;
                }
            }
            return true;
        }

        public void Progress(List<List<Button>> matrix, int i, int j)
        {
            Tap(matrix, i, j);
            if (IsMatrixZero(matrix)) MessageBox.Show("Ты победил, Сладкий пирожок");
        }

        public void StartGameProcess(Slider _slider)
        {
            int n = (int)_slider.Value;
            var Matrix = new List<List<Button>>(n);

            for (int i = 0; i < n; i++)
            {
                Matrix.Add(new List<Button>(n));
                for(int j = 0; j < n; j++)
                {
                    int i1 = i; //Сомнительный костыль, без которого подписка на события работает некоректно => Tap(Matrix, n, n); 
                    int j1 = j;
                    Button button = new Button();
                    button.Height = uniformGrid.Height / n;
                    button.Width = uniformGrid.Width / n;
                    button.Content = 0;
                    button.Click += new RoutedEventHandler((e, v) => Progress(Matrix, i1 , j1)); 
                    button.Background = new SolidColorBrush(Constants.DisActiveColor);
                    Matrix[i].Add(button);
                    uniformGrid.Children.Add(button);
                }
            }
            RandomTaps(Matrix);
        }

        public MainWindow()
        {
            InitializeComponent();
            startButton.Height = grid.Height / Constants.GridButtonHeightPropotion;
            startButton.Width = grid.Width / Constants.GridButtonWidthPropotion;
            startButton.Content = "Запуск";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartGameProcess(slider);
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(label != null) label.Content = (int)slider.Value;
        }
    }
}
