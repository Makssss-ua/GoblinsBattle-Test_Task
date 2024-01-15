using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesCounter : MonoBehaviour
{
    [SerializeField] private Text text;
    private SceneManager sceneManager;

    private void OnEnable()
    {
        sceneManager = SceneManager.Instance;
        sceneManager.updateWaves += UpdateWaves;
    }

    private void OnDisable()
    {
        sceneManager.updateWaves -= UpdateWaves;
    }

    private void UpdateWaves(int wave, int wavesCount)
    {
        text.text = $"Wave {wave}/{wavesCount}";
    }
}
