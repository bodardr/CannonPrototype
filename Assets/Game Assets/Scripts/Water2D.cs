using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteAlways]
public class Water2D : MonoBehaviour
{
    private static readonly int clipHeight = Shader.PropertyToID("_TopmostPosition");
    private static readonly int transformUp = Shader.PropertyToID("_TransformUp");

    private MaterialPropertyBlock propertyBlock;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.GetPropertyBlock(propertyBlock);
    }

    private void Update()
    {
        if (propertyBlock == null || !meshRenderer)
            return;

        var bounds = meshRenderer.bounds;

        var pos = bounds.max;
        pos.x = (pos.x + bounds.min.x) / 2;

        propertyBlock.SetVector(clipHeight, pos);
        propertyBlock.SetVector(transformUp, transform.up);

        meshRenderer.SetPropertyBlock(propertyBlock);
    }
}