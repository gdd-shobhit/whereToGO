using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothness = 0.225f;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 smoothPosition = Vector3.Lerp(transform.position, target.position, smoothness);
            transform.position = new Vector3(smoothPosition.x, smoothPosition.y);

        }
       
        
    }
}
