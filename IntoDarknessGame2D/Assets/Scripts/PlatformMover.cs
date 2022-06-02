using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    // Empty objects to use as points of travel
    public GameObject objStartPos;
    public GameObject objEndPos;

    // Speed of moving platform
    public float platSpeed = 3f;

    // Positions that platform should start and end on 
    private Vector2 startPos;
    private Vector2 endPos;
    private bool buttonClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the start and end position from empty objects in game
        startPos = (Vector2)objStartPos.transform.position;
        endPos = (Vector2)objEndPos.transform.position;

        // Sets the platforms starting position to that of the startPos object
        if ((Vector2)transform.position != startPos)
        {
            transform.position = startPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((Vector2)transform.position == endPos && buttonClicked)
        {
            StartCoroutine(LerpPosition(gameObject, startPos, platSpeed));
        }
        else if ((Vector2)transform.position == startPos && buttonClicked)
        {
            StartCoroutine(LerpPosition(gameObject, endPos, platSpeed));
        }
    }

    IEnumerator LerpPosition(GameObject platform, Vector2 targetPosition, float speed)
    {
        float time = 0f;
        Vector2 startPosition = platform.transform.position;

        while ((Vector2)platform.transform.position != targetPosition)
        {
            platform.transform.position = Vector2.Lerp(startPosition, targetPosition, time / speed);
            time += Time.deltaTime;
            yield return null;
        }
        platform.transform.position = targetPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.transform.SetParent(gameObject.transform, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.transform.parent = null;
    }

    public bool ButtonClicked()
    {
        buttonClicked = true;
        Debug.Log("clicked");
        return buttonClicked;
    }

    public void Activate()
    {
        ButtonClicked();
        Debug.Log("Activated");
    }

    //Draws lines between gizoms to show path
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(objStartPos.transform.position, objEndPos.transform.position);
    }
}
