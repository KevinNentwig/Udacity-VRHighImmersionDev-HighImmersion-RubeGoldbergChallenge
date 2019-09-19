using UnityEngine.SceneManagement;
using UnityEngine;
using Valve.VR;

public class LevelSelection : MonoBehaviour
{
    public string[] levelNames = new string[5] {"Level0", "Level1", "Level2", "Level3", "Level4" };
    public int currentLevel = 0;
    private int maxLevel = 5;
    private int starsListCount;
    private int starsCollected;
    public GameLogic gamelogic;

    private void Update()
    {
        // get the current count of collected stars
        starsListCount = gamelogic.stars.Count;
        starsCollected = gamelogic.starCount;
    }

    void OnTriggerEnter(Collider ball)
    {
        // if the object colliding with the ball has the tag goal
        // and the amount of stars to collect is zero
        if (ball.gameObject.tag == "Ball" && starsListCount == starsCollected)
        {
            // if the current level has reached the maximum
            if (currentLevel == maxLevel)
            {
                // start the game again from the very first level
                currentLevel = 0;
            }
            else
            {
                // increment the level
                currentLevel = currentLevel + 1;
            }
            // load the next level
            SteamVR_LoadLevel.Begin(levelNames[currentLevel]);
        }
    }
}