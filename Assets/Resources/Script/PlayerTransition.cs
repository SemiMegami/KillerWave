using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransition : MonoBehaviour
{
    Vector3 transitionToEnd = new Vector3(-100, 0, 0);
    Vector3 transitionToCompleteGame = new Vector3(7000, 0, 0);
    Vector3 readyPos = new Vector3(900, 0, 0);
    Vector3 startPos;
    float distCovered;
    float journeyLenght;
    bool levelStarted = true;
    bool speedOff = false;
    bool levelEnds = false;
    bool gameCompleted = false;
    public bool LevelEnds
    {
        get { return levelEnds; }
        set { levelEnds = value; }
    }
    public bool GameCompleted
    {
        get { return gameCompleted; }
        set { gameCompleted = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localPosition = Vector3.zero;
        startPos = transform.position;
        Distance();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelStarted)
        {
            StartCoroutine(PlayerMovement(transitionToEnd,10));
        }
        if (levelEnds)
        {
            GetComponent<Player>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            Distance();
            StartCoroutine(PlayerMovement(transitionToEnd, 200));
        }
        if (gameCompleted)
        {
            GetComponent<Player>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(PlayerMovement(transitionToCompleteGame, 200));
        }
        if (speedOff)
        {
            Invoke("SpeedOff", 1f);
        }
    }

    void Distance()
    {
        journeyLenght = Vector3.Distance(startPos, readyPos);
    }

    IEnumerator PlayerMovement(Vector3 point,float translationSpeed)
    {
       // print("readyPos : " + readyPos + ", position" + transform.localPosition);
        if (Mathf.Round(transform.localPosition.x) >= point.x -5 &&
            Mathf.Round(transform.localPosition.x) <= point.x + 5 &&
            Mathf.Round(transform.localPosition.y) >= point.y - 5 &&
            Mathf.Round(transform.localPosition.y) <= point.y + 5)
        {
           
            if (levelEnds)
            {
                levelEnds = false;
                speedOff = true;
            }
            if (levelStarted)
            {
                levelStarted = false;
                distCovered = 0;
                GetComponent<Player>().enabled = true;
            }
            yield return null;
        }
        else
        {
            distCovered += Time.deltaTime * translationSpeed;
            float fractionOfJourney = distCovered / journeyLenght;
            transform.position = Vector3.Lerp(transform.position, point, fractionOfJourney);
        }
    }

    void SpeedOff()
    {
        transform.Translate(Vector3.left * Time.deltaTime * 800);
    }
}
