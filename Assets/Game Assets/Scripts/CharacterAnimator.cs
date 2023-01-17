// using DG.Tweening;
// using UnityEngine;
//
// public class CharacterAnimator : MonoBehaviour
// {
//     private PhysicsController2D physicsController2D;
//     private bool facingRight;
//     
//     [SerializeField]
//     private float facingOffset = 2f;
//
//     [SerializeField]
//     private Transform target;
//
//     public bool FacingRight
//     {
//         get => facingRight;
//         set
//         {
//             if (facingRight == value)
//                 return;
//             
//             facingRight = value;
//             
//             if(facingRight)
//                 OnFacingRight();
//             else
//                 OnFacingLeft();
//         }
//     }
//
//     private void Awake()
//     {
//         physicsController2D = GetComponent<PhysicsController2D>();
//     }
//
//     void OnFacingLeft()
//     {
//         //target.DOLocalRotate(new Vector3(0, 129, 0), 0.5f).SetEase(Ease.InOutSine);
//     }
//
//     void OnFacingRight()
//     {
//         //target.DOLocalRotate(new Vector3(0, -50, 0), 0.5f).SetEase(Ease.InOutSine);
//     }
//
//     public void UpdateFacingDirection(float targetXPosition)
//     {
//         var xPos = transform.position.x;
//         
//         if (FacingRight && targetXPosition + facingOffset < xPos || !FacingRight && targetXPosition - facingOffset > xPos)
//             FacingRight = !FacingRight;
//     }
// }
