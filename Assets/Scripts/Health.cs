using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health { get; private set; }

    [SerializeField] private float startHealth = 100f;
    public float maxHealth = 100f;
    public float minHealth = 0f;
    public Action<float> OnUpdateHealth;

    private void Awake()
    {
        health = Mathf.Clamp(startHealth, minHealth, maxHealth);
    }

    public void ChangeHealth(float value)
    {
        health += value;
        health = Mathf.Clamp(health, minHealth, maxHealth);
        OnUpdateHealth?.Invoke(health);
        Debug.Log($"{gameObject.name} changed health on {value}");
    }
}
