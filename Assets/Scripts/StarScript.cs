using UnityEngine;

public class StarScript : MonoBehaviour
{

    public GameLogic gameLogic;
    public float spinSpeed = 30f;


    // Update is called once per frame
    void Update()
    {
        // rotate the stars clockwise (add a negative in order to spin counter clockwise)
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider ball)
    {
        // if the object the star has come in contact with has the tag Ball
        if (ball.gameObject.tag == "Ball")
        {
            // deactivate the star from the scene
            gameObject.SetActive(false);

            // add to the total starCount
            gameLogic.StarCollected();
        }
    }
}
