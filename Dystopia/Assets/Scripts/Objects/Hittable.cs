using UnityEngine;

public class Hittable : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private bool canTakeDamage = true;
    [SerializeField] private int _maxHealth = 100;
    public int maxHealth { get => _maxHealth; }
    private int _currentHealth;
    public int currentHealth { get => _currentHealth; }
    
    [Space]
    [SerializeField] private SoundManager.Sound hitSound;

    //private Material matWhite;
    //private Material matDefault;
    private Color colorWhite = new Color(1,1,1,1);
    private Color colorDefault;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        _currentHealth = _maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //matWhite = Resources.Load("FlashMaterial", typeof(Material)) as Material;
        //matDefault = spriteRenderer.material;
        colorDefault = spriteRenderer.color;
    }

    public void SetHealth(int newCurrentHealth, int newMaxHealth) {
        _maxHealth = Mathf.Clamp(newMaxHealth, 0, newMaxHealth);
        _currentHealth = Mathf.Clamp(newCurrentHealth, 0, _maxHealth);
    }

    public void TakeDamage(int damage, bool isCriticalHit) {
        SoundManager.PlaySound(hitSound);
        if(canTakeDamage) {
            DamagePopup.Create(transform.position, damage, isCriticalHit);
            _currentHealth -= damage;
            //spriteRenderer.material = matWhite;
            spriteRenderer.color = colorWhite;
            if(_currentHealth<=0) {
                _currentHealth = 0;
                DestroyObject();
            }
            //Invoke("ResetMaterial", .1f);
            Invoke("ResetColor", .1f);
        }
    }

    private void ResetMaterial() {
        //spriteRenderer.material = matDefault;
    }

    private void ResetColor() {
        spriteRenderer.color = colorDefault;
    }

    private void DestroyObject(){
        //TODO: Destroy object and leave destroyed parts on the ground
        Destroy(this.gameObject);
    }

}
