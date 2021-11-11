using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//https://bootcamp.uxdesign.cc/how-to-make-toggle-switch-button-unity-a755b7d6795f
public class ToggleSwitch : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image Frame;
    [SerializeField] Image BGFading;
    [SerializeField] GameObject SlideButton;
    [SerializeField] float LerpDuration;
    [SerializeField] float LeftMargin;
    [SerializeField] float RightMargin;
    [SerializeField] float ActionDistance;
    RectTransform FrameRect;
    RectTransform btRect;

    bool pointerDown = false;

    (float, float) FrameXBoundary;
    private Vector2 touchPos;

    public int SwitchState { get; private set; } = 0;
    int ButtonSwitchState = 0;
    float timeElapsed = 0;


    public void InitialState(int _switchState)
    {
        if (_switchState == 1)
        {
            SwitchOn();
        }
        else
        {
            SwitchOff();
        }
    }
    
    void Awake()
    {
        FrameRect = (RectTransform)Frame.transform;
        btRect = (RectTransform)SlideButton.transform;

        FrameXBoundary = (Frame.transform.position.x - (FrameRect.rect.width / 2),
            Frame.transform.position.x + (FrameRect.rect.width / 2));
    }
    
    void SwitchOn()
    {
        if (SwitchState == 0)
        {
            SwitchState = 1;
        }
    }

    void SwitchOff()
    {
        if (SwitchState == 1)
        {
            SwitchState = 0;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!pointerDown)
        {
            if (SwitchState == 1) // switch off
            {
                SwitchState = 0;
            }
            else // switch on
            {
                SwitchState = 1;
            }
        }
    }
    // OnPointerDown and OnPointerUp serves as rescaling slide button
    public void OnPointerDown(PointerEventData eventData)
    {
        touchPos = Input.GetTouch(0).position;
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
    }

    void Update()
    {
        if (pointerDown)
        {
            if (Input.touchCount > 0)
            {
                Vector2 TempPos = Input.GetTouch(0).position;
                
                if (TempPos.x - touchPos.x > ActionDistance)
                {
                    if (SwitchState != 1)
                    {
                        SwitchOn();
                    }  
                }

                if (TempPos.x - touchPos.x < - ActionDistance)
                {
                    if (SwitchState != 0)
                    {
                        SwitchOff();
                    } 
                } 
            }
        }
        if (SwitchState != ButtonSwitchState)
        {
            SlideMotion(SwitchState);
        }
    }
    // timeElapsed serves as position(percentage position) of button and also a time in slide duration
    void SlideMotion(int _btstate)
    {
        // motion start
        // set button pos state mide state
        
        if (_btstate == 1) // on state, 1
        {
            ButtonSwitchState = -1;
            SlideButton.transform.position = Vector3.Lerp(new Vector3(FrameXBoundary.Item1 + (btRect.rect.width / 2) + LeftMargin,
                SlideButton.transform.position.y, SlideButton.transform.position.z), new Vector3(FrameXBoundary.Item2 - (btRect.rect.width / 2) - RightMargin,
                SlideButton.transform.position.y, SlideButton.transform.position.z), timeElapsed / LerpDuration);
            BGFading.color = new Color(BGFading.color.r, BGFading.color.g, BGFading.color.b, timeElapsed);
            timeElapsed += Time.deltaTime;

            if (timeElapsed / LerpDuration >= 1) // if reach button state 1
            {
                ButtonSwitchState = 1;
                // button margin re assigning, in case of delta time inaccurate, see differemce of third parameter of Learp function
                SlideButton.transform.position = Vector3.Lerp(new Vector3(FrameXBoundary.Item1 + (btRect.rect.width / 2) + LeftMargin,
                    SlideButton.transform.position.y, SlideButton.transform.position.z), new Vector3(FrameXBoundary.Item2 - (btRect.rect.width / 2) - RightMargin,
                        SlideButton.transform.position.y, SlideButton.transform.position.z), 1);
                BGFading.color = new Color(BGFading.color.r, BGFading.color.g, BGFading.color.b, 1);
            }
        }
        else // off state, 0
        {
            ButtonSwitchState = -2;
            SlideButton.transform.position = Vector3.Lerp(new Vector3(FrameXBoundary.Item1 + (btRect.rect.width / 2) + LeftMargin, SlideButton.transform.position.y, SlideButton.transform.position.z),
                new Vector3(FrameXBoundary.Item2 - (btRect.rect.width / 2) - RightMargin, SlideButton.transform.position.y, SlideButton.transform.position.z),
                timeElapsed / LerpDuration);
            BGFading.color = new Color(BGFading.color.r, BGFading.color.g, BGFading.color.b, timeElapsed / LerpDuration);
            timeElapsed -= Time.deltaTime;

            if (timeElapsed / LerpDuration <= 0) // if reach 0 button state
            {
                ButtonSwitchState = 0;

                SlideButton.transform.position = Vector3.Lerp(new Vector3(FrameXBoundary.Item1 + (btRect.rect.width / 2) + LeftMargin, SlideButton.transform.position.y, SlideButton.transform.position.z),
                    new Vector3(FrameXBoundary.Item2 - (btRect.rect.width / 2) - RightMargin, SlideButton.transform.position.y, SlideButton.transform.position.z),
                    0);
                BGFading.color = new Color(BGFading.color.r, BGFading.color.g, BGFading.color.b, 0);
            }
        }
    }

}
