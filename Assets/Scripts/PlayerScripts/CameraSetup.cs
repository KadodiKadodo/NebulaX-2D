using Unity.Cinemachine;
using UnityEngine;

namespace PlayerScripts
{
    public class CameraSetup : MonoBehaviour
    {
        private CinemachineCamera vcam;

        void Start()
        {
            vcam = GetComponent<CinemachineCamera>();
        }

        void LateUpdate()
        {
            // If the camera doesn't have a target, or the target is destroyed/missing
            if (vcam != null && vcam.Target.TrackingTarget == null)
            {
                // Find the player
                GameObject player = GameObject.FindGameObjectWithTag("Player");
            
                if (player != null)
                {
                    vcam.Target.TrackingTarget = player.transform;
                    Debug.Log("Camera found the player and locked on!");
                }
            }
        }
    }
}