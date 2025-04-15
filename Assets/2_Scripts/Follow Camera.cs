using UnityEngine;

public class FollowCamera : MonoBehaviour
{
        [SerializeField] GameObject followTarget;
    
    void Update()
    {
        transform.position = followTarget.transform.position + new Vector3(0, 0, -10);
    }
}   
