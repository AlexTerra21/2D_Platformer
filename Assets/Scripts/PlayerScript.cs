using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контроллер и поведение игрока
/// </summary>
public class PlayerScript : MonoBehaviour
{

	/// <summary>
	/// 1 - скорость движения
	/// </summary>
	public Vector2 speed = new Vector2(50, 50);

	/// <summary>
	// 2 - направление движения
	/// </summary>
	private Vector2 movement;

	void Update()
	{
		// 3 -  извлечь информацию оси
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		// 4 - движение в каждом направлении
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);
		// 5 - Стрельба
		bool shoot = Input.GetButtonDown("Fire1");
		shoot |= Input.GetButtonDown("Fire2");
		// Замечание: Для пользователей Mac, Ctrl + стрелка - это плохая идея

		if (shoot)
		{
			WeaponScript weapon = GetComponent<WeaponScript>();
			if (weapon != null)
			{
				// ложь, так как игрок не враг
				weapon.Attack(false);
			}
		}
        // 6 – Убедиться, что игрок не выходит за рамки кадра
        var dist = (transform.position - Camera.main.transform.position).z;

        var leftBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 0, dist)
        ).x;

        var rightBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(1, 0, dist)
        ).x;

        var topBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 0, dist)
        ).y;

        var bottomBorder = Camera.main.ViewportToWorldPoint(
          new Vector3(0, 1, dist)
        ).y;

        transform.position = new Vector3(
          Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
          Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
          transform.position.z
        );

        // Вот и весь метод Update
    }

    void FixedUpdate()
	{
		// 5 - перемещение игрового объекта
		Rigidbody2D lRigid = GetComponent<Rigidbody2D>();
		lRigid.velocity = movement;
	}


	void OnCollisionEnter2D(Collision2D collision)
	{
		bool damagePlayer = false;

		// Столкновение с врагом
		EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
		if (enemy != null)
		{
			// Смерть врага
			HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
			if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp);

			damagePlayer = true;
		}

		// Повреждения у игрока
		if (damagePlayer)
		{
			HealthScript playerHealth = this.GetComponent<HealthScript>();
			if (playerHealth != null) playerHealth.Damage(1);
		}
	}
}
