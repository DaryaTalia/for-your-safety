using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    public UnityEvent ActionDelegate;

    public bool active;

    private void Start()
    {
        active = false;
    }
}
