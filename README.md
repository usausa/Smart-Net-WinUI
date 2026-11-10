# Smart-Net-WinUI

## Notes

* For TwoWay bindings with `ParameterEqualsConverter`, `BoolTo*Converter`, `ColorToBrushConverter`, `FormatConverter` or `ObjectConvertConverter`, use `{Binding}` instead of `x:Bind`.
* Bind `SelectedItems` of `ListBoxSelectedItemsBehavior`, `SelectedItem` of `ListBoxSingleSelectionBehavior` and `Value` of `MaskedTextBehavior` with `Mode=TwoWay`.
* In Native AOT apps, elements created by templates are wrapped as a base type unless the app uses their type, so type-based searches such as `FindDescendant<T>` and `FindParent<T>` can miss them.
* To start the app with `AddWinUI<TApp>()`, define `DISABLE_XAML_GENERATED_MAIN` in the app project and write `Main` that builds and runs the host.
