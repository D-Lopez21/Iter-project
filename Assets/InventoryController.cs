using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    CanvasGroup canvasGroup;
    private bool activeTransition = false;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        checkInventory();
    }

    void checkInventory(){
        if(Input.GetButtonDown("Inventory") && !activeTransition){
            if(!PlayerController.Instance.InventoryActive){
                PlayerController.Instance.InventoryActive = true;
                StartCoroutine(FadeIn(0.2f));

            }else{
                PlayerController.Instance.InventoryActive = false;
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
