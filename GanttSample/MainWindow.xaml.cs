using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

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
                Text = "Sha task",
                Hint = "BLABLA hint3",
                Color = Brushes.Khaki
            });
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Today.AddHours(13).AddMinutes(12),
                EndDate = DateTime.Today.AddHours(16),
                Text = "On next level",
                Hint = "Next row",
                Color = Brushes.Red
            });
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Today.AddHours(13).AddMinutes(45),
                EndDate = DateTime.Today.AddHours(17),
                Text = "Third row",
                Hint = "Nextest row",
                Color = Brushes.Wheat
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
                StartDate = new DateTime(2014, 3, 24, 15, 0, 0, 0),
                EndDate = new DateTime(2014, 3, 24, 17, 0, 0, 0),
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
}
