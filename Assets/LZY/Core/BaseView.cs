using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace LZY
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BaseView : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;

        public bool IsVisible => _isVisible;

        public UnityEvent OnShowComplete;
        public UnityEvent OnHideComplete;

        protected bool _isVisible = false;
        
        protected SceneCoreView _sceneCoreView;
        protected Tween _fadeTween;

        private void OnValidate()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void Initialize()
        {
            _sceneCoreView = SceneCore.GetService<SceneCoreView>();
            OnInitialize();
        }

        // OnInitialize is run before awake
        protected virtual void OnInitialize() { }

        protected virtual void OnPreShow() { }

        protected virtual void OnShow() { }

        protected virtual void OnHide() { }

        public void Show()
        {
            _sceneCoreView.ShowView(this);
        }

        public void Return()
        {
            _sceneCoreView.ReturnToPreviousView();
        }

        public void ShowOverlay(bool hasAnimation = true)
        {
            if (_isVisible) return;
            _isVisible = true;

            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;

            if (hasAnimation)
            {
                if (_fadeTween != null && _fadeTween.IsPlaying())
                    _fadeTween.Kill(true);
                _fadeTween = canvasGroup.DOFade(1f, 0.25f).OnComplete(() =>
                {
                    OnShow();
                    OnShowComplete?.Invoke();
                });

                OnPreShow();
            }
            else
            {
                canvasGroup.alpha = 1;
                OnPreShow();
                OnShow();
                OnShowComplete?.Invoke();
            }
        }

        public void HideOverlay(bool animation = true)
        {
            if (_isVisible == false) return;
            _isVisible = false;

            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;

            if (animation)
            {
                if (_fadeTween != null && _fadeTween.IsPlaying())
                    _fadeTween.Kill(true);
                _fadeTween = canvasGroup.DOFade(0f, 0.25f).OnComplete(() => OnHideComplete?.Invoke());
                OnHide();
            }
            else
            {
                canvasGroup.alpha = 0;
                OnHide();
                OnHideComplete?.Invoke();
            }
        }

        public void SilentShow()
        {
            _isVisible = true;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public void SilentHide()
        {
            _isVisible = false;
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}
