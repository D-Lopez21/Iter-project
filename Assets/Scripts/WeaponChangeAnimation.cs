using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChangeAnimation : MonoBehaviour
{
    [Header("Sprite objects")]
    [SerializeField] public GameObject spriteBasic;
    [SerializeField] public GameObject spriteLong;
    [SerializeField] public GameObject spriteStaff;
    [SerializeField] public GameObject spriteBow;
    [SerializeField] public GameObject spriteGauntlet;
    [SerializeField] public GameObject spriteSecret;

    private GameObject[] spriteList;

    public static WeaponChangeAnimation Instance;

    void Awake(){
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }
        spriteList = new GameObject[6];
        spriteList[0] = spriteBasic;
        spriteList[1] = spriteLong;
        spriteList[2] = spriteStaff;
        spriteList[3] = spriteBow;
        spriteList[4] = spriteGauntlet;
        spriteList[5] = spriteSecret;
    }

    public void ActivateAnimation(int _weaponNumb){

        stopAll();
        StartCoroutine(ChangeAnimation(_weaponNumb));
        
    }

    void stopAll(){
        for(int i = 0; i < spriteList.Length; i++){
            spriteList[i].SetActive(false);
        }
    }


    IEnumerator ChangeAnimation(int _weaponNumb){
    
        //Fade In
        spriteList[_weaponNumb].SetActive(true);
        Color tmp = spriteList[_weaponNumb].GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        spriteList[_weaponNumb].GetComponent<SpriteRenderer>().color = tmp; 

        while(tmp.a < 1f){
            tmp.a += 0.03f;

            if(tmp.a > 1f){
                tmp.a = 1f;
            }

            spriteList[_weaponNumb].GetComponent<SpriteRenderer>().color = tmp;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        //Fade Out

        while(tmp.a > 0){
            tmp.a -= 0.03f;

            if(tmp.a < 0){
                tmp.a = 0;
            }

            spriteList[_weaponNumb].GetComponent<SpriteRenderer>().color = tmp;
            yield return null;
        }

        spriteList[_weaponNumb].SetActive(false);

    }  
    IEnumerator FadeOut(int _weaponNumb, Color _temp){
        yield return new WaitForSeconds(1);

        while(_temp.a > 0){
            _temp.a -= 0.1f;

            if(_temp.a < 0){
                _temp.a = 0;
            }

            spriteList[_weaponNumb].GetComponent<SpriteRenderer>().color = _temp;
            yield return null;
        }

        spriteList[_weaponNumb].SetActive(false);
    }


}
