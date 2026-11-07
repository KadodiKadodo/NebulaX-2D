using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IInteractable
{
    void Interact();
    void OnTouchingPlayer();
    void OnNotTouchingPlayer();
}
public class Interactor : MonoBehaviour
{
    private IInteractable CurrentInteractable;
    
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
            CurrentInteractable.OnNotTouchingPlayer();
            CurrentInteractable = null;
        }
    }
}

