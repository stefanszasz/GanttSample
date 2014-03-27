using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace GanttSample
{
    public class ProcessChain
    {
        public ProcessChain(string header, ObservableCollection<Process> processes)
        {
            Header = header;
            Processes = processes;
            StartDate = processes.Min(x => x.StartDate);
            EndDate = processes.Max(x => x.EndDate);
        }

        public ObservableCollection<Process> Processes { get; set; }
        public string Header { get; set; }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Hint { get; set; }
    }

    public class Process
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Text { get; set; }
        public string Hint { get; set; }
        public Brush Color { get; set; }
        public bool IsItemVisible { get; set; }
        public string GroupName { get; set; }

        public Process()
        {
            Color = new SolidColorBrush(Colors.DeepPink);
            IsItemVisible = true;
        }

        public override string ToString()
        {
            return string.Format("StartDate: {0}, EndDate: {1}, Text: {2}", StartDate, EndDate, Text);
        }
    }
}