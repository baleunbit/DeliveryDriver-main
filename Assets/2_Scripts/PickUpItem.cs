using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public void OnPickup(float delay = 0.1f)
    {
        Destroy(gameObject, delay);
    }
}
