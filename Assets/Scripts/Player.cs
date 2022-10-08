using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public Transform atkPosRef;
    public Card choosenCard;
    public HealthBar healthBar;
    public TMP_Text healthText;
    public float Health;
    public float MaxHealth;
    public AudioSource audioSource;
    public AudioClip damageClip;
    public AudioClip drawClip;
    private Tweener animationTweener;
    private void Start()
    {
        Health = MaxHealth;
    }
    public Attack? AttackValue
    {
        get => choosenCard == null ? null : choosenCard.AttackValue;
    }

    public void Reset()
    {
        if (choosenCard != null)
        {
            choosenCard.Reset();
            choosenCard = null;
        }
    }

    public void SetChoosenCard(Card newCard)
    {
        if (choosenCard != null)
        {
            choosenCard.Reset();
        }

        choosenCard = newCard;
        choosenCard.transform.DOScale(choosenCard.transform.localScale * 1.1f, 0.2f);
    }

    public void ChangeHealth(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, 100);


        //healthbar
        healthBar.UpdateBar(Health / MaxHealth);
        //text
        healthText.text = Health + " / " + MaxHealth;
    }


    public void AnimateAttack()
    {
        animationTweener = choosenCard.transform
        .DOMove(atkPosRef.position, 0.4f)
        .SetEase(Ease.InOutBack);
    }

    public void AnimateDamage()
    {
        audioSource.PlayOneShot(damageClip);
        var Image = choosenCard.GetComponent<Image>();
        animationTweener = Image
            .DOColor(new Color(255, 0, 0), 0.1f)
            .SetLoops(5, LoopType.Yoyo)
            .SetDelay(0.2f);
    }

    public void AnimateDraw()
    {
        audioSource.PlayOneShot(drawClip);
        animationTweener = choosenCard.transform
        .DOMove(choosenCard.OriginalPositon, 0.5f)
        .SetEase(Ease.InOutBack)
        .SetDelay(0.2f);
    }

    public bool isAnimating()
    {
        return animationTweener.IsActive();
    }

    public void isClickable(bool value)
    {
        Card[] cards = GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.SetClickable(value);
        }
    }
}
