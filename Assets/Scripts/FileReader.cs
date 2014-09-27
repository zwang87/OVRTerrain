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
            }

            
            
        
        terrain.terrainData.SetHeights(0, 0, heightMapData);
	}
	
    public float[] getData()
    {
        return dataArray;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
