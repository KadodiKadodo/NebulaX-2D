using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneBridge : MonoBehaviour
{
    public void LoadSpaceCenter()
    {
        SceneManager.LoadScene("SpaceCenter"); 
    }
}