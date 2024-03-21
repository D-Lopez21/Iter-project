using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public DialogueTrigger trigger;
    
    private void OnCollisionEnter2D(Collision2D collition) {
        if(collition.gameObject.CompareTag("Player") == true){
            trigger.StartDialogue();
        }
    }
}
