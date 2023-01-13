using System;
using Framework;
using LockstepTutorial;
using UnityEngine;
using UnityEngine.UI;

public class UIFrameTimeline : MonoBehaviour
{
    
    public InputField inputField;
    public Text frameText;
    public Text playerBtnText;
    public Button leftButton;
    public Button rightButton;
    public Button playButton;
    public Button jumpButton;


    private int curFrame;
    private int totalFrame;
    private float rightHoldTime;
    private float leftHoldTime;
    
    public void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);
        jumpButton.onClick.AddListener(OnJumpButtonClick);
        
        EventManager.AddEventListener(Notification.UPDATE_FRAME, UpdateFrame);
        
        UpdatePlayerButton();
    }

    public void Update()
    {
        UpdateFrame();
        UpdateKeyboardEvent();
    }

    public void UpdateFrame()
    {
        UpdateCurrentFrame();
        UpdateTotalFrame();
        frameText.text = curFrame + "/" + totalFrame;
    }

    private void UpdateKeyboardEvent()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnRightButtonClick();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnLeftButtonClick();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPlayButtonClick();
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            OnJumpButtonClick();
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            rightHoldTime += Time.deltaTime;
            if (rightHoldTime > 0.2f)
            {
                OnRightButtonClick();
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            rightHoldTime = 0f;
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            leftHoldTime += Time.deltaTime;
            if (leftHoldTime > 0.2f)
            {
                OnLeftButtonClick();
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            leftHoldTime = 0f;
        }
    }
    
    public void UpdateCurrentFrame()
    {
        curFrame = GameManager.Instance.curFrameIdx;
    }
    
    public void UpdateTotalFrame()
    {
        totalFrame = GameManager.Instance.frames.Count;
    }

    public void UpdatePlayerButton()
    {
        if (GameManager.Instance == null)
        {
            return;
        }
        playerBtnText.text = GameManager.Instance.isManualPlayMode ? "Stop" : "Play";
    }

    private void OnPlayButtonClick()
    {
        if (GameManager.Instance == null)
        {
            return;
        }
        GameManager.Instance.isManualPlayMode = !GameManager.Instance.isManualPlayMode;
        UpdatePlayerButton();
    }
    
    private void OnLeftButtonClick()
    {
        GameManager.Instance.StepPreviousFrame();
    }
    
    private void OnRightButtonClick()
    {
        GameManager.Instance.StepNextFrame();
    }

    private void OnJumpButtonClick()
    {
        // GameManager.Instance.isManualPlayMode = true;
        GameManager.Instance.JumpToFrame(Int32.Parse(inputField.text));
    }

    private void OnDestroy()
    { 
        playButton.onClick.RemoveListener(OnPlayButtonClick);
        leftButton.onClick.RemoveListener(OnLeftButtonClick);
        rightButton.onClick.RemoveListener(OnRightButtonClick);
        jumpButton.onClick.RemoveListener(OnJumpButtonClick);
        
        EventManager.RemoveEventListener(Notification.UPDATE_FRAME, UpdateFrame);
    }
}