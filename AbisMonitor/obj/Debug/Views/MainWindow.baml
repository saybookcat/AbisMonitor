   M S B A M L   `   `   `  ����K  GAbisMonitor.UI, Version=2.17.8.15, Culture=neutral, PublicKeyToken=nullE'clr-namespace:AbisMonitor.UI.ConvertersAbisMonitor.UI.Converters  5	   	   ;"clr-namespace:AbisMonitor.UI.UtilsAbisMonitor.UI.Utils  5
   	   "    AbisMonitor.Views.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   S OMvvmDialogs, Version=4.0.10.0, Culture=neutral, PublicKeyToken=3d3b9708589bc73e8md/https://github.com/fantasticfiasco/mvvm-dialogs  5   	   8
converters'clr-namespace:AbisMonitor.UI.Converters  5	   	   .utils"clr-namespace:AbisMonitor.UI.Utils  5
   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   $  MvvmDialogs.DialogServiceViews    IsRegistered  . $	��768��5   	   $
��1024��6   $	��467��5   	   $	��667��6    � WindowStartupLocation$ CenterScreen=�5   	    � WindowState$ 	Maximized=�5   	    J� SnapsToDevicePixels .  � Title 5   	   �� *Title+ � 	Resources 5       �� MergedDictionaries �� Source7   1AbisMonitor.UI.Converters.ConnectedStateConverter   Key �� 
TargetType	 �� Property
 �� Value �� 
TargetType � ColumnDefinitions �� Orientation, �� %ComputedHorizontalScrollBarVisibility � RowDefinitions ��Name  TabHeaderScrollViewer K� IsItemsHost  DefaultBrush ��Name ��Name  CloseButtonStyle �� RelativeSource �� AncestorType �� Triggers P� Property P� Value  TabBorderBrush �� BasedOn�� 5   
    5      �� 5      $ /Themes/FontCommon.xaml?�6%   �� 5      $ /Themes/TabControl.xaml?�6%   �� 5      $ /Themes/ColorTheme.xaml?�6%   �� 5      $" /Themes/MainWindowTheme.xaml?�6%   �� 5        5      ConnectedStateConverter  69   5      �� 5      �� 5"      TabHeaderScrollViewer  " ��6:   .�5#      �� 	 � ��
 5$      �� 5%   "   " ��62   .R�5&   &   �  5'   *   �� 5(   .   �� 5)   .   $
��Auto �6?   .+�5+   *   �� $!��PART_ScrollContentPresenter��6A   $��0��5,   A   �� 5/   *   $��PART_HorizontalScrollBar��64   $��1��50   4   $ 
Horizontal=�51   4   $��10, 0, 0, 0q�52   4   $��30��53   4   #y�z 54   4   55   '   56   #   58      59      �� 5;      TabControlContentTemplate  " ��6H   .R�5<      �  5=      �� 5>   "   $
��Auto �60   �� 5?   "   $��* �60   .+�5A      �� $ TabControlScroller��5B   7   $��0��5A   ,   $��Disabled=�5C   7   $
��Auto=�5D   7   #��[ 5E   7   .��5F   "   ��  .  . 6>   5G      �� 5H      $��1��6%   #��[ 63   $��1q�6_   ._�5I   "   �� $��SelectedContent��63   5J      5K      5L      �� 5O      " ��6   TabItemStyle  .�5P      �� 	 � ��$
 0,3,0,0q�63   �� 5Q      	 � ��$

 Auto��63   �� 5R      	 � ��$	
 300��65   �� 5S      	 � ��$
 5q�64   �� 5T      	 � ��$
 Stretch=�6@   �� 5U      	 � ��$
 Stretch=�6>   �� 5V      	 � ��$
 Stretch=�6G   �� 5W      	 � ��$
 Stretch=�6E   �� 5X      	 �@J�SnapsToDevicePixels
 . 6@   �� 5Y      	 � ��
 5Z      �� 5[   "   .R�5\   &   � |�.  $��Center=�6=    5]   *   �� 5^   .   $
��Auto �6?   �� 5_   .   $��* �6?   �� 5`   .   $
��Auto �6?   .+�5c   *   �� $��0��60   |�.  $��16��5e   0   $��16��6;   $��3,0,3,0q�6G   ��5f   0   �� *TabImageSource+�� 5g   *   $ ContentSite��5j   -   |�.  $��1��5i   -   $��Center=�5k   -   $��Center=�5l   -   $��Header��5m   -   $��0,3,0,3q�5n   -   ��5o   -   �� *	TabName+�� 5q   *   $ CloseButton��61   $��2��5r   1   $��Center=�5s   1   $��Center=�5t   1   $��5,0,5,0q�5u   1   #��[ 5v   1   $y�Visible=�5w   1   ��5x   1   �� *.,DataContext.TabItemViewModel.TabItemCloseCmd+ �� " ����5y   1   �� 5z   '   5{   #   5      �� 5�      	 � ��
 64   �� *UserControl+ 5�      P� 5�       �@��
