namespace Smart.WinUI.Input;

public sealed class CommandExtensionsTests
{
    [Fact]
    public void AsObservableNotifiesCanExecuteChanged()
    {
        // Arrange
        var command = new DelegateCommand(static () => { });
        var count = 0;

        // Act
        using (command.AsObservable().Subscribe(_ => count++))
        {
            command.RaiseCanExecuteChanged();
        }

        command.RaiseCanExecuteChanged();

        // Assert
        Assert.Equal(1, count);
    }
}
