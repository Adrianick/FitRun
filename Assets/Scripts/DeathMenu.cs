using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;
    void Start()
    {
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameOver()
    {
        gameObject.SetActive(true);
        //Pause the whole game
    }

    public void ToggleEndMenu(float score)
    {
        gameObject.SetActive(true);

    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("WS6");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
