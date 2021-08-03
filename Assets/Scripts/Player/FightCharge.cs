using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FightCharge
{
    public const int chargeAmountPerBar = 25; //how much charge each bar can hold

    private static int amount; //"amount" specifically refers to the charge amount
    private static int amountMax; //the maximum charge
    private static int numberOfBars;

    public static void Init() //initialize (initialized by ChargeWindow.cs)
    {
        amount = 0; //charge amount starts at 0
        numberOfBars = 4; //number of bars in charge meter
        amountMax = numberOfBars * chargeAmountPerBar;

    }

    public static int GetTotalNumberOfBars()
    {
        return numberOfBars;
    }

    public static int GetChargeAmount()
    {
        return amount;
    }

    public static void AddFightCharge(int addChargeAmount)
    {
        amount += addChargeAmount;
        if (amount > amountMax) amount = amountMax; //prevent charge from going over the max
        Debug.Log("Current available charge: " + amount); //for testing
    }

    public static int GetNumberOfFilledBars()
    {
        //tells us how many bars are filled
        return Mathf.FloorToInt(amount * 1f / chargeAmountPerBar); //get a whole number value and convert to float
    }

    public static void ConsumeFilledBar()
    {
        //called when using an ability that consumes a bar
        amount -= chargeAmountPerBar;
        if (amount < 0) amount = 0;
    }

    public static bool TryRemoveFilledBar()
    {
        //called to help make this script less error-prone
        if (GetNumberOfFilledBars() >= 1) //confirms that there is at least one filled bar to consume
        {
            ConsumeFilledBar();
            return true;
        }
        else
        {
            return false;
        }
    }

  
}
