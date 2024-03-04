using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveSystem : MonoBehaviour
{
    public PlayerController player;

    // Método para guardar las variables
    public void SavePlayer()
    {
        if(PlayerController.Instance!= null){
        Debug.Log(PlayerController.Instance);
        PlayerPrefs.SetInt("Health", PlayerController.Instance.Health);
        PlayerPrefs.SetInt("Mana", PlayerController.Instance.Mana);
        PlayerPrefs.SetInt("Experience", PlayerController.Instance.Exp);
        PlayerPrefs.SetInt("CurrentLevel", PlayerController.Instance.currentLevel);
        PlayerPrefs.SetInt("SkillPoints", PlayerController.Instance.skillPoints);
        PlayerPrefs.SetInt("MaxHealth", PlayerController.Instance.maxHealth);
        PlayerPrefs.SetInt("MaxMana", PlayerController.Instance.maxMana);
        PlayerPrefs.SetInt("Damage", (int)PlayerController.Instance.damage);
        PlayerPrefs.SetInt("MaxLevel", PlayerController.Instance.maxLevel);
        PlayerPrefs.SetInt("ExpNextLevel", PlayerController.Instance.expNextLevel);

        // Guardar el estado de las habilidades desbloqueadas
        PlayerPrefs.SetInt("UnlockedDash", PlayerController.Instance.unlockedDash ? 1 : 0);
        PlayerPrefs.SetInt("UnlockedDoubleJump", PlayerController.Instance.unlockedDoubleJump ? 1 : 0);
        PlayerPrefs.SetInt("UnlockedWallJump", PlayerController.Instance.unlockedWallJump ? 1 : 0);

        Debug.Log("Game data saved!");
        } else {
            Debug.Log("No player found.");
        }

    }

    // Método para reiniciar las variables a sus valores predeterminados
    public void ResetPlayer()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        PlayerPrefs.DeleteAll();
        Debug.Log("Game data reset!");
    }

    // Opcional: Cargar los datos al iniciar el juego / componente
    void Start()
    {
        if (PlayerPrefs.HasKey("Health"))
        {
            // Cargar los datos guardados
            player.Health = PlayerPrefs.GetInt("Health");
            player.Mana = PlayerPrefs.GetInt("Mana");
            // Asegúrate de cargar el resto de las variables de manera similar
        }
        else
        {
            Debug.Log("No saved data found.");
        }
    }
}
