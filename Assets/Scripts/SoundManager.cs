using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    [SerializeField] private SoundLibrary sfxLibrary;
    [SerializeField] private AudioSource sfx2DSource;
    
    [SerializeField] private SoundManager audioManagerPrefab; 

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        // If no SoundManager exists in the game yet...
        if (instance == null)
        {
            // Look for a prefab named "AudioManager" in a folder named "Resources"
            SoundManager prefab = Resources.Load<SoundManager>("AudioManager");
            
            if (prefab != null)
            {
                Instantiate(prefab);
            }
            else
            {
                Debug.LogError("No AudioManager prefab found in a Resources folder!");
            }
        }
    }

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        var clip = sfxLibrary.GetClipFromName(soundName);
        if (clip != null) sfx2DSource.PlayOneShot(clip);
    }

}
