using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mush_Boss : Enemy
{

    [Header("Zone")]
    [SerializeField] public float playerXL;
    [SerializeField] public float playerXR;
    [SerializeField] public float playerYU;
    [SerializeField] public float playerYD;

    [Header("Attack")]
    [SerializeField] public Transform attackcontrol;
    [SerializeField] public float radioAttack;
    [SerializeField] public float damage2;

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
            //Idle
            case EnemyStates.MushB_idle:
                if(!isRecoiling && (playerXL < PlayerController.Instance.transform.position.x && PlayerController.Instance.transform.position.x < playerXR) && (playerYD < PlayerController.Instance.transform.position.y && PlayerController.Instance.transform.position.y < playerYU))
                {
                    ChangeState(EnemyStates.MushB_walk);
                }
                break;
            
            //Caminar
            case EnemyStates.MushB_walk:

                float distanceP = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

                if(distanceP < 5)
                {
                    ChangeState(EnemyStates.MushB_Attack);
                }

                if(!(!isRecoiling && (playerXL < PlayerController.Instance.transform.position.x && PlayerController.Instance.transform.position.x < playerXR) && (playerYD < PlayerController.Instance.transform.position.y && PlayerController.Instance.transform.position.y < playerYU)))
                {
                    ChangeState(EnemyStates.MushB_idle);
                }                

                lookatplayer();

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
                
                break;
            
            //Ataque
            case EnemyStates.MushB_Attack:
                ChangeState(EnemyStates.MushB_idle);
                break;

            //Corrida
            case EnemyStates.MushB_run:
                
                break;

            //Take hit
            case EnemyStates.MushB_hit:
                ChangeState(EnemyStates.MushB_idle);
                break;

            //Muerte
            case EnemyStates.MushB_die:
                Death(5);
                break;
        }
    }

    public void lookatplayer()
    {
        if(PlayerController.Instance.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(-2, transform.localScale.y);
        }else{
            transform.localScale = new Vector2(2, transform.localScale.y);
        }
    }

    public void AttackB()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(attackcontrol.position, radioAttack);
        foreach(Collider2D colision in objetos)
        {
            if(colision.CompareTag("Player"))
            {
                PlayerController.Instance.TakeDamage(damage2);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackcontrol.position, radioAttack);
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
        anim.SetBool("Attack", GetCurrentEnemyState == EnemyStates.MushB_Attack);

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
