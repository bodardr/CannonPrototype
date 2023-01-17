using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

[AddComponentMenu("Room System/Character Room Transition Handler")]
public class CharacterRoomTransitionHandler : RoomTransitionHandler
{
    private Vector2 posA;
    private Vector3 posB;

    [SerializeField]
    private PhysicsController2D physicsController = null;

    public override IEnumerator MakeTransition(RoomData oldRoom, RoomData newRoom)
    {
        var controllerArrival = new WaitFor2DControllerArrival(physicsController, roomManager.LevelData.SpawnPosition);
        
        yield return transitionImage.DOFade(1, 0.25f).SetEase(Ease.OutSine).WaitForCompletion();
        
        oldRoom.Component.RoomCamera.enabled = false;

        Time.timeScale = 0;
        
        newRoom.Component.RoomCamera.enabled = true;

        Time.timeScale = 1f;

        yield return transitionImage.DOFade(0, 0.25f).From(1).SetEase(Ease.InSine).SetUpdate(true).WaitForCompletion();
        yield return controllerArrival;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(posA, posB);
    }
}
