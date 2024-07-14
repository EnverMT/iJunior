using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class PlayerAnimationHandler : BaseAnimationHandler
{
    private PlayerMover _mover;

    protected override void Awake()
    {
        base.Awake();

        _mover = GetComponent<PlayerMover>();
    }

    protected override void Update()
    {
        base.Update();

        Animator.SetFloat(Params.Jump.VerticalSpeed, Rbody.velocity.y);
        Animator.SetBool(Params.Jump.OnGround, _mover.OnGround);
    }

    protected override float GetAxis()
    {
        return Input.GetAxisRaw(Params.Axis.Horizontal);
    }
}
