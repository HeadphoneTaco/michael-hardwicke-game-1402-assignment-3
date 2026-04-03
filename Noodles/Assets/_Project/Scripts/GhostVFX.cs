using UnityEngine;

namespace _Project.Scripts
{
    /// <summary>
    ///     Controls a transient ghost visual effect that floats upwards,
    ///     and destroys itself after its lifetime expires.
    /// </summary>
    /// TODO:Add billboard and fade effect
    public class GhostVFX : MonoBehaviour
    {
        /// <summary>
        ///     Upward movement speed applied every frame.
        /// </summary>
        [SerializeField] private float floatSpeed = 2f;

        /// <summary>
        ///     Total duration (in seconds) before the effect is destroyed.
        /// </summary>
        [SerializeField] private float lifetime = 2f;

        /// <summary>
        ///     Cached reference to the main camera.
        /// </summary>
        //This field '_mainCamera' IS being used, Intellisense is being silly
        private Camera _mainCamera;

        /// <summary>
        ///     Cached sprite renderer.
        /// </summary>
        //This field '_spriteRender' IS being used, Intellisense is being silly
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        ///     Elapsed time since the effect started.
        /// </summary>
        private float _timer;

        /// <summary>
        ///     Initializes component references and resets the effect timer.
        /// </summary>
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _mainCamera = Camera.main;
            _timer = 0f;
        }

        /// <summary>
        ///     Updates the effect each frame by advancing time, floating upward,
        ///     and destroying the object when complete.
        /// </summary>
        private void Update()
        {
            _timer += Time.deltaTime;

            // Float upward
            transform.position += Vector3.up * (floatSpeed * Time.deltaTime);

            if (_timer >= lifetime) Destroy(gameObject);
        }
    }
}