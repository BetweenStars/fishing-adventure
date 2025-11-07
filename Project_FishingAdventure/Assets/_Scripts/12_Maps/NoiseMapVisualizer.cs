using UnityEngine;
using UnityEngine.UI;

public class NoiseMapVisualizer : MonoBehaviour
{
    public PerlinNoiseGenerator generator;
    public RawImage displayImage;

    private void Start()
    {
        VisualizeMap();
    }

    [ContextMenu("Visualize Map")]
    public void VisualizeMap()
    {
        if (generator == null || displayImage == null)
        {
            return;
        }

        float[,] noiseMap = generator.GetNoiseMap();
        int width = generator.width;
        int height = generator.height;

        Texture2D texture = new Texture2D(width, height);
        Color[] colorMap = new Color[width * height];
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float noiseValue = noiseMap[x, y];
                Color color;

                if (noiseValue < 0.6f)
                {
                    color = Color.blue;
                }
                else if (noiseValue < 0.85f)
                {
                    color = Color.cyan;
                }
                else if (noiseValue < 0.9f)
                {
                    color = Color.lightBlue;
                }
                else
                {
                    color = Color.white;
                }

                colorMap[y * width + x] = color;
            }
        }

        // 4. 텍스처에 색상 적용 및 표시
        texture.SetPixels(colorMap);
        texture.Apply();

        displayImage.texture = texture;
        displayImage.rectTransform.sizeDelta = new Vector2(width, height);
    }
}
