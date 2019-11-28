using UnityEngine;
using UnityEngine.SceneManagement;

class SwitchScene : MonoBehaviour
{
    public string SceneName;

    public void Switch()
    {
        SceneManager.LoadScene(SceneName);
    }
}