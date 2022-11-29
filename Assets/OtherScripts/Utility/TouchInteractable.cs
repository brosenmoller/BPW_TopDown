using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class TouchInteractable : MonoBehaviour
{
    [Header("Events")]
    private UnityEvent OnTouchEnter;
    private UnityEvent OnTouchExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTouchEnter.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTouchExit.Invoke();
    }
}
