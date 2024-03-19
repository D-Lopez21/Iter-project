using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float Speed = 9.5f;
    public float damage;
    public float damageMultiplier;

    private void Start(){
        damage = PlayerController.Instance.damage;
        damageMultiplier = PlayerController.Instance.damageMultiplier;
    }

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer != 6){
            if(collision.gameObject.layer == 8){
                collision.gameObject.GetComponent<Enemy>().EnemyHit((damage * damageMultiplier), (PlayerController.Instance.transform.position - collision.gameObject.transform.position).normalized, 10);
            }

            Destroy(gameObject);
        }

    }

}
