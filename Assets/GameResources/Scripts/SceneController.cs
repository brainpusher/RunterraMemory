using System;
using System.Collections;
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
    
    private FrontCard _firstRevealed;
    private FrontCard _secondRevealed;
    private FrontCard _thirdRevealed;
    
    private Coroutine _checkRoutine = null;

    private int successPicks = 0;
    
    private void Start()
    {
        int[] numbers = {0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4};
        numbers = RandomizeArray(numbers);
        
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

    public bool CanReveal => (_secondRevealed == null || _thirdRevealed == null) && _checkRoutine == null;

    public void CardRevealed(FrontCard card)
    {
        if (_firstRevealed == null)
        {
            _firstRevealed = card;
        }
        else if ( _secondRevealed == null)
        {
            _secondRevealed = card;
            _checkRoutine = StartCoroutine(CheckMatch());
        }
        else if (_thirdRevealed == null)
        {
            _thirdRevealed = card;
            _checkRoutine = StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.CardID == _secondRevealed.CardID)
        {
            score.AddScore(health.HealthCount);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(_firstRevealed.FlipToBack());
            StartCoroutine( _secondRevealed.FlipToBack());
            health.TakeLife();
            _firstRevealed = null;
            _secondRevealed = null;
            _checkRoutine = null;
        }

        if (_thirdRevealed != null)
        {
            if (_secondRevealed.CardID == _thirdRevealed.CardID)
            {
                score.AddScore(3 * health.HealthCount);
                successPicks += 3;
                Debug.Log("Success pick: " + successPicks);
                if (successPicks == gridRows * gridCols)
                    OnWin?.Invoke();
                _firstRevealed.DeleteCard();
                _secondRevealed.DeleteCard();
                _thirdRevealed.DeleteCard();
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(_firstRevealed.FlipToBack());
                StartCoroutine( _secondRevealed.FlipToBack());
                StartCoroutine( _thirdRevealed.FlipToBack());
                health.TakeLife();
                _checkRoutine = null;
            }
            _firstRevealed = null;
            _secondRevealed = null;
            _thirdRevealed = null;
        }
    }
}
