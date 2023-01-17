using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatform2D : MonoBehaviour
{
    private const float ATTACH_DIST = 0.1f;
    private const float MIN_NORMAL_Y = -0.65f;

    private List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();
    private Rigidbody2D rb;

    private RaycastHit2D[] results = new RaycastHit2D[15];

    private Vector2 lastPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        lastPos = rb.position;
    }

    private void FixedUpdate()
    {
        var delta = rb.position - lastPos;

        var num = rb.Cast(transform.up, results, ATTACH_DIST);

        for (int i = 0; i < num; i++)
        {
            var hit = results[i];

            var otherRb = hit.rigidbody;
            
            if (otherRb && otherRb.bodyType != RigidbodyType2D.Static && hit.normal.y < MIN_NORMAL_Y)
                otherRb.position += delta;
        }

        lastPos = rb.position;
    }
}