using LockstepTutorial;
using UnityEngine;
using UnityEngine.UI;

public class InputMonitor : MonoBehaviour
{
    public Text inputTextDown;
    public Text inputTextHold;
    public Text inputTextUp;

    // Update is called once per frame
    void Update()
    {
        var inputDown = "";
        var inputHold = "";
        var inputUp = "";

        if (!GameManager.Instance.IsReplay)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                inputDown += "W";
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                inputDown += "A";
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                inputDown += "S";
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                inputDown += "D";
            }

            if (Input.GetKey(KeyCode.W))
            {
                inputHold += "W";
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputHold += "A";
            }

            if (Input.GetKey(KeyCode.S))
            {
                inputHold += "S";
            }

            if (Input.GetKey(KeyCode.D))
            {
                inputHold += "D";
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                inputUp += "W";
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                inputUp += "A";
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                inputUp += "S";
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                inputUp += "D";
            }
        }

        inputTextDown.text = "InputTextDown : " + inputDown;
        inputTextHold.text = "InputTextHold : " + inputHold;
        inputTextUp.text = "InputTextUp : " + inputUp;

        if (inputDown != "")
        {
            Debug.Log("[" + Time.frameCount + "]" + "InputTextDown : " + inputDown);
        }

        if (inputHold != "")
        {
            Debug.Log("[" + Time.frameCount + "]" + "InputTextHold : " + inputHold);
        }

        if (inputUp != "")
        {
            Debug.Log("[" + Time.frameCount + "]" + "InputTextUp : " + inputUp);
        }
    }
}
