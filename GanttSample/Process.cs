﻿using System;
using System.Windows.Media;

namespace GanttSample
{
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