using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Player Health Settings")]
    [SerializeField] private int maxHealth;
    public int MaxHealth => maxHealth;

    [Header("Hit effects")]
    [SerializeField] private float getHitKnockbackMultiplier;
    [SerializeField] private float stunDuration;
    [SerializeField] private float hitInvincibilityDuration;
    [SerializeField] private Material hitFlashMat;
    [SerializeField] private AudioObject hitAudio;

    private PlayerAbilityManager abilityManager;

    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteHolder;

    private Material normalMat;

    private bool canTakeDamage = true;

    private int _health;
    public int Health {
        private set 
        {
            if (_health != value) 
            {
                _health = value;
                EventManager.InvokeEvent(EventType.ON_PLAYER_HEALTH_CHANGE);
            }

            if (_health <= 0)
            {
                OnPlayerDeath();
            }
        }
        get { return _health; }
    }

    private void Awake()
    {
        Health = maxHealth;
        abilityManager = GetComponent<PlayerAbilityManager>();
        rigidBody2D = abilityManager.rigidBody2D;
        spriteHolder = abilityManager.spriteHolder;
        normalMat = spriteHolder.material;
    }

    public void AddHealth(int healthIncrease)
    {
        Health += healthIncrease;
    }

    public void TakeDamage(Vector2 direction, int damage, float force)
    {
        if (!canTakeDamage) { return; }

        Health -= damage;
        DamageEffects(direction, force);
    }

    private void DamageEffects(Vector2 direction, float force)
    {
        rigidBody2D.AddForce(force * getHitKnockbackMultiplier * direction, ForceMode2D.Impulse);

        hitAudio.Play();

        spriteHolder.material = hitFlashMat;
        new Timer(.1f, () => spriteHolder.material = normalMat);
        
        abilityManager.DeactivateAllMovementAbilities();
        new Timer(stunDuration, () => abilityManager.ReactivateAllMovementAbilities());

        canTakeDamage = false;
        new Timer(hitInvincibilityDuration, () => canTakeDamage = true);
    }


    private void OnPlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
