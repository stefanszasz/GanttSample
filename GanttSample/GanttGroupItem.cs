using System;
using System.Windows;
using System.Windows.Controls;

namespace GanttSample
{
    public class GanttGroupItem : ListBoxItem
    {
        public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register(
            "StartDate", typeof(DateTime), typeof(GanttItem), new PropertyMetadata(default(DateTime)));

        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public static readonly DependencyProperty EndDateProperty = DependencyProperty.Register(
            "EndDate", typeof(DateTime), typeof(GanttItem), new PropertyMetadata(default(DateTime)));

        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(string), typeof(GanttGroupItem), new PropertyMetadata(default(string)));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
    }
}