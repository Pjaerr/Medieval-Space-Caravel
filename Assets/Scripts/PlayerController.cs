using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform trans;

    private UI gUI;

    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float rotationSpeed = 2;

    private int lives = 5;
    
    public int damage = 2;

    void Start()
    {
        trans = GetComponent<Transform>();
        gUI = GameManager.singleton.GetComponent<UI>();
        gUI.updateNumberOfLives(lives);
    }

	void Update ()
    {
        ProtoMovement();
	}

    /*Checks if object is being rotated, then increases value y if the vertical axis has been changed, 1 or -1 respectively,
    then translate this object by the new y value.*/
    void ProtoMovement()
    {
        ProtoTurning();

        float y = 0;

        if (Input.GetAxisRaw("Vertical") == 1)
        {
            y += movementSpeed;
        }
        else if (Input.GetAxisRaw("Vertical") == -1)
        {
            y -= movementSpeed;
        }

        trans.Translate(new Vector2(0, y));
    }


    /*If the horizontal axis is either 1 or -1 (left and right respectively), rotation this object by the
    given rotationSpeed in the relevant direction.*/
    void ProtoTurning()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            trans.Rotate(new Vector3(0, 0, -rotationSpeed));
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            trans.Rotate(new Vector3(0, 0, rotationSpeed));
        }
    }

    /*This function should be called from elsewhere, when calling it, access it via the Player object in the scene on
    each script that needs to call it.*/
    public void deductLives(int numOfLives)
    {
        lives -= numOfLives;

        if (lives <= 0)
        {
            isDead();
        }
    }

    void isDead()
    {
        //Do something on death here.
    }

}
