using UnityEngine;

namespace Enviroment.GlobalShadows
{
    public class ShadowController : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetShadowAlpha(int alpha)
        {
            var normalizedAlpha = Mathf.Clamp01(alpha / 132f);
            var color =_spriteRenderer.color;
            color.a = normalizedAlpha / 2;
            _spriteRenderer.color = color;
        }
    }
}