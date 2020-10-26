using UnityEngine;
using UnityEngine.SceneManagement;

namespace enjoythevibes.UI
{
    public class MainUIManager : MonoBehaviour
    {
        public void OnRestartButton()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }        
    }
}