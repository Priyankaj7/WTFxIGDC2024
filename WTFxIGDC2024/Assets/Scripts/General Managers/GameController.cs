using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public float TotalEarnRate;
    private CardShopManager _cardShopManager;
    private UIController _uiController;

    void Start()
    {
        _cardShopManager = FindObjectOfType<CardShopManager>();
        _uiController = FindObjectOfType<UIController>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
