using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Button spinButton;
    public Button stopButton;
    public TextMeshProUGUI resultText;
    public WinChecker winChecker;

    private void Awake()
    {
        winChecker.WinEvent += OnWin;
        winChecker.LoseEvent += OnLose;
        resultText.gameObject.SetActive(false);
        stopButton.interactable = false;
    }

    private void OnDestroy()
    {
        winChecker.WinEvent -= OnWin;
        winChecker.LoseEvent -= OnLose;
    }

    public void OnSpinStarted()
    {
        spinButton.interactable = false;
        stopButton.interactable = true;
        resultText.gameObject.SetActive(false);
    }

    public void OnSpinEnded()
    {
        spinButton.interactable = true;
        stopButton.interactable = false;
    }

    private void OnWin(SymbolData symbol)
    {
        resultText.text = $"WIN! {symbol.name}";
        resultText.gameObject.SetActive(true);
    }

    private void OnLose()
    {
        resultText.text = "LOSE";
        resultText.gameObject.SetActive(true);
    }
}