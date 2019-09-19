using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanWindForce : MonoBehaviour {

    private readonly float windStrength = 0.1f;
    private Vector3 windDirection = new Vector3 (0f, 0f, 0f);

    // called on once per physics update for every collider "col" that is touching the trigger
    private void OnTriggerStay(Collider col)
    {
        Rigidbody colRigidbody = col.GetComponent<Rigidbody>();

        if (colRigidbody != null)
        {
            // apply wind force to colliding rigidbody based on the direction and strength
            colRigidbody.AddForce(windDirection * windStrength);
        }
    }
}