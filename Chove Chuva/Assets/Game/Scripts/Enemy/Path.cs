using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> Waypoints = new List<Transform>();

    [SerializeField] bool _alwaysDrawPath;
    [SerializeField] bool _drawNumbers;
    [SerializeField] bool _drawAsLoop;
    [SerializeField] Color _debugColor;

#if UNITY_EDITOR

    void DrawPath()
    {
        for(int i = 0; i < Waypoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = _debugColor;

            if(_drawNumbers)
                Handles.Label(Waypoints[i].position, i.ToString(), labelStyle);

            if(i >= 1)
            {
                Gizmos.color = _debugColor;
                Gizmos.DrawLine(Waypoints[i - 1].position, Waypoints[i].position);

                if (_drawAsLoop)
                    Gizmos.DrawLine(Waypoints[Waypoints.Count - 1].position, Waypoints[0].position);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (_alwaysDrawPath)
            DrawPath();
    }

    void OnDrawGizmosSelected()
    {
        if (!_alwaysDrawPath)
            DrawPath();
    }

#endif
}
