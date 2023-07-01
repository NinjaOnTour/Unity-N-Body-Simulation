using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimulationData))]
public class SimulationDataEditor : Editor
{
    SimulationData simData;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        simData = (SimulationData)target;
        if(GUILayout.Button("Create"))
        {
            simData.CreateData();
        }
    }
}
