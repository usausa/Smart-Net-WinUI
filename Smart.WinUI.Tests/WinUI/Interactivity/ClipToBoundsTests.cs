namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;

public sealed class ClipToBoundsTests
{
    [Fact]
    public void EnablingSetsInsetClip()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var element = new Border();

            // Act
            ClipToBounds.SetIsEnabled(element, true);

            // Assert
            Assert.IsType<InsetClip>(ElementCompositionPreview.GetElementVisual(element).Clip);
        });
    }

    [Fact]
    public void DisablingRemovesClip()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var element = new Border();
            ClipToBounds.SetIsEnabled(element, true);

            // Act
            ClipToBounds.SetIsEnabled(element, false);

            // Assert
            Assert.Null(ElementCompositionPreview.GetElementVisual(element).Clip);
        });
    }
}
