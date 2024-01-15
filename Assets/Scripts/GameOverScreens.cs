using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreens : MonoBehaviour
{
    [SerializeField] private SceneManager sceneManager;

    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject winScreen;

    private void OnEnable()
    {
        sceneManager.gameOver += GameOver;
    }

    private void OnDisable()
    {
        sceneManager.gameOver -= GameOver;
    }

    private void GameOver(GameOver value)
    {
        switch (value)
        {
            case global::GameOver.Lose:
                loseScreen.SetActive(true);
                break;
            case global::GameOver.Win:
                winScreen.SetActive(true);
                break;
        }
    }
}
