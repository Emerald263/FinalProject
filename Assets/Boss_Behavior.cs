using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Boss_Behavior : MonoBehaviour
{
    #region Public Variables 
    public Transform rayCast;
    public LayerMask raycastMask;
    public float raycastlength;
    public float maxhealth;
    public float damagetaken;
    public float attackdistance;
    public float movespeed;
    public float timer; //Timer for cooldown between basic attacks

    #endregion

    #region Private Variables
    private RaycastHit2D hit;
    private GameObject target;
    private Animator anim;
    private float distance; //Store the distance b/w enemy and player
    private bool attackmode;
    private bool inRange; //Check if Player is in range
    private bool cooling; //Check if enemy is cooling afterattack
    private float intTimer;

    #endregion

    void Awake()
    {
        intTimer = timer; //Store the inital value of timer
        anim = GetComponent<Animator>();

    }


    void Update()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, raycastlength, raycastMask);
            RaycastDebugger();
            Debug.Log(hit);
        }

        //When Player is detected 
        if (hit.collider != null)
        {
            inRange = true;
            EnemyLogic();


        }
        else if (hit.collider == null)
        {
            inRange = false;
        }
        if (inRange == false)
        {
            anim.SetBool("canWalk", false);
            StopAttack();
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {

            target = trig.gameObject;
            inRange = true;
            anim.SetBool("canWalk", false);
            Debug.Log("Enemy");
        }

    }

    void EnemyLogic()
    {

        distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance < attackdistance)
        {
            Move();
            StopAttack();
        }
        else if (attackdistance >= distance && cooling == false)
        {

            Attack();

        }
        if (cooling)
        {
            Cooldown();
            anim.SetBool("Attack", false);
        }

    }

    void Move()
    {
        anim.SetBool("canWalk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movespeed * Time.deltaTime);

        }
    }

    void Attack()
    {
        timer = intTimer; //Reset Timer when Player enter Attack Range
        attackmode = true; //To check if Boss can still attack or not

        anim.SetBool("canWalk", false);
        anim.SetBool("Attack", true);
    }

    void StopAttack()
    {

        cooling = false;
        attackmode = false;
        anim.SetBool("Attack", false);

    }

    void TriggerCooling()
    {

        cooling = true;

    }

    void Cooldown()
    {

        timer -= Time.deltaTime;
        if (timer <= 0 && cooling && attackmode)
        {

            cooling = false;
            timer = intTimer;

        }

    }

    void Health()
    {

        if (maxhealth < 1)
        {

            Debug.Log("bossdeath");
            Destroy(this.gameObject); //destroy the boss

        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("PlayerWeapon"))
        {
            Debug.Log("Boss hit");

            maxhealth -= damagetaken;
            Health();
        }

    }

    void RaycastDebugger()
    {
        if (distance > attackdistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * raycastlength, Color.red);
        }

        else if (attackdistance > distance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * raycastlength, Color.green);
        }
    }



}

