   M S B A M L   `   `   `  ����E  AMacScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:MacScanner
MacScanner  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089gAclr-namespace:System.Runtime.Remoting.Messaging;assembly=mscorlib!System.Runtime.Remoting.Messaging 5   	       MacScanner.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:MacScanner     5   	   W	messagingAclr-namespace:System.Runtime.Remoting.Messaging;assembly=mscorlib     5   	     � Title$  局域网IP管理��5	   	   $	��500��6   $	��800��6&   .��5
      � .+�5   
   �� -   5         ��Name$ 
settingBtn��5      $��设置��6%   $
��Left=�62   $��
696,11,0,0q�6M   $	��Top=�6a   $��75��6y   �� 5   
   -   5         ��Name$ textBlockIP��5      $
��Left=�6)   $��	10,10,0,0q�6D   $
��Wrap=�6W   $��当前主机IP地址：��6k   $	��Top=�6|   $	��279��6�   �� 5   
   -   5        $ 
textBlock1��5      $
��Left=�6(   $��	10,47,0,0q�6C   $
��Wrap=�6V   $��白名单：��6j   $	��Top=�6v   �� 5   
   -   5        $ 
textBlock2��5      $
��Left=�6(   $��
448,47,0,0q�6C   $
��Wrap=�6W   $��未知主机：��6k   $	��Top=�6x   �� 5   
   -   5        $ textBlockLAN��5      $
��Left=�6*   $��	10,25,0,0q�6E   $
��Wrap=�6X   $��当前局域网网段：��6l   $	��Top=�6|   $	��315��6�   �� 5   
   -   5        $ textBlockNum��5      $
��Left=�6*   $��
63,428,0,0q�6E   $
��Wrap=�6Y   $��当前局域网共有��6m   $	��Top=�6|   $	��351��6�   &   System.Windows.Controls.DataGrid  5   
   -   5        $��	gridWhite��5      $
��Left=�6$   $	��359��6?   $��	63,50,0,0q�6L   $	��Top=�6_   $	��351��6w      
IsReadOnly .     Columns 5      5"        5#   
   $
��Left=�6   $	��319��6.   $��
514,50,0,0q�6;   $	��Top=�6O   $	��257��6g      AutoGenerateColumns .   5$      4  .System.Windows.Controls.DataGridCheckBoxColumn  5%      ,  &System.Windows.Controls.DataGridColumn   Header$ 复选��6%   63   0  *System.Windows.Controls.DataGridTextColumn  5'      $ IP地址��6!      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$
 auto 6\   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6/   �� *ipName+6\     5(      $ 	Mac地址��6!    . $
 auto 6^   	 60   �� *	macName+6^   5*      �� 5+   
   -   5        $��btnAddWhite��5+      $��加入白名单��6$   $
��Left=�64   $��696,387,0,0q�6O   $	��Top=�6d   $��75��6|   �� 5,   
   -	   5        $��
btnRefersh��5,   Z   $��刷新��6   $��23��6   $
��Left=�6*   $��606,386,0,0q�6E   $	��Top=�6l   $��75��6�   �� 5-   
   -
   5        $��
btnAddIntr��5-   \   $��添加备注��6   $��23��6    $
��Left=�6,   $��514,387,0,0q�6G   $	��Top=�6n   $��75��6�   5.      5/      