using UnityEngine;
using Michsky.MUIP;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;

public class Level2Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _icons;
    [SerializeField]
    private DialogSo _gameOverDialog;
    [SerializeField]
    private DialogSo _levelClearDialog;
    [SerializeField]
    private Canvas _dialogCanvas;
    [SerializeField]
    private ModalWindowManager _modalWindowManager;
    [SerializeField]
    private MMF_Player soundPlayer;

    private List<GameObject> _activeIcons;
    private static int _activeCount = 0;

    public static Level2Manager Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        soundPlayer.PlayFeedbacks();
        _activeIcons = new List<GameObject>();
        for (int i = 0; i < _icons.Length; i++)
        {
            InfectAtIndex(i);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (_activeCount <= 0)
        {
            LevelClearDialog();
        }
    }

    private void InfectAtIndex(int index)
    {
        GameObject iconToInfect = _icons[index];
        iconToInfect.GetComponent<Enemy>().Infect();
        _activeIcons.Add(iconToInfect);
        _activeCount++;
    }

    public void GameOverDialog()
    {
        _modalWindowManager.titleText = _gameOverDialog.Title;
        _modalWindowManager.descriptionText = _gameOverDialog.DialogText;
        _modalWindowManager.UpdateUI();
        _modalWindowManager.onConfirm.AddListener(ResetGame);
        _dialogCanvas.gameObject.SetActive(true);
        _modalWindowManager.OpenWindow();
        Time.timeScale = 0f;

    }

    public void LevelClearDialog()
    {
        _modalWindowManager.titleText = _levelClearDialog.Title;
        _modalWindowManager.descriptionText = _levelClearDialog.DialogText;
        _modalWindowManager.UpdateUI();
        _modalWindowManager.onConfirm.AddListener(ResetGame);
        _dialogCanvas.gameObject.SetActive(true);
        _modalWindowManager.OpenWindow();
        Time.timeScale = 0f;
    }

    public static void DisInfect()
    {
        _activeCount--;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
