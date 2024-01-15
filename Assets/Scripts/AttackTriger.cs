using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriger : MonoBehaviour
{
    [SerializeField] private SphereCollider triggerCollider;

    private Enemie closeEnemie;
    private float enemieDistance = Mathf.Infinity;

    public IEnumerator Attack(float range, float damage, Vector2 attackTimeRange)
    {
        closeEnemie = null;
        enemieDistance = Mathf.Infinity;
        yield return new WaitForSeconds(attackTimeRange.x);

        triggerCollider.radius = range;
        triggerCollider.enabled = true;
        yield return new WaitForSeconds(Mathf.Abs(attackTimeRange.y - attackTimeRange.x));
        if (closeEnemie)
        {
            if(enemieDistance <= range)
                closeEnemie.hp.ChangeHealth(-damage);
        }
        triggerCollider.enabled = false;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Enemie>(out Enemie enemie))
        {
            if (enemie.isDead)
                return;
            float distance = Vector3.Distance(transform.position, enemie.transform.position);

            if (distance < enemieDistance)
            {
                closeEnemie = enemie;
                enemieDistance = distance;
            }
        }
    }
}
