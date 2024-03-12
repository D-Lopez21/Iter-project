using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDash : MonoBehaviour
{

    [SerializeField] GameObject particles;
    [SerializeField] GameObject canvasUI;
    CanvasGroup canvasGroup;
    bool used;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerController.Instance.unlockedDash){
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
        
        PlayerController.Instance.unlockedDash = true;
        canvasUI.SetActive(false);
        PlayerController.Instance.pState.activeUI = false;
        Destroy(gameObject);

    }

}
