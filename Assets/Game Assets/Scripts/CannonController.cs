using System;
using System.Collections;
using Bodardr.UI;
using Bodardr.Utility.Runtime;
using DG.Tweening;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
    private static readonly int cannonPower = Shader.PropertyToID("_PowerValue");

    private Coroutine chargeCoroutine;
    private Material cannonMaterial;

    private TargetUpdater targetUpdater;
    private PlayerInput playerInput;

    private Aim2D aim2D;
    private Rigidbody2D rb;
    private PhysicsController2D physicsController2D;
    private Collider2D[] overlappingColliders;

    private bool pressed; 
    private bool isCharging;
    private ChargeState chargeStateVal;

    [SerializeField]
    private bool hasCharge;

    [SerializeField]
    private MeshRenderer cannonMeshRenderer;

    [SerializeField]
    private ChargeState unlockedLevel = ChargeState.Blue;

    [SerializeField]
    private Vector4 cannonForces;

    [SerializeField]
    private Vector4 cannonForcesSelf;

    [SerializeField]
    private float cannonRadius;

    [SerializeField]
    private Transform cannonEmissionPoint;

    [SerializeField]
    private Vector2 cannonAimOffset;

    [Header("UI")]
    [SerializeField]
    private Transform cannonArrowsUI;

    [SerializeField]
    private UIView cannonArrowView;

    [SerializeField]
    private Image cannonArrowImage;

    [SerializeField]
    private Color[] cannonArrowColors = new Color[4];

    public ChargeState ChargeStateVal
    {
        get => chargeStateVal;
        set
        {
            chargeStateVal = (ChargeState)Mathf.Min((int)value, (int)unlockedLevel);
            cannonMaterial.SetFloat(cannonPower, (float)value / 4f);
        }
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        targetUpdater = GetComponentInChildren<TargetUpdater>();
        physicsController2D = GetComponent<PhysicsController2D>();
        aim2D = GetComponent<Aim2D>();
        cannonMaterial = cannonMeshRenderer.material;
    }

    private void OnEnable()
    {
        cannonMeshRenderer.transform.parent.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        cannonMeshRenderer.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        cannonArrowsUI.rotation = Quaternion.Euler(0, 0, 180 + aim2D.Angle);
    }

    void OnCannonPress()
    {
        pressed = true;
        
        if (!enabled || !hasCharge)
            return;

        isCharging = true;
        chargeCoroutine = StartCoroutine(ChargeCoroutine());
        cannonArrowView.Show();
    }

    void OnCannonRelease()
    {
        pressed = false;

        if (!isCharging || !enabled || physicsController2D.PhysicsSolver.Grounded && !hasCharge)
            return;

        if (!physicsController2D.PhysicsSolver.Grounded)
            hasCharge = false;

        StopCoroutine(chargeCoroutine);
        isCharging = false;

        FireCannon();

        ChargeStateVal = ChargeState.Tap;
        cannonArrowView.Hide();
    }

    void OnGroundEnter()
    {
        var hadCharge = hasCharge;
        
        hasCharge = true;
        
        if(!hadCharge && pressed && enabled)
            OnCannonPress();
    }

    private IEnumerator ChargeCoroutine()
    {
        cannonArrowImage.color = Color.white;

        for (ChargeStateVal = 0; ChargeStateVal < unlockedLevel; ChargeStateVal++)
        {
            yield return new WaitForSeconds(0.6f);
            Debug.Log($"Wait Finished : {chargeStateVal}");
            cannonArrowImage.DOColor(cannonArrowColors[(int)ChargeStateVal], 0.2f).SetEase(Ease.OutSine);
        }
    }

    private void FireCannon()
    {
        var cannonDirection = -aim2D.Delta.normalized;

        overlappingColliders = new Collider2D[8];

        var amount = Physics2D.OverlapCircleNonAlloc(cannonEmissionPoint.position, cannonRadius, overlappingColliders);

        for (int i = 0; i < amount; i++)
        {
            var colRB = overlappingColliders[i].attachedRigidbody;

            if (colRB)
                colRB.AddForceAtPosition(-cannonDirection * cannonForces[(int)chargeStateVal],
                    cannonEmissionPoint.position, ForceMode2D.Impulse);
        }

        if (Vector2.Dot(physicsController2D.Velocity, cannonDirection) < 0)
            physicsController2D.Velocity = cannonDirection * cannonForcesSelf[(int)chargeStateVal];
        else
            physicsController2D.Velocity += cannonDirection * cannonForcesSelf[(int)chargeStateVal];
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.magenta;
        Handles.DrawWireDisc(cannonEmissionPoint.position, Vector3.back, cannonRadius);
    }

    public enum ChargeState
    {
        Tap = 0,
        Blue = 1,
        Yellow = 2,
        Red = 3
    }
}