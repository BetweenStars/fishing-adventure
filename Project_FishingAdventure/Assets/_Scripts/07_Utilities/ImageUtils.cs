using UnityEngine;
using UnityEngine.UI;

public static class ImageUtils
{
    public static Color GetTrasparencyColor(Color color, float transparency)
    {
        Color c = color;
        c.a = transparency;
        return c;
    }
}