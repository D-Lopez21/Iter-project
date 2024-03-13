using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mush_Boss : Enemy
{

    [SerializeField] public float playerXL;
    [SerializeField] public float playerXR;
    [SerializeField] public float playerYU;
    [SerializeField] public float playerYD;
    //public GameObject itemDrops;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb.gravityScale = 12f;
        ChangeState(EnemyStates.MushB_idle);
    }

    // Update is called once per frame
    protected override void UpdateEnemyStates()
    {

        switch (GetCurrentEnemyState)
        {
            case EnemyStates.MushB_idle:
                if(!isRecoiling && (playerXL < PlayerController.Instance.transform.position.x && PlayerController.Instance.transform.position.x < playerXR) && (playerYD < PlayerController.Instance.transform.position.y && PlayerController.Instance.transform.position.y < playerYU))
                {
                    ChangeState(EnemyStates.MushB_walk);
                }
                break;
            
            case EnemyStates.MushB_walk:

                if(!(!isRecoiling && (playerXL < PlayerController.Instance.transform.position.x && PlayerController.Instance.transform.position.x < playerXR) && (playerYD < PlayerController.Instance.transform.position.y && PlayerController.Instance.transform.position.y < playerYU)))
                {
                    ChangeState(EnemyStates.MushB_idle);
                }                

                if(PlayerController.Instance.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector2(-2, transform.localScale.y);
                }else{
                    transform.localScale = new Vector2(2, transform.localScale.y);
                }

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                
                break;

            case EnemyStates.MushB_hit:
                ChangeState(EnemyStates.MushB_walk);
                break;

            case EnemyStates.MushB_die:
                Death(5);
                break;
        }
    }

    //private void Run()
    //{
    //    float point = PlayerController.Instance.transform.position.x;
    //    while(transform.position.x != point)
    //    {
    //        transform.position = Vector2.MoveTowards(transform.position, new Vector2(point, transform.position.y), (speed + 3) * Time.deltaTime);
    //    }
    //}

//if(health<=0)
//{
//    DropItem();
//}

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);

        if(health > 0)
        {
            ChangeState(EnemyStates.MushB_hit);
        }else{
            ChangeState(EnemyStates.MushB_die);
        }
    }

    protected override void Death(float _destroyTime)
    {
        base.Death(_destroyTime);
    }

    protected override void ChangeCurrentAnimation()
    {
        anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.MushB_idle);
        anim.SetBool("Walk", GetCurrentEnemyState == EnemyStates.MushB_walk);
        anim.SetBool("Hit", GetCurrentEnemyState == EnemyStates.MushB_hit);

        if(GetCurrentEnemyState == EnemyStates.MushB_die)
        {
            anim.SetTrigger("Death");
        }
    }

    //private void DropItem()
    //{
    //    Instantiate(itemDrops, transform.position, Quaternion.identity);
    //}
}
