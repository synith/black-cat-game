using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Synith
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] Button playButton;
        [SerializeField] Button quitButton;
        void Start()
        {
            if (playButton != null)
                playButton.onClick.AddListener(() => { StartCoroutine(StartGame()); });
            else print("No Play Button!");

            if (playButton != null)
                quitButton.onClick.AddListener(() => { StartCoroutine(QuitGame()); });
            else print("No Quit Button!");

        }
        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(0.2f);
            GameSceneManager.Load(GameSceneManager.Scene.Game_Scene);
        }
        IEnumerator QuitGame()
        {
            yield return new WaitForSeconds(0.2f);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}