using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUI : MonoBehaviour
{

    CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeUIIn(float _seconds)
    {
        StartCoroutine(FadeIn(_seconds));
    }

    public void FadeUIOut(float _seconds)
    {
        StartCoroutine(FadeOut(_seconds));
    }

    IEnumerator FadeOut(float _seconds)
    {
        
        Debug.Log("FadeOut");
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / _seconds;
            yield return null;
        }
        yield return null;

    }
    
IEnumerator FadeIn(float _seconds)
{
    Debug.Log("FadeIn");
    canvasGroup.alpha = 0;
    canvasGroup.interactable = false; // Asegúrate de que no sea interactuable mientras está completamente transparente.
    canvasGroup.blocksRaycasts = false;
    while (canvasGroup.alpha < 1)
    {
        canvasGroup.alpha += Time.deltaTime / _seconds;
        yield return null;
    }
    canvasGroup.interactable = true; // Restablece a interactuable después de hacerse visible.
    canvasGroup.blocksRaycasts = true; // Permite el bloqueo de raycasts nuevamente.
}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
