using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class CreateTerrain : MonoBehaviour
{

    public static string filename = "output.txt";
    public float[] dataArray;
    private int arraySize = 0;
    private string[] stringArray;


    private int height = 480;
    private int width = 360;

    private float[,] heightMapData = new float[480, 360];

    Terrain terrain;
    Mesh mesh;

    float min = 0;
    float max = 0;
    float detailHeight = 0;
    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        try
        {
            //read data from txt to string array
            stringArray = File.ReadAllLines("Assets/Resources/" + filename);
            arraySize = stringArray.Length;
            dataArray = new float[arraySize];
            Debug.Log(arraySize);


            //convert string array to float array
            for (int i = 0; i < arraySize; i++)
            {

                float.TryParse(stringArray[i], out dataArray[i]);
                if (min > dataArray[i])
                    min = dataArray[i];
                if (max < dataArray[i])
                    max = dataArray[i];
            }
            detailHeight = max - min;
            //terrain.terrainData.size = new Vector3(480, detailHeight, 360);
            /*
            for (int i = 0; i < arraySize; i++)
            {

                if (min < 0)
                    dataArray[i] -= min;
                dataArray[i] /= detailHeight;
            }
            */

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        //Vector3[] vertices = new Vector3[width * height/4];

        /*
        for (int i = 0; i < width/4; i++)
            for (int j = 0; j < height; j++)
            {
                //heightMapData[i, j] = dataArray[i * height + j];
                vertices[i * height + j].x = i;
                vertices[i * height + j].y = j;
                vertices[i * height + j].z = dataArray[i * height + j];
                _UV[i * height + j] = new Vector2(i / width / 4, j / height);
            }

        */
        ////////////////////////////////////
        height = 181;
        //width = 120;
        _Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _Mesh;

        _Vertices = new Vector3[width * height];
        _UV = new Vector2[width * height];
        _Triangles = new int[6 * ((width - 1) * (height - 1))];

        int triangleIndex = 0;
        

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width) + x;
                //int index2 = (y - height) * width + x;

                _Vertices[index] = new Vector3(x, y, dataArray[index]);
                _UV[index] = new Vector2(((float)x / (float)width), ((float)y / (float)height));

                // Skip the last row/col
                if (x != (width - 1) && y != height-1)
                {
                    int topLeft = index;
                    int topRight = topLeft + 1;
                    int bottomLeft = topLeft + width;
                    int bottomRight = bottomLeft + 1;

                    _Triangles[triangleIndex++] = topLeft;
                    _Triangles[triangleIndex++] = topRight;
                    _Triangles[triangleIndex++] = bottomLeft;
                    _Triangles[triangleIndex++] = bottomLeft;
                    _Triangles[triangleIndex++] = topRight;
                    _Triangles[triangleIndex++] = bottomRight;
                }
            }
        }

        _Mesh.vertices = _Vertices;
        _Mesh.uv = _UV;
        _Mesh.triangles = _Triangles;
        _Mesh.RecalculateNormals();
        GetComponent<MeshCollider>().mesh = _Mesh;
        /////////////////////////////////////////////



    }

    private Mesh _Mesh;
    private Vector3[] _Vertices;
    private Vector2[] _UV;
    private int[] _Triangles;

    void CreateMesh(int width, int height)
    {
        _Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _Mesh;

        _Vertices = new Vector3[width * height];
        _UV = new Vector2[width * height];
        _Triangles = new int[6 * ((width - 1) * (height - 1))];

        int triangleIndex = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width) + x;

                _Vertices[index] = new Vector3(x, -y, 0);
                _UV[index] = new Vector2(((float)x / (float)width), ((float)y / (float)height));

                // Skip the last row/col
                if (x != (width - 1) && y != (height - 1))
                {
                    int topLeft = index;
                    int topRight = topLeft + 1;
                    int bottomLeft = topLeft + width;
                    int bottomRight = bottomLeft + 1;

                    _Triangles[triangleIndex++] = topLeft;
                    _Triangles[triangleIndex++] = topRight;
                    _Triangles[triangleIndex++] = bottomLeft;
                    _Triangles[triangleIndex++] = bottomLeft;
                    _Triangles[triangleIndex++] = topRight;
                    _Triangles[triangleIndex++] = bottomRight;
                }
            }
        }

        _Mesh.vertices = _Vertices;
        _Mesh.uv = _UV;
        _Mesh.triangles = _Triangles;
        _Mesh.RecalculateNormals();
    }

    void Update()
    {

    }
}
