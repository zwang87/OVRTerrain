using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class FileReader : MonoBehaviour {

    public static string filename = "output.txt";
    public float[] dataArray;
    private int arraySize = 0;
    private string[] stringArray;
    

    private const int width = 360;
    private const int height = 480;

    private float[,] heightMapData = new float[height, width];
    private float[,] alphaMaps = new float[height, width];
    Terrain terrain;

    float min = 0;
    float max = 0;
    float detailHeight = 0;
	// Use this for initialization
	void Start () {
        //terrain = new GameObject(Terrain);
        terrain = GetComponent<Terrain>();
	    try
        {
            //read data from txt to string array
            stringArray = File.ReadAllLines("Assets/Resources/" + filename);
            arraySize = stringArray.Length;
            dataArray = new float[arraySize];
            Debug.Log(arraySize);

            
            //convert string array to float array
            for (int i = 0; i < arraySize;  i++)
            {

                float.TryParse(stringArray[i], out dataArray[i]);
                if (min > dataArray[i])
                    min = dataArray[i];
                if (max < dataArray[i])
                    max = dataArray[i];
            }
            detailHeight = max - min;
            terrain.terrainData.size = new Vector3(513, detailHeight, 513);

            for (int i = 0; i < arraySize; i++)
            {

                if (min < 0)
                    dataArray[i] -= min;
                dataArray[i] /= detailHeight;
            }
            
            
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
            {
                heightMapData[i, j] = dataArray[i * width + j];
                 //alphaMaps[i, j] = 
            }

            
           
        
        terrain.terrainData.SetHeights(0, 0, heightMapData);
       // terrain.terrainData.SetAlphamaps(0, 0, )
	}

    public Color HSBtoRGB(float hue, float saturation, float brightness)
    {
        int r = 0, g = 0, b = 0;
        if (saturation == 0)
        {
            r = g = b = (int)(brightness * 255.0f + 0.5f);
        }
        else
        {
            float h = (hue - (float)Math.Floor(hue)) * 6.0f;
            float f = h - (float)Math.Floor(h);
            float p = brightness * (1.0f - saturation);
            float q = brightness * (1.0f - saturation * f);
            float t = brightness * (1.0f - (saturation * (1.0f - f)));
            switch ((int)h)
            {
                case 0:
                    r = (int)(brightness * 255.0f + 0.5f);
                    g = (int)(t * 255.0f + 0.5f);
                    b = (int)(p * 255.0f + 0.5f);
                    break;
                case 1:
                    r = (int)(q * 255.0f + 0.5f);
                    g = (int)(brightness * 255.0f + 0.5f);
                    b = (int)(p * 255.0f + 0.5f);
                    break;
                case 2:
                    r = (int)(p * 255.0f + 0.5f);
                    g = (int)(brightness * 255.0f + 0.5f);
                    b = (int)(t * 255.0f + 0.5f);
                    break;
                case 3:
                    r = (int)(p * 255.0f + 0.5f);
                    g = (int)(q * 255.0f + 0.5f);
                    b = (int)(brightness * 255.0f + 0.5f);
                    break;
                case 4:
                    r = (int)(t * 255.0f + 0.5f);
                    g = (int)(p * 255.0f + 0.5f);
                    b = (int)(brightness * 255.0f + 0.5f);
                    break;
                case 5:
                    r = (int)(brightness * 255.0f + 0.5f);
                    g = (int)(p * 255.0f + 0.5f);
                    b = (int)(q * 255.0f + 0.5f);
                    break;
            }
        }
        return new Color(r, g, b);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
