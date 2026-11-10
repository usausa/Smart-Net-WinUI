namespace Example.Views;

using System.Collections.ObjectModel;

using Smart.Mvvm;
using Smart.WinUI.Input;
using Smart.WinUI.ViewModels;

internal sealed partial class InteractivityViewModel : ExtendViewModelBase
{
    [ObservableProperty]
    public partial int Number { get; set; }

    [ObservableProperty]
    public partial int MatchCount { get; set; }

    [ObservableProperty]
    public partial bool ShowChild { get; set; }

    [ObservableProperty]
    public partial int DisposeCount { get; set; }

    [ObservableProperty]
    public partial int EnterCount { get; set; }

    [ObservableProperty]
    public partial int LongPressCount { get; set; }

    [ObservableProperty]
    public partial string TypedText { get; set; }

    [ObservableProperty]
    public partial string CallText { get; set; }

    [ObservableProperty]
    public partial string? PhoneNumber { get; set; }

    [ObservableProperty]
    public partial IList<object>? SelectedLetters { get; set; }

    [ObservableProperty]
    public partial string SelectedText { get; set; }

    [ObservableProperty]
    public partial object? SingleLetter { get; set; }

    [ObservableProperty]
    public partial int FirstRequest { get; set; }

    [ObservableProperty]
    public partial int LastRequest { get; set; }

    public ObservableCollection<string> Letters { get; } = ["A", "B", "C"];

    public ObservableCollection<string> Logs { get; } = [];

    public InteractivityChild Child { get; }

    public DelegateCommand AddNumberCommand { get; }

    public DelegateCommand MatchCommand { get; }

    public DelegateCommand EnterCommand { get; }

    public DelegateCommand LongPressCommand { get; }

    public DelegateCommand<string?> TypedCommand { get; }

    public DelegateCommand ReadCommand { get; }

    public DelegateCommand AddLogCommand { get; }

    public DelegateCommand FirstCommand { get; }

    public DelegateCommand LastCommand { get; }

    public InteractivityViewModel()
    {
        TypedText = string.Empty;
        CallText = string.Empty;
        ShowChild = true;
        Child = new InteractivityChild(() => DisposeCount++);
        AddNumberCommand = new DelegateCommand(() => Number++);
        MatchCommand = new DelegateCommand(() => MatchCount++);
        EnterCommand = new DelegateCommand(() => EnterCount++);
        LongPressCommand = new DelegateCommand(() => LongPressCount++);
        TypedCommand = new DelegateCommand<string?>(x => TypedText = x ?? string.Empty);
        SelectedText = string.Empty;
        ReadCommand = new DelegateCommand(() => SelectedText = String.Join(",", SelectedLetters ?? []));
        AddLogCommand = new DelegateCommand(() => Logs.Add($"Item {Logs.Count + 1}"));
        FirstCommand = new DelegateCommand(() => FirstRequest++);
        LastCommand = new DelegateCommand(() => LastRequest++);
    }

    public void Ping() => CallText += "-";

    public void Append(string value) => CallText += value;

    public string MakeCopyText() => $"Number={Number}";
}

internal sealed partial class InteractivityChild : IDisposable
{
    private readonly Action disposed;

    public InteractivityChild(Action disposed)
    {
        this.disposed = disposed;
    }

    public void Dispose() => disposed();
}
