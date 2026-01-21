using UnityEngine;
using UnityEngine.Events;

public enum AllowedInputs
{
    ERightArray = 0,
    ELeftArray,
    EDownArray,
    EUpArray,
    EEnterKey,
    ESpace,
    EEscape

};


public class UserInputs : MonoBehaviour
{
    private UnityAction[] actions = new UnityAction[7];

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (actions[(int)AllowedInputs.ERightArray] != null)
                actions[(int)AllowedInputs.ERightArray].Invoke();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (actions[(int)AllowedInputs.ELeftArray] != null)
                actions[(int)AllowedInputs.ELeftArray].Invoke();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (actions[(int)AllowedInputs.EUpArray] != null)
                actions[(int)AllowedInputs.EUpArray].Invoke();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (actions[(int)AllowedInputs.EDownArray] != null)
                actions[(int)AllowedInputs.EDownArray].Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (actions[(int)AllowedInputs.EEnterKey] != null)
                actions[(int)AllowedInputs.EEnterKey].Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (actions[(int)AllowedInputs.ESpace] != null)
                actions[(int)AllowedInputs.ESpace].Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (actions[(int)AllowedInputs.EEscape] != null)
                actions[(int)AllowedInputs.EEscape].Invoke();
        }
    }

    public void Register(UnityAction action, AllowedInputs input)
    {
        actions[(int)input] += action;
    }
    
    public void UnRegister(UnityAction action, AllowedInputs input)
    {
        actions[(int)input] -= action;
    }



}
