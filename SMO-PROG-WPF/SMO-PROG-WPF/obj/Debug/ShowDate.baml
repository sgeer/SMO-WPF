   M S B A M L   `   `   `  ����  SMO-PROG-WPF    SMO_PROG_WPF.ShowDate   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$#  Просмотр данных��5   	   $	��442��6!   $	��635��6.    � WindowStartupLocation$ CenterScreen=�5   	    � Icon$ Resources/file-icon.ico��6E   .��5      � $	��635��6   $	��444��6   
�������.+�5   
   �� -   5         ��Name$ Button_Load��5      $
��Left=�6'   $��
486,20,0,0q�6B   $	��Top=�6V   $	��124��6n   $��68��6z   
����������	#FFE2A013 � UseLayoutRounding . $��/>��6�   ��5      �� 5	      .R�5
      �� $��Resources/load.png��6   $��0, 0,0,0q�68   $��48��6J   $��48��6U   5       � ToolTip 5      _� 5      
�������$#��Показать данные��61   5      �� 5   
   -   5        $ 	Button_OK��5      $
��Left=�6%   $��486,317,0,0q�6@   $	��Top=�6U   $	��124��6m   $��68��6y   
����������	#FFE2A013 . $��/>��6�   ��5      �� 5      .R�5      �� $��Resources/ok.png��6   $��0, 0,0,0q�66   $��48��6H   $��48��6S   5       5      _� 5      
�������$��OK��61   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$
 Grid��5      $
��Left=�6!   $��	10,10,0,0q�6<   $	��Top=�6O   $	��372��6g   $	��401��6t   
�������   AutoGenerateColumns .    
IsReadOnly . 6�   5      5      