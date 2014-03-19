using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Today.AddHours(9),
                EndDate = DateTime.Today.AddHours(12),
                Text = "Some task",
                Hint = "BLABLA hint1",
                Color = Brushes.RosyBrown
            });
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Today.AddHours(12),
                EndDate = DateTime.Today.AddHours(13),
                Text = "Other task",
                Hint = "BLABLA hint2",
                Color = Brushes.SandyBrown
            });
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Today.AddHours(13),
                EndDate = DateTime.Today.AddHours(14),
                Text = "Sha task",
                Hint = "BLABLA hint3",
                Color = Brushes.RoyalBlue
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
                Color = Brushes.Green
            });

            GanttControl.ItemsSource = observableCollection;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(3),
                Text = DateTime.Now.ToString("t")
            });

        }
    }
}
