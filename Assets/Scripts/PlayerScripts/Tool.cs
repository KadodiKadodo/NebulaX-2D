using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class Tool : MonoBehaviour
    {
        [field: SerializeField] public int MinHarvest { get; set; } = 1;
        [field: SerializeField] public int MaxHarvest { get; set; } = 3;
      
        [Header("Mining Settings")]
        public float mineRadius = 1.5f; // How close the player needs to be
        public float mineCooldown = 0.5f; // Time between hits

        private float nextHarvestTime = 0f;

        void Update()
        {
            // Check if we clicked the left mouse button
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                // Check if the cooldown has passed
                if (Time.time < nextHarvestTime) return;

                // Scan a circle around the player for ANY rocks
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, mineRadius);

                foreach (var hit in hitColliders)
                {
                    Harvestable harvestable = hit.GetComponent<Harvestable>();
               
                    if (harvestable != null)
                    {
                        int amountToHarvest = UnityEngine.Random.Range(MinHarvest, MaxHarvest);
                        harvestable.Harvest(amountToHarvest);
                  
                        // Set cooldown so we don't break it 60 times a second
                        nextHarvestTime = Time.time + mineCooldown; 
                  
                        // Break out of the loop so we only hit one rock per click
                        break; 
                    }
                }
            }
        }
    }
}