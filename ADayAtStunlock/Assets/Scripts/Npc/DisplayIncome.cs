using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayIncome : MonoBehaviour {
    TextMesh incomeText;

    [SerializeField] Color positiveColor;
    [SerializeField] Color negativeColor;
    [SerializeField] Color neutralColor;
    // Use this for initialization
    void Start() {
        incomeText = GetComponent<TextMesh>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplayedIncome();
    }
    private void UpdateDisplayedIncome()
    {
        //There are 7 states, gaining big, medium and small, break even, lose big medium and small amount of money.
        //max value is 36, min value is -18. break even is at 0
        //const float positiveBigAmount     = 36f;
        const float positiveMediumAmount  = 18f;
        const float positiveSmallAmount   = 6f;
        const float breakEven             = 2f;
        const float negativeSmallAmount   = -6f;
        const float negativeMediumAmount  = -12f;                                
        //const float negativeBigAmount     = -18f;                               
        print(MoneyManager.PotentialMoneyDifference);  
        if(MoneyManager.PotentialMoneyDifference > positiveMediumAmount)
        {
            //Display +$$$
            ChangeTextToNew("+$$$");
            ChangeTextColor(positiveColor);
        }
        else if(MoneyManager.PotentialMoneyDifference > positiveSmallAmount)
        {
            //Display +$$
            ChangeTextToNew("+$$");
            ChangeTextColor(positiveColor);
        }
        else if(MoneyManager.PotentialMoneyDifference > breakEven)
        {
            //Display +$
            ChangeTextToNew("+$");
            ChangeTextColor(positiveColor);
        }
        else if(MoneyManager.PotentialMoneyDifference < breakEven && MoneyManager.PotentialMoneyDifference > -breakEven)
        {
            //Display |
            ChangeTextToNew("|");
            ChangeTextColor(neutralColor);
        }
        else if(MoneyManager.PotentialMoneyDifference > negativeSmallAmount)
        {
            //Display -$
            ChangeTextToNew("-$");
            ChangeTextColor(negativeColor);
        }
        else if(MoneyManager.PotentialMoneyDifference > negativeMediumAmount)
        {
            //Display -$$
            ChangeTextToNew("-$$");
            ChangeTextColor(negativeColor);
        }
        else
        {
            //Display -$$$
            ChangeTextToNew("-$$$");
            ChangeTextColor(negativeColor);
        }
    }
    private void ChangeTextToNew(string newText)
    {
        incomeText.text = newText;
    }
    private void ChangeTextColor(Color newColor)
    {
        incomeText.color = newColor;
    }
}
