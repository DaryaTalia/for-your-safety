using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    public UnityEvent ActionDelegate;

    public bool active = true;

    private void Start()
    {
        active = true;
    }

    public void ChangeStatus(bool value)
    {
        active = value;
    }
}
