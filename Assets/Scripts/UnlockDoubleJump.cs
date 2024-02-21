using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoubleJump : MonoBehaviour
{
    [SerializeField] GameObject particles;
    [SerializeField] GameObject canvasUI;
    bool used;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerController.Instance.unlockedDoubleJump){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision){
        if(_collision.CompareTag("Player") && !used){
            used = true;
            StartCoroutine(ShowUI());
        }
    }

    IEnumerator ShowUI(){
        GameObject _particles = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(_particles, 0.5f);
        yield return new WaitForSeconds(0.5f);

        canvasUI.SetActive(true);

        yield return new WaitForSeconds(4f);
        PlayerController.Instance.unlockedDoubleJump = true;
        PlayerController.Instance.maxAirJumps += 1;
        canvasUI.SetActive(false);
        Destroy(gameObject);

    }
}
