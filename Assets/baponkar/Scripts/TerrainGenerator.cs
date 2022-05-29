//Mesh Generator
//Build Date : 29/05/2022
//Builder Name : Bapon Kar
//Reference : https://www.youtube.com/watch?v=64NblGkAabk
//Info : Mesh Filter Containing the mesh data and Mesh Renderer rendering the mesh in gameview
//Without backface cull the generated mesh show only one side



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3 [] vertices;
    int [] triangles;

    public int xSize = 20;
    public int zSize = 20;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        //as example make a grid of 2x2 we need 3*3 = 9 vertices
        vertices = new Vector3[(xSize + 1) * (zSize + 1)]; 

        int i = 0;
        for(int z = 0; z <= zSize; z++)
        {
            for(int x=0; x<= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x*0.3f, z*0.3f) * 2f;
                vertices[i] = new Vector3(x,y,z);
                i++;
            }
            
        }

        triangles = new int [xSize*zSize*6];
        int vert = 0;
        int tris = 0;

        for(int z=0;z<zSize;z++)
        {
            for(int x= 0;x< xSize;x++)
            {
                
                triangles[tris + 0] = vert+ 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }


    private void OnDrawGizmos()
    {
        if(vertices == null)
        {
            return;
        }

        for(int i=0; i<vertices.Length; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
