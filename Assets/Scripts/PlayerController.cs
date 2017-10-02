using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform trans;

    private UI gUI;

    [SerializeField] private GameObject projectile;

    [SerializeField] private Transform[] projectileLaunchPoints = new Transform[2];

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
        ProtoAttack();

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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spawnProjectile(projectileLaunchPoints[0].position, projectileLaunchPoints[0].rotation);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            spawnProjectile(projectileLaunchPoints[1].position, projectileLaunchPoints[1].rotation);
        }
    }

    void spawnProjectile(Vector3 pos, Quaternion rotation)
    {
        Instantiate(projectile, pos, rotation);
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
            //isDead();
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
        Time.timeScale = 0;
        gUI.youLoseScreen.SetActive(true);
    }
}
