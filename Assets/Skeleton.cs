using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{

    private Animator anim;
    public Rigidbody2D rb;
    public PlayerController player;
    private bool lookRigth = true;
    private bool alredyDead = false;

    [Header("Health")]
    [SerializeField] public float health;

    [Header("Damage")]
    [SerializeField] public Transform attackControl1;
    [SerializeField] Vector2 attackArea1;
    [SerializeField] public float damage1;
    [SerializeField] public Transform attackControl2;
    [SerializeField] public float radioAttack2;
    [SerializeField] public float damage2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        anim.SetFloat("Distance", distance);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health<=0)
        {
            anim.SetTrigger("Death");
            Death(5);
        }
    }

    private void Death(float _destroyTime)
    {
        if(!alredyDead){
            PlayerController.Instance.Exp += 20;
            alredyDead = true;
        }
        Destroy(gameObject, _destroyTime);
    }

    public void lookPlayer()
    {
        if((PlayerController.Instance.transform.position.x > transform.position.x && !lookRigth) || (PlayerController.Instance.transform.position.x < transform.position.x && lookRigth))
        {
            lookRigth = !lookRigth;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }

    public void Attack1()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(attackControl1.position, attackArea1, 0);

        foreach (Collider2D colision in objectsToHit)
        {
            if(colision.CompareTag("Player"))
            {
                PlayerController.Instance.TakeDamage(damage1);
            }
        }
    }

    public void Attack2()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(attackControl2.position, radioAttack2);
        foreach(Collider2D colision in objetos)
        {
            if(colision.CompareTag("Player"))
            {
                PlayerController.Instance.TakeDamage(damage2);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackControl1.position, attackArea1);
        Gizmos.DrawWireSphere(attackControl2.position, radioAttack2);
    }
}