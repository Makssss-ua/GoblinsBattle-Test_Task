using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigEnemie : Enemie
{
    [Space(5f)]
    [Header("Sprawl")]
    [SerializeField] protected Enemie enemie;
    [SerializeField] protected int count = 2;
    [SerializeField] protected float spawnRadius = 5f;

    protected override void Die()
    {
        Agent.isStopped = true;
        isDead = true;
        enemieCollider.enabled = false;
        AnimatorController.SetTrigger("Die");
        if (Player.hp.health > Player.hp.minHealth)
            Player.hp.ChangeHealth(healthReward);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));
            SceneManager.Instance.SpawnEnemie(enemie, transform.position + pos);
        }

        SceneManager.Instance.RemoveEnemie(this);
    }
}
