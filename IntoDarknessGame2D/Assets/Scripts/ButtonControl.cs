using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonControl : MonoBehaviour
{
    [SerializeField]
    private bool inRange = false;

    // Custom unity event system for platform activation using button
    public UnityEvent onButtonClick;

    // Update is called once per frame
    void Update()
    {
        // Checks if player character is in range and has pressed F key
        if (Input.GetKeyDown(KeyCode.F) && inRange)
        {
            if (onButtonClick != null)
            {
                // Invokes the function set in inspector
                onButtonClick.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Makes sure player is in range of button trigger
        if (collision.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Makes sure player is out of range of button trigger
        if (collision.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
