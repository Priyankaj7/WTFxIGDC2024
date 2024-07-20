using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]TMP_Text _bankBalanceText;
    [SerializeField]TMP_Text _earnRateText;
    [SerializeField]TMP_Text _levelText;
    [SerializeField] GameObject _winPanel;
  
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
}
