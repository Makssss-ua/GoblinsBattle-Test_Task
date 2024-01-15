using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiFinder : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    public bool enemiesInRange = false;
    private bool inRange = false;

    public Action<bool> enemieInRange;

    private void OnEnable()
    {
        enemieInRange?.Invoke(enemiesInRange);
        StartCoroutine(FindEnemies());
    }

    IEnumerator FindEnemies()
    {
        inRange = false;
        sphereCollider.enabled = true;
        yield return new WaitForFixedUpdate();
        sphereCollider.enabled = false;
        if (enemiesInRange != inRange)
        {
            enemiesInRange = inRange;
            enemieInRange?.Invoke(enemiesInRange);
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FindEnemies());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemie>(out Enemie enemie))
        {
            if (enemie.isDead)
                return;
            inRange = true;
        }
    }
}
