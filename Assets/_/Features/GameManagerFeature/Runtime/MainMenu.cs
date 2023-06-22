using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagerFeature.Runtime
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}