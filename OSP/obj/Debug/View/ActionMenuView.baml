   M S B A M L   `   `   `  ����>  :OSP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:OSP.ViewOSP.View  5   	   -clr-namespace:OSP.BehaviourOSP.Behaviour  5   	   %clr-namespace:OSP.Model	OSP.Model  5   	       OSP.View.ActionMenuView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:OSP.View     5   	   1	behaviourclr-namespace:OSP.Behaviour     5   	   )modelclr-namespace:OSP.Model     5   	   $	��400��5	   '   $	��600��64     � Title  6	   �� *WindowTitle+ � DataContext 5
   	   �� *ActionMenuViewModel+   Locator �� Source# [  .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $��40 �6   6*   �� 5      $��40 �6   6*   .+�5   
   � $��0��6   .+�5      �� -   5        $��FirefightersList��5   #   $��10q�6   $ �� IsSynchronizedWithCurrentItem$
 TrueY�5         OSP.Model.GridViewSort   AutoSort . ��5   ;   �� *	Actions+ �� SelectedItem 6[   �� *SelectedAction+ �� View 5      �� 5      .P�5      �� $��Id��6)      PropertyName$ Id��6Y   	 �� DisplayMemberBinding	 65   �� *Id+6�   �� 5      $��Meldunek��6)   $ NumberOfAction��6k   	 6;   �� *NumberOfAction+6�   �� 5      $
��Data��6)   $ DateOfAction��6e   	 67   �� *DateOfAction+6�   �� 5      $��Czas trwania��6)   $ MinutesOfAction��6p   	 6?   �� *MinutesOfAction+6�   �� 5      $��Miejsce��6)   $ PlaceOfAction��6i   	 6:   �� *PlaceOfAction+6�   �� 5      $��	Liczba km��6)   $ 
NumberOfKM��6h   	 6<   �� *
NumberOfKM+6�   �� 5      $��TypAkcji��6)   $
 Type��6l   	 6;   �� *ActionType.Type+6�   5      5       5!      � 5"   
   $��1��6   .+�5#      �� 
 �� Orientation$
 
Horizontal=�6   $��Center=�62   .+�5$      �� $��	10,5,10,5q�6   ��6,   �� *AddActionCommand+y�6Q   �� *
Visibility+.��6s   Dodaj akcje6�   �� 5%      $��	10,5,10,5q�6   ��6,   �� *EditActionCommand+y�6R   �� *
Visibility+.��6t   Edytuj akcje6�   �� 5&      $��	10,5,10,5q�6   ��6,   �� *RemoveActionCommand+y�6T   �� *
Visibility+.��6v   Usuń akcje6�   5(      5)      � 5*   
   $��2��6   .+�5+      �� $
 
Horizontal=�6   $��Center=�62   .+�5,      �� $��	10,5,10,5q�6   )   #OSP.Behaviour.CloseOnClickBehaviour   	IsEnabled . .��6]   	Powrót6e   5-      5.      50      51      