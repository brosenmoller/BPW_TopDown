using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Player Health Settings")]
    [SerializeField] private int maxHealth;

    [Header("Hit effects")]
    [SerializeField] private float getHitKnockbackMultiplier;
    [SerializeField] private float stunDuration;
    [SerializeField] private Material hitFlashMat;

    private PlayerAbilityManager abilityManager;

    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteHolder;

    private Material normalMat;

    private bool canTakeDamage = true;

    private int _health;
    public int Health {
        private set 
        {
            _health = value;

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

    public void TakeDamage(Vector2 direction, int damage, float force)
    {
        if (!canTakeDamage) { return; }

        Health -= damage;
        StartCoroutine(DamageEffects(direction, force));
    }

    private IEnumerator DamageEffects(Vector2 direction, float force)
    {
        rigidBody2D.AddForce(force * getHitKnockbackMultiplier * direction, ForceMode2D.Impulse);
        spriteHolder.material = hitFlashMat;
        //AudioManager.Instance.Play("FungusHit");

        abilityManager.DeactivateAllMovementAbilities();

        yield return new WaitForSeconds(.1f);

        spriteHolder.material = normalMat;

        yield return new WaitForSeconds(stunDuration - .1f);

        abilityManager.ReactivateAllMovementAbilities();
    }


    private void OnPlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
