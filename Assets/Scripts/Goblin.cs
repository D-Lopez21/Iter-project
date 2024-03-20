using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Animator anim;
    public Rigidbody2D rb;
    public PlayerController player;
    private bool lookRigth = true;

    [Header("Health")]
    [SerializeField] public float health;
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        if(health<=0)
        {
            //anim.SetTrigger("Die");
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void lookPlayer()
    {
        if((PlayerController.Instance.transform.position.x > transform.position.x && !lookRigth) || (PlayerController.Instance.transform.position.x < transform.position.x && lookRigth))
        {
            lookRigth = !lookRigth;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
