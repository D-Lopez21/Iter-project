using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockBow : MonoBehaviour
{
    [SerializeField] GameObject particles;
    [SerializeField] GameObject canvasUI;
    CanvasGroup canvasGroup;
    bool used;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerController.Instance.unlockedBow){
            Destroy(gameObject);
        }
        canvasGroup = canvasUI.GetComponent<CanvasGroup>();
    }

    private void OnTriggerEnter2D(Collider2D _collision){
        if(_collision.CompareTag("Player") && !used){
            used = true;
            StartCoroutine(ShowUI());
        }
    }

    IEnumerator ShowUI(){
        PlayerController.Instance.pState.activeUI = true;
        InventoryController.Instance.bow.color = Color.white;
        GameObject _particles = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(_particles, 0.5f);
        yield return new WaitForSeconds(0.1f);
        canvasUI.SetActive(true);

        while(canvasGroup.alpha < 1){
            canvasGroup.alpha += Time.unscaledDeltaTime / 0.2f;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        while(canvasGroup.alpha > 0){
            canvasGroup.alpha -= Time.unscaledDeltaTime / 0.2f;
            yield return null;
        }
        
        PlayerController.Instance.unlockedBow = true;
        PlayerController.Instance.weaponList[3] = PlayerController.Instance.unlockedBow;
        canvasUI.SetActive(false);
        PlayerController.Instance.pState.activeUI = false;
        Destroy(gameObject);

    }
}
