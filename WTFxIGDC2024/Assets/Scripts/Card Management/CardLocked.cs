using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CardLocked : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    Button _cardButton;
    CardShopManager _shopManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Card enter");
        this.transform.DOScale(new Vector3(2f, 2f, 2f), 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Card Exit");
        this.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        _cardButton = GetComponent<Button>();
        _shopManager = transform.parent.GetComponent<CardShopManager>();
        _cardButton.onClick.AddListener(() => { BuyCard(); });
    }

    private void BuyCard()
    {
        this.transform.DOMoveY(50f, 0.5f);
        this.GetComponent<CanvasGroup>().DOFade(0f, 0.5f);
        _shopManager.SpawnNewCard(this.gameObject);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
