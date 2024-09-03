using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUB : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text hpText;
    public Slider hpSlider;

    private Unit unit;

    public void SetHUD(Unit unit)
    {
        this.unit = unit;
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        hpText.text = unit.currentHP + "/" + unit.maxHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
        hpText.text = hp + "/" + hpSlider.maxValue;
    }

    public int GetHP()
    {
        return (int)hpSlider.value;
    }
}