using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR;

public class SpawnMenu : MonoBehaviour
{
    public LevelSelection levelSelection;

    public string levelName;

    private readonly float trackpadEastXValue = 0.5f;
    private readonly float trackpadWestXValue = -0.5f;
    private readonly float trackpadNorthValue = 0.5f;
    private readonly float trackpadSouthYValue = -0.5f;

    private int currentObject;
    private int woodPlankSpawnLimit;
    private int metalPlankSpawnLimit;
    private int trampolineSpawnLimit;
    private int teleportTargetSpawnLimit;
    private int fanBodySpawnLimit;

    public int woodPlankClones;
    public int metalPlankClones;
    public int trampolineClones;
    public int teleportTargetClones;
    public int fanBodyClones;

    public bool trackpadReset;
    public bool getRightHandTrackPadTouchDown;
    public bool getRightHandGrabState;
    public bool menuObjectSpawnEnable;
    public bool objectInRightHand;
    private bool objectSpawned;
    private bool spawnLimitReached;

    public Vector2 getRightHandTrackPadPosition;

    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject ObjectSpawnMenu;
    public List<GameObject> menuObjectList;
    public List<GameObject> prefabList;

    // Use this for initialization
    void Start()
    {
        // for every child in the object spawn menu
        foreach (Transform interactableObject in ObjectSpawnMenu.transform)
        {
            // add the menu game object into the list
            menuObjectList.Add(interactableObject.gameObject);
        }

        // get the current scene name
        levelName = SceneManager.GetActiveScene().name;

        // set the object spawn limit
        ObjectSpawnLimit();
    }

    void Update()
    {
        // get current status of the user button presses on the right controller
        getRightHandTrackPadTouchDown = SteamVR_Actions.default_TrackPadTouchDown[SteamVR_Input_Sources.RightHand].state;
        getRightHandTrackPadPosition = SteamVR_Actions.default_TrackPadPosition[SteamVR_Input_Sources.RightHand].axis;
        getRightHandGrabState = SteamVR_Actions.default_GrabPinch[SteamVR_Input_Sources.RightHand].state;
        
        // check to see if the object spawn menu should be enabled
        MenuEnabled();

        // if the menu enabled has been triggered
        if (menuObjectSpawnEnable == true)
        {
            // users presses down on trackpad but doesn't have their finger located in the south or north positions
            if (getRightHandTrackPadPosition.y > trackpadSouthYValue && getRightHandTrackPadPosition.y < trackpadNorthValue)
            {
                // get current amount of spawned objects in game
                woodPlankClones = GameObject.FindGameObjectsWithTag("Wood_Plank").Length;
                metalPlankClones = GameObject.FindGameObjectsWithTag("Metal_Plank_WE").Length;
                trampolineClones = GameObject.FindGameObjectsWithTag("Trampoline").Length;
                fanBodyClones = GameObject.FindGameObjectsWithTag("Fan_Body").Length;
                teleportTargetClones = GameObject.FindGameObjectsWithTag("Teleport_Target").Length;

                // enable the object spawn menu to appear
                menuObjectList[currentObject].SetActive(true);

                // if the user presses down on the trigger with menu open and no object has been recently spawned
                if (getRightHandGrabState == true && objectSpawned == false)
                {
                    // set object spawned to true to prevent multiple objects being spawned at once
                    objectSpawned = true;

                    // spawn the current object in the list
                    SpawnCurrentObject();
                }
                // if user is no longer trying to spawn multiple objects by holding the grab state
                else if (getRightHandGrabState == false && objectSpawned == true)
                {
                    // object hasn't been spawned so reset
                    objectSpawned = false;
                }
                // if the user has press down on the left side of the trackpad
                else if (getRightHandTrackPadPosition.x < trackpadWestXValue && trackpadReset == true)
                {
                    MenuLeftClick();

                    // do not allow the user to place more objects till they reset their trackpad location
                    trackpadReset = false;
                }
                // if the user has press down on the right side of the trackpad
                else if (getRightHandTrackPadPosition.x > trackpadEastXValue && trackpadReset == true)
                {
                    MenuRightClick();
                    trackpadReset = false;
                }
                // if the user has press down near the dead center of the trackpad
                else if (getRightHandTrackPadPosition.x < trackpadEastXValue && getRightHandTrackPadPosition.x > trackpadWestXValue)
                {
                    // allow the user to spawn more objects
                    trackpadReset = true;
                }
            }
            // if the user has press down on the bottom side of the trackpad
            else if (getRightHandTrackPadPosition.y < trackpadSouthYValue)
            {
                // disable the object spawn menu
                menuObjectList[currentObject].SetActive(false);

                // for each clone object being held by the player in the right hand
                foreach (Transform interactableCloneObject in rightHand.transform)
                {
                    if (interactableCloneObject.name.Contains("(Clone)"))
                    {
                        // remove the object in the users hand
                        //Destroy(interactableCloneObject);
                        //DestroySpawnedObject();
                    }
                }
                // for each clone object being held by the player in the left hand
                foreach (Transform interactableCloneObject in leftHand.transform)
                {
                    if (interactableCloneObject.name.Contains("(Clone)"))
                    {
                        // remove the object in the users hand
                        //Destroy(interactableCloneObject);
                        //DestroySpawnedObject();
                    }
                }
            }
        }
        // if the user has let go of the trackpad
        else
        {
            // disable the object spawn menu
            menuObjectList[currentObject].SetActive(false);
        }
    }

