using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Goblin : MonoBehaviour
{
    public float speed;
    public int damage;
    public PlayerController player;
    //private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.right);
        float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if(distance > 80)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.CompareTag("Player"))
        {
            //anim.SetTrigger("Boom");
            PlayerController.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
