using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Eye : Enemy
{

    [SerializeField] public float chaseDistance;

    float timer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Eye_idle);
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        switch (currentEnemyState)
        {
            case EnemyStates.Eye_idle:

                if(_dist<chaseDistance)
                {
                    ChangeState(EnemyStates.Eye_chase);
                }

                break;
            
            case EnemyStates.Eye_chase:

                rb.MovePosition(Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, Time.deltaTime*speed));
                Flip_Eye();
                break;

            case EnemyStates.Eye_stunned:

                ChangeState(EnemyStates.Eye_idle);
                break;

            case EnemyStates.Eye_die:
                Death(5);
                break;
        }
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);

        if(health > 0)
        {
            ChangeState(EnemyStates.Eye_stunned);
        }else{
            ChangeState(EnemyStates.Eye_die);
        }
    }

    void Flip_Eye()
    {
        if(PlayerController.Instance.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }else{
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }

    protected override void Death(float _destroyTime)
    {
        rb.gravityScale = 12;
        base.Death(_destroyTime);
    }

    protected override void ChangeCurrentAnimation()
    {
        anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Eye_idle);
        anim.SetBool("Idle", GetCurrentEnemyState == EnemyStates.Eye_chase);
        anim.SetBool("Hit", GetCurrentEnemyState == EnemyStates.Eye_stunned);

        if(GetCurrentEnemyState == EnemyStates.Eye_die)
        {
            anim.SetTrigger("Die");
        }
    }
}
