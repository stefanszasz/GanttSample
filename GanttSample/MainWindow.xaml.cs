﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                Text = "On next level",
                Hint = "Row1",
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

            observableCollection.Add(new Process
            {
                StartDate = DateTime.Today.AddHours(13).AddMinutes(45),
                EndDate = DateTime.Today.AddHours(17),
                Text = "Forth row",
                Hint = "Nextest row",
                Color = Brushes.Orange
            });
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Today.AddHours(14),
                EndDate = DateTime.Today.AddHours(16),
                Text = "Fifth row",
                Hint = "Nextest row",
                Color = Brushes.Purple
            });
            observableCollection.Add(new Process
            {
                StartDate = DateTime.Today.AddHours(13).AddMinutes(63),
                EndDate = DateTime.Today.AddHours(17),
                Text = "Sixth row",
                Hint = "Nextest row",
                Color = Brushes.Orange
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

            var processChain = new ProcessChain("PC1", observableCollection);
            var processes = new ObservableCollection<Process>(observableCollection);
            //processes[0].StartDate = processes[0].StartDate.AddMinutes(20);
            //processes[2].StartDate = processes[2].StartDate.AddHours(3);
            var chain = new ProcessChain("PC2", processes);
            processChains = new ObservableCollection<ProcessChain> { processChain, chain };

            GanttControl.ItemsSource = processChains;
        }

        private int clickCount;
        private ObservableCollection<ProcessChain> processChains = new ObservableCollection<ProcessChain>();
        private Process selectedProcess;

        public Process SelectedProcess
        {
            get { return selectedProcess; }
            set { selectedProcess = value; }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            processChains[0].Processes.Add(new Process
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(3),
                Text = "New Item " + (++clickCount)
            });
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedProcess != null)
                SelectedProcess.IsItemVisible = false;
        }
    }
}
