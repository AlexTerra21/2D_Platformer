using UnityEngine;

// Начало или конец игры
public class GameOverScript : MonoBehaviour
{
    void OnGUI()
    {
        const int buttonWidth = 120;
        const int buttonHeight = 60;

        if (
          GUI.Button(
            // по оси Х - по середине, по оси Y - 1/3 от высоты
            new Rect(
              Screen.width / 2 - (buttonWidth / 2),
              (1 * Screen.height / 3) - (buttonHeight / 2),
              buttonWidth,
              buttonHeight
            ),
            "Начать сначала!"
          )
        )
        {
            // загрузить уровень Level1
            Application.LoadLevel("Level1");
        }

        if (
          GUI.Button(
            // по оси Х - по середине, по оси Y - 2/3 от высоты
            new Rect(
              Screen.width / 2 - (buttonWidth / 2),
              (2 * Screen.height / 3) - (buttonHeight / 2),
              buttonWidth,
              buttonHeight
            ),
            "Назад в меню"
          )
        )
        {
            // загрузить уровень Menu
            Application.LoadLevel("Menu");
        }
    }
}