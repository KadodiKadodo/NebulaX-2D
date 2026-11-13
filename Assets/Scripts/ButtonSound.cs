using AudioScripts;
using UnityEngine;

public class ButtonSoundPlayer : MonoBehaviour
{
    // This is the method you will select in your Event Trigger / OnClick list
    public void PlaySound(string soundName)
    {
        // It looks for the permanent AudioManager automatically
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound2D(soundName);
        }
        else
        {
            Debug.LogWarning("SoundManager not found in the scene!");
        }
    }
}