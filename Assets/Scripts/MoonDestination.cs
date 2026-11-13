using UnityEngine;
using UnityEngine.SceneManagement;

public class MoonDestination : MonoBehaviour
{
    [Header("Moon Settings")]
    public string sceneName; 
    
    [Tooltip("0 = Phobos, 1 = Io/Europa, 2 = Callisto/Titan")]
    public int requiredEngineLevel = 0;

    [Header("UI References")]
    public GameObject errorText; 

    public void TryToFly()
    {
        if (errorText != null) errorText.SetActive(false);

        // Check if the player's engine level is high enough
        if (PlayerStats.engineLevel >= requiredEngineLevel)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            if (errorText != null) errorText.SetActive(true);
            Debug.Log("Need Engine Level " + requiredEngineLevel + " to fly here!");
        }
    }
}