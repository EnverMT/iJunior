using BehaviorTree;
using UnityEngine;


public class TaskAttack : Node
{
    private readonly Animator _animator;

    private Rigidbody2D _rb;
    private CapsuleCollider2D _lastTarget;
    private BaseUnit _targetUnit;

    private float _attackTime;
    private float _attackCounter = 0f;
    private float _attackDamage;

    public TaskAttack(Rigidbody2D rigidbody2D, Animator animator, float attackTime, float attackDamage)
    {
        _rb = rigidbody2D;
        _animator = animator;
        _attackTime = attackTime;
        _attackDamage = attackDamage;
    }

    public override NodeState Evaluate()
    {
        CapsuleCollider2D target = GetData(Data.TARGET) as CapsuleCollider2D;

        if (target != _lastTarget)
        {
            _lastTarget = target;
            _targetUnit = target.GetComponent<BaseUnit>();
        }

        _attackCounter += Time.deltaTime;
        _rb.velocity = Vector3.zero;

        if (_attackCounter > _attackTime)
        {
            bool isTargetDead = _targetUnit.TakeDamage(_attackDamage);
            _animator.SetTrigger(Params.Attack.Attacking);

            if (isTargetDead)
                ClearData(Data.TARGET);
            else
                _attackCounter = 0f;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
