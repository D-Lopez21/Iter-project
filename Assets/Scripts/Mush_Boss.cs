using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mush_Boss : Mushroom
{

    public GameObject itemDrops;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void UpdateEnemyStates()
    {
        base.UpdateEnemyStates();

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Mush_walk:
                if(PlayerController.Instance.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector2(-2, transform.localScale.y);
                }else{
                    transform.localScale = new Vector2(2, transform.localScale.y);
                }
                break;
        }
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);

        if(health<=0)
        {
            DropItem();
        }
    }

    protected override void Death(float _destroyTime)
    {
        base.Death(_destroyTime);
    }

    protected override void ChangeCurrentAnimation()
    {
        base.ChangeCurrentAnimation();
    }

    private void DropItem()
    {
        Instantiate(itemDrops, transform.position, Quaternion.identity);
    }
}
