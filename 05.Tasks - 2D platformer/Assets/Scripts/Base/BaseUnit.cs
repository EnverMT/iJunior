﻿using UnityEngine;


[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Attack))]
public abstract class BaseUnit : MonoBehaviour
{
    [HideInInspector] public Health Health;
    [HideInInspector] public Attack Attack;


    protected virtual void Awake()
    {
        Health = GetComponent<Health>();
        Attack = GetComponent<Attack>();
    }

    protected virtual void OnEnable()
    {
        Health.Died += OnDie;
    }

    protected virtual void OnDisable()
    {
        if (Health != null)
            Health.Died -= OnDie;
    }

    protected virtual void OnDie(IAttribute<float> health)
    {
        Health.Died -= OnDie;
        Destroy(Health.gameObject);
    }
}
