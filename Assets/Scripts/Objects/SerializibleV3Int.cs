using UnityEngine;

[System.Serializable]
public class SerializibleV3Int
{
    public int x;
    public int y;
    public int z;

    public SerializibleV3Int(Vector3Int v3i) {
        x = v3i.x;
        y = v3i.y;
        z = v3i.z;
    }

    public Vector3Int ToUnityVector() {
        Vector3Int vector = new Vector3Int();
        vector.x = x;
        vector.y = y;
        vector.z = z;
        return vector;
    }
}
