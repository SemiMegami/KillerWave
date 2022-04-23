using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static int currentScene = 0;
    public static int gameLevelScene = 3;
    bool died = false;

    public static int playerLives = 3;
    public bool Died
    {
        get { return died; }
        set { died = value; }
    }

    static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }
    // Start is called before the first frame update

    void Awake()
    {
        CheckGameManagerIsInTheScene();
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        LightAndCameraSetup(currentScene);
    }

    private void Start()
    {
        SetliveDisplay(playerLives);
    }

    void CheckGameManagerIsInTheScene()
    {

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);

    }

 
    void CameraSetup(float camSpeed)
    {
        GameObject gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
        gameCamera.transform.position = new Vector3(0, 0, -300);
        gameCamera.transform.eulerAngles = new Vector3(0, 0, 0);
        gameCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        gameCamera.GetComponent<Camera>().backgroundColor = new Color32(0,0,0,255);
        gameCamera.GetComponent<CameraMovement>().CamSpeed = camSpeed;
    }

    void LightSetUp()
    {
        if(GameObject.Find("Directional Light")){
            GameObject dirLight = GameObject.Find("Directional Light");
            dirLight.transform.eulerAngles = new Vector3(50, -30, 0);
            dirLight.GetComponent<Light>().color = new Color32(152, 204, 255, 255);
        }
       
    }

    void LightAndCameraSetup(int sceneNumber)
    {
        switch (sceneNumber)
        {
            case 3: // level1
            case 4: // level2
                CameraSetup(0);
                LightSetUp();
                break;
            case 5: // level3
                CameraSetup(150);
                LightSetUp();
                break;
        }
    }

    //lose life
    public void LifeLost()
    {
        if(playerLives >= 1)
        {
            playerLives--;
            Debug.Log("Lives left: " + playerLives);
            GetComponent<ScenesManager>().ResetScene();
        }
        else
        {
            playerLives = 3;
            GetComponent<ScenesManager>().GameOver();
        }
    }

    public void SetliveDisplay(int players)
    {
        if (GameObject.Find("lives"))
        {
            GameObject lives = GameObject.Find("lives");
            for(int i = 0; i < 5; i++)
            {
                GameObject life = GameObject.Instantiate(Resources.Load("Prefab/life")) as GameObject;
                life.transform.SetParent(lives.transform);
            }

            for(int i = 0; i < lives.transform.childCount; i++)
            {
                lives.transform.GetChild(i).localScale = new Vector3(1, 1, 1);
            }
            for (int i = 0; i < lives.transform.childCount - players; i++)
            {
                lives.transform.GetChild(lives.transform.childCount - i - 1).localScale = Vector3.zero;
            }
        }
    }
}
