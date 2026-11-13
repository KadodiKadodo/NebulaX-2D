using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; 

public interface IInteractable
{
    void Interact();
    void OnTouchingPlayer();
    void OnNotTouchingPlayer();
}

public class Interactor : MonoBehaviour
{
    private IInteractable CurrentInteractable;
 
    private void Start()
    {
        // Subscribe to the scene load event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // When a new scene loads, FORGET the old computer/objects!
        CurrentInteractable = null;
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && CurrentInteractable != null)
        {
            CurrentInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            CurrentInteractable = interactable;
            CurrentInteractable.OnTouchingPlayer();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        
        if (interactable != null && interactable == CurrentInteractable)
        {
            // Cast to MonoBehaviour to check if the object is actually destroyed by Unity
            MonoBehaviour mono = interactable as MonoBehaviour;
            if (mono != null)
            {
                CurrentInteractable.OnNotTouchingPlayer();
            }
            CurrentInteractable = null;
        }
    }
}