using UnityEngine;
using UnityEngine.UI;

public static class ImageUtils
{
    public static Color GetTrasparencyColor(Image image, float transparency)
    {
        Color c = image.color;
        c.a = transparency;
        return c;
    }
}