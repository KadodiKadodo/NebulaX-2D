using UnityEngine;

namespace PlayerScripts
{
    public class Inventory : MonoBehaviour
    {
        [field: SerializeField] private SerializableDictionary<Resource, int> Resources { get; set; }
        public static Inventory Instance; 
    
        private void Awake()
        {
        
            if (Instance != null && Instance != this)
            {
                // destroy dupe
                Destroy(gameObject);
                return; 
            }

        
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }


        public int GetResourceCount(Resource type)
        {
            if (Resources.TryGetValue(type, out int currentCount))
            {
                return currentCount;
            }
            else
            {
                return 0;
            }
        }

        public int AddResources(Resource type, int count)
        {
            if (Resources.TryGetValue(type, out int currentCount))
            {
                return Resources[type] += count;
            }
            else
            {
                Resources.Add(type, count);
                return count;
            }
        }
    
        public int GetTotalValue()
        {
            int totalMoney = 0;
            foreach (var resource in Resources)
            {
                // resource.Key is the Resource ScriptableObject (which holds the Value)
                // resource.Value is the int count of how many we have
                totalMoney += Mathf.RoundToInt(resource.Key.Value) * resource.Value;
            }
            return totalMoney;
        }

        // Add this function to clear the inventory after selling
        public void ClearInventory()
        {
            Resources.Clear();
        }
    }
}
