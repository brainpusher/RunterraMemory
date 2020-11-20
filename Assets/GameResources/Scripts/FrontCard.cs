using System.Collections;
using UnityEngine;

public class FrontCard : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private GameObject frontCard;
    
    private int _id;

    private bool canClick = false;
    
    public void OnMouseDown()
    {
        if (sceneController.CanReveal && canClick)
        {
            StartCoroutine(FlipToFront());
            sceneController.CardRevealed(this);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(FirstFlip());
    }

    public int CardID => _id;

    public void ChangeMaterial(int id, Material material)
    {
        _id = id;
        GetComponent<MeshRenderer>().material = material;
    }
    
    private IEnumerator FirstFlip()
    {
        float count = 0f;
        float secondsToCount = 3f;
        StartCoroutine(FlipToFront());
        while (count < secondsToCount)
        {
            yield return new WaitForSeconds(1f);
            count++;
        }

        canClick = true;
        StartCoroutine(FlipToBack());
    }
    
    
    public IEnumerator FlipToFront()
    {
        for (float i = 180f; i >= 0f; i -= 10f)
        {
            frontCard.transform.rotation = Quaternion.Euler(0f,i,0f);
            yield return null;
        }
    }

    public IEnumerator FlipToBack()
    {
        for (float i = 0f; i <= 180f; i += 10f)
        {
            frontCard.transform.rotation = Quaternion.Euler(0f,i,0f);
            yield return null;
        }
    }

    public void DeleteCard()
    {
        Destroy(frontCard,0.7f);
    }
}
