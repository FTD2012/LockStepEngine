using LockstepTutorial;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public Text fpsText;
    public Text logicFpsText;
    

    // Update is called once per frame
    void Update()
    {
        fpsText.text = "ViewFPS: " + Time.frameCount.ToString();
        logicFpsText.text = "LogicFPS: " + GameManager.Instance.curFrameIdx;
    }
}
