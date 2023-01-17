using System;
using UnityEngine;

public class PhysicsController2DAnimator : MonoBehaviour
{
    private static readonly int forwardVelocity = Animator.StringToHash("forwardVelocity");
    
    private PhysicsController2D physicsController;
    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 lastPosition;
    
    [SerializeField]
    private float rotationSpeed = 10;

    private void Awake()
    {
        physicsController = GetComponentInParent<PhysicsController2D>();
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        lastPosition = rb.position;
    }

    private void FixedUpdate()
    {
        var vel = physicsController.Velocity;
        
        anim.SetFloat(forwardVelocity, Mathf.Abs(vel.x));
        
        var pos = rb.position;
        var delta = pos - lastPosition;

        if (Mathf.Abs(delta.x) > 0.01f)
        {
            var localRot = transform.localRotation;
            transform.localRotation = Quaternion.Lerp(localRot, Quaternion.Euler(0, delta.x > 0 ? 90 : -90, 0), rotationSpeed * Time.fixedDeltaTime);
        }

        lastPosition = pos;
    }
}
