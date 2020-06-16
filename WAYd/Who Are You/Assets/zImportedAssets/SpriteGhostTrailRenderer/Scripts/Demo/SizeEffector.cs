using UnityEngine;

namespace ActionCode.Demo
{
    [DisallowMultipleComponent]
    public sealed class SizeEffector : MonoBehaviour
    {
        [Range(0.1f, 5f)] public float speed = 1.2f;
        private Vector3 _defaultScale;

        private const float MAX_SIZE = 2.8f;
        private const float MIN_SIZE = 0.7f;

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }

        public void IncreaseSize()
        {
            if (transform.localScale.x > MAX_SIZE)
            {
                transform.localScale = Vector3.one * MAX_SIZE;
                return;
            }
            transform.localScale += Vector3.one * speed * Time.deltaTime;
        }

        public void DecreaseSize()
        {
            if(transform.localScale.x < MIN_SIZE)
            {
                transform.localScale = Vector3.one * MIN_SIZE;
                return;
            }
            transform.localScale -= Vector3.one * speed * Time.deltaTime;
        }

        public void ResetSize()
        {
            transform.localScale = _defaultScale;
        }
    }
}