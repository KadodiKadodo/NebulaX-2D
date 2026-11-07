using UnityEngine;
using UnityEngine.InputSystem;

public class OpenComputer : MonoBehaviour, IInteractable
{
    private SpriteRenderer sr;
    private Color OriginalColor;
    [SerializeField] private GameObject promptText;
    [SerializeField] private GameObject menuPanel;
    private bool isOpen;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        OriginalColor = sr.color;
    }
    private void Update()
    {
        if (!isOpen) return;
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            CloseMenu();
    }
    
    public void Interact()
    {
        if (isOpen) CloseMenu();
        else OpenMenu();
        Debug.Log("E pressed on computer");
    }

    private void OpenMenu()
    {
        isOpen = true;
        menuPanel.SetActive(true);
        promptText.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void CloseMenu()
    {
        isOpen = false;
        menuPanel.SetActive(false);
        promptText.SetActive(true);
        Time.timeScale = 1f;
        // Uncomment if your game locks the cursor during gameplay:
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }
    public void OnNotTouchingPlayer()
    {
        sr.color = OriginalColor;
        promptText.SetActive(false);
    }
    public void OnTouchingPlayer()
    {
        sr.color = Color.red;
        promptText.SetActive(true);
    }

    
}
