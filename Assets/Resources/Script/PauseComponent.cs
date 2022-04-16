using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseComponent : MonoBehaviour
{
    [SerializeField]
    GameObject pauseScreen;
    // Start is called before the first frame update
    private void Awake()
    {
        pauseScreen.SetActive(false);
        SetPauseButtonActive(false);
        Invoke("DelayPauseAppear", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPauseButtonActive(bool switchButton)
    {
        ColorBlock col = GetComponentInChildren<Toggle>().colors;
        if(switchButton == false)
        {
            col.normalColor = new Color32(0, 0, 0, 0);
            col.highlightedColor = new Color32(0, 0, 0, 0);
            col.pressedColor = new Color32(0, 0, 0, 0);
            col.disabledColor = new Color32(0, 0, 0, 0);
            GetComponentInChildren<Toggle>().interactable = false;
        }
        else
        {
            col.normalColor = new Color32(245,245,245,255);
            col.highlightedColor = new Color32(245, 245, 245, 255);
            col.pressedColor = new Color32(200, 200, 200, 255);
            col.disabledColor = new Color32(200, 200, 200, 255);
            GetComponentInChildren<Toggle>().interactable = true;
        }
        GetComponentInChildren<Toggle>().colors = col;
        GetComponentInChildren<Toggle>().transform.GetChild(0).GetChild(0).gameObject.SetActive(switchButton);
    }

    void DelayPauseAppear()
    {
        SetPauseButtonActive(true);
    }

    public void PauseGame()
    {
        Debug.Log("Pause");
        pauseScreen.SetActive(true);
        SetPauseButtonActive(false);
        Time.timeScale = 0;
    }
}
