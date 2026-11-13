using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmReturn : MonoBehaviour
{
    public void ReturnToSpaceCenter()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SpaceCenter"); 
    }
}