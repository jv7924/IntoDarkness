using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject platformPathStart;
    [SerializeField]
    private GameObject platformPathEnd;
    [SerializeField]
    private float speed;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool buttonClick = false;

    void Start()
    {
        startPosition = platformPathStart.transform.position;
        endPosition = platformPathEnd.transform.position;
        //StartCoroutine(Vector2LerpCoroutine(gameObject, endPosition, speed));
    }

    void Update()
    {
        if ((Vector2)transform.position == endPosition && buttonClick)
        {
            StartCoroutine(Vector2LerpCoroutine(gameObject, startPosition, speed));
            Debug.Log("Hi");
        }
        else if ((Vector2)transform.position == startPosition && buttonClick)
        {
            StartCoroutine( Vector2LerpCoroutine(gameObject, endPosition, speed));
            Debug.Log("Hey");
        }
    }

    IEnumerator Vector2LerpCoroutine(GameObject obj, Vector2 target, float speed)
    {
        Vector2 startPosition = obj.transform.position;
        float time = 0f;

        while ((Vector2)obj.transform.position != target)
        {
            obj.transform.position = Vector2.Lerp(startPosition, target, (time / Vector2.Distance(startPosition, target)) * speed);
            time += Time.deltaTime;
            yield return null;
        }
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            buttonClick = true;
        }
        return buttonClick;
    }

    public void Activate()
    {
        ButtonClicked();
    }
}
