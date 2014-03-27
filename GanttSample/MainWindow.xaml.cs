using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GanttSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Process> observableCollection = new ObservableCollection<Process>();

        public MainWindow()
        {
            InitializeComponent();

            //observableCollection.Add(new Process
            //{
            //    StartDate = DateTime.Today.AddHours(9),
            //    EndDate = DateTime.Today.AddHours(12),
            //    Text = "Some task",
            //    Hint = "BLABLA hint1",
            //    Color = Brushes.RosyBrown
            //});
            //observableCollection.Add(new Process
            //{
            //    StartDate = DateTime.Today.AddHours(12),
            //    EndDate = DateTime.Today.AddHours(13),
            //    Text = "Other task",
            //    Hint = "BLABLA hint2",
            //    Color = Brushes.SandyBrown
            //});
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Today.AddHours(13),
                EndDate = DateTime.Today.AddHours(14),
                Text = "Green process completed",
                Hint = "BLABLA hint3",
                Color = Brushes.Green
            });
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Today.AddHours(13).AddMinutes(12),
                EndDate = DateTime.Today.AddHours(16),
                Text = "On next level failed",
                Hint = "Next row",
                Color = Brushes.Red
            });
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Today.AddHours(13).AddMinutes(45),
                EndDate = DateTime.Today.AddHours(17),
                Text = "Third row in progress",
                Hint = "Nextest row",
                Color = Brushes.Yellow
            });

            /*
            for (int i = 0; i < 100; i++)
            {
                var process = new Process
                {
                    StartDate = DateTime.Today.AddHours(i),
                    Text = "Row " + i,
                    Hint = "Tooltip" + i,
                    Color = Brushes.Goldenrod
                };

                process.EndDate = process.StartDate.AddHours(1);

                observableCollection.Add(process);
            }*/

            GanttControl.ItemsSource = observableCollection;
        }

        private int clickCount;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(2),
                Text = "New Item " + (++clickCount)
            });
        }

        private void DecreaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            GanttControl.ZoomOut();
        }

        private void IncreaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            GanttControl.ZoomIn();
        }

        private void ChangeDateFormatButton_OnClick(object sender, RoutedEventArgs e)
        {
            GanttControl.DateFormat = "dddd hh:mm tt";
        }

        private void MoveLeftButton_OnClick(object sender, RoutedEventArgs e)
        {
            GanttControl.MoveLeft();
        }

        private void MoveRightButton_OnClick(object sender, RoutedEventArgs e)
        {
            GanttControl.MoveRight();
        }
    }

    public class GanttItemWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
