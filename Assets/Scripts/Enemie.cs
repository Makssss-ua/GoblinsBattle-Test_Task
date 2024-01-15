using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public abstract class Enemie : MonoBehaviour
{
    public Health hp { get; protected set; }

    [SerializeField] protected float Damage;
    [SerializeField] protected float AtackSpeed;
    [SerializeField] protected float AttackRange = 2;

    [SerializeField] protected Animator AnimatorController;
    [SerializeField] protected NavMeshAgent Agent;
    [SerializeField] protected Collider enemieCollider;

    [SerializeField] protected float healthReward = 10f;

    protected float lastAttackTime = 0;
    public bool isDead { get; protected set; } = false;
    protected Player Player;

    protected virtual void Awake()
    {
        hp = GetComponent<Health>();
    }

    protected virtual void LateUpdate()
    {
        if(Player == null && SceneManager.Instance.Player)
        {
            Player = SceneManager.Instance.Player;
        }

        if(isDead)
        {
            return;
        }

        if (hp.health <= hp.minHealth)
        {
            Die();
            return;
        }

        if(Player == null)
        {
            return;
        }

        AnimatorController.SetFloat("Speed", Agent.velocity.normalized.magnitude * Agent.speed);

        if (Player.hp.health <= Player.hp.minHealth)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, Player.transform.position);
     
        if (distance <= AttackRange)
        {
            Agent.isStopped = true;
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
                Player.hp.ChangeHealth(-Damage);
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.isStopped = false;
            Agent.SetDestination(Player.transform.position);
        }
    }

    protected virtual void Die()
    {
        Agent.isStopped = true;
        isDead = true;
        enemieCollider.enabled = false;
        SceneManager.Instance.RemoveEnemie(this);
        AnimatorController.SetTrigger("Die");
        if(Player.hp.health > Player.hp.minHealth)
            Player.hp.ChangeHealth(healthReward);
        this.enabled = false;
    }
}
