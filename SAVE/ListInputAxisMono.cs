using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ListenInputAxisMono : MonoBehaviour
{
    public InputActionReference m_whatToListen;
    public UnityEvent<float> m_onAxisChanged;
    public float m_currentValue;

    private void NotifyChanged(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if (value != m_currentValue)
        {
            m_currentValue = value;
            m_onAxisChanged.Invoke(m_currentValue);
        }
    }

    private void OnEnable()
    {
        m_whatToListen.action.Enable();
        m_whatToListen.action.performed += NotifyChanged;
        m_whatToListen.action.canceled += NotifyChanged;
    }

    private void OnDisable()
    {
        m_whatToListen.action.performed -= NotifyChanged;
        m_whatToListen.action.canceled -= NotifyChanged;
    }
}
