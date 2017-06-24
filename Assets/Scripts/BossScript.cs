using UnityEngine;

/// 

/// Общее поведение для врагов
/// 

public class BossScript : MonoBehaviour
{
    private bool hasSpawn;
    // Массив спрайтов тела Босса в зависимости от повреждений
    public Sprite[] BossBodies;
    // Параметры компонентов
    private MoveScript moveScript;
    private WeaponScript[] weapons;
    private Animator animator;
    private SpriteRenderer[] renderers;
    private SpriteRenderer BodySprite;
    private HealthScript HS;

    // Поведение босса (не совсем AI)
    public float minAttackCooldown = 0.5f;
    public float maxAttackCooldown = 2f;

    private float aiCooldown;
    private bool isAttacking;
    private Vector2 positionTarget;

    private int MaxHP;
    void Awake()
    {
        HS = GetComponent<HealthScript>();
        MaxHP = HS.hp;
        
        GameObject gm = GameObject.Find("Body");
        BodySprite = gm.GetComponent<SpriteRenderer>();

        // Получить оружие только один раз
        weapons = GetComponentsInChildren<WeaponScript>();

        // Отключить скрипты при отсутствии спауна
        moveScript = GetComponent<MoveScript>();

        // Получить аниматор
        animator = GetComponent<Animator>();

        // Получить рендереры в детях
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Start()
    {
        hasSpawn = false;
        //BodySprite.sprite = BossBodies[1];
        // Отключить все
        // -- Collider
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
        // -- Движение
        moveScript.enabled = false;
        // -- Стрельба
        foreach (WeaponScript weapon in weapons)
        {
            weapon.enabled = false;
        }

        // Дефолтное поведение
        isAttacking = false;
        aiCooldown = maxAttackCooldown;
    }

    void Update()
    {
       
        if (HS.hp > MaxHP * 3 / 4)
        {
            BodySprite.sprite = BossBodies[0];
        } else if (HS.hp > MaxHP / 2) 
        {
            BodySprite.sprite = BossBodies[1];
        }
        else if (HS.hp > MaxHP / 4)
        {
            BodySprite.sprite = BossBodies[2];
        }
        else
        {
            BodySprite.sprite = BossBodies[3];
        }
        // Проверим появился ли враг
        if (hasSpawn == false)
        {
            // Для простоты проверим только первый рендерер
            // Но мы не знаем, если это тело, и глаз или рот ...
            if (ScrollingScript.IsVisibleFrom(renderers[0],Camera.main))
            {
                Spawn();
            }
        }
        else
        {
            // AI
            //------------------------------------
            // Перемещение или атака.
            aiCooldown -= Time.deltaTime;

            if (aiCooldown <= 0f)
            {
                isAttacking = !isAttacking;
                aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
                positionTarget = Vector2.zero;

                // Настроить или сбросить анимацию атаки
                animator.SetBool("Attack", isAttacking);
            }

            // Атака
            //----------
            if (isAttacking)
            {
                // Остановить все движения
                moveScript.direction = Vector2.zero;

                foreach (WeaponScript weapon in weapons)
                {
                    if (weapon != null && weapon.enabled && weapon.CanAttack)
                    {
                        weapon.Attack(true);
                        SoundEffectsHelper.Instance.MakeEnemyShotSound();
                    }
                }
            }
            // Перемещение
            //----------
            else
            {
                // Выбрать цель?
                if (positionTarget == Vector2.zero)
                {
                    // Получить точку на экране, преобразовать ее в цель в игровом мире
                    Vector2 randomPoint = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

                    positionTarget = Camera.main.ViewportToWorldPoint(randomPoint);
                }

                // У нас есть цель? Если да, найти новую
                Collider2D col = GetComponent<Collider2D>();
                if (col.OverlapPoint(positionTarget))
                {
                    // Сбросить, выбрать в следующем кадре
                    positionTarget = Vector2.zero;
                }

                // Идти к точке
                Vector3 direction = ((Vector3)positionTarget - this.transform.position);

                // Помните об использовании скрипта движения
                moveScript.direction = Vector3.Normalize(direction);
            }
        }
    }

    private void Spawn()
    {
        hasSpawn = true;

        // Включить все
        // -- Коллайдер
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = true;
        // -- Движение
        moveScript.enabled = true;
        // -- Стрельба
        foreach (WeaponScript weapon in weapons)
        {
            weapon.enabled = true;
        }

        // Остановить основной скроллинг
        foreach (ScrollingScript scrolling in FindObjectsOfType<ScrollingScript>())
        {
            if (scrolling.isLinkedToCamera)
            {
                scrolling.speed = Vector2.zero;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        // В случае попадания изменить анимацию
        ShotScript shot = otherCollider2D.gameObject.GetComponent<ShotScript>();
        if (shot != null)
        {
            if (shot.isEnemyShot == false)
            {
                // Stop attacks and start moving awya
                aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
                isAttacking = false;

                // Изменить анимацию 
                animator.SetTrigger("Hit");
            }
        }
    }

    void OnDrawGizmos()
    {
        // Небольшой совет: Вы можете отобразить отладочную информацию в вашей сцене с Гизмо
        if (hasSpawn && isAttacking == false)
        {
            Gizmos.DrawSphere(positionTarget, 0.25f);
        }
    }
}
