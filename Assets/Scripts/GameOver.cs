using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public static void Show()
    {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
    }

    public Button button;

    void Start()
    {
        FindObjectOfType<SimulateTickRate>().Pause();

        FindObjectOfType<Setup>().DoBlur(true);

        button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Start");
        });
    }
}
