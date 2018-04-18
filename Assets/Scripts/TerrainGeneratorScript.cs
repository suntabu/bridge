using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorScript : MonoBehaviour
{
    public Texture2D heightMap;

    public Material terrianMat;

    public Vector3 range;


    void Start()
    {
    }

    void Update()
    {
    }

    [ContextMenu("Generate")]
    public void Generate()
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        Debug.Log(heightMap.width);
        //Bottom left section of the map, other sections are similar
        for (int i = 0; i < heightMap.width; i++)
        {
            for (int j = 0; j < heightMap.height; j++)
            {
                //Add each new vertex in the plane
                var y = (heightMap.GetPixel(i, j).grayscale - 1) * range.y;
                Debug.Log("y   --->   " + y + "   " + heightMap.GetPixel(i, j).grayscale);
                verts.Add(new Vector3(i * range.x, y, j * range.z));
                //Skip if a new square on the plane hasn't been formed
                if (i == 0 || j == 0) continue;
                //Adds the index of the three vertices in order to make up each of the two tris
                tris.Add(heightMap.width * i + j); //Top right
                tris.Add(heightMap.width * i + j - 1); //Bottom right
                tris.Add(heightMap.width * (i - 1) + j - 1); //Bottom left - First triangle
                tris.Add(heightMap.width * (i - 1) + j - 1); //Bottom left 
                tris.Add(heightMap.width * (i - 1) + j); //Top left
                tris.Add(heightMap.width * i + j); //Top right - Second triangle
            }
        }

        Vector2[] uvs = new Vector2[verts.Count];
        for (var i = 0; i < uvs.Length; i++) //Give UV coords X,Z world coords
            uvs[i] = new Vector2(verts[i].x / heightMap.width / range.x, verts[i].z / heightMap.height / range.z);

        Mesh procMesh = new Mesh();
        procMesh.vertices = verts.ToArray(); //Assign verts, uvs, and tris to the mesh
        procMesh.uv = uvs;
        procMesh.triangles = tris.ToArray();
        procMesh.RecalculateNormals(); //Determines which way the triangles are facing
        GetComponent<MeshFilter>().mesh = procMesh; //Assign Mesh object to MeshFilter
    }
}