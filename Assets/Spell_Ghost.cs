using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Ghost : MonoBehaviour
{
    [SerializeField] public float damage;
    [SerializeField] public Vector2 boxDimensions;
    [SerializeField] public Transform boxPosition;
    [SerializeField] public float lifeTime;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Punch()
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(boxPosition.position, boxDimensions, 0f);
        foreach (Collider2D colision in objectsToHit)
        {
            if(colision.CompareTag("Player"))
            {
                PlayerController.Instance.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxPosition.position, boxDimensions);
    }
}
