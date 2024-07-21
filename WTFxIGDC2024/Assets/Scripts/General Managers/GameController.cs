using Minimalist.MapBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public float LevelTarget;
    public int levelDays = 1;
    private float dayduration = 20f;
    private float levelTimer = 0f;
    [SerializeField] Image sunDayFiller;

    private CardShopManager _cardShopManager;
    private UIController _uiController;
    private MapBuilderRuntime _mapBuilder;
    [SerializeField]
    EditModeInstanceBhv[] _allCardItems;

    private int floorCount = 0;

    [SerializeField] private CardItemData _cardItemData;
    float _EarningInterval = 1f;
    public List<GameObject> _currentItems = new List<GameObject>();

    [SerializeField] GameObject _customer;
    int customerCount = 0;
    public int[] ItemWeight = new int[9];
    void Start()
    {
        _cardShopManager = FindObjectOfType<CardShopManager>();
        _uiController = FindObjectOfType<UIController>();
        _mapBuilder = FindObjectOfType<MapBuilderRuntime>();
        _uiController.SetLevelText("Day " + levelDays + "/25");
    }



    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void AddCurrentItem(GameObject item)
    {
        _currentItems.Add(item);
        UpdateTotalEarnRate();
    }
    public void UpdateCurrentItem(GameObject item)
    {
        if (_currentItems.Contains(item))
        {
            _currentItems.Remove(item);
            _currentItems.Add(item);
        }
    }
    private void UpdateTotalEarnRate()
    {
        TotalEarnRate = 0;
        for (int i = 0; i < _currentItems.Count; i++)
        {
            TotalEarnRate += _currentItems[i].GetComponent<ICardItem>().GetEarnRate();
        }
    }
    // Update is called once per frame
    void Update()
    {

        _EarningInterval -= Time.deltaTime;
        if (_EarningInterval <= 0)
        {
            _cardShopManager.CurrentBalance += TotalEarnRate;
            UpdateTotalEarnRate();
            _uiController.UpdateBankBalance(_cardShopManager.CurrentBalance, TotalEarnRate);
            _EarningInterval = 1f;

            if (_cardShopManager.CurrentBalance >= (LevelTarget * 0.05))
            {
                ItemWeight[3] = 5;
                ItemWeight[4] = 5;
            }
            if (_cardShopManager.CurrentBalance >= (LevelTarget * 0.1))
            {
                ItemWeight[5] = 5;
                ItemWeight[6] = 5;
            }
            if (_cardShopManager.CurrentBalance >= (LevelTarget * 0.3))
            {
                ItemWeight[7] = 5;
                ItemWeight[8] = 5;
            }
        }

        HandleLevelDayLogic();
    }

    private void HandleLevelDayLogic()
    {
        levelTimer += Time.deltaTime;
        sunDayFiller.fillAmount = (levelTimer/dayduration);

        if ((levelTimer >= dayduration))
        {
            levelTimer = 0;
            levelDays++;
            _uiController.SetLevelText("Day " + levelDays + "/25");
            if (levelDays > 25)
            {
                _uiController.GameOver();
            }
        }
    }


    #region Shop Methods

    public bool CanBuy(float price)
    {
        return price <= _cardShopManager.CurrentBalance ? true : false;
    }

    public void DeductBalance(float price)
    {
        _cardShopManager.CurrentBalance -= price;
    }
    #endregion

    #region Card Management


    public ItemData GetRandomItemData()
    {

        ItemData itemData = new ItemData();
        int totalChance = 0;
        for (int i = 0; i < _cardItemData.AllItems.Length; i++)
        {
            totalChance += /*_cardItemData.AllItems[i].Chance*/ ItemWeight[i];
        }
        int cummalativeChance = 0;
        int rand = UnityEngine.Random.Range(0, totalChance + 1);
        for (int i = 0; i < _cardItemData.AllItems.Length; i++)
        {
            cummalativeChance += ItemWeight[i];
            if (rand <= cummalativeChance)
            {
                itemData = _cardItemData.AllItems[i];
                break;
            }
        }
        return itemData;
    }

    public void SetCurrentCard(string itemName)
    {
        switch (itemName)
        {
            case "Floor":
                _mapBuilder.SetTileObject(_allCardItems[0], true);
                floorCount++;
                customerCount++;
                if (floorCount >= 46)
                {
                    ItemWeight[0] = 0;
                }
                if (customerCount >= 8)
                {
                    customerCount=0;
                    GameObject customer = Instantiate(_customer, new Vector3(1f, 1f, 1f), Quaternion.identity);
                }
                break;
            case "Arcade Machine":
                _mapBuilder.SetTileObject(_allCardItems[1], false);
                break;
            case "Vending Machine":
                _mapBuilder.SetTileObject(_allCardItems[2], false);
                break;
            case "Ticket Machine":
                _mapBuilder.SetTileObject(_allCardItems[3], false);
                break;
            case "Slot Machine":
                _mapBuilder.SetTileObject(_allCardItems[4], false);
                break;
            case "Dart Board":
                _mapBuilder.SetTileObject(_allCardItems[5], false);
                break;
            case "Basketball Machine":
                _mapBuilder.SetTileObject(_allCardItems[6], false);
                break;
            case "Airhockey":
                _mapBuilder.SetTileObject(_allCardItems[7], false);
                break;
            case "Dance Floor":
                _mapBuilder.SetTileObject(_allCardItems[8], false);
                break;
        }
    }

    #endregion
}
