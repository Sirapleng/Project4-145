using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHub : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text hpText;
    public Slider hpSlider;

    public void SetHUB(Unit unit)
    {
        nameText.text = unit.unitName;
        //hpText.text = "5/5" + unit.unitHP;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
