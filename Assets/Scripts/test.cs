using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Texture2D texture = (Texture2D)Resources.Load("Assets/Textures/screenshot.jpg", typeof(Texture2D));
        transform.renderer.material.mainTexture = texture;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
