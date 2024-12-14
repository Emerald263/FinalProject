using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine.SceneManagement; //importing SceneManagement Library
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    Rigidbody2D rb;
    public float jumpForce;
    public float speed;
    public float dash;


    //Ground check
    public bool isGrounded;
    public bool Wallclimb;

    //Animation variables
    Animator anim;
    public bool moving;
    public bool attack;
    public bool jump;

    //audio variables
    public AudioSource soundEffects;
    public AudioClip[] sounds;


    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        soundEffects = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;

        //variables to mirror the player
        Vector3 newScale = transform.localScale;
        float currentScale = Mathf.Abs(transform.localScale.x); //take absolute value of the current x scale, this is always positive


        rb.velocity = new Vector2(rb.velocity.x * 5f, rb.velocity.y);

        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition.x -= speed;
            newScale.x = -currentScale;
            //is moving
            moving = true;
            attack = false;

        }

        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition.x += speed;
            newScale.x = +currentScale;
            //is moving
            moving = true;
            attack = false;
        }

        if (Input.GetKey("a") && Input.GetKey(KeyCode.LeftShift))
        {
            newPosition.x -= speed + dash;
            newScale.x = -currentScale;
            //is dashing
            moving = true;
            attack = false;

        }

        if (Input.GetKey("d") && Input.GetKey(KeyCode.LeftShift))
        {
            newPosition.x += speed + dash;
            newScale.x = +currentScale;
            //is dashing
            moving = true;
            attack = false;
        }


        if (Input.GetKey("space") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jump = true;
            attack = false;
        }


        if (Input.GetMouseButtonDown(0))
        {
            moving = false;
            isGrounded = true;
            anim.SetBool("Attack1", attack);
            attack = true;
        }


        anim.SetBool("isMoving", moving);
        anim.SetBool("Jumping", jump);
        anim.SetBool("Attack1", attack);

        transform.position = newPosition;
        transform.localScale = newScale;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            Debug.Log("i hit the ground");
            soundEffects.PlayOneShot(sounds[2], .2f); //play walk sound effect
            isGrounded = true;
            jump = false;
            attack = false;
        }

        if (collision.gameObject.tag.Equals("wall") || Input.GetKeyUp("e"))
        {
            Debug.Log("climbing");
            Wallclimb = true;
            // Temporarily disable gravity

            rb.simulated = false;
        }

        if (collision.gameObject.tag.Equals("death"))
        {
            Debug.Log("death");
            soundEffects.PlayOneShot(sounds[1], .7f); //play death sound effect
            SceneManager.LoadScene(0);
        }

        if (collision.gameObject.tag.Equals("door"))
        {
            Debug.Log("change scene");
            soundEffects.PlayOneShot(sounds[0], .7f); //play door sound effect
            SceneManager.LoadScene(2);
        }

        if (collision.gameObject.tag.Equals("coin"))
        {

            soundEffects.PlayOneShot(sounds[3], .7f); //play coincollect sound effect


        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            isGrounded = false;
        }

    }


}
