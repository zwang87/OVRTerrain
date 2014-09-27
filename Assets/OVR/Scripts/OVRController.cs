using UnityEngine;
using System.Collections;

public class OVRController : MonoBehaviour {
    public float moveScale = 1.0f;
    public float jumpLimit = 60.0f;
    private bool isJump = false;

	// Use this for initialization
	void Start () {
	
	}

    

	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += transform.up * moveScale;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position -= transform.up * moveScale;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position -= transform.right * moveScale;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += transform.right * moveScale;
        }
        else if (Input.GetKeyDown(KeyCode.Equals))
        {
            transform.position += transform.forward * moveScale;
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            transform.position -= transform.forward * moveScale;
        }
        if (Input.GetKey(KeyCode.Space) && transform.position.y < jumpLimit)
        {
            isJump = true;
        }
        else if (!Input.GetKey(KeyCode.Space) || transform.position.y >= jumpLimit)
        {
            isJump = false;
        }

        if (isJump)
        {
            transform.position += transform.up * moveScale;
        }
	}
}
