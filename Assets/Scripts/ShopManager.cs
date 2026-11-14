using PlayerScripts;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI References")]
    public Text budgetText; 
    public Text feedbackText;

    [Header("Player Stats")]
    public int currentBudget = 500; 

    void Start()
    {
        UpdateUI();
    }

    public void SellResources()
    {
        if (Inventory.Instance == null)
        {
            if(feedbackText) feedbackText.text = "Error: No Player found!";
            Debug.LogWarning("Tried to sell, but Inventory.Instance is null. Did you start the scene directly?");
            return;
        }
        
        int moneyEarned = Inventory.Instance.GetTotalValue();
        if (moneyEarned > 0)
        {
            currentBudget += moneyEarned;
            Inventory.Instance.ClearInventory();
            UpdateUI();
            if(feedbackText) feedbackText.text = $"Total samples sold for ${moneyEarned}!";
        }
        else
        {
            if(feedbackText) feedbackText.text = "Nothing to sell!";
        }
    }

    // --- ENGINE UPGRADES ---
    public void BuyEngineLevel1()
    {
        int cost = 700;
        if (currentBudget >= cost && PlayerStats.engineLevel == 0)
        {
            currentBudget -= cost;
            PlayerStats.engineLevel = 1;
            UpdateUI();
            if(feedbackText) feedbackText.text = "Engine Level 1! (Reaches Europa)";
        }
        else { HandleError(PlayerStats.engineLevel > 0, cost); }
    }

    public void BuyEngineLevel2()
    {
        int cost = 1100;
        if (currentBudget >= cost && PlayerStats.engineLevel == 1)
        {
            currentBudget -= cost;
            PlayerStats.engineLevel = 2;
            UpdateUI();
            if(feedbackText) feedbackText.text = "Engine Level 2! (Reaches Titan)";
        }
        else { HandleError(PlayerStats.engineLevel > 1, cost); }
    }

    // --- OXYGEN UPGRADES ---
    public void BuyOxygenLevel1()
    {
        int cost = 200;
        if (currentBudget >= cost && PlayerStats.oxygenLevel == 0)
        {
            currentBudget -= cost;
            PlayerStats.oxygenLevel = 1;
            UpdateUI();
            if(feedbackText) feedbackText.text = "Oxygen Tank upgraded (+50 Max)";
        }
        else { HandleError(PlayerStats.oxygenLevel > 0, cost); }
    }

    public void BuyOxygenLevel2()
    {
        int cost = 400;
        if (currentBudget >= cost && PlayerStats.oxygenLevel == 1)
        {
            currentBudget -= cost;
            PlayerStats.oxygenLevel = 2;
            UpdateUI();
            if(feedbackText) feedbackText.text = "Oxygen Tank upgraded (+100 Max)";
        }
        else { HandleError(PlayerStats.oxygenLevel > 1, cost); }
    }

    private void HandleError(bool alreadyOwned, int cost)
    {
        if (feedbackText == null) return; // Safety check

        if (alreadyOwned)
        {
            feedbackText.text = "Already upgraded!";
        }
        else
        {
            feedbackText.text = $"The item exceeds your budget";
        }
    }

    private void UpdateUI()
    {
        budgetText.text = $"Budget: ${currentBudget}";
    }
    
    public void ClearFeedbackText()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }
}