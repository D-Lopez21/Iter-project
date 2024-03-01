using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{

    BoxCollider2D stationColli;

    // Start is called before the first frame update
    void Start()
    {
        stationColli = GetComponent<BoxCollider2D>();
        Debug.Log(stationColli);    
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
        if(stationColli.IsTouching(PlayerController.Instance.colli) && Input.GetButtonDown("SkillTree") && PlayerController.Instance.skillPoints > 0 && !PlayerController.Instance.skillTreeActive){
            PlayerController.Instance.skillTreeActive = true;
            Debug.Log("Mejoras Activas");
        }
    }

}
