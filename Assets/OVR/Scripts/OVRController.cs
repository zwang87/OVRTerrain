using UnityEngine;
using System.Collections;

public class OVRController : MonoBehaviour {
    public float moveScale = 1.0f;
    public float jumpLimit = 100.0f;
    private bool isJump = false;


    float jumpScale;
	// Use this for initialization
	void Start () {
        jumpScale = 2 * moveScale;
	}

    

	// Update is called once per frame
	void Update () {

        if (transform.position.x > 5 && transform.position.x < 175 && transform.position.z < 235 && transform.position.z > 5)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.position += transform.up * moveScale;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
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
        }
        else
            Debug.Log("exceeded!!!!");
        

        
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
            if (transform.position.y > jumpLimit * 0.65f)
                jumpScale = moveScale * (jumpLimit - transform.position.y) / (jumpLimit * 0.35f - 5);
            transform.position += transform.up * jumpScale;
        }
        else
        {
            jumpScale = moveScale;
        }
	}
}
