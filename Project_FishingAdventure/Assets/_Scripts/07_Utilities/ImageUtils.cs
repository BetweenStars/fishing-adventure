using UnityEngine;
using UnityEngine.UI;

public static class ImageUtils
{
    public static Color GetTransparencyColor(Color color, float transparency)
    {
        Color c = color;
        c.a = transparency;
        return c;
    }
}