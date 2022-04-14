using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBuild : MonoBehaviour
{


    [SerializeField]
    GameObject[] shopButtons;
    GameObject target;
    GameObject tmpSelection;
    GameObject textBoxPanel;

    [SerializeField]
    GameObject[] visualWeapons;
    [SerializeField]
    SOActorModel defaultPlayerShip;
    GameObject playerShip;
    GameObject buyButton;
    GameObject bankObj;
    int bank = 600;
    bool purchaseMode = false;
    // Start is called before the first frame update
    void Start()
    {
        TurnOffSelectionHighlights();
        textBoxPanel = GameObject.Find("textBoxPanel");

        purchaseMode = false;
        bankObj = GameObject.Find("bank");
        bankObj.GetComponentInChildren<TextMesh>().text = bank.ToString();
        buyButton = textBoxPanel.transform.Find("BUY ?").gameObject;

        TurnOffPlayershipVisuals();
        PreparePlayerShipForUpgrade();
    }

    // Update is called once per frame
    void Update()
    {
        AttemptSeletion();
    }
    void TurnOffSelectionHighlights()
    {
        for(int i = 0; i < shopButtons.Length; i++)
        {
            shopButtons[i].SetActive(false);
        }
    }

    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray.origin,ray.direction * 100, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }

    void AttemptSeletion()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            target = ReturnClickedObject(out hitInfo);

            if(target != null)
            {
                if (target.transform.Find("itemText"))
                {
                    TurnOffSelectionHighlights();
                    Select();
                    UpdateDescriptionBox();

                    // not alreadt sold
                    if (target.transform.Find("itemText").GetComponent<TextMesh>().text != "SOLD")
                    {
                        // Can afford
                        Affordable();
                        // Lack of Credit
                        LackOfCredits();
                    }
                    else if (target.transform.Find("itemText").GetComponent<TextMesh>().text == "SOLD")
                    {
                        SoldOut();
                    }
                }
                else if (target.name == "WATCH AD")
                {
                    WatchAdvert();
                }
                else if (target.name == "BUY ?")
                {
                    BuyItem();
                }
                else if (target.name == "START")
                {
                    StartGame();
                }
            }
        }
    }

    void Select()
    {
        tmpSelection = target.transform.Find("SelectionQuad").gameObject;
        tmpSelection.SetActive(true);
    }

    void UpdateDescriptionBox()
    {
        textBoxPanel.transform.Find("name").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponentInParent<ShopPiece>().ShopSelection.iconName;
        textBoxPanel.transform.Find("desc").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponentInParent<ShopPiece>().ShopSelection.description;
    }

    void TurnOffPlayershipVisuals()
    {
        for(int i = 0; i < visualWeapons.Length; i++)
        {
            visualWeapons[i].gameObject.SetActive(false);
        }
    }
    void PreparePlayerShipForUpgrade()
    {
        playerShip = GameObject.Instantiate(Resources.Load("Prefab/Player/Player_Ship")) as GameObject;
        playerShip.GetComponent<Player>().enabled = false;
        playerShip.transform.position = new Vector3(0, 10000, 0);
        playerShip.GetComponent<IActorTemplate>().ActorStats(defaultPlayerShip);
    }

    void Affordable()
    {
        if(bank >= System.Int32.Parse(target.transform.GetComponent<ShopPiece>().ShopSelection.cost))
        {
            Debug.Log("Can buy");
            buyButton.SetActive(true);
        }
    }

    void LackOfCredits()
    {
        if (bank < System.Int32.Parse(target.transform.GetComponent<ShopPiece>().ShopSelection.cost))
        {
            Debug.Log("Can't buy");
            
        }
    }

    void SoldOut() {
        Debug.Log("Sold Out"); }

    void WatchAdvert()
    {
        
    }
    void BuyItem()
    {
        Debug.Log("PURCHASED");
        purchaseMode = true;
        buyButton.SetActive(false);
        tmpSelection.SetActive(false);

        for(int i = 0; i < visualWeapons.Length; i++)
        {
            if(visualWeapons[i].name == tmpSelection.transform.parent.gameObject.GetComponent<ShopPiece>().ShopSelection.iconName)
            {
                visualWeapons[i].SetActive(true);
            }
        }
        UpgrateToShip(tmpSelection.transform.parent.gameObject.GetComponent<ShopPiece>().ShopSelection.iconName);

        bank = bank - System.Int32.Parse(tmpSelection.transform.parent.gameObject.GetComponent<ShopPiece>().ShopSelection.cost);
        bankObj.transform.Find("bankText").GetComponent<TextMesh>().text = bank.ToString();
        tmpSelection.transform.parent.transform.Find("itemText").GetComponent<TextMesh>().text = "SOLD";

    }

    void UpgrateToShip(string upgrade)
    {
        GameObject shipItem = GameObject.Instantiate(Resources.Load("Prefab/Player/" + upgrade)) as GameObject;
        shipItem.transform.SetParent(playerShip.transform);
        shipItem.transform.localPosition = Vector3.zero;
    }
    void StartGame()
    {
        if (purchaseMode)
        {
            playerShip.name = "UpgradedShip";
            if(playerShip.transform.Find("energy +1(Clone)"))
            {
                playerShip.GetComponent<Player>().Health = 2;
            }
            DontDestroyOnLoad(playerShip);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestLevel");
    }
}
