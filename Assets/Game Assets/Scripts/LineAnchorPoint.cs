using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class LineAnchorPoint : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField]
    private Transform anchor;

    [SerializeField]
    private int index = 1;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!anchor)
            return;
        
        lineRenderer.SetPosition(index, transform.InverseTransformPoint(anchor.position));
    }
}