    // controls whether the object spawn menu should be enabled or not
    void MenuEnabled()
    {
        // check to see if the player has an object in their right hand before allowing them to use the menu
        foreach (Transform child in rightHand.transform)
        {
            if (child.gameObject.CompareTag("Wood_Plank") ||
                child.gameObject.CompareTag("Metal_Plank_WE") || 
                child.gameObject.CompareTag("Trampoline") ||
                child.gameObject.CompareTag("Fan_Body") ||
                child.gameObject.CompareTag("Teleport_Target"))
            {
                objectInRightHand = true;
            }
            else
            {
                objectInRightHand = false;
            }
        }

        // if the touchpad has been touched down and no object is found in the right hand
        if (getRightHandTrackPadTouchDown == true && objectInRightHand == false)
        {
            menuObjectSpawnEnable = true;
        }
        else if (getRightHandTrackPadTouchDown == false || objectInRightHand == true)
        {
            menuObjectSpawnEnable = false;
        }
        
    }

    void MenuLeftClick()
    {
        // turn off the object list
        menuObjectList[currentObject].SetActive(false);

        // select the next object by decrementing the list
        currentObject--;

        // if the object value exceeds the list count
        if (currentObject < 0)
        {
            // go back to the end of the list
            currentObject = menuObjectList.Count - 1;
        }

        // turn on the object list with the new object selected
        menuObjectList[currentObject].SetActive(true);
    }

    void MenuRightClick()
    {
        // turn off the object list
        menuObjectList[currentObject].SetActive(false);

        // select next object by incrementing the list
        currentObject++;

        // if the object count exceeds the list count
        if (currentObject > menuObjectList.Count - 1)
        {
            // set the current object to the value in 0
            currentObject = 0;
        }

        // turn on the object list with the new object selected
        menuObjectList[currentObject].SetActive(true);
    }

    // limit the amount of objects the user can spawn in the game
    void ObjectSpawnLimit()
    {
        // if the current scene is a specific scene
        
        if (levelName == "Level0")
        {
            // limit the amount of objects that can be spawned
            woodPlankSpawnLimit = 1;
            metalPlankSpawnLimit = 1;
            trampolineSpawnLimit = 1;
            teleportTargetSpawnLimit = 1;
            fanBodySpawnLimit = 1;
}
        else if (levelName == "Level1")
        {
            woodPlankSpawnLimit = 2;
            metalPlankSpawnLimit = 1;
            trampolineSpawnLimit = 1;
            teleportTargetSpawnLimit = 1;
            fanBodySpawnLimit = 1;
        }
        else if (levelName == "Level2")
        {
            woodPlankSpawnLimit = 2;
            metalPlankSpawnLimit = 2;
            trampolineSpawnLimit = 1;
            teleportTargetSpawnLimit = 1;
            fanBodySpawnLimit = 1;
        }
        else if (levelName == "Level3")
        {
            woodPlankSpawnLimit = 3;
            metalPlankSpawnLimit = 3;
            trampolineSpawnLimit = 1;
            teleportTargetSpawnLimit = 1;
            fanBodySpawnLimit = 1;
        }
        else if (levelName == "Level4")
        {
            woodPlankSpawnLimit = 4;
            metalPlankSpawnLimit = 4;
            trampolineSpawnLimit = 1;
            teleportTargetSpawnLimit = 1;
            fanBodySpawnLimit = 1;
        }
    }

    // initialize interactable objects from the spawn menu
    public void SpawnCurrentObject()
    {
        // if the menu isn't underneath the map to prevent object spawning where user's can't reach
        if (menuObjectList[currentObject].transform.position.y > 0)
        {
            // if the limit of the object spawn hasn't been reached
            if (prefabList[currentObject].tag == "Wood_Plank" && woodPlankClones < woodPlankSpawnLimit ||
                prefabList[currentObject].tag == "Metal_Plank_WE" && metalPlankClones < metalPlankSpawnLimit ||
                prefabList[currentObject].tag == "Trampoline" && trampolineClones < trampolineSpawnLimit ||
                prefabList[currentObject].tag == "Teleport_Target" && teleportTargetClones < teleportTargetSpawnLimit ||
                prefabList[currentObject].tag == "Fan_Body" && fanBodyClones < fanBodySpawnLimit)
            {
                // create the game object currently selected
                Instantiate(prefabList[currentObject], menuObjectList[currentObject].transform.position, menuObjectList[currentObject].transform.rotation);
            }
            else
            {
                // do not spawn anything
            }
        }

    }
}