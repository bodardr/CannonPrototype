using System;
using Bodardr.Utility.Runtime;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterCameraController : MonoBehaviour
{
    private PlayerInput playerInput;

    private Vector3 offset = Vector3.zero;
    private Vector2 input;
    
    private CannonController cannonController;
    private Aim2D aim2D;

    private float currentRadius;
    private bool cannonCharging;
    
    [SerializeField]
    private Vector2 initialOffset = Vector2.zero;

    [SerializeField]
    private Transform cameraTarget;

    [SerializeField]
    private float chargingRadius = 3;

    [Range(0, 5)]
    [SerializeField]
    private float panningTime = 1;

    private void Start()
    {
        aim2D = GetComponent<Aim2D>();
        cannonController = GetComponent<CannonController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnLook(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    private void OnCannonPress()
    {
        if (!cannonController.enabled)
            return;
        
        DOTween.To(() => currentRadius, val => currentRadius = val, chargingRadius, panningTime).SetEase(Ease.OutQuad);
    }

    private void OnCannonRelease()
    {
        if (!cannonController.enabled)
            return;

        DOTween.To(() => currentRadius, val => currentRadius = val, 0, panningTime).SetEase(Ease.OutQuad);
    }

    private void Update()
    {
        cameraTarget.position = (Vector2)transform.position + initialOffset - (aim2D.Delta.normalized * currentRadius);
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(Application.isPlaying ? transform.TransformPoint(initialOffset) : cameraTarget.position,
            Vector3.forward, chargingRadius, 1);
    }
}