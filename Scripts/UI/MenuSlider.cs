using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSlider : MonoBehaviour
{
    [SerializeField] GameObject targetForvard;
    [SerializeField] GameObject targetBack;
    private int speed = 5000;
    public bool flag = false;
    RectTransform rt;
    RectTransform rtt;

    public void Start()
    {
        rt = GetComponent<RectTransform>();
    }
    public void SlideMenu()
    {
        flag = true;
        rtt = targetForvard.GetComponent<RectTransform>();
    }
    public void SlideBack()
    {
        flag = true;
        rtt = targetBack.GetComponent<RectTransform>();
    }
    public void Update()
    {
        if (flag)
        {
            rt.localPosition = Vector2.MoveTowards(rt.localPosition, rtt.localPosition, speed * Time.deltaTime);
            if (Vector2.Distance(rt.localPosition, rtt.localPosition) < 0.1f)
            {
                flag = false;
                //buttonForvard.GetComponent<Button>().enabled = (buttonForvard.GetComponent<Button>().enabled) ? false : true;
                //buttonForvard2.GetComponent<Button>().enabled = (buttonForvard2.GetComponent<Button>().enabled) ? false : true;
                //buttonBack.GetComponent<Button>().enabled = (buttonBack.GetComponent<Button>().enabled) ? false : true;
            }
        }
        
    }

}
