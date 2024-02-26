using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider healthSlider;
    public TMP_Text healthBarText;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(PlayerController.Instance.Health, PlayerController.Instance.maxHealth);
        healthBarText.text = "HP " + PlayerController.Instance.Health + " / " + PlayerController.Instance.maxHealth; 
        PlayerController.Instance.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnEnable()
    {
        PlayerController.Instance.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        PlayerController.Instance.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth){
        return currentHealth / maxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP " + newHealth + " / " + maxHealth; 
    }
}
