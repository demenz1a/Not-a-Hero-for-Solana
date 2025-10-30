using UnityEngine;
using UnityEngine.SceneManagement;

public class Perehod : MonoBehaviour
{
    [SerializeField] private string SceneName;
    public void StartGame()
    {
        SceneManager.LoadScene(SceneName);
    }


}
