﻿using UnityEngine;
using UnityEngine.UI;

public class Player : BaseCharacter
{
    [SerializeField]
    private float jumpForce;

    private string hpText;
    private string expText;
    private string lvlText;
    public GameObject projectile;
    public float bulletSpeed;

    private AudioSource audioSource;

    public float volume;

    public AudioClip jumpSound;
    public AudioClip takeHitSound;
    public AudioClip LevelUpSound;
    public AudioClip shootProjectileSound;
    public AudioClip landingHit;

    private int oldExp;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    override public void Start()
    {
        base.Start();
        HandleHPText();

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleHPText();
    }


    // FixedUpdate is called every fixed framerate frame
    void FixedUpdate()
    {
        Move();
    }

    override public void Move()
    {
        // Player Movements
        if (gameController.HasGameStarted)
        {

            animator.SetBool("Jumping", !IsGrounded());

        }
    }

    private void HandleInput()
    {
        // Start Game
        if (!gameController.HasGameStarted && Input.anyKey)
        {
            gameController.HasGameStarted = true;
        }

        // Go Left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.right * getVelocity() * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 180);

            animator.SetInteger("Speed", -1);
            isFacingLeft = true;
        }

        // Go Right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * getVelocity() * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);

            animator.SetInteger("Speed", 1);
            isFacingLeft = false;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            PlayJumpSound();
        }

        //Stop walk animation
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.Play("PlayerIdle");
            animator.SetInteger("Speed", 0);
        }

        //Shoot projectile
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayShootProjectileSound();

            int modifier = isFacingLeft ? -1 : 1;
            Vector2 forceVector = isFacingLeft ? Vector2.left : Vector2.right;

            Vector3 firePosition = new Vector3(transform.position.x + (1.5f * modifier), transform.position.y - 0.75f, 0);
            GameObject bPrefab = Instantiate(projectile, firePosition, Quaternion.identity) as GameObject;
            bPrefab.GetComponent<Rigidbody2D>().AddForce(forceVector * bulletSpeed);
        }

    }

    //Handle Enemy contact
    void OnCollisionEnter2D(Collision2D coll)
    {
        //Contact with enemy
        if (coll.gameObject.tag == "Enemy")
        {
            //get the collision object attack and pass it as parameter.
            GameObject getEnemy = coll.gameObject;
            BaseEnemy enemy = getEnemy.GetComponent<BaseEnemy>();
            PlayTakeHitSound();
            DecreaseHP(enemy.getAttackPoints());
        }

        //Checks if the hero has no hp
        if (getCurrentHealthPoints() <= 0)
        {
            //TODO: Show Game over screen
            gameObject.SetActive(false);

        }

        HandleHPText();
    }

    public void HandleHPText()
    {
        hpText = "HP: " + getCurrentHealthPoints() + "/" + getMaxHealthPoints();
        lvlText = "Lvl: " + getLevel();
        ManageExperience();
    }

    void OnGUI()
    {

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;

        GUI.Label(new Rect(20, 20, 100, 100), hpText, style);

        style.normal.textColor = Color.green;
        GUI.Label(new Rect(20, 50, 100, 100), lvlText, style);

        style.normal.textColor = Color.cyan;
        GUI.Label(new Rect(20, 80, 100, 100), expText, style);

    }

    void ManageExperience()
    {
        int currentExp = getExperiencePoints();
        expText = "EXP: " + currentExp;

        //Example level-exp ratio
        if (oldExp != currentExp)
        {

            if (currentExp >= 100 && currentExp <= 101)
            {
                LevelUp(2);
            }
            if (currentExp >= 200 && currentExp <= 201)
            {
                LevelUp(3);
            }
            if (currentExp >= 300 && currentExp <= 301)
            {
                LevelUp(4);
            }
            if (currentExp >= 500 && currentExp <= 501)
            {
                LevelUp(5);
            }
            if (currentExp >= 600 && currentExp <= 601)
            {
                LevelUp(6);
            }
            if (currentExp >= 7000 && currentExp <= 7001)
            {
                LevelUp(7);
            }
            if (currentExp >= 8000 && currentExp <= 8001)
            {
                LevelUp(8);
            }
            if (currentExp >= 10000 && currentExp <= 10001)
            {
                LevelUp(9);
            }
            if (currentExp >= 20000 && currentExp <= 20001)
            {
                LevelUp(10);

            }
            oldExp = currentExp;

        }


    }

    void LevelUp(int val)
    {
        setLevel(val);
        jumpForce += 100;
        PlayLevelUpSound();
    }

    void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound, volume);
    }

    void PlayTakeHitSound()
    {
        audioSource.PlayOneShot(takeHitSound, volume);
    }

    void PlayLevelUpSound()
    {
        audioSource.PlayOneShot(LevelUpSound, volume);
    }

    void PlayShootProjectileSound()
    {
        audioSource.PlayOneShot(shootProjectileSound, volume);
    }

    public void PlayLandingShoot()
    {
        audioSource.PlayOneShot(landingHit, volume);
    }


}
