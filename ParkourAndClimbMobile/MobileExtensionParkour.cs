using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FC_ParkourSystem;

public class MobileExtensionParkour : MonoBehaviour
{
    public InputManager inputManager;
    public bool isMobile;
    public VariableJoystick variableJoystick;
    public FixedTouchField touchScreen;

    public List<GameObject> mobileComponents;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        variableJoystick = FindObjectOfType<VariableJoystick>();
        touchScreen = FindObjectOfType<FixedTouchField>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMobile)
        {
            inputManager.DirectionInput = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical);
            inputManager.CameraInput = new Vector2(touchScreen.GetComponent<FixedTouchField>().TouchDist.x, touchScreen.GetComponent<FixedTouchField>().TouchDist.y);
           
        }
        foreach(GameObject mobileComponent in mobileComponents)
        {
            mobileComponent.SetActive(isMobile);
        }
       
    }
    public void JumpDown()
    {
        inputManager. JumpKeyDown = true;
        inputManager. Jump = true;
    }
    public void JumpUP()
    {
        inputManager. JumpKeyDown = false;
        inputManager. Jump = false;
    }

    public void OnDropBtnUp()
    {
        inputManager.Drop = false;
    }
    public void OnDropBtnDown()
    {
        inputManager.Drop = true;
    }

    public void JumpHangBackUP()
    {
        inputManager.JumpFromHang = false;
    }
    public void JumpHangBackDown()
    {
        inputManager.JumpFromHang = true;
    }
}
