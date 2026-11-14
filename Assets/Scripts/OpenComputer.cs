using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OpenComputer : MonoBehaviour, IInteractable
{
    private SpriteRenderer sr;
    private Color OriginalColor;
    [SerializeField] private GameObject promptText;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        OriginalColor = sr.color;
    }
    
    public void Interact()
    {
        Debug.Log("E pressed on computer, loading scene...");
        
        Time.timeScale = 1f; // Unpause the game just in case!
        
        // Load her Shop Scene!
        SceneManager.LoadScene("AstraMoonSelection"); 
    }

    public void OnNotTouchingPlayer()
    {
        sr.color = OriginalColor;
        if (promptText != null) promptText.SetActive(false); 
    }
    
    public void OnTouchingPlayer()
    {
        sr.color = Color.grey;
        if (promptText != null) promptText.SetActive(true);
    }
}