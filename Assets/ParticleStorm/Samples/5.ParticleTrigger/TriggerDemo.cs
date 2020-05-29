using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticleStorm;
using ParticleStorm.Script;

public class TriggerDemo : MonoBehaviour
{
    public ParticlePrefeb prefeb;
    public Collider box;
    public float maxAngle;
    private Vector3 boxPosition;
    
    void TurnToBox(ParticleStatus particle)
    {
        particle.Velocity = Vector3.RotateTowards(
            particle.Velocity,
            boxPosition - particle.Position,
            maxAngle * particle.DeltaTime * Mathf.Deg2Rad,
            0
        );
    }

    void Start()
    {
        ParticleScript.AddUpdateScript(new UpdateEvent("TurnToBox", TurnToBox));
        ParticleScript.AddCollisionScript(new CollisionEvent("Collision",
            (go, collisions) =>
            {
                for (int i = 0; i < collisions.Count; i++)
                {
                    Debug.Log(go.name + " collided");
                }
            }
        ));
        var generator = GetComponent<StormGenerator>();
        var storm = new Storm();
        var particle = new Particle(prefeb);
        particle.SetTriggerCollider(0, box);
        storm.AddBehavior(0, EmitList.Cone(100, 2, 90, 0.5f), particle, 1);
        generator.Generate(storm);
    }

    void Update()
    {
        boxPosition = box.transform.position;
    }
}
