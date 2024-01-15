using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperAttackTimer : AttackTimer
{
    [SerializeField] private EnemiFinder enemiFinder;
    private float attackTimer;

    private void OnEnable()
    {
        attackTimer = player.attackSpeed;
        timer = player.superSpeed;

        player.playerSuperAttack += StartTimer;
        player.playerAttack += StartAttackTimer;

        enemiFinder.enemieInRange += ChangeIconStatus;
        ChangeIconStatus(enemiFinder.enemiesInRange);
    }

    private void OnDisable()
    {
        player.playerSuperAttack -= StartTimer;
        player.playerAttack -= StartAttackTimer;

        enemiFinder.enemieInRange -= ChangeIconStatus;
    }

    private void ChangeIconStatus(bool value)
    {
        switch (value)
        {
            case true:
                if(!inProgress)
                    image.fillAmount = 0f;
                break;
            case false:
                if (!inProgress)
                    image.fillAmount = 1f;
                break;
        }
    }

    protected override void StartTimer()
    {
        StartCoroutine(Timer(timer));
    }

    private void StartAttackTimer()
    {
        StartCoroutine(Timer(attackTimer));
    }

    protected override void AfterTimer()
    {
        ChangeIconStatus(enemiFinder.enemiesInRange);
    }
}
