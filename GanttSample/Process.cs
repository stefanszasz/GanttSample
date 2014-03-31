using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using GanttSample.Annotations;

namespace GanttSample
{
    public class ProcessChain : IHaveStartAndEndDate, INotifyPropertyChanged
    {
        private DateTime startDate;
        private DateTime endDate;

        public ProcessChain(string header, ObservableCollection<Process> processes)
        {
            Header = header;
            Processes = processes;
            StartDate = processes.Min(x => x.StartDate);
            EndDate = processes.Max(x => x.EndDate);
            Processes.CollectionChanged += Processes_CollectionChanged;
        }

        void Processes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StartDate = Processes.Min(x => x.StartDate);
            EndDate = Processes.Max(x => x.EndDate);
        }

        public ObservableCollection<Process> Processes { get; set; }
        public string Header { get; set; }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        public string Hint { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Process : IHaveStartAndEndDate, INotifyPropertyChanged
    {
        private DateTime startDate;
        private DateTime endDate;
        private bool isItemVisible;

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (value.Equals(startDate)) return;
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (value.Equals(endDate)) return;
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        public string Text { get; set; }
        public string Hint { get; set; }
        public Brush Color { get; set; }

        public bool IsItemVisible
        {
            get { return isItemVisible; }
            set
            {
                if (value.Equals(isItemVisible)) return;
                isItemVisible = value;
                OnPropertyChanged("IsItemVisible");
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}