using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayIncome : MonoBehaviour {
    TextMesh incomeText;
	// Use this for initialization
	void Start () {
        incomeText = GetComponent<TextMesh>();

    }
	
	// Update is called once per frame
	void Update ()
    {

    }
    private void  UpdateDisplayedIncome()
    {
        //There are 7 states, gaining big, medium and small, break even, lose big medium and small amount of money.
        //Break even -5 <-> 5 
        //
        //

        private int m_breakEven       = 5;
        private int m_smallAmount     = 10;
        private int m_mediumAmount    = 15;
        private int m_bigAmount       = 20;

        if(MoneyManager.MoneyDifferenceLastGenerate <= 5 && 
           MoneyManager.MoneyDifferenceLastGenerate >= -5)
        {

        }
    }
    
}
