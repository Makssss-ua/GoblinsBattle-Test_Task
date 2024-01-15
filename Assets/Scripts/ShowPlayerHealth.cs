using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerHealth : MonoBehaviour
{
    [SerializeField] private ImageSlider imageSlider;
    private Player player;

    private void Start()
    {
        player = SceneManager.Instance.Player;
        imageSlider.SetMinMaxValue(player.hp.minHealth, player.hp.maxHealth);
        imageSlider.SetValue(player.hp.health);
        player.hp.OnUpdateHealth += ShowHealth;
    }

    private void OnEnable()
    {
        player = SceneManager.Instance.Player;

        if (player.hp == null)
            return;

        imageSlider.SetMinMaxValue(player.hp.minHealth, player.hp.maxHealth);
        imageSlider.SetValue(player.hp.health);
        player.hp.OnUpdateHealth += ShowHealth;
    }

    private void OnDisable()
    {
        player.hp.OnUpdateHealth -= ShowHealth;
    }

    private void ShowHealth(float health)
    {
        imageSlider.SetValue(health);
    }
}
