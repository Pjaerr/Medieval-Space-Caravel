using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1;
    private Transform trans;
    private Transform mainCamTransform;

    void Start()
    {
        trans = GetComponent<Transform>();
        mainCamTransform = Camera.main.GetComponent<Transform>();
    }
	void Update ()
    {
        ProtoMovement();
	}

    void ProtoMovement()
    {
        float x = 0;
        float y = 0;

        if (Input.GetKey(KeyCode.W))
        {
            y += movementSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            y -= movementSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            x -= movementSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            x += movementSpeed;
        }

        trans.Translate(new Vector2(x, y));
    }
}
