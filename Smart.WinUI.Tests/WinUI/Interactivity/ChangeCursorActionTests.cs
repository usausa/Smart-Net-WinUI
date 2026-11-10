namespace Smart.WinUI.Interactivity;

using System.Runtime.CompilerServices;

using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

public sealed class ChangeCursorActionTests
{
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_ProtectedCursor")]
    private static extern InputCursor? GetProtectedCursor(UIElement element);

    [Fact]
    public void SetsCursorOfSender()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var border = new Border();
            var action = new ChangeCursorAction { Cursor = InputSystemCursorShape.Hand };

            // Act
            var result = action.Execute(border, null);

            // Assert
            Assert.True((bool)result);
            Assert.Equal(InputSystemCursorShape.Hand, (GetProtectedCursor(border) as InputSystemCursor)?.CursorShape);
        });
    }

    [Fact]
    public void ClearsCursorWhenCursorIsNull()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var border = new Border();
            new ChangeCursorAction { Cursor = InputSystemCursorShape.Hand }.Execute(border, null);
            var action = new ChangeCursorAction();

            // Act
            var result = action.Execute(border, null);

            // Assert
            Assert.True((bool)result);
            Assert.Null(GetProtectedCursor(border));
        });
    }

    [Fact]
    public void ReturnsFalseWhenSenderIsNotElement()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ChangeCursorAction { Cursor = InputSystemCursorShape.Hand };

            // Act
            var result = action.Execute(null, null);

            // Assert
            Assert.False((bool)result);
        });
    }
}
