using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]TMP_Text _bankBalanceText;
    [SerializeField]TMP_Text _earnRateText;
    [SerializeField]TMP_Text _levelText;
  
    public void UpdateBankBalance(float balance , float earnRate)
    {
        _bankBalanceText.text = "Bal: "+balance.ToString()+"$";
        _earnRateText.text = "Rate : "+ earnRate.ToString()+"$/s";

    }
    public void SetLevelText(string s)
    {
        _levelText.text = s;
    }
}
