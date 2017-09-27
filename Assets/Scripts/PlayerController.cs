using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform trans;

    private UI gUI;

    [SerializeField] private GameObject projectile;

    [SerializeField] private Transform projectileLaunchPoint;

    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float rotationSpeed = 2;

    private int lives = 5;
    
    public int damage = 2;

    private bool isInvincible = false;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProtoAttack();
        }

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

        trans.Translate(new Vector2(0, y * Time.deltaTime));
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

    void ProtoAttack()
    {
        Instantiate(projectile, projectileLaunchPoint.position, trans.rotation);
    }

    /*This function should be called from elsewhere, when calling it, access it via the Player object in the scene on
    each script that needs to call it.*/
    public void deductLives(int numOfLives)
    {
        if (isInvincible)
        {
            return;
        }

        lives -= numOfLives;
        StartCoroutine(invincibility());

        if (lives <= 0)
        {
            isDead();
        }

        gUI.updateNumberOfLives(lives);
    }

    IEnumerator invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(3);
        isInvincible = false;
    }

    void isDead()
    {
        //Do something on death here.
    }
}
