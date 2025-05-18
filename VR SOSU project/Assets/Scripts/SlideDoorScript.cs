using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class SlideDoorScript : MonoBehaviour
{
    public GameObject SlidingDoor1;
    public GameObject SlidingDoor2;


    public float speed = 0.8f;
    public float MaxTimer = 1f;

    
    private bool opening = false;
    private bool closing = false;

    private bool open = false;
    private bool closed = true;

    private void Update()
    {
        if (open && closed)
        {
            open = false;
            closed = false;
        }
        
    }

    private void FixedUpdate()
    {
        if (opening)
        {
            
            SlidingDoor1.transform.Translate(Vector3.down * speed * Time.deltaTime);
            SlidingDoor2.transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (closing )
        {
            
            SlidingDoor1.transform.Translate(Vector3.up * speed * Time.deltaTime);
            SlidingDoor2.transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        
        
    }
    
    private void AntiOpen()
    {
        
        if (opening)
        {
            opening = false;
            
            open = true;
        }
        
    }
    private void AntiClosed()
    {
        if (closing)
        {
            closing = false;
            
            closed = true;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && closed)
        {
            closed = false;
            opening = true;
            
            Invoke("AntiOpen", 1);


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && open)
        {
            open = false;
            closing = true;
            Invoke("AntiClosed", 1);

        }
    }





}
