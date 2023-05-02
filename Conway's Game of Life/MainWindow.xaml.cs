using System;
using System.Diagnostics.Eventing.Reader;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Conway_s_Game_of_Life
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Generation currentGeneration = new Generation(30,30);

        private const double gapSize = 2.0;
        private readonly SolidColorBrush tileColor = new(Color.FromRgb(0, 186, 0));
        private readonly DispatcherTimer timer = new();
        private bool IsTimerActive = false;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += DrawNextGenerationTick;
            
            DrawCanva();
        }

        private void DrawNextGenerationTick(object? sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
            currentGeneration.ResetGeneration();
            
        }
        private void StopTimer()
        {
            timer.Stop();
            IsTimerActive = false;
            btnStartStop.Content = "Start";
        }

        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (!IsTimerActive)
            {
                timer.Start();
                IsTimerActive = true;
                btnStartStop.Content = "Stop";
            }
            else
            {
                StopTimer();
            }            
        }

        private void btnNextGen_Click(object sender, RoutedEventArgs e)
        {            
            DrawNextGeneration();
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
            currentGeneration.SetRandomGeneration();            
        }

        private void DrawCanva()
        {
            
            double cvDrawBoardWidthTile = cvDrawBoard.Width / currentGeneration.columnLenght;
            double cvDrawBoardHeightTile = cvDrawBoard.Height / currentGeneration.rowLenght;

            cvDrawBoard.Children.Clear();

            currentGeneration.ForEach((tile) => {
        
                Rectangle r = new()
                {
                    Width = cvDrawBoardWidthTile - gapSize,
                    Height = cvDrawBoardHeightTile - gapSize,
                };

                
                if(tile.Alive) { r.Fill = tileColor; }
                else { r.Fill = Brushes.Black; }

                cvDrawBoard.Children.Add(r);
                Canvas.SetRight(r, (tile.Column * cvDrawBoardWidthTile) + (gapSize / 2));
                Canvas.SetBottom(r, (tile.Row * cvDrawBoardHeightTile) + (gapSize / 2));

                //Connect rectangle with class
                tile.RTile = r;

                //Set Event
                r.MouseDown += (object sender, MouseButtonEventArgs arg) => { tile.Alive = !tile.Alive; };


            });
        }

        private void DrawNextGeneration()
        {
            currentGeneration.SetNextGeneration();            
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timer.Interval = comboBox.SelectedIndex switch
            {
                0 => TimeSpan.FromMilliseconds(200),
                1 => TimeSpan.FromMilliseconds(500),
                2 => TimeSpan.FromMilliseconds(1000),
                3 => TimeSpan.FromMilliseconds(3000),
                _ => TimeSpan.FromMilliseconds(1000),
            };
        }

        private void comboBoxProbability_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentGeneration.Probability = comboBoxProbability.SelectedIndex switch
            {
                0 => 50,
                1 => 30,
                2 => 20,
                3 => 10,
                4 => 5,
                _ => 50,
            };
        }
    }
}
