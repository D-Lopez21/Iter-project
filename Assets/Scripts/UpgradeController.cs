using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{

    [SerializeField] GameObject upgradeCanvas;
    BoxCollider2D stationColli;
    CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        stationColli = GetComponent<BoxCollider2D>();
        canvasGroup = upgradeCanvas.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        upgradeStart();
    }


//    void OnTriggerStay2D(Collider2D _collision){
//        if(_collision.CompareTag("Player") && Input.GetButtonDown("SkillTree") && PlayerController.Instance.skillPoints > 0 && !PlayerController.Instance.skillTreeActive){
//            Debug.Log("Mejoras activas");
//            PlayerController.Instance.skillTreeActive = true;
//        }
//    }

    void upgradeStart(){
        if(stationColli.IsTouching(PlayerController.Instance.colli) && Input.GetButtonDown("SkillTree") && PlayerController.Instance.skillPoints > 0 && !PlayerController.Instance.skillTreeActive && !PlayerController.Instance.pState.inventoryActive){
            PlayerController.Instance.skillTreeActive = true;
            PlayerController.Instance.rb.velocity = new Vector2(0, 0);
            upgradeCanvas.SetActive(true);
            FadeUIIn(0.2f);
        }
    }

    public void FadeUIOut(float _seconds){
        StartCoroutine(FadeOut(_seconds));
    }

    public void FadeUIIn(float _seconds){
        StartCoroutine(FadeIn(_seconds));
    }

    IEnumerator FadeOut(float _seconds){
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1;
        while(canvasGroup.alpha > 0){
            canvasGroup.alpha -= Time.unscaledDeltaTime / _seconds;
            yield return null;
        }
        upgradeCanvas.SetActive(false);
        PlayerController.Instance.skillTreeActive = false;
        yield return null;
    }

    IEnumerator FadeIn(float _seconds){
        canvasGroup.alpha = 0;
        while(canvasGroup.alpha < 1){
            canvasGroup.alpha += Time.unscaledDeltaTime / _seconds;
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        yield return null;
    }

    public void upgradeHealth(){
        PlayerController.Instance.skillPoints--;
        PlayerController.Instance.maxHealth += 10;
        PlayerController.Instance.Health += 10;
    }

    public void upgradeDamage(){
        PlayerController.Instance.skillPoints--;
        PlayerController.Instance.damage += 1;
    }

    public void upgradeMana(){
        PlayerController.Instance.skillPoints--;
        PlayerController.Instance.maxMana += 10;
        PlayerController.Instance.Mana += 10;
    }
}
