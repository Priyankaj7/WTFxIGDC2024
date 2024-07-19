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

    // Start is called before the first frame update
    void Start()
    {
        GenerateCards();
    }

    private void GenerateCards()
    {
        float PosX = 0f;
        for (int i = 0; i < 4; i++)
        {
            GameObject card = Instantiate(_cardPrefab);
            card.transform.SetParent(transform, false);
            card.GetComponent<RectTransform>().position = new Vector3(PosX, 0, 0);
            PosX += 100f;

        }
    }

    
    public async void SpawnNewCard(GameObject oldCard)
    {
        //Logic to be added for new card
        await Task.Delay(1000);
        float PosX = oldCard.GetComponent<RectTransform>().position.x;
        oldCard.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        oldCard.GetComponent<RectTransform>().position = new Vector3(PosX, 0, 0);
        oldCard.transform.position = new Vector3(oldCard.transform.position.x, -50f, oldCard.transform.position.z);
        oldCard.transform.DOMoveY(0f, 0.5f);
    }
}
