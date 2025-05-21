using UnityEngine;

public class Wit_Check : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.One) || Input.GetKey(KeyCode.K))
        {
            Destroy(gameObject);
        }
    }
}
