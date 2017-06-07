using UnityEngine;
using System.Collections;

// Создаем экземпляр класса для звука на основе кода без усилий
public class SoundEffectsHelper : MonoBehaviour
{

    // Синглтон
    public static SoundEffectsHelper Instance;

    public AudioClip explosionSound;
    public AudioClip playerShotSound;
    public AudioClip enemyShotSound;

    void Awake()
    {
        // регистрируем синглтон
        if (Instance != null)
        {
            Debug.LogError("Несколько экземпляров SoundEffectsHelper!");
        }
        Instance = this;
    }

    public void MakeExplosionSound()
    {
        MakeSound(explosionSound);
    }

    public void MakePlayerShotSound()
    {
        MakeSound(playerShotSound);
    }

    public void MakeEnemyShotSound()
    {
        MakeSound(enemyShotSound);
    }

    // Играть данный звук
    private void MakeSound(AudioClip originalClip)
    {
        // Поскольку это не 3D-звук, его положение на сцене не имеет значения.
        AudioSource.PlayClipAtPoint(originalClip, transform.position);
    }
}
