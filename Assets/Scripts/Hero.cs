using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hero : MonoBehaviour {

    [HideInInspector]public Vector2 targetPos = new Vector2(1,1);

    private new Rigidbody2D rigidbody;

    public int smoothing = 1;

    public float moveTime = 0.1f;

    public int hp = 100;

    public double restTime = 0.05;

    private double restTimer = 0;

    private new BoxCollider2D collider;

    public Animator animator;

    private static Hero _instance;

    public static Hero Instance{
        get{
            return _instance;
        }
    }

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    


    // Use this for initialization
    void Start () {

        rigidbody = GetComponent<Rigidbody2D>();

        collider = GetComponent<BoxCollider2D>();

        animator = GetComponent<Animator>();

	}

    // Update is called once per frame
    void Update() {

        rigidbody.MovePosition(targetPos);

        if (GameMgr.Instance.foodNum <= 0)
        {
            //Debug.Log("hero cant move, isEnd:" + GameMgr.Instance.isEnd);
            return;
        }

        restTimer += Time.deltaTime;

        if (restTimer < restTime)
        {
            return;
        }


        float h = Input.GetAxisRaw("Horizontal");

        float v = Input.GetAxisRaw("Vertical");

        Debug.Log("Horizontal: " + h + " Vertical: " + v);

        if (h != 0)
        {
            v = 0;
        }

        if (h != 0 || v != 0)
        {
            GameMgr.Instance.ReduceFood(1);

            RaycastHit2D hit = CheckCollision(targetPos, targetPos + new Vector2(h / 5, v / 5));

            if (hit.transform == null) // no obstacles
            {
                targetPos += new Vector2(h / 5, v / 5);
                //targetPos = Vector3.MoveTowards(rigidbody.position, targetPos + new Vector2(h, v), 1f / moveTime * Time.deltaTime);
                SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
            }
            else
            {
                //Debug.Log("hero collide entity tag: " + hit.collider.tag);
                switch (hit.collider.tag)
                {
                    case "OutWall":
                        break;
                    case "Wall":
                        animator.SetTrigger("Attack");
                        hit.collider.SendMessage("TakeDamage");
                        break;
                    case "Food":
                        GameMgr.Instance.AddFood(10);
                        Restore(10);
                        Destroy(hit.transform.gameObject);
                        SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
                        break;
                    case "Soda":
                        GameMgr.Instance.AddFood(20);
                        Restore(20);
                        Destroy(hit.transform.gameObject);
                        SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
                        break;
                    case "Enemy":
                        animator.SetTrigger("Damage");
                        break;
                    case "Exit":
                        Debug.Log("hero collide exit!");
                        //GameMgr.Instance.isEnd = true;
                        GameMgr.Instance.ReStartGame();
                        break;

                }
            }

            GameMgr.Instance.OnHeroMove();

            restTimer = 0;

        }

    }

    public void AttackLogic()
    {
        if (Input.GetKeyDown("x"))
        {
            animator.SetTrigger("Attack");
        }
    }

    public void Damage(int points)
    {
        animator.SetTrigger("Damage");
        hp -= points;
    }

    public void Restore(int points)
    {
        hp += points;
    }

    public RaycastHit2D CheckCollision(Vector2 pos_1, Vector2 pos_2)
    {
        collider.enabled = false;

        RaycastHit2D hit = Physics2D.Linecast(pos_1, pos_2);

        collider.enabled = true;

        return hit;
    }

}
