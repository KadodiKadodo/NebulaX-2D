using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class OxygenSystem : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI oxygenText;
    [Header("Death UI")]
    public GameObject deathScreenPanel;

    [Header("Oxygen Settings")]
    public float maxOxygen = 100f;
    public float currentOxygen;
    public float drainRate = 0f; // How much oxygen you lose per second

    private bool isDead = false;

    void Start()
    {
        Time.timeScale = 1f; 
        // Start with a full tank
        maxOxygen = PlayerStats.GetMaxOxygen();
        currentOxygen = maxOxygen;
    }

    void Update()
    {
        if (isDead) return;

        // Drain the oxygen based on the engine weight!
        currentOxygen -= (drainRate + PlayerStats.GetOxygenDrainRate()) * Time.deltaTime;

        if (oxygenText != null)
        {
            oxygenText.text = "Oxygen: " + Mathf.CeilToInt(currentOxygen).ToString();
        }

        if (currentOxygen <= 0)
        {
            currentOxygen = 0;
            isDead = true;
            MissionFailed();
        }
    }

    private void MissionFailed()
    {
        Debug.Log("Mission Failed: Out of Oxygen!");
        if (deathScreenPanel != null) deathScreenPanel.SetActive(true);
        
        // 2. Pause the game so the player can read it
        Time.timeScale = 0f;
        
        // 3. Unlock the mouse so they can click the button
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ReturnToSpaceCenter()
    {
        Time.timeScale = 1f; // Unpause before leaving
        SceneManager.LoadScene("SpaceCenter");
    }
}