using UnityEngine;

/// <summary>
///     Controls a transient ghost visual effect that floats upwards,
///     and destroys itself after its lifetime expires.
/// </summary>
public class GhostVFX : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float lifetime = 2f;
    private Camera _mainCamera;
    private SpriteRenderer _spriteRenderer;
    private float _timer;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mainCamera = Camera.main;
        _timer = 0f;
    }
    private void Update()
    {
        _timer += Time.deltaTime;

        // Float upward
        transform.position += Vector3.up * (floatSpeed * Time.deltaTime);

        if (_timer >= lifetime) Destroy(gameObject);
    }
}