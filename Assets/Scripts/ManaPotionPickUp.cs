using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotionPickUp : MonoBehaviour
{
    [SerializeField] GameObject particles;
    CanvasGroup canvasGroup;
    bool used;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D _collision){
        if(_collision.CompareTag("Player") && !used){
            used = true;
            GameObject _particles = Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(_particles, 0.2f);

            PlayerController.Instance.manaPotions += 1;
            Destroy(gameObject);
        }
    }

}

