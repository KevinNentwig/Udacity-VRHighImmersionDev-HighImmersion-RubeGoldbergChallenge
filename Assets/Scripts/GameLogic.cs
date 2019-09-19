using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public CheatDetection cheatDetection;
    private GameObject ball;
    public GameObject collectible_Stars;
    private GameObject goal;
    public List<GameObject> stars;
    public Vector3 startPosition;
    private Quaternion startRotation;
    public Rigidbody ballRigidBody;
    private Renderer ballRenderer;
    public Material ballDefaultMaterial;
    public Material ballCheatingMaterial;
    public int starCount;
    public bool ballHeld = false;

    // Use this for initializationz
    void Start()
    {
        // find the game object goal
        goal = GameObject.Find("Goal");

        // find the game object holding the collectible stars
        collectible_Stars = GameObject.Find("Collectible_Stars");

        // for every collectible star child in collectible_Stars
        foreach (Transform collectibleStar in collectible_Stars.transform)
        {
            // add the star into the list
            stars.Add(collectibleStar.gameObject);
        }

        // find game object ball
        ball = GameObject.Find("Ball");

        // get rigidbody component
        ballRigidBody = ball.GetComponent<Rigidbody>();

        // get the renderer component of the ball
        ballRenderer = ball.gameObject.GetComponent<Renderer>();

        // get ball's starting position and rotation
        startPosition = ball.transform.position;
        startRotation = ball.transform.rotation;
    }

    private void Update()
    {
        // if the ball goes beyond the game boundaries
        if (ball.transform.position.y > 8 || ball.transform.position.y < 0)
        { 
            // reset the level
            ResetLevel();
        }
    }

    // increment value of stars being collected once ball has collided with the star
    public void StarCollected()
    {
        starCount++;
    }

    // detect whether the user is trying to cheat
    public void IsCheating()
    {
        // if the user tries to take the ball outside of the platform while holding onto it
        if (cheatDetection.isCheating == true)
        {
            // change ball material to show that the user is cheating
            ballRenderer.material = ballCheatingMaterial;
        }
        else if (cheatDetection.isCheating == false)
        {
            // change ball material to show that the user is no longer cheating
            ballRenderer.material = ballDefaultMaterial;

            // reset the level
            ResetLevel();
        }
    }

    // restart the level by resetting the ball, stars, and the goal
    void ResetLevel()
    {
        // reset position of the ball
        ball.transform.position = startPosition;
        ball.transform.rotation = startRotation;

        // reset velocity of the ball
        ballRigidBody.velocity = Vector3.zero;
        ballRigidBody.angularVelocity = Vector3.zero;

        // reset material of the ball
        ballRenderer.material = ballDefaultMaterial;

        // go through the star list
        for (int i = 0; i < this.stars.Count; i++)
        {
            // set the current star in the list to active
            stars[i].SetActive(true);
        }

        // reset the star collected count
        starCount = 0;

        // enable the goal
        goal.SetActive(true);
    }

    // when the ball collides with the building gameobject
    void OnCollisionEnter(Collision building)
    {
        if (building.gameObject.tag == "Building")
        {
            // reset the level
            ResetLevel();
        }
    }
}