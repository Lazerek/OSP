   M S B A M L   `   `   `  ����>  :OSP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:OSP.BehaviourOSP.Behaviour  5   	   #clr-namespace:OSP.ViewOSP.View  5   	   clr-namespace:OSPOSP  5   	   %clr-namespace:OSP.Model	OSP.Model  5	   	   "    OSP.View.FirefighterMenuView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )bclr-namespace:OSP.Behaviour     5   	   (localclr-namespace:OSP.View     5   	   !ospclr-namespace:OSP     5   	   )modelclr-namespace:OSP.Model     5	   	   $	��400��5   '   $	��600��64     � Title  6	   �� *WindowTitle+ � DataContext 5   	   �� *FirefighterMenuViewModel+   Locator �� Source# [  .��5      �  � RowDefinitions 5      �� 5   
   $��* �6   6%   �� 5   
   $��40 �6   6&   �� 5      $��40 �6   6'   .+�5   
   � $��0��6   .+�5      �� -   5        $��FirefightersList��5   #   $��10q�6   $ �� IsSynchronizedWithCurrentItem$
 TrueY�5         OSP.Model.GridViewSort   AutoSort . ��5   ;   �� *Firefighters+ �� SelectedItem 6`   �� *SelectedFirefighter+ �� View 5      �� 5      .P�5      �� $��Id��6%      PropertyName$ Id��6U   	 �� DisplayMemberBinding	 61   �� *Id+6|   �� 5      $��Nazwisko��6%   $ LastName��6a   	 67   �� *
LastName+6�   �� 5      $
��Imie��6%   $ 	FirstName��6^   	 63   �� *	FirstName+6�   �� 5      $��Pesel��6%   $ Pesel��6[   	 64   �� *Pesel+6�   �� 5      $��Data urodzenia��6%   $ 	BirthDate��6h   	 6=   �� *	BirthDate+6�   �� 5      $��Data dołączenia��6%   $ JoinDate��6h   	 6>   �� *
JoinDate+6�   �� 5      $��Stopień��6%   $ Degree��6^   	 66   �� *Degree+6�   �� 5       $��Telefon��6%   $ Phone��6]   	 66   �� *Phone+6�   5!      5#      5$      � 5%   
   $��1��6   .+�5&      �� 
 �� Orientation$
 
Horizontal=�6   $��Center=�62   .+�5'      �� $��	10,5,10,5q�6   ��6,   �� *AddFirefighterCommand+y�6V   �� *
Visibility+.��6x   Dodaj strażaka6�   �� 5(      $��	10,5,10,5q�6   ��6,   �� *EditFirefighterCommand+y�6W   �� *
Visibility+.��6y   Edytuj strażaka6�   �� 5)      $��	10,5,10,5q�6   ��6,   �� *RemoveFirefigterCommand+y�6X   �� *
Visibility+.��6z   Usuń strażaka6�   5+      5,      � 5-   
   $��2��6   .+�5.      �� $
 
Horizontal=�6   $��Center=�62   .+�5/      �� $��	10,5,10,5q�6   )   #OSP.Behaviour.CloseOnClickBehaviour   	IsEnabled . ��6,   �� *CancelCommand+.��6w   	Powrót6   50      51      52      53      