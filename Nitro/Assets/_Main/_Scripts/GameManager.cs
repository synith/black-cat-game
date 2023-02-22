using UnityEngine;

namespace Synith
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Pause() => Time.timeScale = 0f;
        public void Resume() => Time.timeScale = 1f;
    }
}