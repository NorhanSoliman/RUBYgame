using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public float timeInvincible = 2.0f; //she should only get damaged every two seconds

    int currentHealth;
    public int health { get { return currentHealth; } } //like a getter function in c++,
                                                        //instead of making everything punlic

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    public float speed = 3.0f;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0); //Ruby can stand still

    public GameObject projectilePrefab;

    AudioSource audioSource;

    public AudioClip hitSound;
    public AudioClip cogThrow;

    public ParticleSystem HitEffect;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);


        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize(); //normalize to length of 1
        }

        //send the data to the animator
        //the direction you look and the speed
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C)) //launch projectile at c
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X)) //if x key is pressed, talk to jambi
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            //start: upward offset from ruby's position.
            //direction: where ruby is looking
            //maximum distance of the ray: 1.5F
            //layer mask: NPC
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            PlaySound(hitSound);

            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            // Instantiate the hit effect particle system
            if (HitEffect != null)
            {
                ParticleSystem effectInstance = Instantiate(HitEffect, transform.position, Quaternion.identity);
                Destroy(effectInstance.gameObject, effectInstance.main.duration);
            }

        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //Clamping ensures that the first parameter (here currentHealth + amount)
        //never goes lower than the second parameter (here 0)
        //and never goes above the third parameter (maxHealth).
        //So Ruby’s health will always stay between 0 and maxHealth.

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        //instantiate: first parameter: object, second: copy at position, third: rotation

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(cogThrow);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
