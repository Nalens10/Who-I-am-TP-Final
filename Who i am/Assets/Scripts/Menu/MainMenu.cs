using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Conecciones")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private string gameSceneName;

    private void Awake()
    {
        optionPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void Exit()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }
}
