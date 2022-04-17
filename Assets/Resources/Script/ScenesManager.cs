using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{

    float gameTimer = 0;
    float[] endLevelTimer = { 30, 30, 45 };
    int curentSceneNumber = 0;
    bool gameEnding = false;
    Scenes scenes;
    public enum Scenes
    {
        bootUp,
        titile,
        shop,
        level1,
        level2,
        level3,
        gameOver
    }

    public MusicMode musicMode;
    public enum MusicMode
    {
        noSound,fadeDown,musicOn
    }
    // Start is called before the first frame update

    private void Start()
    {
        StartCoroutine(MusicVolume(MusicMode.musicOn));
        SceneManager.sceneLoaded += OnsceneLoaded;
    }
    private void Update()
    {
        if(curentSceneNumber != SceneManager.GetActiveScene().buildIndex)
        {
            curentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            GetScene();
        }
        GameTimer();
    }

    public void ResetScene()
    {
        StartCoroutine(MusicVolume(MusicMode.noSound));
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("gameOver");
        Debug.Log("EndScore: " + GameManager.Instance.GetComponent<ScoreManager>().PlayerScore);
    }

    public void BeginGame(int gameLevel)
    {
        SceneManager.LoadScene(gameLevel);
    }
    
    void GetScene()
    {
        scenes = (Scenes)curentSceneNumber;
    }

    void GameTimer()
    {
        switch (scenes)
        {
            case Scenes.level1: case Scenes.level2: case Scenes.level3:
                if(GetComponentInChildren<AudioSource>().clip == null)
                {
                    AudioClip lvlMusic = Resources.Load<AudioClip>("Sound/lvlMusic") as AudioClip;
                    GetComponentInChildren<AudioSource>().clip = lvlMusic;
                    GetComponentInChildren<AudioSource>().Play();
                }
                if(gameTimer < endLevelTimer[curentSceneNumber - 3])
                {
                    gameTimer += Time.deltaTime;
                }
                else
                {
                    StartCoroutine(MusicVolume(MusicMode.fadeDown));
                    if (!gameEnding)
                    {
                        gameEnding = true;
                        if(SceneManager.GetActiveScene().name!= "level3")
                        {
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().LevelEnds = true;
                        }
                        else
                        {
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().GameCompleted = true;
                        }
                        SendInJsonFormat(SceneManager.GetActiveScene().name);
                        Invoke("NextLevel", 4);
                    }
                }
                break;
        }
    }

    void SendInJsonFormat(string lastLevel)
    {
        if(lastLevel == "level3")
        {
            GameStats gameStats = new GameStats();
            gameStats.livesLeft = GameManager.playerLives;
            gameStats.completed = System.DateTime.Now.ToString();
            gameStats.score = ScoreManager.playerScore;
            string json = JsonUtility.ToJson(gameStats);
            Debug.Log(json);

            Debug.Log(Application.persistentDataPath + "/GameStatsSaved.json");
            System.IO.File.WriteAllText(Application.persistentDataPath + "/GameStatsSaved.json", json);
        }
    }
    void NextLevel()
    {
        
        gameEnding = false;
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene + 1);
        StartCoroutine(MusicVolume(MusicMode.musicOn));
    }

    void OnsceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        StartCoroutine(MusicVolume(MusicMode.musicOn));
        GetComponent<GameManager>().SetliveDisplay(GameManager.playerLives);

        if (GameObject.Find("score"))
        {
            GameObject.Find("score").GetComponent<Text>().text =  ScoreManager.playerScore.ToString();
        }
        
    }

    IEnumerator MusicVolume(MusicMode musicMode)
    {
        switch (musicMode)
        {
            case MusicMode.noSound:
                GetComponentInChildren<AudioSource>().volume = 0;
                break;
            case MusicMode.fadeDown:
                GetComponentInChildren<AudioSource>().volume -= Time.deltaTime / 3;
                break;
            case MusicMode.musicOn:
                if(GetComponentInChildren<AudioSource>().clip != null)
                {
                    GetComponentInChildren<AudioSource>().Play();
                    GetComponentInChildren<AudioSource>().volume = 1;

                }
                break;
            default:
                GetComponentInChildren<AudioSource>().clip = null;
                break;
        }
        yield return new WaitForSeconds(0.1f);
    }
}
