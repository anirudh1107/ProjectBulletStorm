using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Michsky.MUIP;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _icons;
    [SerializeField]
    private DialogSo[] _initialDialogs;
    [SerializeField]
    private DialogSo _enterTriggerDialog;
    [SerializeField]
    private  DialogSo _gameOverDialog;
    [SerializeField]
    private  Canvas _dialogCanvas;
    [SerializeField]
    private  ModalWindowManager _modalWindowManager;
    [SerializeField]
    private static int _activeCount = 0;

    private List<GameObject> _activeIcons;
    private List<GameObject> _inactiveIcons;
    private int _maxActiveCount = 10;
    private int _maxInactiveCount = 10;
    private int _defaultActiveCount = 3;
    private bool isBlinking;
    private int _currentDialogIndex = -1;
    private bool _completedInfection = false;
    public static LevelManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        _activeIcons = new List<GameObject>();
        _inactiveIcons = _icons.ToList();
        isBlinking = false;
        InfectAtIndex(0);
        Time.timeScale = 0f;
        _dialogCanvas.gameObject.SetActive(true);
        DialogTrigger();
        InvokeRepeating("chooseRandomToInfect", 0f, 3f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_completedInfection && (!isBlinking) && (_activeCount <= 0))
        {
            ExitTriggerDialog();
            Enemy exitTrigger = _icons[0].GetComponent<Enemy>();
            BoxCollider2D gateCollider = _icons[0].GetComponent<BoxCollider2D>();
            gateCollider.enabled = true;
            gateCollider.isTrigger = true;
            StartCoroutine(EntryBlink(exitTrigger));
        }
    }

    public void chooseRandomToInfect()
    {
        int randomIndex = Random.Range(0, _inactiveIcons.Count());
        InfectAtIndex(randomIndex);
        if (_activeIcons.Count() > _defaultActiveCount)
        {
            _completedInfection = true;
            CancelInvoke("chooseRandomToInfect");
        }

    }

    private void InfectAtIndex(int index)
    {
        GameObject iconToInfect = _inactiveIcons[index];
        iconToInfect.GetComponent<Enemy>().Infect();
        _activeIcons.Add(iconToInfect);
        _activeCount++;
        _inactiveIcons.RemoveAt(index);
    }

    public static void DisInfect()
    {
        _activeCount--;
    }

    private IEnumerator EntryBlink(Enemy enemy) 
    {
        isBlinking = true;
        while (true)
        {
            enemy.TriggerEntryFeedback();
            yield return new WaitForSeconds(2f);
        }
    }

    public void DialogTrigger()
    {
        _currentDialogIndex++;
        if (_currentDialogIndex >= _initialDialogs.Length)
        {
            _dialogCanvas.gameObject.SetActive(false);
            Time.timeScale = 1f;
            return;
        }
        _modalWindowManager.titleText = _initialDialogs[_currentDialogIndex].Title;
        _modalWindowManager.descriptionText = _initialDialogs[_currentDialogIndex].DialogText;
        _modalWindowManager.UpdateUI();
        if (_currentDialogIndex == 0)
            _modalWindowManager.OpenWindow();
    }

    public void CloseDialog()
    {
        DialogTrigger();
    }

    public void ExitTriggerDialog()
    {
        _modalWindowManager.titleText = _enterTriggerDialog.Title;
        _modalWindowManager.descriptionText = _enterTriggerDialog.DialogText;
        _modalWindowManager.UpdateUI();
        _dialogCanvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public  void GameOverDialog()
    {
        _modalWindowManager.titleText = _gameOverDialog.Title;
        _modalWindowManager.descriptionText = _gameOverDialog.DialogText;
        _modalWindowManager.UpdateUI();
        _modalWindowManager.onConfirm.AddListener(ResetGame);
        _dialogCanvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public static void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
