using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPbar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text angka;

    public void SetMaxStamina(float Stamina)
    {
        slider.maxValue = Stamina;
        slider.value = Stamina;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetStamina(float Stamina)
    {
        slider.value = Stamina;
        angka.text = Stamina.ToString();   
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


}
