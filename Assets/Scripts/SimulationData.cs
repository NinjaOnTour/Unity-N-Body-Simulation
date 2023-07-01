using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum SimulationType
{
    None,
    InSphere,
    OnSphere,
    Galaxy
}

public enum VelocityType
{
    None,
    Orbital,
    Random
}

[CreateAssetMenu()]
public class SimulationData : ScriptableObject
{
    public CelestialBodyData[] ObjectData;
    public SimulationType CreationType;
    [Header("Simulation Values")]
    public int ObjectCount;
    public float MaxMass;
    public float MinMass;
    public VelocityType StartVelocity;
    public float MaxVelocity;
    public float SpawnRadius;
    public float CenterMass;

    public void CreateData()
    {
        if (CreationType == SimulationType.None) return;

        ObjectData = new CelestialBodyData[ObjectCount+1];

        if(CreationType == SimulationType.InSphere)
        {
            CreateInSphere();
        }

        ObjectData[ObjectCount] = new CelestialBodyData(CenterMass, 3f, Vector3.zero, Vector3.zero);
    }


    private void CreateInSphere()
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            float mass = Random.Range(MinMass, MaxMass);
            Vector3 velocity = Vector3.zero;
            Vector3 position = Random.insideUnitSphere * SpawnRadius;
            if (StartVelocity == VelocityType.Random)
            {
                velocity = new Vector3(Random.Range(-MaxVelocity, MaxVelocity), Random.Range(-MaxVelocity, MaxVelocity), Random.Range(-MaxVelocity, MaxVelocity));
            }
            else if(StartVelocity == VelocityType.Orbital)
            {
                Vector3 vecy = Vector3.up * position.y;
                Vector3 vecx = position;
                vecx.y = 0f;
                Vector3 dir = Vector3.Cross(vecx, vecy).normalized;
                Debug.Log(dir);
                velocity = dir * MaxVelocity;
            }
            
            ObjectData[i] = new CelestialBodyData(mass, 1f, position, velocity);
        }
    }
}
