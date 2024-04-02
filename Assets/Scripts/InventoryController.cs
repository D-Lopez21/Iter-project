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
    [SerializeField] TMP_Text weaponMultiplier;
    [SerializeField] TMP_Text weaponSpeed;
    [SerializeField] TMP_Text weaponSpecial;
    [SerializeField] TMP_Text weaponSpecialInfo;
    [SerializeField] TMP_Text weaponSpecialCost;
    [SerializeField] TMP_Text arrowCount;
    [SerializeField] TMP_Text healthCount;
    [SerializeField] TMP_Text manaCount;
    [Space(5)]

    int previousWeapon;
    int nextWeapon;

    CanvasGroup canvasGroup;
    private bool activeTransition = false;

    public static InventoryController Instance;

    PlayerController playerObj;

    public void Awake(){
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerObj = PlayerController.Instance;
        canvasGroup = GetComponent<CanvasGroup>();
        previousWeapon = 0;
        nextWeapon = 0;
    }

    // Update is called once per frame
    void Update()
    {
        checkInventory();
    }

    void checkInventory(){
        if(Input.GetButtonDown("Inventory") && !activeTransition && !playerObj.pState.activeUI && !playerObj.skillTreeActive){
            if(!playerObj.pState.inventoryActive){
                playerObj.pState.inventoryActive = true;
                playerObj.rb.velocity = new Vector2(0, 0);
                playerObj.rb.gravityScale = 0;

                arrowCount.text = "" + playerObj.arrowAmount;
                healthCount.text = "" + playerObj.healthPotions;
                manaCount.text = "" + playerObj.manaPotions;

                StartCoroutine(FadeIn(0.2f));

            }else{
                playerObj.pState.inventoryActive = false;
                playerObj.rb.gravityScale = playerObj.gravity;
                if(nextWeapon != previousWeapon){
                    previousWeapon = nextWeapon;
                    playerObj.ChangeWeapon(nextWeapon);
                    WeaponChangeAnimation.Instance.ActivateAnimation(nextWeapon);
                }
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
                    weaponMultiplier.text = "Daño: 1x";
                    weaponSpeed.text = "Velocidad: 0.4s";
                    weaponSpecial.text = "Especial: Corte Volador";
                    weaponSpecialInfo.text = "Proyectil que atravieza enemigos";
                    weaponSpecialCost.text = "5MP";
                    bCurrent = bBasic;
                    nextWeapon = 0;
                    break;

                case 1:
                    weaponName.text = "Espada Larga";
                    weaponDescription.text = "Arma de gran alcance y daño pero ataques lentos";
                    weaponMultiplier.text = "Daño: 1.7x";
                    weaponSpeed.text = "Velocidad: 1s";
                    weaponSpecial.text = "Especial: Rafaga Instantanea";
                    weaponSpecialInfo.text = "3 ataques en una area";
                    weaponSpecialCost.text = "10MP";
                    bCurrent = bLong;
                    nextWeapon = 1;
                    break;

                case 2:
                    weaponName.text = "Baston";
                    weaponDescription.text = "Gran alcance y proyectiles, pero poco daño";
                    weaponMultiplier.text = "Daño: 1.3x";
                    weaponSpeed.text = "Velocidad: 0.7s";
                    weaponSpecial.text = "Especial: Tormenta de Hechizos";
                    weaponSpecialInfo.text = "3 proyectiles aleatorios";
                    weaponSpecialCost.text = "2MP";
                    bCurrent = bStaff;
                    nextWeapon = 2;
                    break;

                case 3:
                    weaponName.text = "Arco";
                    weaponDescription.text = "Dispara proyectiles, pero utiliza municion";
                    weaponMultiplier.text = "Daño: 1.1x";
                    weaponSpeed.text = "Velocidad: 0.2s";
                    weaponSpecial.text = "Especial: Triple flecha";
                    weaponSpecialInfo.text = "Dispara 3 flechas por el coste de 1";
                    weaponSpecialCost.text = "3MP";
                    bCurrent = bBow;
                    nextWeapon = 3;
                    break;

                case 4:
                    weaponName.text = "Guantes";
                    weaponDescription.text = "Gran daño y velocidad pero poco alcance";
                    weaponMultiplier.text = "Daño: 1.4x";
                    weaponSpeed.text = "Velocidad: 0.05s";
                    weaponSpecial.text = "Especial: Onda vital / Patada heroica";
                    weaponSpecialInfo.text = "En el piso: gran rayo, en el aire: poderoso ataque aereo";
                    weaponSpecialCost.text = "10MP";
                    bCurrent = bGauntlet;
                    nextWeapon = 4;
                    break;

                case 5:
                    weaponName.text = "Full Bottle Buster";
                    weaponDescription.text = "\"Ya tengo la formula ganadora!\"";
                    weaponMultiplier.text = "Daño: 1.2x";
                    weaponSpeed.text = "Velocidad: 0.3s";
                    weaponSpecial.text = "Especial: Full Match Break";
                    weaponSpecialInfo.text = "Proyectil cuyo daño aumenta segun el mana";
                    weaponSpecialCost.text = "10MP min, gasta todo";
                    bCurrent = bSecret;
                    nextWeapon = 5;
                    break;

            }
            bPrevious.GetComponent<Image>().color = Color.gray;
            bCurrent.GetComponent<Image>().color = Color.cyan;
            bPrevious = bCurrent;

        }else{
            weaponName.text = "Bloqueado";
            weaponDescription.text = "No has encontrado esta arma";
            weaponMultiplier.text = "Daño: ?x";
            weaponSpeed.text = "Velocidad: ?s";
            weaponSpecial.text = "Especial: ???";
            weaponSpecialInfo.text = "?????";
            weaponSpecialCost.text = "?MP";
        }
    }

    public void usePotion(int _potion){

        switch(_potion){
            case 1:
                if(playerObj.healthPotions > 0){
                    if(playerObj.Health < playerObj.maxHealth){
                        playerObj.healthPotions--;
                        
                        if((playerObj.Health + 10) > playerObj.maxHealth){
                            playerObj.Health = playerObj.maxHealth;

                        }else{
                            playerObj.Health += 10;
                        }

                        healthCount.text = "" + playerObj.healthPotions;
                    }
                }
                break;

            case 2:
                if(playerObj.Mana < playerObj.maxMana){
                    if(playerObj.manaPotions > 0){

                    playerObj.manaPotions--;
                    
                    if((playerObj.Mana + 10) > playerObj.maxMana){
                        playerObj.Mana = playerObj.maxMana;

                    }else{
                        playerObj.Mana += 10;
                    }

                    manaCount.text = "" + playerObj.manaPotions;
                    }
                }
                break;
        }

    }

}
