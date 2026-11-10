namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using Windows.Graphics;

public static class WindowPlacementHelper
{
    public static bool UpdatePlacement(AppWindow window, RectInt32 area, WindowPlacement placement, Thickness margin = default, double scale = 1)
    {
        var size = window.Size;
        if ((size.Width <= 0) || (size.Height <= 0))
        {
            return false;
        }

        var left = (int)(margin.Left * scale);
        var top = (int)(margin.Top * scale);
        var right = (int)(margin.Right * scale);
        var bottom = (int)(margin.Bottom * scale);
        var position = placement switch
        {
            WindowPlacement.TopLeft => new PointInt32(area.X + left, area.Y + top),
            WindowPlacement.TopRight => new PointInt32(area.X + area.Width - size.Width - right, area.Y + top),
            WindowPlacement.BottomLeft => new PointInt32(area.X + left, area.Y + area.Height - size.Height - bottom),
            WindowPlacement.BottomRight => new PointInt32(area.X + area.Width - size.Width - right, area.Y + area.Height - size.Height - bottom),
            _ => new PointInt32(area.X + ((area.Width - size.Width) / 2), area.Y + ((area.Height - size.Height) / 2))
        };
        window.Move(position);

        return true;
    }
}
