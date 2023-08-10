using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class playerHealth : MonoBehaviour
{
    public float health;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI healthText;

    public void Update()
    {
        healthText.SetText("Health: " + health);
    }


    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(GameOver), 0.5f);
        }
    }

    public void Heal(int healing)
    {
        if (health < 200)
        {
            health += healing;
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("Game Over");
        
    }

}