IsSelected . . �5�   "   �� 	 � ��$
 0q�6;   6G   �� 5�   "   	 � ��#
 [ 6?   �� 5�   "   	 � |�
 . 6>   �� 5�   "   	 �@K�ZIndex$	
 100��6A   �� 5�   "   	 � ��$
 0q�6D   5�      5�      �� 5�      " R�6   .�5�      �� 	 � ��$

 Auto=�6U   �� 5�      	 � ��$

 Auto=�6W   �� 5�      	 � ��$
 Stretch=�6@   �� 5�      	 � ��$
 Stretch=�6>   �� 5�      	 � ��$
 1,0,2,0q�63   �� 5�      	 � ��$
 2q�64   5�      �� 5�      " Q�6   .�5�      �� 	 � ��$
 0,2,5,2q�63   �� 5�      	 � ��$
 2,2,2,2q�64   �� 5�      	 � ��$
 Center=�6E   �� 5�      	 �@Q�
IsSelected
 67   �� *
IsSelected+5�      �� 5�       DeviceTrackTreeViewItemStyle  " Q�6A   # [BQ�5�      .�5�      �� 	 �@�Tag$)
 #AbisMonitor.UI.Controls.DeviceTrack��60   5�      5�      %   '��       'R�       'Q�       5�       J� InputBindings 5�      �� 5�   
    �� Gesture$ Ctrl+U��6;    �� Command 6   �� *ModifyUserInfoCmd+6;   �� 5�   
   $ Ctrl+P��66    6   �� *ModifyPwdCmd+66   �� 5�   
   $ Ctrl+L��63    6   �� *	LogoutCmd+63   �� 5�   
   $ Alt+F4��61    6   �� *	ExitCmd+61   .��5�      �  5�   
   �� 5�      $
��auto �6   �� 5�      $��* �6   �� 5�      $
��auto �6   .+�5�   
   o� $��	0,0,0.4,0q�6   $��0��6"   $��5,3,0,3q�6/   .@�5�      m� $��系统功能��6   .@�5�      m� $��注销��6    m� InputGestureText$ Ctrl+L��6'   �� 5�      m� 5�      $��退出系统��6   $ Alt+F4��6)   5�      m� 5�      $��账户设置��6   .@�5�      m� $��修改密码��6   $ Ctrl+P��6)   m� 5�      $��修改个人资料��6   $ Ctrl+U��6+   5�      m� 5�      $��关于��6    m� Command 6#   �� *ShowAboutDialogCmd+5�      � 5�   
   $��1��6    5�      �� 5�      $	��260 �6#   �� 5�      $
��Auto �6#   �� 5�      $��* �6#   .+�5�      R� -   5        $��tvNavigation��5�   '   $��0��6   #��[ 5�      .@�5�      Q�  Q� 
IsExpanded . ��5�      �� 5�        �� Orientation$  
Horizontal=�6%   $��Center=�6>   .+�5�      �� $��16��6$   $��16��6/   $-��'..\Resources\Images\AbisRes\Monitor.png��6;   �� 5�      $��Abis接口监视��6(   $��5,0,0,0q�68   5�      .@�5�      Q� 
! � Tag$(! "AbisMonitor.UI.Controls.DeviceInfo��6#   ��5�      �� 5�      $  
Horizontal=�6)   $��Center=�6B   .+�5�   "   �� $��16��6(   $��16��63   $0��*..\Resources\Images\AbisRes\DeviceInfo.png��6?   �� 5�   "   $��设备配置��6,   $��5,0,0,0q�68   5�      5�      Q� 5�      $,! &AbisMonitor.UI.Controls.AllSignalQuery��6#   ��5�      �� 5�      $  
Horizontal=�6)   $��Center=�6B   .+�5�   "   �� $��16��6(   $��16��63   $1��+..\Resources\Images\AbisRes\Information.png��6?   �� 5�   "   $��全部信令��6,   $��5,0,0,0q�68   5�      5�      Q� 5�      ��5�      �� 5�      $  
Horizontal=�6)   $��Center=�6B   .+�5�   "   �� $��16��6(   $��16��63   $2��,..\Resources\Images\AbisRes\Communicator.png��6?   �� 5�   "   $��用户跟踪��6,   $��5,0,0,0q�68   5�      .@�5�      Q� $'! !AbisMonitor.UI.Controls.IMSITrack��6'   ��5�      �� 5�   "   $  
Horizontal=�6-   $��Center=�6F   .+�5�   &   �� $��16��6,   $��16��67   $(��"..\Resources\Images\AbisRes\NO.png��6C   �� 5�   &   $��主叫IMSI号码��60   $��5,0,0,0q�6@   5�   #   ��5�      �� 5�   "   5�      5�      Q� 5�      ��6#   �� " �� Path$0" *DeviceNavigationViewModel.DeviceDic.Values���5�      �� 5�      $  
Horizontal=�6)   $��Center=�6B   .+�5�   "   �� $��16��6(   $��16��63   $1��+..\Resources\Images\AbisRes\DeviceTrack.png��6?   �� 5   "   $��设备跟踪��6,   $��5,0,0,0q�68   5     ��5     �� 5     # �� ItemsSource# 67   �� *PortDic.Values+.R�5  "   �� $  
Horizontal=�6-   $��Center=�6F   .+�5  &   �� $��16��6,   $��16��67   $1��+..\Resources\Images\AbisRes\DeviceTrack.png��6C   �� 5  &   $��5,0,0,0q�6Q   ��60   �� $" 
DeviceName�6Q   5  #   $ �� ItemTemplate$ 5	  "   �� 5
  &      DeviceTrackTreeViewItemStyle% �� ItemContainerStyle#% [ 5  ?   # 5
  ?   �� *CellDic.Values+.R�5  *   �� $  
