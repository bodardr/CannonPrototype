using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonMachine : MonoBehaviour
{
    private ArcHandle defaultArcHandle;
    private ArcHandle throwArcHandle;

    [SerializeField]
    private float defaultAngle = 0;

    [SerializeField]
    private float throwAngle = 180;

    private void InitializeArcHandles()
    {
        defaultArcHandle = new ArcHandle()
        {
            angle = transform.rotation.z, 
            radius = 2f,
            fillColor = Color.clear,
            wireframeColor = Color.cyan,
            angleHandleColor = Color.cyan
        };        
        
        throwArcHandle = new ArcHandle()
        {
            angle = transform.rotation.z + 180, 
            radius = 2f,
            fillColor = Color.clear,
            wireframeColor = Color.cyan,
            angleHandleColor = Color.cyan,
            angleHandleSizeFunction = pos => 0.2f,
            radiusHandleSizeFunction = pos => 0.4f,
            radiusHandleColor = Color.clear,
        };
    }

    private void OnDrawGizmos()
    {
        if (defaultArcHandle == null || throwArcHandle == null)
            InitializeArcHandles();

        using (new Handles.DrawingScope())
        {
            
        }
        
        EditorGUI.BeginChangeCheck();

        defaultArcHandle.angle = defaultAngle;
        throwArcHandle.angle = throwAngle;
        
        defaultArcHandle.DrawHandle();
        throwArcHandle.DrawHandle();

        if (EditorGUI.EndChangeCheck())
        {
            defaultAngle = defaultArcHandle.angle;
            throwAngle = throwArcHandle.angle;
        }
    }
}