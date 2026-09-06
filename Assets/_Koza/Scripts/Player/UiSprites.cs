using UnityEngine;

// Kodla üretilen yuvarlak sprite (UI butonlar kare görünmesin diye)
public static class UiSprites
{
    static Sprite circle;

    public static Sprite Circle()
    {
        if (circle) return circle;
        int s = 256;
        var tex = new Texture2D(s, s, TextureFormat.ARGB32, false);
        tex.filterMode = FilterMode.Bilinear;
        float r = s / 2f;
        for (int y = 0; y < s; y++)
            for (int x = 0; x < s; x++)
            {
                float dx = x - r + 0.5f, dy = y - r + 0.5f;
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                float a = Mathf.Clamp01((r - d) / 3f); // yumuşak kenar
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, a));
            }
        tex.Apply();
        circle = Sprite.Create(tex, new Rect(0, 0, s, s), new Vector2(0.5f, 0.5f), 256f);
        return circle;
    }
}
