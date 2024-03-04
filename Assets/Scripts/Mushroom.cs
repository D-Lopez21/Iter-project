using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    
    [SerializeField] public float playerXL;
    [SerializeField] public float playerXR;
    [SerializeField] public float playerYU;
    [SerializeField] public float playerYD;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
        ChangeState(EnemyStates.Mush_idle);
    }

    protected override void UpdateEnemyStates()
    {
        switch (GetCurrentEnemyState)
        {
            case EnemyStates.Mush_idle:
                if(!isRecoiling && (playerXL < PlayerController.Instance.transform.position.x && PlayerController.Instance.transform.position.x < playerXR) && (playerYD < PlayerController.Instance.transform.position.y && PlayerController.Instance.transform.position.y < playerYU))
                {
                    ChangeState(EnemyStates.Mush_walk);
                }
                break;
            
            case EnemyStates.Mush_walk:

                if(!(!isRecoiling && (playerXL < PlayerController.Instance.transform.position.x && PlayerController.Instance.transform.position.x < playerXR) && (playerYD < PlayerController.Instance.transform.position.y && PlayerController.Instance.transform.position.y < playerYU)))
                {
                    ChangeState(EnemyStates.Mush_idle);
                }                

                if(PlayerController.Instance.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector2(-1, transform.localScale.y);
                }else{
                    transform.localScale = new Vector2(1, transform.localScale.y);
                }

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                
                break;
            
            case EnemyStates.Mush_hit:
                ChangeState(EnemyStates.Mush_walk);
                break;
            
            case EnemyStates.Mush_die:
                Death(5);
                break;
        }
    }
        
    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);

        if(health > 0)
        {
            ChangeState(EnemyStates.Mush_hit);
        }else{
            ChangeState(EnemyStates.Mush_die);
        }
    }

    protected override void Death(float _destroyTime)
    {
        base.Death(_destroyTime);
    }

    protected override void ChangeCurrentAnimation()
    {
        anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Mush_idle);
        anim.SetBool("Walking", GetCurrentEnemyState == EnemyStates.Mush_walk);
        anim.SetBool("TakeHit", GetCurrentEnemyState == EnemyStates.Mush_hit);

        if(GetCurrentEnemyState == EnemyStates.Mush_die)
        {
            anim.SetTrigger("Die");
        }
    }
    
}
