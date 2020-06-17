using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class SliderValueScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider mySlider;
    public TextMeshProUGUI myText;
    void Start()
    {
        UpdateSliderValue();
    }

    public void UpdateSliderValue()
    {
        myText.text = mySlider.value.ToString();
    }
}
