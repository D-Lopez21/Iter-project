using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGoblin : MonoBehaviour
{
    public float speed;
    public int damage;
    public PlayerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
            PlayerController.Instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
