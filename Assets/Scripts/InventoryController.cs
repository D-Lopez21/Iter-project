using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("Button Images")]
    [SerializeField] Image basicSword;
    [SerializeField] Image longSword;
    [SerializeField] Image staff;
    [SerializeField] Image bow;
    [SerializeField] Image gauntlet;
    [SerializeField] Image spellBook;
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
        Debug.Log(playerObj);
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
}
