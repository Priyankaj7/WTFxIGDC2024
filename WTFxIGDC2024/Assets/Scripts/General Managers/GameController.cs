using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    [SerializeField]private CardItemData _cardItemData;
    float _EarningInterval = 1f;
    public List<ICardItem> _currentItems = new List<ICardItem>();
    void Start()
    {
        _cardShopManager = FindObjectOfType<CardShopManager>();
        _uiController = FindObjectOfType<UIController>();
    }

    public void AddCurrentItem(ICardItem item)
    {
        _currentItems.Add(item);
        UpdateTotalEarnRate();
    }
    private void UpdateTotalEarnRate()
    {
        TotalEarnRate = 0;
        for (int i = 0; i < _currentItems.Count; i++)
        {
            TotalEarnRate += _currentItems[i].GetEarnRate();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        _EarningInterval -= Time.deltaTime;
        if (_EarningInterval <= 0)
        {
            _cardShopManager.CurrentBalance += TotalEarnRate;
            _uiController.UpdateBankBalance(_cardShopManager.CurrentBalance);
            _EarningInterval = 1f;
        }
    }

    public ItemData GetRandomItemData()
    {
        int rand = Random.RandomRange(0, _cardItemData.AllItems.Length);
        return _cardItemData.AllItems[rand];
    }
}
