using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Animator anim;
    public Rigidbody2D rb;
    public PlayerController player;
    private bool lookRigth = true;
    private bool alredyDead = false;
    private bool invin = false;
    public GameObject wall;

    [Header("Zone")]
    [SerializeField] public float playerXL;
    [SerializeField] public float playerXR;
    [SerializeField] public float playerYU;
    [SerializeField] public float playerYD;
    [SerializeField] public bool zone = false;

    [Header("Health")]
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public int fase;

    [Header("Damage")]
    [SerializeField] public Transform attackControl1;
    [SerializeField] public Vector2 attackArea1;
    [SerializeField] public float damage1;
    [SerializeField] public Transform attackControl2;
    [SerializeField] public float radioAttack2;
    [SerializeField] public float damage2;

    [Header("Shoot")]
    [SerializeField] public Transform shootController;
    [SerializeField] public GameObject slashGoblin;
    [SerializeField] public Transform bombController;
    [SerializeField] public GameObject bombGoblin;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        fase = 1;
        health = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        anim.SetFloat("Distance", distance);
        if((playerXL < PlayerController.Instance.transform.position.x && PlayerController.Instance.transform.position.x < playerXR) && (playerYD < PlayerController.Instance.transform.position.y && PlayerController.Instance.transform.position.y < playerYU))
        {
            zone = true;
            wall.transform.position = new Vector2(274.49f, 40.5f);
        }
    }

    public void TakeDamage(float damage)
    {
        if(!invin)
        {
            health -= damage;
            if(health<=0 && fase == 2)
            {
                anim.SetTrigger("Die");
                Death(5);
            }
            if(health<=0 && fase == 1)
            {
                anim.SetTrigger("Fall");
                health = maxHealth;
                fase = 2;
                StartCoroutine(StandUp());
            }
        }
        
    }

    IEnumerator StandUp()
    {
        invin = true;
        yield return new WaitForSeconds(3f);
        anim.SetTrigger("Up");
        invin = false;
    }

    private void Death(float _destroyTime)
    {
        Destroy(wall);
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
        Shoot();
        
        Collider2D[] objetos = Physics2D.OverlapCircleAll(attackControl2.position, radioAttack2);
        foreach(Collider2D colision in objetos)
        {
            if(colision.CompareTag("Player"))
            {
                PlayerController.Instance.TakeDamage(damage2);
            }
        }
    }

    public void Attack3()
    {
        Bomb();
    }

    private void Bomb()
    {
        Instantiate(bombGoblin, bombController.position, bombController.rotation);
    }

    private void Shoot()
    {
        Instantiate(slashGoblin, shootController.position, shootController.rotation);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackControl1.position, attackArea1);
        Gizmos.DrawWireSphere(attackControl2.position, radioAttack2);
    }
}
