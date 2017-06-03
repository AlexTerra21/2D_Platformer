using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Скрипт параллакс-скроллинга, который нужно прописать для слоя
public class ScrollingScript : MonoBehaviour
{
	// Скорость прокрутки
	public Vector2 speed = new Vector2(2, 2);

	// Направление движения
	public Vector2 direction = new Vector2(-1, 0);

	// Движения должны быть применены к камере
	public bool isLinkedToCamera = false;

	void Update()
	{
		// Перемещение
		Vector3 movement = new Vector3(
			speed.x * direction.x,
			speed.y * direction.y,
			0);

		movement *= Time.deltaTime;
		transform.Translate(movement);

		// Перемещение камеры
		if (isLinkedToCamera)
		{
			Camera.main.transform.Translate(movement);
		}
	}
}