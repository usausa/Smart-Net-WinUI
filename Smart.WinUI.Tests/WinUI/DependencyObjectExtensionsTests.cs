namespace Smart.WinUI;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;

public sealed class DependencyObjectExtensionsTests
{
    private static (Grid Root, StackPanel Panel, Border Border) Build()
    {
        var border = new Border { Tag = "border", Child = new TextBlock() };
        var panel = new StackPanel { Tag = "panel", Children = { border } };
        var root = new Grid { Tag = "root", Children = { panel } };
        return (root, panel, border);
    }

    private static (TextBlock Text, Span Outer, Span Inner, Run Run) BuildText()
    {
        var run = new Run();
        var inner = new Span { Inlines = { run } };
        var outer = new Span { Inlines = { inner } };
        var text = new TextBlock { Inlines = { outer } };
        return (text, outer, inner, run);
    }

    private static bool HasTag(FrameworkElement element, string tag) => Equals(element.Tag, tag);

    [Fact]
    public void IsSetReturnsWhetherLocalValueIsSet()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var border = new Border { Tag = "border" };

            // Act & Assert
            Assert.True(border.IsSet(FrameworkElement.TagProperty));
            Assert.False(border.IsSet(FrameworkElement.WidthProperty));
        });
    }

    [Fact]
    public void PropertyChangedAsObservableNotifiesValueUntilDisposed()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var border = new Border();
            var values = new List<double>();

            // Act
            using (border.PropertyChangedAsObservable<double>(FrameworkElement.WidthProperty).Subscribe(values.Add))
            {
                border.Width = 10;
                border.Width = 20;
            }

            border.Width = 30;

            // Assert
            Assert.Equal([10d, 20d], values);
        });
    }

    [Fact]
    public void FindParentFollowsTextElementToContainer()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (text, _, inner, run) = BuildText();

            // Act & Assert
            Assert.Same(inner, run.FindParent<Span>());
            Assert.Same(text, run.FindParent<TextBlock>());
            Assert.Null(run.FindParent<Border>());
        });
    }

    [Fact]
    public void FindParentWithPredicateSkipsNotMatching()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (_, outer, _, run) = BuildText();

            // Act & Assert
            Assert.Same(outer, run.FindParent<Span>(x => x == outer));
            Assert.Null(run.FindParent<Span>(static _ => false));
        });
    }

    [Fact]
    public void FindParentWithStatePassesState()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (_, outer, _, run) = BuildText();

            // Act & Assert
            Assert.Same(outer, run.FindParent<Span, Span>(outer, static (x, s) => x == s));
        });
    }

    [Fact]
    public void FindParentOrSelfReturnsSelfFirst()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (_, _, inner, run) = BuildText();

            // Act & Assert
            Assert.Same(inner, inner.FindParentOrSelf<Span>());
            Assert.Same(inner, run.FindParentOrSelf<Span>());
        });
    }

    [Fact]
    public void FindParentOrSelfWithPredicateChecksSelf()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (_, outer, inner, _) = BuildText();

            // Act & Assert
            Assert.Same(inner, inner.FindParentOrSelf<Span>(x => x == inner));
            Assert.Same(outer, inner.FindParentOrSelf<Span>(x => x == outer));
            Assert.Same(outer, inner.FindParentOrSelf<Span, Span>(outer, static (x, s) => x == s));
        });
    }

    [Fact]
    public void ParentOfDetachedTextElementIsNull()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var run = new Run();

            // Act & Assert
            Assert.Null(run.Parent());
            Assert.Null(run.FindParent<TextBlock>());
        });
    }

    [Fact]
    public void IsDescendantOfFollowsParent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (text, outer, inner, run) = BuildText();

            // Act & Assert
            Assert.True(run.IsDescendantOf(text));
            Assert.True(inner.IsDescendantOf(outer));
            Assert.False(outer.IsDescendantOf(inner));
            Assert.False(run.IsDescendantOf(run));
        });
    }

    [Fact]
    public void FindDescendantWithStatePassesState()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (root, _, border) = Build();

            // Act & Assert
            Assert.Same(border, root.FindDescendant<FrameworkElement, string>("border", static (x, s) => HasTag(x, s)));
            Assert.Null(root.FindDescendant<FrameworkElement, string>("none", static (x, s) => HasTag(x, s)));
        });
    }

    [Fact]
    public void FindDescendantsWithStatePassesState()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (root, panel, border) = Build();

            // Act
            var descendants = root.FindDescendants<FrameworkElement, object?>(null, static (x, s) => x.Tag != s).ToList();

            // Assert
            Assert.Equal([panel, border], descendants);
        });
    }

    [Fact]
    public void FindDescendantOrSelfReturnsSelfFirst()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (root, _, border) = Build();

            // Act & Assert
            Assert.Same(root, root.FindDescendantOrSelf<Grid>());
            Assert.Same(border, root.FindDescendantOrSelf<Border>());
            Assert.Null(root.FindDescendantOrSelf<Button>());
        });
    }

    [Fact]
    public void FindDescendantOrSelfWithPredicateChecksSelf()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (root, panel, _) = Build();

            // Act & Assert
            Assert.Same(root, root.FindDescendantOrSelf<Panel>(static x => HasTag(x, "root")));
            Assert.Same(panel, root.FindDescendantOrSelf<Panel>(static x => HasTag(x, "panel")));
            Assert.Same(panel, root.FindDescendantOrSelf<Panel, string>("panel", static (x, s) => HasTag(x, s)));
        });
    }
}
