using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]TMP_Text _bankBalanceText;
    [SerializeField]TMP_Text _earnRateText;
    [SerializeField]TMP_Text _levelText;
    [SerializeField] GameObject _winPanel;
    [SerializeField] Button _settingsButton;
    [SerializeField] GameObject _settingsPanel;
    [SerializeField] Sprite[] _settinButtonImage;
    public void UpdateBankBalance(float balance , float earnRate)
    {
        _bankBalanceText.text = "Bal: "+balance.ToString()+"$/ "+ GameController.instance.LevelTarget.ToString()+"$";
        _earnRateText.text = "Rate : "+ earnRate.ToString()+"$/s";

        if (balance >= GameController.instance.LevelTarget)
        {
            LevelComplete();
        }
    }
    public void LevelComplete()
    {
        _winPanel.SetActive(true);
    }
    public void SetLevelText(string s)
    {
        _levelText.text = s;
    }

    public void ToggleSettings()
    {
        if (_settingsPanel.gameObject.activeInHierarchy)
        {
            _settingsPanel.gameObject.SetActive(false);
            _settingsButton.transform.GetChild(0).GetComponent<Image>().sprite = _settinButtonImage[0];
        }
        else
        {
            _settingsPanel.gameObject.SetActive(true);
            _settingsButton.transform.GetChild(0).GetComponent<Image>().sprite = _settinButtonImage[1];
        }
    }
}
