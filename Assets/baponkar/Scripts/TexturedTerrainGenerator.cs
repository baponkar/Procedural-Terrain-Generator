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
public class TexturedTerrainGenerator : MonoBehaviour
{
    Vector3 startPos;
    Mesh mesh;

    Vector3 [] vertices;
    int [] triangles;

    //Vector2 [] uvs;
    public Gradient gradient;
    private float maxTerrainHeight;
    private float minTerrainHeight;
    Color [] colors;

    [Range(0,500)]
    public int xSize = 100;
    [Range(0,500)]
    public int zSize = 100;

    public GameObject [] treePrefab;
    public GameObject [] rockPrefab;
    public int totalTree = 10;
    public int totalRock = 10;

    public Transform trees;
    public Transform rocks;

    public GameObject playerPrefab;

    void Start()
    {
        startPos = transform.position;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        CreateShape();
        UpdateMesh();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
   
    }

    void CreateShape()
    {
        //as example make a grid of 2x2 we need 3*3 = 9 vertices
        vertices = new Vector3[(xSize + 1) * (zSize + 1)]; 

        
        for(int i=0, z = 0; z <= zSize; z++)
        {
            for(int x=0; x<= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x*0.1f, z*0.1f) * 10f;
                vertices[i] = new Vector3(x, y,z) + startPos;

                if(y > maxTerrainHeight)
                {
                    maxTerrainHeight = y;
                }
                if(y<minTerrainHeight)
                {
                    minTerrainHeight = y;
                }
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

        //uvs = new Vector2 [vertices.Length];
        colors = new Color [ vertices.Length];

        for(int i=0, z = 0; z <= zSize; z++)
        {
            for(int x=0; x<= xSize; x++)
            {
                //uvs[i] = new Vector2((float) z / zSize, (float) x /xSize);
                //making height range from 0 to 1 using InverseLerp
                float height = Mathf.InverseLerp(minTerrainHeight,maxTerrainHeight,vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
            
        }

        
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.uv = uvs;
        mesh.colors = colors;
        

        mesh.RecalculateNormals();
        ObjectGenerate(trees,treePrefab,totalTree);
        ObjectGenerate(rocks,rockPrefab,totalRock); 

        Vector3 playerPos = vertices[(int) vertices.Length/2];
        playerPos.y = playerPos.y + 20f;
        Instantiate(playerPrefab,playerPos + startPos,Quaternion.identity);
    }

    void ObjectGenerate(Transform parent, GameObject[] prefab, int total)
    {
        List<int> positions = new List<int>();

        while(true)
        {
            if(total <= 0)
            {
                break;
            }

            int index = (int) Random.Range(0,vertices.Length);
            Vector3 pos = vertices[index];

            if(pos.y < (maxTerrainHeight* 0.6) && pos.y > (maxTerrainHeight* 0.2))
            {
                positions.Add(index);
                total--;
            }
        }

        for(int i=0; i< positions.Count; i++)
        {
            var obj = Instantiate(prefab[(int)Random.Range(0,prefab.Length)], startPos + vertices[positions[i]], Quaternion.identity);
            float scale = Random.Range(0.4f,1.5f);
            obj.transform.localScale = new Vector3(scale,scale,scale);
            // Vector3 v1 = vertices[positions[i]];
            // Vector3 v2 = vertices[positions[i] + 1];
            // Vector3 normal = Vector3.Cross(v1,v2);
            // obj.transform.rotation = Quaternion.LookRotation(normal);

            obj.transform.SetParent(parent);
        }
    }
   
}
