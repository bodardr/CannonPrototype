using UnityEngine;

public class Parallax2D : MonoBehaviour
{
    private Transform mainCam;
    
    private Vector3 initialCamPos;
    private Vector3 initialPos;

    [Range(0,2)]
    [SerializeField]
    private float parallaxSpeed = 1;

    private void Awake()
    {
        var main = Camera.main;
        mainCam = main ? main.transform : Camera.current.transform;
        initialCamPos = mainCam.position;
        initialPos = transform.position;
    }

    private void Update()
    {
        transform.position = initialPos + (mainCam.position - initialCamPos) * parallaxSpeed;
    }
}