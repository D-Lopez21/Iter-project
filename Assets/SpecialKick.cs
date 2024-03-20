using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialKick : MonoBehaviour
{
    public float damage;
    public float damageMultiplier;
    int tempDir;

    void Start()
    {
        if(PlayerController.Instance.pState.lookingRight){
            tempDir = 1;
        }else{
            tempDir = -1;
        }
        damage = PlayerController.Instance.damage + 3f;
        damageMultiplier = PlayerController.Instance.damageMultiplier + 1f;

        PlayerController.Instance.pState.specialActive = true;
        PlayerController.Instance.rb.gravityScale = 0;
        PlayerController.Instance.rb.velocity = new Vector2(18f * tempDir, -37f);
    }

    private void OnTriggerEnter2D(Collider2D collider){
        {
        if(collider.gameObject.layer == 8){
            collider.gameObject.GetComponent<Enemy>().EnemyHit((damage * damageMultiplier), (PlayerController.Instance.transform.position - collider.gameObject.transform.position).normalized, 10);
            StartCoroutine(StopKick());

        }else if(collider.gameObject.layer == 3){
            StartCoroutine(StopKick());
        }

        }
    }

    IEnumerator StopKick(){
        PlayerController.Instance.rb.velocity = new Vector2(-15f * tempDir, 20f);
        PlayerController.Instance.rb.gravityScale = PlayerController.Instance.gravity;
        yield return new WaitForSeconds(0.1f);
        PlayerController.Instance.pState.specialActive = false;
        Destroy(gameObject);
    }

}
