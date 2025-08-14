using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LZY
{
    public class SceneCoreView : SceneService
    {
        protected override string GetId() => "SceneCoreView";

        public Canvas Canvas => _canvas;
        private Canvas _canvas;

        public CanvasScaler CanvasScaler => _canvasScaler;
        private CanvasScaler _canvasScaler;

        [Header("Cached Views"), ShowInInspector]
        private List<BaseView> _views = new List<BaseView>();

        [Header("Stacked Views"), ShowInInspector]
        private Stack<BaseView> _stackedViews = new Stack<BaseView>();

        [SerializeField] private BaseView firstView;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasScaler = GetComponent<CanvasScaler>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            PopulateView();
        }

        protected override void OnActivate()
        {
            ShowView(firstView);
        }

        /// <summary>
        /// Populate all views in the scene.
        /// Child counted = only the first level of hierarchy.
        /// </summary>
        private void PopulateView()
        {
            var childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                var view = child.GetComponent<BaseView>();
                if (view != null)
                {
                    view.Initialize();
                    view.SilentHide();
                    _views.Add(view);
                }
            }
        }

        public void ShowView(BaseView view)
        {
            if (view == null) return;
            
            if (_stackedViews.Count > 0)
            {
                var topView = _stackedViews.Peek();
                topView.HideOverlay(false);
            }

            _stackedViews.Push(view);
            view.ShowOverlay();
        }

        public void ReturnToFirstView()
        {
            if (_stackedViews.Count == 1) return;

            var veryTopView = _stackedViews.Pop();
            veryTopView.HideOverlay();

            while (_stackedViews.Count >= 1)
            {
                if (_stackedViews.Count == 1)
                {
                    var firstView = _stackedViews.Peek();
                    firstView.ShowOverlay();
                    break;
                }

                var view = _stackedViews.Pop();
                view.HideOverlay(false);
            }
        }

        public void ReturnToPreviousView()
        {
            if (_stackedViews.Count == 1)
            {
                Debug.LogWarning("Hide view invalid. There should only be at least one view in this scene.");
                return;
            }

            var view = _stackedViews.Pop();
            var topView = _stackedViews.Peek();
            view.HideOverlay();
            topView.ShowOverlay(false);
        }

        public T GetView<T>() where T : BaseView
        {
            var view = _views.Find(v => v.GetType() == typeof(T));
            if (view == null)
            {
                Debugger.LogError($"GetView fail, {typeof(T).Name} is not exist.");
                return null;
            }

            return (T)view;
        }
    }
}
