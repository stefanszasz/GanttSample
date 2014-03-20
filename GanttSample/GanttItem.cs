using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace GanttSample
{
    public class GanttItem : ListBoxItem
    {
        public readonly Guid Id;

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Brush), typeof(GanttItem), new PropertyMetadata(default(Brush)));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

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

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(GanttItem), new PropertyMetadata(default(string)));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty OrderProperty = DependencyProperty.Register(
            "Order", typeof(int), typeof(GanttItem), new PropertyMetadata(default(int)));

        public int Order
        {
            get { return (int)GetValue(OrderProperty); }
            set { SetValue(OrderProperty, value); }
        }

        public static readonly DependencyProperty IsItemVisibleProperty = DependencyProperty.Register(
            "IsItemVisible", typeof (bool), typeof (GanttItem), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool IsItemVisible
        {
            get { return (bool) GetValue(IsItemVisibleProperty); }
            set { SetValue(IsItemVisibleProperty, value); }
        }

        public GanttItem()
        {
            DefaultStyleKey = typeof(GanttItem);
            Id = Guid.NewGuid();
            MouseLeftButtonDown += GanttItem_MouseLeftButtonDown;
        }

        void GanttItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Selector.SetIsSelected(this, true);                     
            IsSelected = true;
        }

        public override string ToString()
        {
            return string.Format("{0}, StartDate: {1}, EndDate: {2}, Text: {3}", base.ToString(), StartDate, EndDate, Text);
        }
    }
}