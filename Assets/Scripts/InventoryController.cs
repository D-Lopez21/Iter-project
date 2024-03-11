using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] public Button bBasic;
    [SerializeField] public Button bLong;
    [SerializeField] public Button bStaff;
    [SerializeField] public Button bBow;
    [SerializeField] public Button bGauntlet;
    [SerializeField] public Button bSecret;

    public Button bPrevious;
    private Button bCurrent;

    [Header("Button Images")]
    [SerializeField] public Image basicSword;
    [SerializeField] public Image longSword;
    [SerializeField] public Image staff;
    [SerializeField] public Image bow;
    [SerializeField] public Image gauntlet;
    [SerializeField] public Image secret;
    [Space(5)]

    [Header("Inventory Text")]
    [SerializeField] TMP_Text weaponName;
    [SerializeField] TMP_Text weaponDescription;
    [SerializeField] TMP_Text arrowCount;
    [SerializeField] TMP_Text healthCount;
    [SerializeField] TMP_Text manaCount;
    [Space(5)]

    CanvasGroup canvasGroup;
    private bool activeTransition = false;

    public static InventoryController Instance;

    PlayerController playerObj;

    public void awake(){
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerObj = PlayerController.Instance;
        canvasGroup = GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    void Update()
    {
        checkInventory();
    }

    void checkInventory(){
        if(Input.GetButtonDown("Inventory") && !activeTransition){
            if(!playerObj.pState.inventoryActive){
                playerObj.pState.inventoryActive = true;
                playerObj.rb.velocity = new Vector2(0, 0);
                playerObj.rb.gravityScale = 0;

                StartCoroutine(FadeIn(0.2f));

            }else{
                playerObj.pState.inventoryActive = false;
                playerObj.rb.gravityScale = playerObj.gravity;
                StartCoroutine(FadeOut(0.2f));
            }

        }
    }

    IEnumerator FadeIn(float _seconds){
        activeTransition = true;
        canvasGroup.alpha = 0;
        while(canvasGroup.alpha < 1){
            canvasGroup.alpha += Time.unscaledDeltaTime / _seconds;
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        activeTransition = false;
        yield return null;
    }

    IEnumerator FadeOut(float _seconds){
        activeTransition = true;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1;
        while(canvasGroup.alpha > 0){
            canvasGroup.alpha -= Time.unscaledDeltaTime / _seconds;
            yield return null;
        }
        activeTransition = false;
        yield return null;
    }

    public void changeUIWeapon(int _weaponNumb){
        if(playerObj.weaponList[_weaponNumb]){
            switch(_weaponNumb){

                case 0:
                    weaponName.text = "Espada";
                    weaponDescription.text = "Arma basica y confiable";
                    bCurrent = bBasic;
                    break;

                case 1:
                    weaponName.text = "Espada Larga";
                    weaponDescription.text = "Arma de gran alcance y daño pero ataques lentos";
                    bCurrent = bLong;
                    break;

                case 2:
                    weaponName.text = "Baston";
                    weaponDescription.text = "Gran alcance y proyectiles, pero poco daño";
                    bCurrent = bStaff;
                    break;

                case 3:
                    weaponName.text = "Arco";
                    weaponDescription.text = "Dispara proyectiles, pero utiliza municion";
                    bCurrent = bBow;
                    break;

                case 4:
                    weaponName.text = "Guantes";
                    weaponDescription.text = "Gran daño y velocidad pero poco alcance";
                    bCurrent = bGauntlet;
                    break;

                case 5:
                    weaponName.text = "Secreto";
                    weaponDescription.text = "Goku?";
                    bCurrent = bSecret;
                    break;

            }
            bPrevious.GetComponent<Image>().color = Color.gray;
            bCurrent.GetComponent<Image>().color = Color.cyan;
            bPrevious = bCurrent;

        }else{
            weaponName.text = "Bloqueado";
            weaponDescription.text = "No has encontrado esta arma";
        }
    }

}
