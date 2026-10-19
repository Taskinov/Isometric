using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    [SerializeField] GameObject rightButtons;
    [SerializeField] GameObject leftButtons;
    [SerializeField] GameObject hitButton;
    [SerializeField] GameObject talkButton;
    [SerializeField] GameObject touchButton;

    
    public void HideUIElements()
    {
        rightButtons.SetActive(false);
        leftButtons.SetActive(false);
    }

    public void UnHideUIElements()
    {
        rightButtons.SetActive(true);
        leftButtons.SetActive(true);
    }

    public void TalkAction()
    {
        hitButton.SetActive(false);
        talkButton.SetActive(true);
    }
    public void HitAction()
    {
        talkButton.SetActive(false);
        hitButton.SetActive(true);
    }
    public void TouchAction()
    {
        touchButton.SetActive(true);
        hitButton.SetActive(false);
    }
}
