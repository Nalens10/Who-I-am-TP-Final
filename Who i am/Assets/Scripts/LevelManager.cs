using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Conecciones")]
    [SerializeField] private string gameSceneName;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private AudioSource gameOverSound;
    [SerializeField] private AudioSource gameSound;


    private bool isPaused;
    private bool isGameOver;

    private void Awake()
    {
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        isPaused = false;
        isGameOver = false;
    }

    private void Update()
    {
        Pause();

    }

    private void ChangePanels(GameObject objective, bool state, int num)
    {
        objective.SetActive(state);
        isPaused = state;
        Time.timeScale = num;
    }

    //Pausa del juego
    public void Pause()
    {
        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            ChangePanels(menuPanel, false, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePanels(menuPanel, true, 0);
        }

    }

    public void Resume()
    {
        ChangePanels(menuPanel, false, 1);
        Debug.Log("NoPausado");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Reiniciado");
        isPaused = false;
        isGameOver = false;
    }
    public void EndGame()
    {
        isGameOver = true;

        ChangePanels(gameOverPanel, true, 0);
        menuPanel.SetActive(false);
        gameSound.Stop();
        gameOverSound.Play();
        Debug.Log("NoPausado");
    }

    public void Back()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
    public void Exit()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }


    

}
