using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpell : MonoBehaviour
{
    public float Speed = 9.5f;
    public float damage;
    public float damageMultiplier;

    void Start()
    {
        damage = PlayerController.Instance.damage + 3;
        damageMultiplier = PlayerController.Instance.damageMultiplier + 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerController.Instance.pState.inventoryActive){
            transform.position += transform.right * Time.deltaTime * Speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 8){
            collider.gameObject.GetComponent<Enemy>().EnemyHit((damage * damageMultiplier), (PlayerController.Instance.transform.position - collider.gameObject.transform.position).normalized, 10);
            Destroy(gameObject, 0.1f);

        }else if(collider.gameObject.layer == 3){
            Destroy(gameObject);
        }

    }
}
