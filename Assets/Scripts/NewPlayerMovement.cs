using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerMovement : MonoBehaviour
{
private void onMove(InputAction.CallbackContext context)
{
    context.ReadValue<Vector2>();   
}
    private void onUse(InputAction.CallbackContext context)
    {
        
    }

}
