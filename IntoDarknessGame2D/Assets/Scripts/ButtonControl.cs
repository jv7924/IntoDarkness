using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonControl : MonoBehaviour
{
    [SerializeField]
    private bool inRange = false;

    // Custom unity event system for platform activation using button
    public UnityEvent OnButtonClick;
    private GameObject interactGui;
    private AudioSource buttonSound;

    void Start(){
        buttonSound = GetComponent<AudioSource>();
        interactGui = Instantiate(Resources.Load("InteractGui")) as GameObject;
        interactGui.transform.position = gameObject.transform.position + new Vector3(0, 1.25f, 0);
        interactGui.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if player character is in range and has pressed F key
        if (Input.GetKeyDown(KeyCode.F) && inRange)
        {
            if (OnButtonClick != null)
            {
                // Invokes the function set in inspector
                OnButtonClick.Invoke();
                buttonSound.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Makes sure player is in range of button trigger
        if (collision.CompareTag("Player"))
        {
            inRange = true;
            interactGui.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Makes sure player is out of range of button trigger
        if (collision.CompareTag("Player"))
        {
            inRange = false;
            interactGui.SetActive(false);
        }
    }
}
