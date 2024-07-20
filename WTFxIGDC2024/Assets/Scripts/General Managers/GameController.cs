using Minimalist.MapBuilder;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private CardShopManager _cardShopManager;
    private UIController _uiController;
    private MapBuilderRuntime _mapBuilder;
    [SerializeField]
    EditModeInstanceBhv[] _allCardItems;

    [SerializeField] private CardItemData _cardItemData;
    float _EarningInterval = 1f;
    public List<GameObject> _currentItems = new List<GameObject>();
    void Start()
    {
        _cardShopManager = FindObjectOfType<CardShopManager>();
        _uiController = FindObjectOfType<UIController>();
        _mapBuilder = FindObjectOfType<MapBuilderRuntime>();
        _uiController.SetLevelText("Day 1");
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
            TotalEarnRate += _currentItems[i].GetComponent<ArcadeMachine>().GetEarnRate();
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
        }
    }


    #region Card Management


    public ItemData GetRandomItemData()
    {
        int rand = Random.RandomRange(0, _cardItemData.AllItems.Length);
        return _cardItemData.AllItems[rand];
    }

    public void SetCurrentCard(string itemName)
    {
        switch (itemName)
        {
            case "Floor":
                _mapBuilder.SetTileObject(_allCardItems[0], true);
                //  AddCurrentItem(_allCardItems[0].GetComponent<ICardItem>());
                break;
            case "Arcade Machine":
                _mapBuilder.SetTileObject(_allCardItems[1], false);
                // AddCurrentItem(_allCardItems[0].GetComponent<ICardItem>());

                break;
        }
    }

    #endregion
}
