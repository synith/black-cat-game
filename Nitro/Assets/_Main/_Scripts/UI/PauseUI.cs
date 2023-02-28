using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Synith
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] GameObject pauseScreen;
        [SerializeField] Button resumeButton;
        [SerializeField] Button menuButton;

        [SerializeField] AudioClip _buttonSound;

        bool isPaused;

        void Start()
        {
            pauseScreen.SetActive(false);

            resumeButton.onClick.AddListener(() => StartCoroutine(ResumeGame()));
            menuButton.onClick.AddListener(() => StartCoroutine(GoToMainMenu()));
        }


        void PlaySound(AudioClip clip) => SoundEffects.Instance.PlayClip(clip);

        void TogglePause()
        {
            isPaused = !isPaused;
            pauseScreen.SetActive(isPaused);
            Time.timeScale = isPaused ? 0 : 1;
        }

        IEnumerator ResumeGame()
        {
            PlaySound(_buttonSound);
            yield return new WaitForSecondsRealtime(0.2f);
            TogglePause();
        }

        IEnumerator GoToMainMenu()
        {
            PlaySound(_buttonSound);
            yield return new WaitForSecondsRealtime(0.2f);
            GameSceneManager.Load(GameSceneManager.Scene.Menu_Scene);
        }
    }
}
