using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;
    private SoundManager soundManager;
    void Start()
    {
        gameObject.SetActive(false);
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameOver()
    {
        soundManager.StopMusic();
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
        SceneManager.LoadScene("WS11");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

  


    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
