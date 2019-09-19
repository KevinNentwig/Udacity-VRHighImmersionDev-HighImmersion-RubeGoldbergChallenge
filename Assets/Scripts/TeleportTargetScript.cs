using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTargetScript : MonoBehaviour
{
    private GameLogic gameLogic;
    private GameObject ball;
    public GameObject goal;
    public Vector3 teleportOffset = new Vector3(0, 2, 0);

    private void Start()
    {
        // find the gameobject ball and it's script gamelogic
        ball = GameObject.Find("Ball");
        gameLogic = ball.GetComponent<GameLogic>();
    }

    private void Update()
    {
        // if no goal gameobject is assigned
        if (goal == null)
        {
            // find the fan prefab
            goal = GameObject.Find("Goal");
        }
    }

    // on object collision entering collider
    private void OnTriggerEnter(Collider other)
    {
        // if the ball is the colliding object
        if (other.gameObject.tag == "Ball")
        {
                // transform the position of the ball to be in front of the fan and reset ball's velocity  
                gameLogic.ballRigidBody.velocity = Vector3.zero;
                gameLogic.ballRigidBody.angularVelocity = Vector3.zero;
                other.gameObject.transform.position = goal.transform.position + teleportOffset;
        } 
    }
}