using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroAbilityButtonsShowPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RawImage abilityImage;
    private Text nameText;
    private Text dmgText;
    private Text rangeText;
    private Text cooldownText;
    private Text effectText;

    public int heroNumber;
    public int abilityType;

    void Start()
    {
        abilityImage = transform.GetChild(0).GetChild(0).gameObject.GetComponent<RawImage>();
        nameText = transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
        dmgText = transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>();
        rangeText = transform.GetChild(0).GetChild(3).gameObject.GetComponent<Text>();
        effectText = transform.GetChild(0).GetChild(4).gameObject.GetComponent<Text>();
        cooldownText = transform.GetChild(0).GetChild(5).gameObject.GetComponent<Text>();
        if (abilityType == 1)
            abilityImage.texture = Resources.Load<Texture>("Icons/Abilities/" + YourHeroTeam.heroNames[heroNumber] + "MainAttack");
        else if (abilityType == 2)
            abilityImage.texture = Resources.Load<Texture>("Icons/Abilities/" + YourHeroTeam.heroNames[heroNumber] + "Ability1");
        else if (abilityType == 3)
            abilityImage.texture = Resources.Load<Texture>("Icons/Abilities/" + YourHeroTeam.heroNames[heroNumber] + "Ability2");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(PlaceHero.gameBegun)
            transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void UpdateText(GameObject hero)
    {
        if (abilityType == 1)
        {
            nameText.text = hero.GetComponent<Hero>().mainAttackName;
            dmgText.text = "Dmg: " + hero.GetComponent<Hero>().mainAttackDmg.ToString();
            rangeText.text = "Range: " + hero.GetComponent<Hero>().mainAttackRange.ToString();
            cooldownText.text = "";
            effectText.text = "";
        }
        else if (abilityType == 2)
        {
            nameText.text = hero.GetComponent<Hero>().ability1Name;
            dmgText.text = "Dmg: " + hero.GetComponent<Hero>().ability1Dmg.ToString();
            rangeText.text = "Range: " + hero.GetComponent<Hero>().ability1Range.ToString();
            cooldownText.text = "Cooldown: " + hero.GetComponent<Hero>().ability1Cooldown.ToString();
            effectText.text = "Effect: " + hero.GetComponent<Hero>().firstAbility;
        }
        else if (abilityType == 3)
        {
            nameText.text = hero.GetComponent<Hero>().ability2Name;
            if (hero.GetComponent<Hero>().ability2Passive)
            {
                dmgText.text = "Passive ability";
                rangeText.text = "";
                cooldownText.text = "";
            }
            else
            {
                dmgText.text = "Dmg: " + hero.GetComponent<Hero>().ability2Dmg.ToString();
                rangeText.text = "Range: " + hero.GetComponent<Hero>().ability2Range.ToString();
                cooldownText.text = "Cooldown: " + hero.GetComponent<Hero>().ability2Cooldown.ToString();
            }
            effectText.text = "Effect: " + hero.GetComponent<Hero>().secondAbility;
        }
    }
}
