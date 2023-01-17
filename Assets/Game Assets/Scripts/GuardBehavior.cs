using System;
using System.Collections;
using UnityEngine;

public class GuardBehavior : MonoBehaviour
{
    private Vector2 initialPosition = Vector2.zero;

    private new Collider2D collider2D;

    private PhysicsController2D physicsController2D;

    private Rigidbody2D rb;

    [Header("Patrol Details")]
    [SerializeField]
    private float patrolA = -4;

    [SerializeField]
    private float patrolB = 4;

    [SerializeField]
    private float waitDuration = 4;

    private void OnValidate()
    {
        if (!rb)
            rb = GetComponent<Rigidbody2D>();
        
        initialPosition = rb.position;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        physicsController2D.RegainControl();
    }

    private void Start()
    {
        physicsController2D = GetComponent<PhysicsController2D>();
        rb = GetComponent<Rigidbody2D>();

        initialPosition = rb.position;
        StartCoroutine(GuardCoroutine());
    }

    private IEnumerator GuardCoroutine()
    {
        var wait = new WaitForSeconds(waitDuration);

        while (isActiveAndEnabled)
        {
            yield return new WaitFor2DControllerArrival(physicsController2D, initialPosition + Vector2.right * patrolA);
            yield return wait;
            yield return new WaitFor2DControllerArrival(physicsController2D, initialPosition + Vector2.right * patrolB);
            yield return wait;
        }
    }

    private void OnDrawGizmos()
    {
        if (!collider2D)
            collider2D = GetComponent<Collider2D>();

        if (initialPosition == Vector2.zero)
            initialPosition = transform.position;

        var center = collider2D.bounds.center.y;

        Gizmos.color = new Color(0.36f, 0f, 1f);
        Gizmos.DrawLine(new Vector2(initialPosition.x + patrolA, center),
            new Vector2(initialPosition.x + patrolB, center));
    }
}