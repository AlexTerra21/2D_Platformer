using UnityEngine;

/// Общенное поведение врага
public class EnemyScript : MonoBehaviour
{
    private bool hasSpawn;
    private MoveScript moveScript;
    private WeaponScript[] weapons;

    void Awake()
    {
        // Получить оружие только один раз
        weapons = GetComponentsInChildren<WeaponScript>();

        // Отключить скрипты, чтобы деактивировать объекты при отсутствии спавна
        moveScript = GetComponent<MoveScript>();
    }

    // 1 - Отключить все
    void Start()
    {
        hasSpawn = false;

        // Отключить
        // -- коллайдеры
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        
        // -- Перемещение
        moveScript.enabled = false;
        // -- стрельбу
        foreach (WeaponScript weapon in weapons)
        {
            weapon.enabled = false;
        }
    }

    void Update()
    {
        // 2 - Проверить, начался ли спавн врагов.
        if (hasSpawn == false)
        {
            if (ScrollingScript.IsVisibleFrom(GetComponent<Renderer>(),Camera.main))  //renderer.IsVisibleFrom(Camera.main)
            {
                Spawn();
            }
        }
        else
        {
            // автоматическая стрельба
            foreach (WeaponScript weapon in weapons)
            {
                if (weapon != null && weapon.enabled && weapon.CanAttack)
                {
                    weapon.Attack(true);
                    SoundEffectsHelper.Instance.MakeEnemyShotSound();
                }
            }

            // 4 – Выход за рамки камеры? Уничтожить игровой объект.
            if (!ScrollingScript.IsVisibleFrom(GetComponent<Renderer>(), Camera.main))
            {
                Destroy(gameObject);
            }
        }
    }

    // 3 - Самоактивация.
    private void Spawn()
    {
        hasSpawn = true;

        // Включить все
        // -- Коллайдеры
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        // -- Перемещение
        moveScript.enabled = true;
        // -- Стрельбу
        foreach (WeaponScript weapon in weapons)
        {
            weapon.enabled = true;
        }
    }
}
