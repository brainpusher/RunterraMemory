using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SceneController : MonoBehaviour
{
    public event  Action OnWin = delegate { };
    
    [SerializeField] private int gridRows = 5;
    [SerializeField] private int gridCols = 3;
    [SerializeField] private float xOffset = 4f;
    [SerializeField] private float yOffset = 5f;

    [SerializeField] private FrontCard originalCard;
    [SerializeField] private Material[] materials;

    [SerializeField] private Health health;
    [SerializeField] private Score score;

    private Coroutine _checkRoutine = null;

    private int _successPicks;
    private List<FrontCard> cards = new List<FrontCard>();
    
    private void Start()
    {
        int[] numbers = {0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4};
        numbers = RandomizeArray(numbers);
        _successPicks = 0;
        InstantiateCards(numbers);
    }

    private void InstantiateCards(int[] numbers)
    {
        Vector3 startPos = originalCard.transform.position;
        
        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                FrontCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard);
                }

                int index = j * gridCols + i;
                int id = numbers[index];
                card.ChangeMaterial(id,materials[id]);

                float posX = (xOffset * i) + startPos.x;
                float posY = (yOffset * j) + startPos.y;
                
                card.transform.position = new Vector3(posX,posY,startPos.z);
            }
        }
    }
    
    private int[] RandomizeArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];

        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        return newArray;
    }
    
   public bool CanReveal => (cards.Count <= 2) && _checkRoutine == null;
    public void CardRevealed(FrontCard card)
    {
        if (cards.Count == 0)
        {
            cards.Add(card);
        }
        else
        {
            cards.Add(card);
            _checkRoutine = StartCoroutine(CheckOpenedCard());
        }
    }

    private IEnumerator CheckOpenedCard()
    {
        if (cards[cards.Count - 1].CardID == cards[cards.Count - 2].CardID)
        {
            score.AddScore(cards.Count == 2 ? health.HealthCount : 3 * health.HealthCount);
            if (cards.Count == 3)
            {
                _successPicks += 3;
                if (_successPicks == gridRows * gridCols)
                    OnWin?.Invoke();
                RemoveCardsFromField();
                ClearVariables();
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            FlipCardsToBack();
            health.TakeLife();
            ClearVariables();
        }
    }
    
    private void FlipCardsToBack()
    {
        foreach (FrontCard card in cards)
        {
            StartCoroutine(card.FlipToBack());
        }
    }
    
    private void ClearVariables()
    {
        _checkRoutine = null;
        cards.Clear();
    }
    
    private void RemoveCardsFromField()
    {
        foreach (FrontCard card in cards)
        {
            card.DeleteCard();
        }
    }
}
