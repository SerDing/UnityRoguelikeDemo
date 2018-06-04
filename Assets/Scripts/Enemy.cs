using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Animator animator;

    private Transform heroTran;

    private Hero hero;

    private Rigidbody2D rigidbody;

    private float smoothing = 2;

    private Vector2 targetPosition;

    private BoxCollider2D collider;

    public AudioClip attackSound1;
    public AudioClip attackSound2;

    private void Start()
    {
        heroTran = GameObject.FindGameObjectWithTag("Player").transform;

        hero = Hero.Instance;

        animator = GetComponent<Animator>();

        rigidbody = GetComponent<Rigidbody2D>();

        collider = GetComponent<BoxCollider2D>();

        GameMgr.Instance.enemyList.Add(this);

        targetPosition = transform.position;
    }

    void Update()
    {
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime));
    }

    public void AI()
    {
        Vector2 offset = heroTran.position - transform.position; // distance between hero and self

        if (offset.magnitude <= 1.1) // is hero in attack area
        {
            Attack(10);
        }
        else
        {
            Movement(offset);
        }


    }

    private void Attack(int points)
    {
        animator.SetTrigger("Attack");

        SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);

        if (hero != null)
        {
            RaycastHit2D hit = hero.CheckCollision(hero.transform.position,transform.position);

            if (hit.transform != null)
            {
                hero.Damage(points);
            }
        }
    }

    private void Movement(Vector2 offset)
    {
        float vx = 0;
        float vy = 0;

        if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
        {
            vy = offset.y < 0 ? -1 : 1;
        }
        else
        {
            vx = offset.x < 0 ? -1 : 1;
        }

        collider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(targetPosition, targetPosition + new Vector2(vx, vy));
        collider.enabled = true;

        if (hit.transform == null) // no obstacle
        {
            targetPosition += new Vector2(vx / 2, vy / 2);
        }
        else
        {
            if (hit.collider.tag == "Food" || hit.collider.tag == "Soda")
            {
                targetPosition += new Vector2(vx, vy);
            }
            else if(hit.collider.tag == "Player")
            {
                Debug.Log("Enemy hit Hero!!!!!!!");
            }

        }

    }

}
