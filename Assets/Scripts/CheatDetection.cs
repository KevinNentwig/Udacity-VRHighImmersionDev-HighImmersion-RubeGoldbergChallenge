using UnityEngine;
using Valve.VR;

public class CheatDetection : MonoBehaviour
{
    public bool getGrabStateLeftHand;
    public bool getGrabStateRightHand;
    public bool isCheating;
    public GameLogic gameLogic;
    private GameObject goal;

    void Start()
    {
        goal = GameObject.Find("Goal");
    }

    void Update()
    {
        // get the current state of the controllers if the user is pressing the trigger
        getGrabStateLeftHand = SteamVR_Actions.default_GrabPinch[SteamVR_Input_Sources.LeftHand].state;
        getGrabStateRightHand = SteamVR_Actions.default_GrabPinch[SteamVR_Input_Sources.RightHand].state;
    }

    // when the user is holding onto the ball and brings it back into the platform area reset the level
    void OnTriggerEnter(Collider other)
    {
        // if the user enters the ball back into the cheating collider while holding it
        if (other.gameObject.CompareTag("Ball") && getGrabStateRightHand == true || getGrabStateLeftHand == true)
        {
            // turn back on the goal
            goal.SetActive(true);

            // toggle cheat variable
            isCheating = false;
            gameLogic.IsCheating();
        }
    }

    // when the user is holding onto the ball and brings it outside the platform area turn on the cheat mechanism
    private void OnTriggerExit(Collider other)
    {
        // if the user is holding down the trigger and trying to bypass the cheating collider
        if (other.gameObject.CompareTag("Ball") && getGrabStateRightHand == true || getGrabStateLeftHand == true)
        {
            // turn off the goal
            goal.SetActive(false);

            // toggle cheat variable
            isCheating = true;
            gameLogic.IsCheating();
        }
    }
}