using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBeam : MonoBehaviour
{

    [SerializeField] Vector2 attackArea;
    [SerializeField] Transform attackTransform;
    [SerializeField] LayerMask attackableLayer;
    float damage;
    float damageMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        damage = PlayerController.Instance.damage;
        damageMultiplier = PlayerController.Instance.damageMultiplier - 0.2f;
        PlayerController.Instance.rb.velocity = new Vector2(0, 0);
        PlayerController.Instance.pState.specialActive = true;

        StartCoroutine(activeAttack());
        Destroy(gameObject, 0.21f);
    }

    IEnumerator activeAttack(){
        while(true){
            yield return new WaitForSeconds(0.05f);
            checkHit();
            yield return null;
        }

    }


    void checkHit(){
        Collider2D[] objectsToHit;
        objectsToHit = Physics2D.OverlapBoxAll(attackTransform.position, attackArea, 0, attackableLayer);
        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<Enemy>() != null)
            {
                objectsToHit[i].GetComponent<Enemy>().EnemyHit(damage * damageMultiplier, (attackTransform.position - objectsToHit[i].transform.position).normalized, 10f * -1f);
            }
        }
    }
    
    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackTransform.position, attackArea);
    }
}
