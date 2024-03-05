using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider healthSlider;
    public TMP_Text healthBarText;

    public Slider manaSlider;
    public TMP_Text manaBarText;

    public Slider expSlider;
    public TMP_Text expBarText;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(PlayerController.Instance.Health, PlayerController.Instance.maxHealth);
        healthBarText.text = "HP " + PlayerController.Instance.Health + " / " + PlayerController.Instance.maxHealth; 
        manaSlider.value = CalculateSliderPercentage(PlayerController.Instance.Mana, PlayerController.Instance.maxMana);
        manaBarText.text = "Mana " + PlayerController.Instance.Mana + " / " + PlayerController.Instance.maxMana;
        expSlider.value = CalculateSliderPercentage(PlayerController.Instance.Exp, PlayerController.Instance.expNextLevel);
        expBarText.text = "XP " + PlayerController.Instance.Exp + " / " + PlayerController.Instance.expNextLevel + " Nivel: " + PlayerController.Instance.currentLevel;
        
        PlayerController.Instance.healthChanged.AddListener(OnPlayerHealthChanged);
        PlayerController.Instance.manaChanged.AddListener(OnPlayerManaChanged);
        PlayerController.Instance.expChanged.AddListener(OnPlayerExpChanged);
    }

    private void OnEnable()
    {
        PlayerController.Instance.healthChanged.AddListener(OnPlayerHealthChanged);
        PlayerController.Instance.manaChanged.AddListener(OnPlayerManaChanged);
        PlayerController.Instance.expChanged.AddListener(OnPlayerExpChanged);
    }

    private void OnDisable()
    {
        PlayerController.Instance.healthChanged.RemoveListener(OnPlayerHealthChanged);
        PlayerController.Instance.manaChanged.RemoveListener(OnPlayerManaChanged);
        PlayerController.Instance.expChanged.RemoveListener(OnPlayerExpChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float CalculateSliderPercentage(float currentStat, float maxStat){
        return currentStat / maxStat;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP " + newHealth + " / " + maxHealth; 
            if(newHealth <= 0)
    {
        // Aquí puedes cargar la escena de "Game Over" o el menú principal
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Death Menu");
    }
        
    }

    private void OnPlayerManaChanged(int newMana, int maxMana){
        manaSlider.value = CalculateSliderPercentage(newMana, maxMana);
        manaBarText.text = "Mana " + newMana + " / " + maxMana;
    }

    private void OnPlayerExpChanged(int newExp, int maxExp){
        expSlider.value = CalculateSliderPercentage(newExp, maxExp);
        expBarText.text = "XP " + newExp + " / " + maxExp + " Nivel: " + PlayerController.Instance.currentLevel;
    }
    
}
