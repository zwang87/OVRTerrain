using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class TerrainGenerator : MonoBehaviour {

    public string filename = "output.txt";
    public UnityEngine.Object prefab;
    public float colorScale = 1.1f;
    private string[] stringArray;
    public float[] dataArray;
    private int arraySize = 0;
    

    private Mesh _Mesh;
    private Vector3[] _Vertices;
    private Vector2[] _UV;
    private int[] _Triangles;
    private Color[] colors;

    public int height = 480;
    public int width = 360;

    float min = 0;
    float max = 0;
    float heightDifference = 0;

    // Use this for initialization
    void Start()
    {
         try
         {
            using(StreamReader sr = new StreamReader("Assets/Resources/" + filename))
            {
                stringArray = sr.ReadToEnd().Split('\n');
                arraySize = stringArray.Length;
                dataArray = new float[arraySize];

                for (int i = 0; i < arraySize; i++)
                {
                    float.TryParse(stringArray[i], out dataArray[i]);
                    if (min > dataArray[i])
                        min = dataArray[i];
                    if (max < dataArray[i])
                        max = dataArray[i];
                }
                heightDifference = max - min;
                sr.Close();
            }
         }
         catch (Exception e)
         {
             Debug.Log(e.Message);
             return;
         }
        //another way to read raw data, which is able to get the difference between min and max height
        /*
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
            heightDifference = max - min;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }*/
        int tileNumber = (int)Mathf.Ceil(arraySize / 65000.0f);
        int tileSize = height / tileNumber;
        Debug.Log("tileNumber:   " + tileNumber);
        
        //split the mesh into several tiles 
        for (int i = 0; i < tileNumber - 1; i++ )
            CreateMesh(width, i * tileSize, tileSize + 1);
        CreateMesh(width, (tileNumber - 1) * tileSize, height - (tileNumber - 1) * tileSize);
         
    }

    //create mesh from raw data; Mesh create from the tile begins with heightIndex and end to heightIndex + heightSize
    private void CreateMesh(int width, int heightIndex, int heightSize)
    {
        GameObject terrainTile = Instantiate(prefab, Vector3.zero, Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
        terrainTile.transform.parent = transform;
        _Mesh = new Mesh();
        terrainTile.transform.GetComponent<MeshFilter>().mesh = _Mesh;
        _Vertices = new Vector3[width * heightSize];
        _UV = new Vector2[width * heightSize];
        _Triangles = new int[6 * ((width - 1) * (heightSize - 1))];

        Texture2D texture = new Texture2D(width, heightSize);
        terrainTile.renderer.material.mainTexture = texture;

        //for adding color to terrain
        //colors = new Color[width * height];

        int triangleIndex = 0;

        for (int y = heightIndex; y < heightIndex + heightSize; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width) + x;
                int localIndex = (y - heightIndex) * width + x;

                _Vertices[localIndex] = new Vector3(x, y, dataArray[index]);

                //for adding color to terrain
                //float ratio = (dataArray[index] + 35) / 80.0f;
                //colors[index] = ToColor(ratio, 1.0f, 1.0f);

                //texture
                float cH = (dataArray[index] - min) / (max - min) / colorScale;
                Color color = new Color(cH, cH, cH);
                texture.SetPixel(x, y, color);
                
                _UV[localIndex] = new Vector2(((float)x / (float)width), ((float)y / (float)heightSize));

                // Skip the last row/col
                if (x != (width - 1) && y != heightIndex + heightSize - 1)
                {
                    int topLeft = localIndex;
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
        texture.Apply();
        _Mesh.vertices = _Vertices;
        _Mesh.uv = _UV;
        _Mesh.triangles = _Triangles;
        //add color to vertices
        //_Mesh.colors = colors;
        _Mesh.RecalculateNormals();
        terrainTile.transform.GetComponent<MeshCollider>().sharedMesh = _Mesh;
    }

    //convert HSB to RGB color, for color map purpose
    /*
    private Color ToColor(float hsbColorh, float hsbColors, float hsbColorb)
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
     * */
}
