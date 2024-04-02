using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye_Boss : MonoBehaviour
{
    public float health;
    [SerializeField] public float chaseDistance;
    public GameObject itemDrops;
    private bool dropped = false;
    private Animator anim;
    [SerializeField] public PlayerController player;
    public Rigidbody2D rb;
    private bool lookRigth = true;
    private bool alredyDead = false;
    public Transform objetivo;

    [Header("Zone")]
    [SerializeField] public float playerXL;
    [SerializeField] public float playerXR;
    [SerializeField] public float playerYU;
    [SerializeField] public float playerYD;
    [SerializeField] public bool zone = false;

    [Header("Shoot")]
    [SerializeField] public Transform shootController;
    [SerializeField] public GameObject proyectil;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if((playerXL < PlayerController.Instance.transform.position.x && PlayerController.Instance.transform.position.x < playerXR) && (playerYD < PlayerController.Instance.transform.position.y && PlayerController.Instance.transform.position.y < playerYU))
        {
            zone = true;
        }
        lookPlayer();

        float radianes = Mathf.Atan2(objetivo.position.y - shootController.position.y, objetivo.position.x - shootController.position.x);
        float grados = (180/Mathf.PI) * radianes;
        shootController.rotation = Quaternion.Euler(0, 0, grados);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health<=0)
        {
            anim.SetTrigger("Die");
            Death(5);
            if(!dropped)
            {
                DropItem();
                dropped = true;
            }
        }
    }

    private void Death(float _destroyTime)
    {
        rb.gravityScale = 12;
        if(!alredyDead){
            PlayerController.Instance.Exp += 20;
            alredyDead = true;
        }
        Destroy(gameObject, _destroyTime);
    }

    public void Attack()
    {
        Shoot();
    }

    private void Shoot()
    {
        Instantiate(proyectil, shootController.position, shootController.rotation);
    }

    private void DropItem()
    {
        Instantiate(itemDrops, transform.position, Quaternion.identity);
    }

    public void lookPlayer()
    {
        if((PlayerController.Instance.transform.position.x > transform.position.x && !lookRigth) || (PlayerController.Instance.transform.position.x < transform.position.x && lookRigth))
        {
            lookRigth = !lookRigth;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }
}
