using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class listenInputButtonMono : MonoBehaviour
{
    public InputActionReference m_whatToListen;
    public UnityEvent m_onButtonPressed;
    public UnityEvent m_onButtonReleased;
    public UnityEvent<bool> m_onButtonChanged;
    public bool m_currentValue;

    //public UDPThreadSender udpSender; // Référence UDPThreadSender

    //private void OnEnable()
    //{
    //    m_whatToListen.action.performed += NotifyChanged;
    //    m_whatToListen.action.canceled += NotifyChanged;
    //}

    //private void NotifyChanged(InputAction.CallbackContext context)
    //{
    //    bool value = context.ReadValue<float>() > 0.5f;
    //    if (value != m_currentValue)
    //    {
    //        m_currentValue = value;
    //        if (m_currentValue)
    //        {
    //            m_onButtonPressed.Invoke();
    //            SendUdpMessage();
    //        }
    //        else
    //        {
    //            m_onButtonReleased.Invoke();
    //        }
    //    }
    //}

}
