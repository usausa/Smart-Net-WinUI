namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using Windows.Graphics;

public sealed class WindowPlacementHelperTests
{
    [Theory]
    [InlineData(WindowPlacement.TopLeft, 115, 80)]
    [InlineData(WindowPlacement.TopRight, 855, 80)]
    [InlineData(WindowPlacement.BottomLeft, 115, 690)]
    [InlineData(WindowPlacement.BottomRight, 855, 690)]
    [InlineData(WindowPlacement.Center, 500, 400)]
    public void MovesWindowIntoArea(WindowPlacement placement, int x, int y)
    {
        XamlThread.Run(() =>
        {
            // Arrange
            var window = AppWindow.Create();
            try
            {
                window.Resize(new SizeInt32(200, 100));
                var area = new RectInt32(100, 50, 1000, 800);

                // Act
                var result = WindowPlacementHelper.UpdatePlacement(window, area, placement, new Thickness(10, 20, 30, 40), 1.5);

                // Assert
                Assert.True(result);
                Assert.Equal(x, window.Position.X);
                Assert.Equal(y, window.Position.Y);
            }
            finally
            {
                window.Destroy();
            }
        });
    }
}
