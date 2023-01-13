using System;
using Framework;
using LockstepTutorial;
using UnityEngine;
using UnityEngine.UI;
    
public class UIFrameInfo : MonoBehaviour
{
    [Header("Info")]
    public Text FrameText;
    public Text MousePosText;
    public Text InputUVText;
    public Text IsInputFireText;
    public Text IsSpeedUpText;

    [Header("Input")]
    public Text MoveInputText;
    public Text SkillInputText;

    [Header("State")]
    public Text CmdText;
    
    private int curFrame;
    private int totalFrame;

    public void Awake()
    {
        EventManager.AddEventListener(Notification.UPDATE_FRAME, UpdateFrame);

    }

    public void UpdateFrame()
    {
        UpdateCurrentFrame();
        UpdateTotalFrame();

        var frameInfo = GameManager.Instance.frames[curFrame];
        if (frameInfo == null)
        {
            GLog.Error("Invalid Frame Info = " + curFrame);
            return;
        }

        var playerInput = frameInfo.GetPlayerInputById(GameManager.Instance.localPlayerId);
        if (playerInput == null)
        {
            GLog.Error("Invalid Player Input = " + GameManager.Instance.localPlayerId);
            return;
        }
        
        FrameText.text = "FrameIdx: " + curFrame + "/" + totalFrame;
        MousePosText.text = "MousePos: " + playerInput.mousePos;
        InputUVText.text = "InputUV: " + playerInput.inputUV;
        IsInputFireText.text = "IsInputFire: " +  playerInput.isInputFire;
        IsSpeedUpText.text = "IsSpeedUp: " + playerInput.isSpeedUp;
    }
    
    public void UpdateCurrentFrame()
    {
        curFrame = GameManager.Instance.curFrameIdx;
    }
    
    public void UpdateTotalFrame()
    {
        totalFrame = GameManager.Instance.frames.Count;
    }
    
    private void OnDestroy()
    { 
        EventManager.RemoveEventListener(Notification.UPDATE_FRAME, UpdateFrame);
    }
}
