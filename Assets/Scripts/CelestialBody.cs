using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [SerializeField] private float mass = 1f;
    public float Mass
    {
        get
        {
            return mass;
        }
        set
        {
            mass = value;
            rb.mass = mass;
        }
    }
    public float Radius = 1f;
    public Vector3 StartVelocity;
    public int Index;

    public NBodySimulation Simulation;
    public const float G_Constant = 6.67f;

    public Vector3 Velocity
    {
        get
        {
            return rb.velocity;
        }
        set
        {
            rb.velocity = value;
        }
    }

    public float Speed
    {
        get
        {
            return rb.velocity.magnitude;
        }
    }

    public Vector3 Momentum
    {
        get
        {
            return Velocity * Mass;
        }
    }

    public float KineticEnergy
    {
        get
        {
            return 0.5f * Mass * Mathf.Pow(Speed, 2);
        }
    }

    private Rigidbody rb;

    public void Construct(CelestialBodyData data)
    {
        rb = GetComponent<Rigidbody>();
        Mass = data.Mass;
        Radius = data.Radius;
        StartVelocity = data.Velocity;
        transform.position = data.Position;
        transform.localScale = Vector3.one * Radius;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.localScale = Vector3.one * Radius;
        rb.mass = Mass;
        rb.velocity = StartVelocity;
    }

    private void Update()
    {
        //Debug.Log(Velocity.z);
    }


    public void CalculateGraivty(CelestialBody other)
    {
        Vector3 vec = transform.position - other.transform.position;
        Vector3 force = vec.normalized * (G_Constant * Mass * other.Mass / vec.sqrMagnitude);
        other.rb.AddForce(force, ForceMode.Force);
        rb.AddForce(-force, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "CelestialBody")
        {
            CelestialBody body = collision.gameObject.GetComponent<CelestialBody>();
            if (Simulation.CelestialBodies.IndexOf(body) < Simulation.CelestialBodies.IndexOf(this)) 
            {
                Mass += body.Mass;
                rb.velocity = (Momentum + body.Momentum) / (Mass + body.Mass);
                Destroy(body.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        Simulation.CelestialBodies.Remove(this);
    }

    public static float CalculatePotentialEnergy(CelestialBody body0, CelestialBody body1)
    {
        return -G_Constant * body0.Mass * body1.Mass / Vector3.Distance(body0.transform.position, body1.transform.position);
    }
}

[System.Serializable]
public struct CelestialBodyData
{
    public float Mass;
    public float Radius;
    public Vector3 Position;
    public Vector3 Velocity;

    public CelestialBodyData(CelestialBody body)
    {
        Mass = body.Mass;
        Radius = body.Radius;
        Position = body.transform.position;
        Velocity = body.Velocity;
    }

    public CelestialBodyData(float mass, float radius, Vector3 position, Vector3 velocity)
    {
        Mass = mass;
        Radius = radius;
        Position = position;
        Velocity = velocity;
    }
}