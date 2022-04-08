using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenesManager : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("gameOver");
        Debug.Log("EndScore: " + GameManager.Instance.GetComponent<ScoreManager>().PlayerScore);
    }

    public void BeginGame()
    {
        SceneManager.LoadScene("testLevel");
    }




}
