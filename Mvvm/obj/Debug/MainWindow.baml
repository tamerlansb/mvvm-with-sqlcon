   M S B A M L   `   `   `  ����?  ;Mvvm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:MvvmMvvm  5   	       Mvvm.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   !vmclr-namespace:Mvvm     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��700��6)    � 
ResizeMode$ NoResize=�65   .��5
      �  � ColumnDefinitions 5   
   �� 5      $
��auto �6   $	��321��6,   �� 5      .+�5   
   �� $��0��6   $��Center=�6D   $	��150��6a   $��
10,30,11,0q�6n   $	��Top=�6�   $	��300��6�    J� RenderTransformOrigin$ 0.788,0.454&�6�    � DataContext 6%   �� *People+.+�5      �� -   5         ��Name$ LastNameBlock��5      $
��Wrap=�6/   $��	LastName:��6C   $
��auto��6T   $��Right=�6b   $	��300��6~   �� 5      -   5         ��Name$ LastNameBox��5       �� TextWrapping$
 Wrap=�6+   $��23��6?   ��6K   �� *
LastName+ �� Mode$ TwoWay=�	 �� UpdateSourceTrigger$	 PropertyChanged=�
 �� NotifyOnValidationError
 .  �� ValidatesOnDataErrors . �� 5      -   5        $ DateOfBirthBlock��5      $
��Wrap=�62   $��Date of birth:��6F   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ DateOfBirth��5         SelectedDate 6.   �� *DateOfBirth+$ TwoWay=�$	 PropertyChanged=�
 .  . �� 5      -   5        $ HeightBlock��5      $
��Wrap=�6-   $��Height:��6A   $
��auto��6P   �� 5      -   5        $ 	HeightBox��5      $
 Wrap=�6)   $��23��6=   ��6J   �� *Height+$ TwoWay=�$	 PropertyChanged=�
 .  . 5      �� 5   
   -   5         ��Name$ listView��5      $��1��6%   $��Right=�6T   $	��323��6p   $	��Top=�6}   $	��300��6�   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6�   $��	0,18,52,0q�6�   ��65   �� *People+ �� SelectedItem 6�   �� *CurrentPerson+ �� View 5      �� 5      .P�5      �� $��LastName��6%    �� Width$	 100��67    �� DisplayMemberBinding 5   %   ��  �� Path$ LastName��� 5      $��Date of birth��6%   $	 100��6<    5   %   �� $ 	ShortDate��� 5      $��Height��6%    5   %   �� $ Height�5       5"      �� 5#   
   -   5         ��Name$ Delete��5#      $��0��6!   $��Delete��61   $
��Left=�6B   $��
10,144,0,0q�6]   $	��Top=�6q   $��76��6�   $��22��6�   ��6�   �� *DeleteCommand+9� 5$   
   -	   5         9�Name$ expander��5$      $��0��6%   $��Adding new person��65   $
��Left=�6P   $��
10,180,0,0q�6k   $	��Top=�6   $	��232��6�   $	��261��6�    9� 
IsExpanded . .��5%      �� $��1q�6   ._�5&      �� $��Stretch=�6   $	��Top=�6;   $
��auto��6S   $	��220��6b   $��	20,0,19,0q�6n   .+�5'      �� -
   5        $ 	textBlock��5'       $
��Wrap=�63   $��	LastName:��6G   �� 5(      -   5        $ LastNameAdd��5(      $��23��63   $
 Wrap=�6?   ��6S   �� $ NewPerson.LastName�$ TwoWay=� �� NotifyOnSourceUpdated . 
 . $	 PropertyChanged=� . �� 5)      -   5        $ 
textBlock1��5)       $
��Wrap=�64   $��Date of birth:��6H     5*      -   5        $��DateOfBirthAdd��5*   !    5+      �� 5,      $ NewPerson.DateOfBirth�6&   $ TwoWay=�6C   
 .  . $	 PropertyChanged=�6�   6�   5.      �� 5/      -   5        $ 
textBlock2��5/       $
��Wrap=�64   $��Height:��6H   �� 50      -   5        $ 	HeightAdd��50      $��23��61   $
 Wrap=�6=   ��6Q   �� $ NewPerson.Height�$ TwoWay=� . 
 . $	 PropertyChanged=� . �� 51      -   5        $	��Add��51      $��
Add To End��6(   $
��Left=�6=   $��Bottom=�6Y   ��6t   �� *
AddCommand+�� 52      -   5        $ InsertIndexblock��52       $
��Wrap=�6:   $��Index:��6N   �� 53      -   5        $ HeightAddByIndex��53      $��23��68   $
 Wrap=�6D   ��6X   �� $ IndexForAdd�$ TwoWay=� . 
 . $	 PropertyChanged=� . �� 54      -   5        $��
AddByIndex��54      $��Add By Index��6/   $
��Left=�6F   $��Bottom=�6b   ��6~   �� *AddCommandByIndex+55      56      57      �� 58   
   -   5        $ 
SortButton��58      $��Sort By LastName��6%   $��1��6@   $
��Left=�6P   $��
21,356,0,0q�6k   $	��Top=�6   ��6�   �� *SortByLastName+�� 59   
   -   5        $ Clearbutton��59      $��	ClearList��6&   $��1��6:   $
��Left=�6J   $��200,356,0,0q�6e   $	��Top=�6z   $��75��6�   ��6�   �� *ClearListCom+o� 5:   
   -   5         o�Name$
 menu��5:      $
��Left=�6   $��20��68   $��0,1,0,0q�6D   $	��Top=�6U   $	��694��6m   $��2��6y   .@�5;      m� $��Save and Download��6   $
��auto��63   $
��auto��6A   .@�5<      m� $
��Save��6    m� Command 6)   �� *SaveToDateBase+6P   m� 5=      $��Download From DateBase��6    6;   �� *DownloadFromDateBase+6h   5>      5?      5@      5A      