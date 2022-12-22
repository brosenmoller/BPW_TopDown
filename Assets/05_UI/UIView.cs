// Adapted from Code by Ralf Zeilstra (Game Developer HKU Year 1 in 2022)

using UnityEngine;

public abstract class UIView : MonoBehaviour
{
    public bool defaultView;

    public virtual void Initialize() { }
    public virtual void Hide() => gameObject.SetActive(false);
    public virtual void Show() => gameObject.SetActive(true);
}
