using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class CreateTerrain3 : MonoBehaviour
{

    public static string filename = "output.txt";
    public float[] dataArray;
    private int arraySize = 0;
    private string[] stringArray;


    private int height = 480;
    private int width = 360;

    private float[,] heightMapData = new float[480, 360];
    private Color[] colors;

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
        height = 120;
        //width = 120;
        _Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _Mesh;

        _Vertices = new Vector3[width * height];
        _UV = new Vector2[width * height];
        _Triangles = new int[6 * ((width - 1) * (height - 1))];
        colors = new Color[width * height];

        int triangleIndex = 0;
        

        for (int y = 360; y < 480; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width) + x;
                int index2 = (y - 360) * width + x;

                _Vertices[index2] = new Vector3(x, y, dataArray[index]);

                float ratio = (dataArray[index] + 35) / 80.0f;

                colors[index2] = ToColor(ratio, 1.0f, 1.0f);

                _UV[index2] = new Vector2(((float)x / (float)width), ((float)y / (float)height));

                // Skip the last row/col
                if (x != (width - 1) && y != 480 - 1)
                {
                    int topLeft = index2;
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
        _Mesh.colors = colors;
        _Mesh.RecalculateNormals();
       
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

    public Color ToColor(float hsbColorh, float hsbColors, float hsbColorb)
    {
        float r = hsbColorb;
        float g = hsbColorb;
        float b = hsbColorb;
        if (hsbColors != 0)
        {
            float max = hsbColorb;
            float dif = hsbColorb * hsbColors;
            float min = hsbColorb - dif;

            float h = hsbColorh * 360f;

            if (h < 60f)
            {
                r = max;
                g = h * dif / 60f + min;
                b = min;
            }
            else if (h < 120f)
            {
                r = -(h - 120f) * dif / 60f + min;
                g = max;
                b = min;
            }
            else if (h < 180f)
            {
                r = min;
                g = max;
                b = (h - 120f) * dif / 60f + min;
            }
            else if (h < 240f)
            {
                r = min;
                g = -(h - 240f) * dif / 60f + min;
                b = max;
            }
            else if (h < 300f)
            {
                r = (h - 240f) * dif / 60f + min;
                g = min;
                b = max;
            }
            else if (h <= 360f)
            {
                r = max;
                g = min;
                b = -(h - 360f) * dif / 60 + min;
            }
            else
            {
                r = 0;
                g = 0;
                b = 0;
            }
        }
        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b));
    }
}
