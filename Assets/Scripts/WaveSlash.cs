using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSlash : MonoBehaviour
{
    public float Speed = 9.5f;
    public float damage;
    public float damageMultiplier;

    private void Start(){
        damage = PlayerController.Instance.damage;
        damageMultiplier = PlayerController.Instance.damageMultiplier + 0.3f;
        Destroy(gameObject, 0.5f);
    }

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 8){
            collider.gameObject.GetComponent<Enemy>().EnemyHit((damage * damageMultiplier), (PlayerController.Instance.transform.position - collider.gameObject.transform.position).normalized, 10);

        }else if(collider.gameObject.layer == 3){
            //Destroy(gameObject);
        }

    }

}
