using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private AudioObject pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent(out PlayerHealthManager player);
        if (player != null)
        {
            UIViewManager.Instance.Show(typeof(WinView));
            Time.timeScale = 0;
            pickupSound.Play();
        }
    }
}
