//Mesh Generator
//Build Date : 29/05/2022
//Builder Name : Bapon Kar
//Reference : https://www.youtube.com/watch?v=eJEpeUH1EMg
//Info : Mesh Filter Containing the mesh data and Mesh Renderer rendering the mesh in gameview
//Without backface cull the generated mesh show only one side



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3 [] vertices;
    int [] triangles;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1)
        };

        triangles = new int[]
        {
            0,1,2,
            2,1,3
        };
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
