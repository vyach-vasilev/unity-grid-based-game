using UnityEngine;

public static class Extensions
{
    public static Vector2 XZ(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
    
    public static void ToInt(this Transform transform)
    {
        transform.position =transform.position.ToInt();
        transform.eulerAngles = transform.eulerAngles.ToInt();
        transform.localScale =transform.localScale.ToInt();
    }
    
    public static Vector3Int ToInt(this Vector3 vector)
    {
        return new Vector3Int(
            Mathf.CeilToInt(vector.x),
            Mathf.CeilToInt(vector.y),
            Mathf.CeilToInt(vector.z)
            );
    }
}