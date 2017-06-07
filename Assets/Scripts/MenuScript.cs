using UnityEngine;

/// 

/// Скрипт главного меню
/// 

public class MenuScript : MonoBehaviour
{
    private GUISkin skin;

    void Start()
    {
        // Загрузить скин для кнопки
        skin = Resources.Load("GUISkin") as GUISkin; 
    }

    void OnGUI()
    {
        const int buttonWidth = 84;
        const int buttonHeight = 60;


        // Задать скин
        GUI.skin = skin;

        // Определяем место кнопки на экране:
        // по оси X - в центре, по оси Y - 2/3 от высоты
        Rect buttonRect = new Rect(
              Screen.width / 2 - (buttonWidth / 2),
              (2 * Screen.height / 3) - (buttonHeight / 2),
              buttonWidth,
              buttonHeight
            );

        // Нарисуйте кнопку, чтобы начать игру
        if (GUI.Button(buttonRect, "Поехали!"))
        {
            // По щелчку по кнопке, загрузите первый уровень.
            // "2DPlatformer" - название первой сцены, которую мы создали.
            // Ее то мы и загрузим.
            Application.LoadLevel("Level1");
        }
    }
}