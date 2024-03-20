using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritSlash : MonoBehaviour
{
    [SerializeField] Vector2 attackArea;
    [SerializeField] Transform attackTransform;
    [SerializeField] LayerMask attackableLayer;
    float damage;
    float damageMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        damage = PlayerController.Instance.damage + 0.2f;
        damageMultiplier = PlayerController.Instance.damageMultiplier + 0.3f;
        StartCoroutine(activeAttack());
        Destroy(gameObject, 1.2f);
    }

    IEnumerator activeAttack(){
        for(int i = 0; i < 3; i++){
            yield return new WaitForSeconds(0.33f);
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
                objectsToHit[i].GetComponent<Enemy>().EnemyHit(damage * damageMultiplier, (attackTransform.position - objectsToHit[i].transform.position).normalized, 10f);
            }
        }
    }


    private void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackTransform.position, attackArea);
    }
}
