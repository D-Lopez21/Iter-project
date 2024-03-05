using UnityEngine;
using UnityEngine.Rendering;

public class ColorEffectToggle : MonoBehaviour
{
    public Volume colorVolume; // Asigna tu Volume desde el Inspector

    public void ToggleColorEffect()
    {
        if (colorVolume != null)
        {
            colorVolume.enabled = !colorVolume.enabled; // Alterna el estado de activación
        }
    }
}
