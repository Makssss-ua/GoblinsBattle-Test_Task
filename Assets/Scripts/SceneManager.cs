using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public enum GameOver
{
    Win,
    Lose
}

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    public Player Player;
    public List<Enemie> Enemies { get; private set; } = new List<Enemie>();
    public Action<GameOver> gameOver;
    public Action<int, int> updateWaves;

    private int currWave = 0;
    [SerializeField] private LevelConfig Config;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        if(Player)
            Player.playerDie += GameOver;
    }

    private void OnDisable()
    {
        if(Player)
            Player.playerDie -= GameOver;
    }

    private void Start()
    {
        SpawnWave();
    }

    public void AddEnemie(Enemie enemie)
    {
        Enemies.Add(enemie);
    }

    public void SpawnEnemie(Enemie enemie, Vector3 pos)
    {
        Enemie enem = Instantiate(enemie, pos, Quaternion.identity);
        AddEnemie(enem);
    }

    public void RemoveEnemie(Enemie enemie)
    {
        Enemies.Remove(enemie);
        if(Enemies.Count == 0)
        {
            SpawnWave();
        }
    }

    private void GameOver()
    {
        gameOver?.Invoke(global::GameOver.Lose);
    }

    private void SpawnWave()
    {
        if (currWave >= Config.Waves.Length)
        {
            gameOver?.Invoke(global::GameOver.Win);
            return;
        }

        Wave wave = Config.Waves[currWave];
        updateWaves?.Invoke(currWave + 1, Config.Waves.Length);
        foreach (Enemie character in wave.Characters)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
            if(NavMesh.SamplePosition(pos, out NavMeshHit hit, 100f, 1))
                SpawnEnemie(character, hit.position);
        }
        currWave++;

    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    

}
