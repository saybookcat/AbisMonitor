   M S B A M L   `   `   `  ����R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType �� Property �� Value   EditButtonTemplateStyle ��Name �� Triggers P� Property P� Value �� 
TargetName)   #System.Windows.Controls.DataGridRow �� Triggers  DataGridRowCdrResultStyle	 �� BasedOn
 �� Binding �� Value  IsSelectedRowSytle  BlueLightGradientBrush �� 
StartPoint �� EndPoint � Offset � Color  DataGridContentCenterSytle  RawSignalDataContenSytle&   System.Windows.Controls.DataGrid  DataGridStyle=  7System.Windows.Controls.Primitives.DataGridColumnHeader  DataGridColumnHeaderStyle  DataContentHeaderStyle*  $System.Windows.Controls.DataGridCell 	 RawSignalHeaderStyle�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml      5      %�  '��       '�� �     &	  k    '   Z    &	 %    7  &	 %    7  &	 �    &	 �    &	 &    &	 �    7  &	 �    &	 8    7  '  �    &		 n    7  �� 5      "  ��6   .�5   
   ��  � ��$ Center=�60   �� 5   
    � ��$ Center=�67   �� 5   
    �@d�	IsChecked$ FalseY�6&   �� 5   
    � ��$ 0,0,0,0q�6$   �� 5   
    � ��$ 0,0,0,0q�6#   5	      �� 5      "  ��6   .�5   
   ��  � ��$ Center=�60   �� 5   
    � ��$ Center=�6.   5      �� 5      "  ��6,   .�5   
   ��  � �� 5      �� 5      .R�5      �� ._�5      �� $	 img��6    $<��6pack://application:,,,/Resources/Images/Edit_16x16.png��6-   5       5      P� 5       � �� . . �5      �� $	 img��6%    � ��$= 7pack://application:,,,/Resources/Images/Edit2_16x16.png��5   %   5      P� 5       � {� .  . �5      �� $	 img��6%    �@J�Opacity$	 0.6Q�6I   6W   5       5"      5$      5%      �� 5'      "    6   .�5(   
   ��  � ��$ 21��6#   �� 5)   
    � ��$ 12(�6%   62   �� 5+   
    � ��$ 0q�6,   68   �� 5,   
    � ��$ Center=�65   6F   �� 5-   
    �@�FocusVisualStyle 6-   Z� 6@    5.   
   P� 5/       �@  
IsSelected . . �52      ��  � ��$
 Bold"�6/   6>   53      P� 54       �@J�IsMouseOver . . �55      ��  � �� �#BFDCF06A   56      58      �� 5;      "    6.   8	    5<   
   �� 5>      
 6   �� *Result+ -165   .a�5?      ��  � �� �#ff90006/   5@      �� 5B      
 6   �� *Result+ 365   .a�5C      ��  � ��
 �  ��6/   5D      5F      �� 5H      "    6'   8	    5I   
   �� 5J      
 6   �� *
IsSelect+ True67   .a�5K      ��  � �� �#ff90006A   �� 5L       � ��$ Italic$�6.   6?   5M      5O      �� 5S      $	 0,0&�69   $	 0,1&�6J   .��5T   
   � $ 0Q�6   $ White��6"   � 5U   
   $ 0.654Q�6   $ White��6&   � 5V   
   $ 1Q�6   $ White��6"   5W      �� 5Y      "  ��6/   .�5Z   
   ��  � ��$ Center=�60   �� 5[   
    � ��$ Center=�6.   5\      �� 5^      "  ��6-   .�5_   
   ��  � ��$
 Left=�60   6?   �� 5`   
    � ��$ Center=�6.   6?   �� 5a   
    � ��$ 5,0,10,0q�6$   67   5b      �� 5e      "   6   .�5f   
   ��  �@ SelectionMode$ Single=�6*   6;   �� 5g   
    �@ SelectionUnit$ FullRow=�6*   �� 5h   
    �@ ClipboardCopyMode$ ExcludeHeader=�6.   6F   �� 5j   
    � ��$ Center=�67   6H   �� 5k   
    � ��$ Center=�65   6F   �� 5l   
     �@ HorizontalGridLinesBrush �#B0CEDD6G   �� 5m   
    �@ VerticalGridLinesBrush �#B0CEDD6E   �� 5n   
     �@ AlternatingRowBackground �#F2F4F76G   �� 5o   
    � �� �	#FFA6CCF26(   �� 5p   
    �@ RowBackground
 �����6*   �� 5r   
    �@ AutoGenerateColumns .  6@   �� 5s   
    �@ 
IsReadOnly . 66   �� 5v   
    �@��AlternationCount$ 2��6-   69   �� 5w   
    � ��8   6'   �� 5y   
   % �@ HorizontalScrollBarVisibility$
 Auto=�6:   6I   �� 5z   
   # �@ VerticalScrollBarVisibility$
 Auto=�68   6G   �� 5|   
    �@ RowHeaderWidth$ 0Q�6+   67   5}      �� 5      "   6   .�5�   
   ��  �@J�SnapsToDevicePixels . 6?   �� 5�   
    � ��$ 28��6&   �� 5�   
    � �� �#4545456'   �� 5�   
    � ��$
 Bold"�6'   66   �� 5�   
    � ��$ 13(�6%   �� 5�   
    � ��$ Microsoft YaHei)�6'   6A   �� 5�   
    � ��$ Center=�67   6H   �� 5�   
    � ��$ Center=�65   6F   5�      �� 5�      "   6   8	   .�5�   
   ��  � ��$
 Left=�67   6F   5�      �� 5�      "   6    5�   
   P� 5�       �@ 
IsSelected . . �5�      ��  � �� �#B1D6F06A   �� 5�       � �� �#B1D6F06B   �� 5�       � �� �	#FF3031316C   5�      5�      �� 5�      "   6)   8	   .�5�   
   ��  � ��$
 Left=�67   6F   �� 5�   
    � ��$ 5,0,0,0q�6$   66   5�      5�      