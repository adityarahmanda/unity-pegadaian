using UnityEngine;
using Random = UnityEngine.Random;

namespace LZY.Pegadaian.Misc
{
    public class BobbingBehaviour : MonoBehaviour
    {
        public bool isBobbing = true;
        
        [Header("Floating Settings")]
        [SerializeField] private Vector2 bobHeightRange = new Vector2(10f, 15f);
        [SerializeField] private Vector2 bobSpeedRange = new Vector2(.4f, .6f);
        
        private float _bobSpeed;
        private float _bobHeight;
        private float _bobPhase;
        
        private RectTransform _rectTransform;
        private Vector2 _initPos;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _initPos = _rectTransform.anchoredPosition;
        }

        private void OnEnable()
        {
            _bobSpeed = Random.Range(bobSpeedRange.x, bobSpeedRange.y);
            _bobHeight = Random.Range(bobHeightRange.x, bobHeightRange.y);
            _bobPhase = Random.Range(0f, 2f * Mathf.PI);
        }

        private void Update()
        {
            if (!isBobbing) return;
            
            var bob = Mathf.Sin(Time.time * _bobSpeed + _bobPhase) * _bobHeight;
            _rectTransform.anchoredPosition = _initPos + new Vector2(0, bob);
        }
    }
}