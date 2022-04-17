using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class PauseComponent : MonoBehaviour
{
    [SerializeField]
    GameObject pauseScreen;
    [SerializeField]
    AudioMixer masterMixer;
    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    Slider effectsSlider;

    // Start is called before the first frame update
    private void Awake()
    {
        musicSlider.GetComponent<Slider>().value = GetMusicLevelFromMixer();
        effectsSlider.GetComponent<Slider>().value = GetEffectsLevelFromMixer();
        masterMixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
        masterMixer.SetFloat("effectsVol", PlayerPrefs.GetFloat("effectsVol"));
        pauseScreen.SetActive(false);
        SetPauseButtonActive(false);
        Invoke("DelayPauseAppear", 5);


    }

    


    public void SetMusicLevelFromSlider()
    {
        masterMixer.SetFloat("musicVol", musicSlider.GetComponent<Slider>().value);
        PlayerPrefs.SetFloat("musicVol", musicSlider.GetComponent<Slider>().value);
    }

    public void SetEffectsLevelFromSlider()
    {
        masterMixer.SetFloat("effectsVol", effectsSlider.GetComponent<Slider>().value);
        PlayerPrefs.SetFloat("effectsVol", effectsSlider.GetComponent<Slider>().value);
    }

    float GetMusicLevelFromMixer()
    {
        float musicMixersValue;
        bool result = masterMixer.GetFloat("musicVol", out musicMixersValue);
        if (result)
        {
            return musicMixersValue;
        }
        else
        {
            return 0;
        }
    }
    float GetEffectsLevelFromMixer()
    {
        float effectsMixersValue;
        bool result = masterMixer.GetFloat("effectsVol", out effectsMixersValue);
        if (result)
        {
            return effectsMixersValue;
        }
        else
        {
            return 0;
        }
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
        pauseScreen.SetActive(true);
        SetPauseButtonActive(false);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        SetPauseButtonActive(true);
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        pauseScreen.SetActive(false);
        SetPauseButtonActive(true);
        Time.timeScale = 1;
        GameManager.Instance.GetComponent<ScenesManager>().BeginGame(1);
    }
}
