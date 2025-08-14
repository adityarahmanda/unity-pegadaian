using Sirenix.OdinInspector;
using UnityEngine;

namespace LZY.Utilities
{

    [ExecuteInEditMode]
    public class FlipbookAnimator : MonoBehaviour
    {
        public enum PlayType
        {
            ByFrameRate,
            ByDuration
        }
        
        public float Duration
        {
            get
            {
                if (frames == null || frames.Length == 0)
                    return 0f;

                return playType switch
                {
                    PlayType.ByDuration => duration,
                    PlayType.ByFrameRate => frames.Length / frameRate,
                    _ => 0f
                };
            }
        }
        
        public Sprite[] frames;
        public PlayType playType = PlayType.ByFrameRate;
        [ShowIf(nameof(playType), PlayType.ByFrameRate)]
        public float frameRate = 10f;
        [ShowIf(nameof(playType), PlayType.ByDuration)]
        [SerializeField] private float duration = 1f;
        public bool loop = false;
        
        [Header("State")]
        public bool isPlaying = false;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        private SpriteRenderer _spriteRenderer;
        private int _currentFrameIndex = 0;
        private float _frameTimer = 0f;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (frames != null && frames.Length > 0)
            {
                _spriteRenderer.sprite = frames[0];
            }
        }

        private void Update()
        {
            if (!isPlaying || frames == null || frames.Length == 0)
                return;

            _frameTimer += Time.deltaTime;
            var frameTime = playType == PlayType.ByFrameRate
                ? 1f / frameRate
                : duration / frames.Length;

            if (_frameTimer >= frameTime)
            {
                _frameTimer = 0f;
                _currentFrameIndex++;

                if (_currentFrameIndex >= frames.Length)
                {
                    if (loop)
                    {
                        _currentFrameIndex = 0;
                    }
                    else
                    {
                        isPlaying = false;
                        return;
                    }
                }

                _spriteRenderer.sprite = frames[_currentFrameIndex];
            }
        }

        [Button]
        public void Play()
        {
            if (frames == null || frames.Length == 0)
            {
                Debug.LogWarning("No frames assigned.");
                return;
            }

            _currentFrameIndex = 0;
            _frameTimer = 0f;
            _spriteRenderer.sprite = frames[0];
            isPlaying = true;
        }

        [Button]
        public void Resume()
        {
            if (isPlaying) return;

            _frameTimer = 0f;
            isPlaying = true;
        }

        [Button]
        public void Stop()
        {
            if (!isPlaying) return;

            isPlaying = false;
        }
    }
}