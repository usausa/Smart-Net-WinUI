# Smart-Net-WinUI

## Notes

* For TwoWay bindings with `ParameterEqualsConverter`, `BoolTo*Converter`, `ColorToBrushConverter`, `FormatConverter` or `ObjectConvertConverter`, use `{Binding}` instead of `x:Bind`.
* Bind `SelectedItems` of `ListBoxSelectedItemsBehavior`, `SelectedItem` of `ListBoxSingleSelectionBehavior` and `Value` of `MaskedTextBehavior` with `Mode=TwoWay`.
* In Native AOT apps, elements created by templates are wrapped as a base type unless the app uses their type, so type-based searches such as `FindDescendant<T>` and `FindParent<T>` can miss them.
* To start the app with `AddWinUI<TApp>()`, define `DISABLE_XAML_GENERATED_MAIN` in the app project and write `Main` that builds and runs the host.
* In apps with `PublishAot`, an `ItemsSource` that is not an `ObservableCollection<T>`, such as `ObservableView<T>` of Smart.Mvvm, throws when its items change. Add `[assembly: WinRT.GeneratedWinRTExposedExternalType(typeof(System.Collections.ObjectModel.ObservableCollection<object>))]` to the app.
* `x:Bind` passes only literal text to `ConverterParameter`, and `StaticResource` or markup extensions become `null` without a warning. Use `{Binding}` for converters that take such parameters, such as `ContainsConverter` and `ArrayIndexConverter` with an `ObjectList`.
* To catch exceptions thrown after `await` in `async void` methods such as `AsyncCommand`, set `ExceptionHandlingSynchronizationContext` with `SynchronizationContext.SetSynchronizationContext` in `OnLaunched`.
