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

    //is set when the card is instantiated
    public float _cardCost;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Card enter");
        this.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
        this.GetComponent<RectTransform>().SetAsLastSibling();
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
        if (_cardCost <= _shopManager.CurrentBalance)
        {
            this.transform.DOMoveY(50f, 0.5f);
            this.GetComponent<CanvasGroup>().DOFade(0f, 0.5f);
            _shopManager.SpawnNewCard(this.gameObject);

        }
        else
        {
            this.transform.DOShakePosition(1.5f,10f);
        }
    }

}
