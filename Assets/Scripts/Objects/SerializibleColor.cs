using UnityEngine;

[System.Serializable]
public class SerializibleColor
{
    float r;
    float g;
    float b;
    float a;

    public SerializibleColor(Color color) {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }

    public Color ToUnityColor() {
        return new Color(r, g, b, a);
    }
}
