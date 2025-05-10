using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _icons;
    private List<GameObject> _activeIcons;
    private List<GameObject> _inactiveIcons;
    private int _maxActiveCount = 10;
    private int _maxInactiveCount = 10;
    private int _defaultActiveCount = 3;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        _activeIcons = new List<GameObject>();
        _inactiveIcons = _icons.ToList();
        InfectAtIndex(0);
        InvokeRepeating("chooseRandomToInfect", 0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void chooseRandomToInfect()
    {
        int randomIndex = Random.Range(0, _inactiveIcons.Count());
        InfectAtIndex(randomIndex);
        if (_activeIcons.Count() > _defaultActiveCount)
        {
            CancelInvoke("chooseRandomToInfect");
        }

    }

    private void InfectAtIndex(int index)
    {
        GameObject iconToInfect = _inactiveIcons[index];
        iconToInfect.GetComponent<Enemy>().Infect();
        _activeIcons.Add(iconToInfect);
        _inactiveIcons.RemoveAt(index);
    }
}
