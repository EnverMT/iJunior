using Assets.Scripts.Base;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Animator))]
public class Player : BaseUnit
{
    [SerializeField] private int _attackMouseButton = 0;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackDamage = 10f;

    private bool _attack = false;
    private Mover _mover;
    private Animator _animator;

    public override bool HasJumpAbility { get => true; }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _attack = Input.GetMouseButtonDown(_attackMouseButton);
    }

    private void FixedUpdate()
    {
        if (_attack)
        {
            _animator.SetTrigger(Params.Attack.Attacking);
            Enemy[] enemies = GetEnemies();

            if (enemies.Length > 0)
                Attack(enemies);
        }
    }

    private Enemy[] GetEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();
        RaycastHit2D[] hits = Physics2D.RaycastAll(gameObject.transform.position, _mover.Direction * _attackRange);

        foreach (RaycastHit2D hit in hits)
            if (hit.collider.gameObject.TryGetComponent(out Enemy enemy))
                enemies.Add(enemy);

        return enemies.ToArray();
    }

    private void Attack(Enemy[] enemies)
    {
        foreach (Enemy enemy in enemies)
            enemy.TakeDamage(_attackDamage);
    }
}
