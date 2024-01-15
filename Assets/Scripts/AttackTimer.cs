using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTimer : MonoBehaviour
{
    [SerializeField] protected Player player;
    [SerializeField] protected Image image;
    protected float timer;
    protected bool inProgress = false;

    private void OnEnable()
    {
        timer = player.attackSpeed;
        player.playerAttack += StartTimer;
    }

    private void OnDisable()
    {
        player.playerAttack -= StartTimer;
    }

    protected virtual void StartTimer()
    {
        StartCoroutine(Timer(timer));
    }

    protected IEnumerator Timer(float timer)
    {
        image.fillAmount = 1f;
        float time = 0f;
        inProgress = true;
        while (image.fillAmount > 0f)
        {
            image.fillAmount = Mathf.Lerp(1f, 0f, time);
            time += (1/timer) * Time.deltaTime;
            yield return null;
        }
        inProgress = false;
        AfterTimer();
        yield return null;
    }

    protected virtual void AfterTimer()
    {
        return;
    }
}
