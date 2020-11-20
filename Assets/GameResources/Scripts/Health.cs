using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public event Action OnHealthEnded = delegate {  };
    
    [SerializeField] private int healthCount = 5;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;


    private int savedHealthCount;

    private void Awake()
    {
        savedHealthCount = healthCount;
    }

    public void ResetHealthCount()
    {
        healthCount = savedHealthCount;
        UpdateHealthBar();
    }
    
    private void UpdateHealthBar()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < healthCount) ? fullHeart : emptyHeart;
        }
    }

    public void TakeLife()
    {
        healthCount--;
        if (healthCount >= 0)
        {
            UpdateHealthBar();
        }
        if(healthCount == 0)
            OnHealthEnded?.Invoke();
    }

    public int HealthCount => healthCount;
}
