using Bodardr.Utility.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetUpdater : MonoBehaviour
{
    private static readonly int facingRight = Animator.StringToHash("FacingRight");

    private Vector3 pos = Vector3.zero;
    private Vector3 playerPos;
    
    private Aim2D aim2D;

    [SerializeField]
    Transform target;

    [SerializeField]
    private float zOffset = -2;


    [SerializeField]
    private float controllerRadius;

    public Transform Target
    {
        get => target;
        private set => target = value;
    }

    private void Start()
    {
        aim2D = GetComponentInParent<Aim2D>();
    }

    void Update()
    {
        if (aim2D.IsGamepad)
            pos = transform.position + (Vector3)(aim2D.Delta + aim2D.AimOffset) * controllerRadius;
        else
            pos = aim2D.MouseWorldPos;

        pos.z = zOffset;
        Target.position = pos;
    }
}