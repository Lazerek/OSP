   M S B A M L   `   `   `  ���� >  :OSP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:OSP.ViewOSP.View  -clr-namespace:OSP.BehaviourOSP.Behaviour  %clr-namespace:OSP.Model	OSP.Model      OSP.View.ActionMenuView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:OSP.View     1	behaviourclr-namespace:OSP.Behaviour     )modelclr-namespace:OSP.Model     $	��400��$	��600��  � Title  �� *WindowTitle+ � DataContext �� *ActionMenuViewModel+   Locator �� Source# [  .���  � RowDefinitions �� $��* ��� $��40 ��� $��40 �.+�� $��0��.+��� -   $��FirefightersList��$��10q�$ �� IsSynchronizedWithCurrentItem$
 TrueY�   OSP.Model.GridViewSort   AutoSort . ���� *	Actions+ �� SelectedItem �� *SelectedAction+ �� View �� .P��� $��Id��   PropertyName$ Id��	 �� DisplayMemberBinding	 �� *Id+�� $��Meldunek��$ NumberOfAction��	 �� *NumberOfAction+�� $
��Data��$ DateOfAction��	 �� *DateOfAction+�� $��Czas trwania��$ MinutesOfAction��	 �� *MinutesOfAction+�� $��Miejsce��$ PlaceOfAction��	 �� *PlaceOfAction+�� $��	Liczba km��$ 
NumberOfKM��	 �� *
NumberOfKM+�� $��TypAkcji��$
 Type��	 �� *ActionType.Type+� $��1��.+��� 
 �� Orientation$
 
Horizontal=�$��Center=�.+��� $��	10,5,10,5q����� *AddActionCommand+y��� *
Visibility+.��Dodaj akcje�� $��	10,5,10,5q����� *EditActionCommand+y��� *
Visibility+.��Edytuj akcje�� $��	10,5,10,5q����� *RemoveActionCommand+y��� *
Visibility+.��Usuń akcje� $��2��.+��� $
 
Horizontal=�$��Center=�.+��� $��	10,5,10,5q�)   #OSP.Behaviour.CloseOnClickBehaviour   	IsEnabled . .��	Powrót