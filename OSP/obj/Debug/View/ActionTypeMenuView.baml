   M S B A M L   `   `   `  ����>  :OSP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:OSP.ViewOSP.View  5   	   -clr-namespace:OSP.BehaviourOSP.Behaviour  5   	   %clr-namespace:OSP.Model	OSP.Model  5   	   !    OSP.View.ActionTypeMenuView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:OSP.View     5   	   1	behaviourclr-namespace:OSP.Behaviour     5   	   )modelclr-namespace:OSP.Model     5   	   $	��400��5
   '   $	��600��64     � Title  6	   �� *WindowTitle+ � DataContext 5   	   �� *ActionTypeMenuViewModel+   Locator �� Source# [  .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $��40 �6   6*   �� 5      $��40 �6   6*   .+�5   
   � $��0��6   .+�5      �� -   5        $��CarsList��5   #   $��10q�6   $ �� IsSynchronizedWithCurrentItem$
 TrueY�5         OSP.Model.GridViewSort   AutoSort . ��5   3   �� *ActionTypes+ �� SelectedItem 6W   �� *SelectedActionType+ �� View 5      �� 5      .P�5      �� $��Id��6)      PropertyName$ Id��6Y   	 �� DisplayMemberBinding	 65   �� *Id+6�   �� 5      $��	Typ akcji��6)   $
 Type��6b   	 6<   �� *Type+6�   5      5      5      � 5   
   $��1��6   .+�5      �� 
 �� Orientation$
 
Horizontal=�6   $��Center=�62   .+�5       �� $��	10,5,10,5q�6   ��6,   �� *AddActionTypeCommand+y�6U   �� *
Visibility+.��6w   Dodaj Typ Akcji6�   �� 5!      $��	10,5,10,5q�6   ��6,   �� *EditActionTypeCommand+y�6V   �� *
Visibility+.��6x   Edytuj Typ Akcji6�   �� 5"      $��	10,5,10,5q�6   ��6,   �� *RemoveActionTypeCommand+y�6X   �� *
Visibility+.��6z   Usuń Typ Akcji6�   5$      5%      � 5&   
   $��2��6   .+�5'      �� $
 
Horizontal=�6   $��Center=�62   .+�5(      �� $��	10,5,10,5q�6   )   #OSP.Behaviour.CloseOnClickBehaviour   	IsEnabled . .��6^   	Powrót6f   5)      5*      5+      5,      