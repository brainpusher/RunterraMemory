using UnityEngine;

public class EndgameController : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private GameObject endGamePopup;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private GameObject looseText;
    [SerializeField] private GameObject winText;
    
    private void OnEnable()
    {
        health.OnHealthEnded += ShowLoose;
        sceneController.OnWin += ShowWin;
    }
    
    private void OnDisable()
    {
        health.OnHealthEnded -= ShowLoose;
        sceneController.OnWin -= ShowWin;
    }

    private void ShowLoose()
    {
        endGamePopup.SetActive(true);
        looseText.SetActive(true);
    }
    public void ShowWin()
    {
        endGamePopup.SetActive(true);
        winText.SetActive(true);
    }
}
