using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using AbisMonitor.Domain;
using AbisMonitor.UI.Messager;
using AbisMonitor.UI.Models;
using AbisMonitor.UI.Utils;
using AbisMonitor.UI.ViewModels;
using AbisMonitor.ViewModels;
using Framework;
using GalaSoft.MvvmLight.Threading;


namespace AbisMonitor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainView_Closing;
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<UserControlSelectChangedMessage>(this,
                UserControlSelectChangedMessageAction);

            this.Unloaded += (s, e) => GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }

        private void MainView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*
                if (((MainViewModel)(this.DataContext)).Data.IsModified)
                if (!((MainViewModel)(this.DataContext)).PromptSaveBeforeExit())
                {
                    e.Cancel = true;
                    return;
                }
            */
            Log.Info("Closing App");
        }

        private void TreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem clickNode =
                WindowTool.VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (clickNode == null)
                return;

            var tag = clickNode.Tag;
            if (tag == null || string.IsNullOrWhiteSpace(tag.ToString())) return;
            var control = WindowTool.CreateTabItemControl(tag.ToString());
            if (control == null) return;

            var title = string.Empty;
            var image = clickNode.GetChildOfType<Image>() as Image;
            var imageSource = string.Empty;
            if (image != null)
            {
                imageSource = image.Source.ToString();
            }


            var stackPanel = clickNode.GetChildOfType<StackPanel>() as StackPanel;

            if (stackPanel != null && stackPanel.Tag is AbisDeviceSimple)
            {
                //设备跟踪
                var abisDeviceSimple = stackPanel.Tag as AbisDeviceSimple;
       
                ((MainViewModel) this.DataContext).AddTabDeviceTrack(clickNode, imageSource, tag.ToString(), control,
                    abisDeviceSimple);
            }
            else
            {
                //常规导航
                var textBlock = clickNode.GetChildOfType<TextBlock>() as TextBlock;

                if (textBlock != null)
                {
                    title = textBlock.Text;
                }
                ((MainViewModel) this.DataContext).AddTabItemControl(clickNode, title, imageSource, tag.ToString(),
                    control);
            }
        }

        private void UserControlSelectChangedMessageAction(UserControlSelectChangedMessage message)
        {
            if (message == null) return;
            if (message.TabItemModel == null)  //没有TabItem项，清除选择
            {
                var selectTreeViewItem = this.tvNavigation.SelectedItem as TreeViewItem;
                if (selectTreeViewItem != null)
                {
                    selectTreeViewItem.IsSelected = false;
                }
            }
            else
            {
                var treeViewItem = message.TabItemModel.TreeViewItem;
                if (treeViewItem == null) return;
                WindowTool.ExpandParentNodes(treeViewItem);
                treeViewItem.IsSelected = true;
            }
        }


        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScrollViewer scroller = (ScrollViewer) tabControl.Template.FindName("TabControlScroller", tabControl);
            if (scroller != null)
            {   
                double index = (double) (tabControl.SelectedIndex);

                double offset = scroller.ScrollableWidth * (index / (double)(tabControl.Items.Count));
                if (tabControl.SelectedIndex + 1 == tabControl.Items.Count)
                {
                    scroller.ScrollToRightEnd();
                }
                else if (tabControl.SelectedIndex == 0)
                {
                    scroller.ScrollToLeftEnd();
                }
                else
                {
                    scroller.ScrollToHorizontalOffset(offset);
                }
            }
        }
    }
}
