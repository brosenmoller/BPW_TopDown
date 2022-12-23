using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameOverlayView : UIView
{
    [SerializeField] private Slider healthBar;

    private PlayerHealthManager playerHealthManager;

    public override void Initialize()
    {
        playerHealthManager = FindObjectOfType<PlayerHealthManager>();
        EventManager.AddListener(EventType.ON_PLAYER_HEALTH_CHANGE, SetHealthBar);
    }

    public void SetHealthBar() => healthBar.value = playerHealthManager.Health / (float)playerHealthManager.MaxHealth;
}
