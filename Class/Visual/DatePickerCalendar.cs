﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace FilesBoxing.Class.Visual
{
    public class DatePickerCalendar
    {
        public static readonly DependencyProperty IsMonthYearProperty = DependencyProperty.RegisterAttached("IsMonthYear", typeof(bool), typeof(DatePickerCalendar), new PropertyMetadata(OnIsMonthYearChanged));

        public static readonly DependencyProperty IsYearProperty = DependencyProperty.RegisterAttached("IsYear", typeof(bool), typeof(DatePickerCalendar), new PropertyMetadata(OnIsMonthYearChanged));

        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.RegisterAttached("IsEditable", typeof(bool), typeof(DatePickerCalendar), new PropertyMetadata(true, OnIsEditableChanged));

        public static bool IsMonthYear(DependencyObject dobj)
        {
            return (bool)dobj.GetValue(IsMonthYearProperty);
        }
        public static bool IsYear(DependencyObject dobj)
        {
            return (bool)dobj.GetValue(IsYearProperty);
        }
        public static void SetIsMonthYear(DependencyObject dobj, bool value)
        {
            dobj.SetValue(IsMonthYearProperty, value);
        }
        public static bool IsEditable(DependencyObject dobj)
        {
            return (bool)dobj.GetValue(IsEditableProperty);
        }
        public static void SetIsEditable(DependencyObject dobj, bool value)
        {
            dobj.SetValue(IsEditableProperty, value);
        }
        private static void OnIsEditableChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)dobj;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action<DatePicker, DependencyPropertyChangedEventArgs>(ApplyIsEditable), datePicker, e);
        }
        public static void SetIsYear(DependencyObject dobj, bool value)
        {
            dobj.SetValue(IsYearProperty, value);
        }


        private static void ApplyIsEditable(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)dobj;

            Application.Current.Dispatcher
                .BeginInvoke(DispatcherPriority.Loaded,
                    new Action<DatePicker, DependencyPropertyChangedEventArgs>(SetTextBoxReadOnly),
                    datePicker, e);
        }
        private static void OnIsMonthYearChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)dobj;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action<DatePicker, DependencyPropertyChangedEventArgs>(SetCalendarEventHandlers), datePicker, e);

        }
        private static void SetTextBoxReadOnly(DatePicker datePicker, DependencyPropertyChangedEventArgs e)
        {
            var isEdit = IsEditable(datePicker);
            TemplateButton(datePicker).IsEnabled = isEdit;
            TemplateTextBox(datePicker).IsReadOnly = !isEdit;
        }
        private static DatePickerTextBox TemplateTextBox(Control control)
        {
            return (DatePickerTextBox)control.Template.FindName("PART_TextBox", control);
        }
        private static Button TemplateButton(Control control)
        {
            return (Button)control.Template.FindName("PART_Button", control);
        }
        private static void SetCalendarEventHandlers(DatePicker datePicker, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            if ((bool)e.NewValue)
            {
                datePicker.CalendarOpened += DatePickerOnCalendarOpened;
                datePicker.CalendarClosed += DatePickerOnCalendarClosed;
            }
            else
            {
                datePicker.CalendarOpened -= DatePickerOnCalendarOpened;
                datePicker.CalendarClosed -= DatePickerOnCalendarClosed;
            }
        }
        private static void DatePickerOnCalendarOpened(object sender, RoutedEventArgs routedEventArgs)
        {


            var calendar = ConvertToDatePickerCalendar(sender);
            var dobj = (DependencyObject)sender;

            calendar.DisplayMode = IsMonthYear(dobj) ? CalendarMode.Year : CalendarMode.Decade;
            calendar.DisplayModeChanged += CalendarOnDisplayModeChanged;
        }
        private static void DatePickerOnCalendarClosed(object sender, RoutedEventArgs routedEventArgs)
        {
            var datePicker = (DatePicker)sender;
            var calendar = ConvertToDatePickerCalendar(sender);
            datePicker.SelectedDate = calendar.SelectedDate;

            calendar.DisplayModeChanged -= CalendarOnDisplayModeChanged;
        }
        private static void CalendarOnDisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var calendar = (Calendar)sender;
            var dobj = (DependencyObject)sender;
            if (calendar.DisplayMode != CalendarMode.Month && IsMonthYear(dobj))
                return;
            if (calendar.DisplayMode != CalendarMode.Decade && IsYear(dobj))
                return;
            calendar.SelectedDate = SelectedCalendarDate(calendar.DisplayDate);
            var datePicker = RootCalendarsDatePicker(calendar);
            datePicker.IsDropDownOpen = false;
        }
        private static Calendar ConvertToDatePickerCalendar(object sender)
        {
            var datePicker = (DatePicker)sender;
            var popup = (Popup)datePicker.Template.FindName("PART_Popup", datePicker);
            return ((Calendar)popup.Child);
        }
        private static DatePicker RootCalendarsDatePicker(FrameworkElement child)
        {
            while (true)
            {
                var parent = (FrameworkElement)child.Parent;
                if (parent.Name == "PART_Root") return (DatePicker)parent.TemplatedParent;
                child = parent;
            }
        }
        private static DateTime? SelectedCalendarDate(DateTime? selectedDate)
        {
            if (!selectedDate.HasValue)
                return null;
            return new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, 1);
        }
    }
}