using UnityEngine;

namespace Synith
{
    public class GameOverUI : MonoBehaviour
    {
        void Start()
        {
            Hide();
        }

        void GameManager_OnGameOver()
        {
            Show();
        }

        void Show() => gameObject.SetActive(true);
        void Hide() => gameObject.SetActive(false);

    }
}