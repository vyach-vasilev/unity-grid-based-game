using System.Collections.Generic;
using UnityEngine;

public static class MeshUtility
{
    private static Mesh _circle;
   
    public static Mesh Quad(float width = 1, float height = 1)
    {
        var mesh = new Mesh();
        
        var vertices = new Vector3[] { new(0, 0, 0), new(width, 0, 0), new(0, 0, height), new(width, 0, height) };
        var triangles = new[] { 0, 2, 1, 2, 3, 1 };
        var normals = new[] { -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward };
        var uv = new Vector2[] { new(0, 0), new(1, 0), new(0, 1), new(1, 1) };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        return mesh;
    }
    
    public static Mesh Circle(float radius, int n)
    {
        if (_circle == null)
        {
            _circle = GetCircle(radius, n);
        }
        return _circle;
    }
    
    private static Mesh GetCircle(float radius, int n)
    {
        Mesh mesh = new Mesh();
        float x, y;
        
        var verticesList = new List<Vector3>();
        var trianglesList = new List<int>();
        var uvs = new List<Vector2>();
        
        for (int i = 0; i < n; i ++)
        {
            x = radius * Mathf.Sin((2 * Mathf.PI * i) / n);
            y = radius * Mathf.Cos((2 * Mathf.PI * i) / n);
            verticesList.Add(new Vector3(x, 0, y));
            uvs.Add(new Vector2((x + radius), (y +radius)));
        }
        
        var vertices = verticesList.ToArray();

        for(int i = 0; i < n-2; i++)
        {
            trianglesList.Add(0);
            trianglesList.Add(i+1);
            trianglesList.Add(i+2);
        }
        
        var triangles = trianglesList.ToArray();

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.SetUVs(0, uvs);
        return mesh;
    }
}