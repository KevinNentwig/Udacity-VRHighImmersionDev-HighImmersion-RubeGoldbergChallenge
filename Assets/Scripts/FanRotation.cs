using UnityEngine;

public class FanRotation : MonoBehaviour
{
    // controls the speed at which the fan blades rotate
    private readonly float spinSpeed = 300f;

    // Update is called once per frame
    void Update()
    {
        //rotate the fan blades counterclockwise (remove the negative if clockwise spin is desired)
        transform.Rotate(0, 0, -spinSpeed * Time.deltaTime);
    }
}