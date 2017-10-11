using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Local References
    private Transform trans;
    private SpriteRenderer spriteRenderer;
    private UI gUI;

    //References to objects.
    [SerializeField] private GameObject projectile;

    [Header("GameObjects holding left and right launch points respectively")]  
     [SerializeField] private Transform[] launchPointObjects = new Transform[2];

     [SerializeField] private int numberOfCanonSlots = 1;
    private Transform[,] projectileLaunchPoints; //[left and right][number of spots for canons]


    [Header("Player Attributes")]
    //Player Attributes
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private int lives = 5;
    public int damage = 2;
    [SerializeField] private float cooldownTime = 1.0f;
    
    //Checks
    private bool isInvincible = false;
    private bool cooldownHasEnded = true;

    void Start()
    {
        projectileLaunchPoints = new Transform[2, numberOfCanonSlots];
        trans = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gUI = GameManager.singleton.GetComponent<UI>();
        gUI.updateNumberOfLives(lives);
        for (int i = 0; i < launchPointObjects.Length; i++)
        {
            storeLaunchPoint(launchPointObjects[i], i);
        }
       
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
        if (cooldownHasEnded)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                for (int i = 0; i < numberOfCanonSlots; i++)
                {
                    StartCoroutine(fireProjectile(projectileLaunchPoints[0, i]));
                }
                
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                for (int i = 0; i < numberOfCanonSlots; i++)
                {
                    StartCoroutine(fireProjectile(projectileLaunchPoints[1, i]));
                }
            }
        }
    }

    IEnumerator fireProjectile(Transform launchPoint)
	{
        GameManager.singleton.fireSound.Play();
		cooldownHasEnded = false;
		spawnProjectile(launchPoint.position, launchPoint.rotation);
		yield return new WaitForSeconds(cooldownTime);
		cooldownHasEnded = true;
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
            isDead();
        }

        gUI.updateNumberOfLives(lives);
    }

    IEnumerator invincibility()
    {
        StartCoroutine(flashSprite());
        isInvincible = true;
        yield return new WaitForSeconds(2);
        isInvincible = false;
    }

    IEnumerator flashSprite()
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.enabled = true;
    }

    void isDead()
    {
        GameManager.singleton.endGame(false);
    }

    void storeLaunchPoint(Transform launchPointObj, int leftRight)
    {
        for (int i = 0; i < launchPointObj.childCount; i++)
        {
            projectileLaunchPoints[leftRight, i] = launchPointObj.GetChild(i);
        }
    }
}
