using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChargeWindow : MonoBehaviour
{
    private Transform barTemplate;
    private List<Bar> barList;

    private void Awake()
    {
        barTemplate = transform.Find("Bar Template");
        barTemplate.gameObject.SetActive(false); 
        FightCharge.Init(); 
    }

    private void Start()
    {
        barList = new List<Bar>();

        float paddingBetweenBars = 5f;
        float totalBarWidth = transform.Find("Window Background").GetComponent<RectTransform>().sizeDelta.x;
        Vector2 barStartingPosition = transform.Find("Window Background").GetComponent<RectTransform>().anchoredPosition + new Vector2(-totalBarWidth / 2.67f, 0); //2.67 selected through triasl and error to make the bars align properly on the background
        float perBarWidth = (totalBarWidth - paddingBetweenBars * FightCharge.GetTotalNumberOfBars()) / FightCharge.GetTotalNumberOfBars();
        Vector2 barSize = new Vector2(perBarWidth, 20);


        for (int i = 0; i < FightCharge.GetTotalNumberOfBars(); i++) 
        {
            Vector2 anchoredPosition = barStartingPosition + new Vector2(i *(perBarWidth + paddingBetweenBars), 0);
            Bar bar = CreateBar(anchoredPosition, barSize);
            barList.Add(bar);
        }
    }

   private void Update() 
    {
        for (int i = 0; i < FightCharge.GetTotalNumberOfBars(); i++) 
        { 
            Bar bar = barList[i];
            int chargeMin = i * FightCharge.chargeAmountPerBar;
            int chargeMax = (i + 1) * FightCharge.chargeAmountPerBar;
            int chargeAmount = FightCharge.GetChargeAmount();

            if (chargeAmount < chargeMin)
            {
                // Bar is empty
                bar.SetSize(0f);
            } 
            else {
                if (chargeAmount >= chargeMax)
                {
                    // Bar is completely full
                    bar.SetSize(1f);
                } 
                else {
                    // Bar is partly filled
                    float barPartialFillAmount = chargeAmount * 1f % FightCharge.chargeAmountPerBar;
                    bar.SetSize(barPartialFillAmount / FightCharge.chargeAmountPerBar);
                }
            }
        }

    }

    private Bar CreateBar(Vector2 anchoredPosition, Vector2 size) 
    {
        
        Transform barTransform = Instantiate(barTemplate, transform);
        barTransform.gameObject.SetActive(true); //now the bar appears in the UI so the player sees it

        RectTransform barRectTransform = barTransform.GetComponent<RectTransform>();
        barRectTransform.anchoredPosition = anchoredPosition;
        barRectTransform.sizeDelta = size;

        Bar bar = new Bar(barTransform);
        return bar;
    }

    private class Bar 
    {
        private Transform barTransform;
        private Image barImage;
        public Bar(Transform barTransform)
        {
            this.barTransform = barTransform;
            barImage = barTransform.Find("Bar Filled").GetComponent<Image>();
            SetSize(0f);
        }

        public void SetSize(float fillAmount) 
        {
            barImage.fillAmount = fillAmount;
        }
    }
}
