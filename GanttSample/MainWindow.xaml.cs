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
        public MainWindow()
        {
            InitializeComponent();
            var observableCollection = new ObservableCollection<Process>();
            observableCollection.Add(new Process
            {
                StartDate = DateTime.MinValue.AddHours(9),
                EndDate = DateTime.MinValue.AddHours(12),
                Text = "Some task",
                Hint = "BLABLA hint",
                Color = Brushes.RosyBrown
            });
            observableCollection.Add(new Process
            {
                StartDate = DateTime.MinValue.AddHours(12),
                EndDate = DateTime.MinValue.AddHours(13),
                Text = "Other task",
                Hint = "BLABLA hint",
                Color = Brushes.SandyBrown
            });
            observableCollection.Add(new Process
            {
                StartDate = DateTime.MinValue.AddHours(13),
                EndDate = DateTime.MinValue.AddHours(17),
                Text = "Sha task",
                Hint = "BLABLA hint",
                Color = Brushes.RoyalBlue
            });

            GanttControl.ItemsSource = observableCollection;
        }
    }

    public class Process
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Text { get; set; }
        public string Hint { get; set; }
        public Brush Color { get; set; }

        public override string ToString()
        {
            return string.Format("StartDate: {0}, EndDate: {1}, Text: {2}", StartDate, EndDate, Text);
        }
    }
}
