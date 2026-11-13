using UnityEngine;
using PlayerScripts;

public class PickupResources : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ResourcePickup pickup = collision.gameObject.GetComponent<ResourcePickup>();
        if (pickup)
        {
            // Use the Singleton directly!
            Inventory.Instance.AddResources(pickup.ResourceType, 1);
            Destroy(pickup.gameObject);
        }
    }
}