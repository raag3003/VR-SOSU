using UnityEngine;

public class MicTester : MonoBehaviour
{

        void Start()
        {
            foreach (var mic in Microphone.devices)
            {
                Debug.Log("Mic detected: " + mic);
            }
        }
}
