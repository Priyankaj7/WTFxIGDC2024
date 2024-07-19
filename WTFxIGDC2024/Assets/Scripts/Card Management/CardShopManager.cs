using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;
using System.Threading.Tasks;

public class CardShopManager : MonoBehaviour
{
    //prefab of the card that is locked and in shop
    [SerializeField] GameObject _cardPrefab;
    
    private UIController _uiController;
    //Vlaue for Current bank balance
    public float CurrentBalance = 50;
    // Start is called before the first frame update
    void Start()
    {
        
        GenerateCards();
        _uiController = GameObject.FindObjectOfType<UIController>();
        _uiController.UpdateBankBalance(CurrentBalance);
    }

    private void GenerateCards()
    {
        float PosX = 0f;
        for (int i = 0; i < 4; i++)
        {
            GameObject card = Instantiate(_cardPrefab);
            card.transform.SetParent(transform, false);
            card.GetComponent<RectTransform>().position = new Vector3(PosX, 0, 0);
            card.GetComponent<CardLocked>()._cardCost = 5f;
            PosX += 100f;

        }
    }

    
    public async void SpawnNewCard(GameObject oldCard)
    {
        CurrentBalance -= oldCard.GetComponent<CardLocked>()._cardCost;
        _uiController.UpdateBankBalance(CurrentBalance);
        //Logic to be added for new card
        await Task.Delay(1000);
        float PosX = oldCard.GetComponent<RectTransform>().position.x;
        oldCard.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        oldCard.GetComponent<RectTransform>().position = new Vector3(PosX, 0, 0);
        oldCard.transform.position = new Vector3(oldCard.transform.position.x, -50f, oldCard.transform.position.z);
        oldCard.transform.DOMoveY(0f, 0.5f);
    }
}
