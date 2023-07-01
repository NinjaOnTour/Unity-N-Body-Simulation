using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NBodySimulation : Simulation
{
    public List<CelestialBody> CelestialBodies;

    public GameObject CelestialBodyPrefab;
    public SimulationData _SimulationData;

    public float TotalMomentum;
    public float TotalKineticEnergy;
    public float TotalPotentialEnergy;
    public float TotalEnergy;

    public Text TotalMomentumText;
    public Text TotalKineticEnergyText;
    public Text TotalPotentialEnergyText;
    public Text TotalEnergyText;

    public List<float> CDatas;
    public float LastCData;

    private void Start()
    {
        CDatas = new List<float>();
        CelestialBodies = new List<CelestialBody>();
        foreach (GameObject body in GameObject.FindGameObjectsWithTag("CelestialBody"))
        {
            AddBody(body.GetComponent<CelestialBody>());
        }

        if(_SimulationData) UseSimulationData();

        InvokeRepeating("CalculateValues", 0.5f, 0.5f);
        InvokeRepeating("WriteValues", 0.6f, 0.5f);
        InvokeRepeating("CalculateCData", 1f, 10f);
    }

    void CalculateCData()
    {
        float val = 0;
        for (int i = 0; i < CelestialBodies.Count; i++)
        {
            Vector3 vec = CelestialBodies[i].transform.position;
            val += vec.x * vec.y * vec.z;
        }
        CDatas.Add(val);
        LastCData = 0;
        for (int i = 0; i < CDatas.Count; i++)
        {
            LastCData += CDatas[i];
        }
    }

    public void AddBody(CelestialBody body)
    {
        if (CelestialBodies.Contains(body)) return;

        body.Simulation = this;
        CelestialBodies.Add(body);
    }

    private void UseSimulationData()
    {
        for (int i = 0; i < _SimulationData.ObjectData.Length; i++)
        {
            CelestialBody body = Instantiate(CelestialBodyPrefab, new Vector3(), Quaternion.identity).GetComponent<CelestialBody>();
            body.Construct(_SimulationData.ObjectData[i]);
            AddBody(body);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < CelestialBodies.Count-1; i++)
        {
            for (int k = i+1; k < CelestialBodies.Count; k++)
            {
                CelestialBodies[i].CalculateGraivty(CelestialBodies[k]);
            }
        }
    }

    public void WriteValues()
    {
        TotalMomentumText.text = "Momentum = " + TotalMomentum;
        TotalKineticEnergyText.text = "Kinetic Energy = " + TotalKineticEnergy;
        TotalPotentialEnergyText.text = "Potential Energy = " + TotalPotentialEnergy;
        TotalEnergyText.text = "Energy = " + TotalEnergy;
    }

    public void CalculateValues()
    {
        Vector3 totalMomentum = new Vector3();
        TotalKineticEnergy = 0;
        TotalPotentialEnergy = 0;
        
        for (int i = 0; i < CelestialBodies.Count; i++)
        {
            totalMomentum += CelestialBodies[i].Momentum;
            TotalKineticEnergy += CelestialBodies[i].KineticEnergy;

            for (int k = i+1; k < CelestialBodies.Count; k++)
            {
                TotalPotentialEnergy += CelestialBody.CalculatePotentialEnergy(CelestialBodies[i], CelestialBodies[k]);
            }
        }
        TotalEnergy = TotalKineticEnergy + TotalPotentialEnergy;
        TotalMomentum = totalMomentum.magnitude;
    }
}
