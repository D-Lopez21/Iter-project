using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpam : MonoBehaviour
{
    [SerializeField] GameObject stone;
    [SerializeField] GameObject fire;
    [SerializeField] GameObject nerf;
    // Start is called before the first frame update
    void Start()
    {
        randomSpell(0.3f);
        randomSpell(0f);
        randomSpell(-0.3f);
        Destroy(gameObject, 3f);
    }
    
    void randomSpell(float _offset){
        float tempRandom;
        tempRandom = Random.Range(0, 10f);
        if(tempRandom >= 9f){
            Instantiate(nerf, new Vector2(transform.position.x, transform.position.y + _offset), transform.rotation);

        }else if(tempRandom >= 5f){
            Instantiate(stone, new Vector2(transform.position.x, transform.position.y + _offset), transform.rotation);
        }else{
            Instantiate(fire, new Vector2(transform.position.x, transform.position.y + _offset), transform.rotation);
        }

    }
}
