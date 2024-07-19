using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]TMP_Text _bankBalanceText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateBankBalance(float balance)
    {
        _bankBalanceText.text = "Current Balance: "+balance.ToString();
    }
}
