using Bodardr.Utility.Runtime;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private static readonly int forwardVelocity = Animator.StringToHash("forwardVelocity");
    private static readonly int yVelocity = Animator.StringToHash("yVelocity");
    private static readonly int rotationYParam = Animator.StringToHash("yRotation");

    private static readonly int grounded = Animator.StringToHash("grounded");
    private static readonly int jump = Animator.StringToHash("jump");

    private Animator anim;
    private PhysicsSolver2D physicsSolver;
    private PhysicsController2D physicsController;

    private Aim2D aim2D;

    [SerializeField]
    private Transform charTransform;

    private void Start()
    {
        aim2D = GetComponentInParent<Aim2D>();
        physicsSolver = GetComponentInParent<PhysicsSolver2D>();
        physicsController = GetComponentInParent<PhysicsController2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        var rotationY = Mathf.Lerp(-90, 90, MathExtensions.Remap(aim2D.Delta.x,-0.1f, 0.1f, 0, 1));
        var vel = physicsController.Velocity;
        var forwardVel =
            Vector2.Dot(vel, physicsController.PhysicsSolver.GroundNormal.ToBinormal()) *
            Mathf.Sign(aim2D.Delta.x);

        charTransform.transform.localRotation = Quaternion.Euler(0, rotationY, 0);

        anim.SetFloat(forwardVelocity, forwardVel);
        anim.SetFloat(yVelocity, vel.y);
        anim.SetFloat(rotationYParam, rotationY);
        anim.SetBool(grounded, physicsSolver.Grounded);
    }

    void OnJumpEnter()
    {
        anim.SetTrigger(jump);
    }
}