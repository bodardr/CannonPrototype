using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnMoveAttribute : MonoBehaviour
{
    private PhysicsController2D physicsController2D;

    private void Start()
    {
        physicsController2D = GetComponent<PhysicsController2D>();
    }

    [YarnCommand("move")]
    public void HeadToPosition(string objName)
    {
        var go = GameObject.Find(objName);
        if(go != null)
            physicsController2D.HeadToPosition(go.transform);
    }
}
