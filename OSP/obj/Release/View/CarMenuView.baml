   M S B A M L   `   `   `  ���� >  :OSP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:OSP.ViewOSP.View  -clr-namespace:OSP.BehaviourOSP.Behaviour  %clr-namespace:OSP.Model	OSP.Model      OSP.View.CarMenuView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:OSP.View     )bclr-namespace:OSP.Behaviour     )modelclr-namespace:OSP.Model     $	��400��$	��600��  � Title  �� *WindowTitle+ � DataContext �� *CarMenuViewModel+   Locator �� Source# [  .���  � RowDefinitions �� $��* ��� $��40 ��� $��40 �.+�� $��0��.+��� -   $��CarsList��$��10q�$ �� IsSynchronizedWithCurrentItem$
 TrueY�   OSP.Model.GridViewSort   AutoSort . ���� *Cars+ �� SelectedItem �� *SelectedCar+ �� View �� .P��� $��Id��   PropertyName$ Id��	 �� DisplayMemberBinding	 �� *Id+�� $��Rodzaj Wozu��$ CarType��	 �� *	CarType+�� $��
Numer Wozu��$ 	CarNumber��	 �� *	CarNumber+�� $��Pojemność wody��$ WaterVolume��	 �� *WaterVolume+� $��1��.+��� 
 �� Orientation$
 
Horizontal=�$��Center=�.+��� $��	10,5,10,5q����� *AddCarCommand+y��� *
Visibility+.��
Dodaj Wóz�� $��	10,5,10,5q����� *EditCarCommand+y��� *
Visibility+.��Edytuj Wóz�� $��	10,5,10,5q����� *RemoveCarCommand+y��� *
Visibility+.��
Usuń Wóz� $��2��.+��� $
 
Horizontal=�$��Center=�.+��� $��	10,5,10,5q�)   #OSP.Behaviour.CloseOnClickBehaviour   	IsEnabled . .��	Powrót