using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>();
        switch(PlayerController.Instance.currentWeapon){

            case 1:
                transform.localScale = new Vector2(2.3f, 1f);
                this.GetComponent<SpriteRenderer>().color = new Color32(254, 212, 82, 255);
                break;
            case 2:
                transform.localScale = new Vector2(1.3f, 1.4f);
                this.GetComponent<SpriteRenderer>().color = new Color32(21, 142, 211, 255);
                break;
            case 4:
                transform.localScale = new Vector2(0.4f, 0.6f);
                this.GetComponent<SpriteRenderer>().color = new Color32(227, 29, 29, 255);
                break;
            case 5:
                transform.localScale = new Vector2(1.6f, 1.1f);
                this.GetComponent<SpriteRenderer>().color = new Color32(226, 201, 29, 255);
                break;

        }
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
