using System;
using System.Collections.Generic;
using UnityEngine;

public static class MeshUtility
{
    private static Mesh _circle;
    private static float _radius;
    private static int _edges;
    
    public static Mesh Circle(float radius, int edges)
    {
        if (_circle == null || Math.Abs(_radius - radius) > 0.01f || _edges != edges)
        {
            _circle = GetCircle(radius, edges);
        }
        
        _radius = radius;
        _edges = edges;
        
        return _circle;
    }
    
    private static Mesh GetCircle(float radius, int edges)
    {
        Mesh mesh = new Mesh();
        float x, y;
        
        var verticesList = new List<Vector3>();
        var trianglesList = new List<int>();
        var uvs = new List<Vector2>();
        
        for (int i = 0; i < edges; i ++)
        {
            x = radius * Mathf.Sin((2 * Mathf.PI * i) / edges);
            y = radius * Mathf.Cos((2 * Mathf.PI * i) / edges);
            verticesList.Add(new Vector3(x, 0, y));
            uvs.Add(new Vector2((x + radius), (y +radius)));
        }
        
        var vertices = verticesList.ToArray();

        for(int i = 0; i < edges-2; i++)
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