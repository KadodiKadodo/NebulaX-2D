using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour, IInteractable
{
    private SpriteRenderer sr;
    private Color OriginalColor;
    
    [Header("UI References")]
    [SerializeField] private GameObject promptText; // The "[E] Return" text
    [SerializeField] private GameObject returnMenuPanel; // Your hidden return panel
    
    private bool isOpen;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        OriginalColor = sr.color;
    }
    
    private void Update()
    {
        if (!isOpen) return;
        
        // If they press Escape, close the menu
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            CloseMenu();
    }
    
    public void Interact()
    {
        if (isOpen) CloseMenu();
        else OpenMenu();
    }

    private void OpenMenu()
    {
        isOpen = true;
        if (returnMenuPanel != null) returnMenuPanel.SetActive(true);
        if (promptText != null) promptText.SetActive(false);
        
        Time.timeScale = 0f; // Pause the game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void CloseMenu()
    {
        isOpen = false;
        if (returnMenuPanel != null) returnMenuPanel.SetActive(false);
        if (promptText != null) promptText.SetActive(true);
        
        Time.timeScale = 1f; // Unpause the game
    }

    public void OnNotTouchingPlayer()
    {
        if (sr != null) sr.color = OriginalColor;
        if (promptText != null) promptText.SetActive(false);
    }
    
    public void OnTouchingPlayer()
    {
        if (sr != null) sr.color = Color.grey;
        if (promptText != null) promptText.SetActive(true);
    }
}