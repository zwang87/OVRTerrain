using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string test = FileReader.filename;

        float[] data = GameObject.Find("Directional light").GetComponent<FileReader>().getData();
        for(int i = 0; i < 10; i++)
            Debug.Log(data[i]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