Horizontal=�65   $��Center=�6N   .+�5  .   �� $��16��64   $��16��6?   $5��/..\Resources\Images\AbisRes\CollectionPoint.png��5  4   �� 5  .   $��5,0,0,0q�6h   ��68   �� $" PortNum�& �� StringFormat$& 环{0}��6h   5  +   $ 5  *   �� 5  .   .R�5  2   �� $  
Horizontal=�6=   $��Center=�6V   ! 5  =   �� *AbisDeviceSimple+.+�5  6   �� $��16��6<   $��16��6G   $5��/..\Resources\Images\AbisRes\CollectionPoint.png��5  <   �� 5  6   $��5,0,0,0q�6@   ��5   :   `� 5!  >   $& {0}小区 (时隙{1})��6K   .1�5"  B   �� $" BtsName�6J   �� 5#  B   $" SlotNum�6J   5$  ?   5&  7   5'  3   5(  /   5*  '   5,     5.     50     51     �� 54     $��1��6   $��Center=�6+   $��Stretch=�6H   $��2��6d   $��1,0,1,0q�55     
������ ' �� ResizeBehavior$' PreviousAndNext=�56     ( �� ResizeDirection$( Columns=�6<   �� 58     -   5        $��
tabControl��59     $��2��58     .   (AbisMonitor.UI.Utils.TabControlExtension)   SelectItemOnRightClick) .   TabItemStyle#��[ 5=       TabControlContentTemplate#��[ 5>     ��5:     �� *TabItemViewModel.TabItems+* �� SelectedItem* 5;     �� * TabItemViewModel.SelectedModel++ �� Mode$+ TwoWay=�, � ContextMenu, 5@     �� 5A     $��	0,0,0.4,0q�6"   .@�5B     m� $��关闭��6#    5C  #   �� **(DataContext.TabItemViewModel.CloseTabCmd+ �� *FindAncestor+" ��- m� CommandParameter- 5D  #   �� $" PlacementTarget.Parent� �� . �� Mode$. FindAncestor=�" ��m� 5F     $��关闭所有页面��6#    5G  #   �� *-+DataContext.TabItemViewModel.CloseAllTabCmd+ �� *FindAncestor+" ��- 5H  #   �� $" PlacementTarget.Parent� �� $. FindAncestor=�" ��m� 5J     $��除此之外全部关闭��6#    5K  #   �� */-DataContext.TabItemViewModel.CloseOtherTabCmd+ �� " ��- 5L  #   �� $" PlacementTarget.Parent� �� $. FindAncestor=�" ��5M     5O     5Q     � 5S  
   $��2��6    5T     �� 5U     $
��Auto �6#   �� 5V     $
��Auto �6#   �� 5W     $��* �6#   .+�5Y     �� $��0��6   $
��Left=�6(   $��0,0,30,0q�6C   .@�5Z     �� $��16��6   $��16��6#   $!��../Resources/Images/red.png��6/    5[     / �� Binding0 �� Value��%�   '��       �� 5\     " ��6     5]     �� 5^  "   / 6.   �� *IsConnected+0 True6N   .a�5_  &   �� 	 � ��$"
 ../Resources/Images/blue.png��6E   5`  #   5b     5c     5d     �� 5e     ��6    �� *IsConnected+  ConnectedStateConverter1 �� 	Converter#1 [ 5f     �� 5g     $��1��6   $
��Left=�6(   $��5,0,30,0q�6C   .@�5h     �� $��5,0,5,0q�6   �� 5i     2 �� ContentStringFormat$!2 当前跟踪用户数：{0}��6?   ��6    �� *
TrackCount+6?   5j     �� 5k     $��2��6   $��0,0,5,0q�6(   $��Right=�6:   .@�5l     �� ��6    �� *LoginedUser+�� 5m     $��5,0,5,0q�6   �� 5n     $2 版本号：{0}��6<   ��6    �� *	Version+6<   5o     5p     5q     5s     