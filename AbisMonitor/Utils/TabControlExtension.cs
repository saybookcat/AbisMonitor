using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AbisMonitor.UI.Utils
{
    public static class TabControlExtension
    {
        public static readonly DependencyProperty SelectItemOnRightClickProperty = DependencyProperty.RegisterAttached(
           "SelectItemOnRightClick",
           typeof(bool),
           typeof(TabControlExtension),
           new UIPropertyMetadata(false, OnSelectItemOnRightClickChanged));

        public static bool GetSelectItemOnRightClick(DependencyObject d)
        {
            return (bool)d.GetValue(SelectItemOnRightClickProperty);
        }

        public static void SetSelectItemOnRightClick(DependencyObject d, bool value)
        {
            d.SetValue(SelectItemOnRightClickProperty, value);
        }

        private static void OnSelectItemOnRightClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool selectItemOnRightClick = (bool)e.NewValue;

            TabControl tabControl = d as TabControl;
            if (tabControl != null)
            {
                if (selectItemOnRightClick)
                    tabControl.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;
                else
                    tabControl.PreviewMouseRightButtonDown -= OnPreviewMouseRightButtonDown;
            }
        }

        private static void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TabItem tabControlItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (tabControlItem != null)
            {
                tabControlItem.IsSelected = true;
                tabControlItem.Focus();
                e.Handled = true;
            }
        }

        public static TabItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TabItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TabItem;
        }

    }
}
