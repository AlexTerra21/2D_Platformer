using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy generic behavior
/// </summary>
public class EnemyScript : MonoBehaviour
{
	private WeaponScript[] weapons;

	void Awake()
	{
		// Получить оружие только один раз
		weapons = GetComponentsInChildren<WeaponScript>();
	}

	void Update()
	{
		foreach (WeaponScript weapon in weapons)
		{
			// автоматическая стрельба
			if (weapon != null && weapon.CanAttack)
			{
				weapon.Attack(true);
			}
		}
	}
}

