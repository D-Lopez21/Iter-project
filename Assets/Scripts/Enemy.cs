using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] protected PlayerController player;
    [SerializeField] protected float speed;

    [SerializeField] protected float damage;

    private bool alredyDead = false;

    protected float recoilTimer;
    protected Rigidbody2D rb;
    protected Animator anim;

    protected enum EnemyStates
    {
        //Mushroom
        Mush_idle,
        Mush_walk,
        Mush_hit,
        Mush_die,
    }
    protected EnemyStates currentEnemyState;

    protected virtual EnemyStates GetCurrentEnemyState
    {
        get{return currentEnemyState;}
        set
        {
            if(currentEnemyState != value)
            {
                currentEnemyState = value;
                ChangeCurrentAnimation();
            }
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        //if(health <= 0){
        //    Destroy(gameObject);
        //}
        if(isRecoiling){
            if(recoilTimer < recoilLength){
                recoilTimer += Time.deltaTime;

            }else{
                isRecoiling = false;
                recoilTimer = 0;
            }
        }else{
            UpdateEnemyStates();
        }
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce){
        health -= _damageDone;
        if(!isRecoiling){
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
        }

    }

    protected void OnTriggerStay2D(Collider2D _other)
    {
        if(_other.CompareTag("Player") && !PlayerController.Instance.pState.invincible && health > 0)
        {
            Attack();
        }
    }

    protected virtual void Death(float _destroyTime)
    {
        if(!alredyDead){
            PlayerController.Instance.Exp += 20;
            alredyDead = true;
        }
        Destroy(gameObject, _destroyTime);
    }

    protected virtual void UpdateEnemyStates()
    {

    }

    protected virtual void ChangeCurrentAnimation()
    {

    }

    protected void ChangeState(EnemyStates _newState)
    {
        GetCurrentEnemyState = _newState;
    }

    protected virtual void Attack()
    {
        PlayerController.Instance.TakeDamage(damage);
    }
}
