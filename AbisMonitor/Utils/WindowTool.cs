using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AbisMonitor.UI.Controls;
using AbisMonitor.Views;

namespace AbisMonitor.UI.Utils
{
    public static class WindowTool
    {



        /// <summary>
        /// 使用示例 
        ///  TreeViewItem selectNode = DependencyVisual.VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }

        public static DependencyObject GetChildOfType<T>(this DependencyObject source) where T : DependencyObject
        {
            if (source == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(source); i++)
            {
                var child = VisualTreeHelper.GetChild(source, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        public static UserControl CreateControl(string tabNameSpace)
        {
            UserControl userControl = null;
            userControl = typeof(MainWindow).Assembly.CreateInstance(tabNameSpace) as UserControl;
            return userControl;
        }

        public static UserControl CreateTabItemControl(string tabNamespace)
        {
            UserControl userControl = null;
            userControl = typeof(MainWindow).Assembly.CreateInstance(tabNamespace) as UserControl;
            return userControl;
        }

        /// <summary>
        /// 展开选中项所有父节点
        /// </summary>
        /// <param name="treeViewItem"></param>
        public static void ExpandParentNodes(TreeViewItem treeViewItem)
        {
            if (treeViewItem == null) return;
            var parent = treeViewItem.Parent;
            var parentTreeViewItem = parent as TreeViewItem;
            if (parentTreeViewItem != null)
            {
                parentTreeViewItem.IsExpanded = true;
                ExpandParentNodes(parentTreeViewItem);
            }

        }
    }
}
