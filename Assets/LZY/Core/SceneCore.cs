using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LZY
{
    [DefaultExecutionOrder(-5)]
    public abstract class SceneCore : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        protected bool _isInitialized;

        // scene core UI acts as the main canvas for the scene
        [SerializeField] protected SceneCoreView sceneCoreUI;
        [SerializeField] protected List<SceneService> sceneServices = new List<SceneService>();
        [SerializeField] protected bool initializeOnAwake = true;

        // Store in dictionary for easy access
        protected static Dictionary<string, SceneService> _serviceMap = new Dictionary<string, SceneService>();
        protected static SceneCoreView _sceneCoreView;

        public void Initialize()
        {
            if (_isInitialized) return;

            OnPreInitialize();
            OnInitialize();
            OnPostInitialize();
        }

        public void Deinitialize()
        {
            if (!_isInitialized) return;
            OnPreDeinitialize();
            OnDeinitialize();
            OnPostDeinitialize();
        }

        public void Activate()
        {
            if (!_isInitialized) return;
            OnActivate();
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            OnDeactivate();
        }

        public static T GetService<T>() where T : SceneService
        {
            var name = typeof(T).Name;
            var isValueExist = _serviceMap.TryGetValue(name, out var sceneService);
            if (isValueExist == false)
            {
                Debugger.LogColor($"GetService fail, {name} is not exist.", Color.red);
            }

            return isValueExist ? (T)sceneService : default;
        }

        public static T GetServiceView<T>() where T : BaseView
        {
            if (_sceneCoreView == null)
                _sceneCoreView = GetService<SceneCoreView>();
            
            if (_sceneCoreView != null)
                return _sceneCoreView.GetView<T>();

            return null;
        }

        public void Quit()
        {
            Deinitialize();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		    Application.Quit();
#endif
        }

        protected virtual void Awake()
        {
            if (initializeOnAwake)
                Initialize();

            Input.multiTouchEnabled = true;
        }

        protected virtual void Update()
        {
            if (!IsActive) return;
            
            OnUpdate();
        }

        protected void OnDestroy()
        {
            Deinitialize();
        }

        // PROTECTED METHODS
        protected virtual void OnPreInitialize() { }

        protected void OnInitialize()
        {
            RefreshServices();
            for (int i = 0; i < sceneServices.Count; i++)
            {
                sceneServices[i].Initialize();
            }
            _isInitialized = true;
            Activate();
        }

        protected virtual void OnPostInitialize() { }

        protected void OnUpdate()
        {
            for (int i = 0, count = sceneServices.Count; i < count; i++)
            {
                sceneServices[i].DoUpdate();
            }
        }

        protected void OnDeactivate()
        {
            for (int i = 0; i < sceneServices.Count; i++)
            {
                sceneServices[i].Deactivate();
            }
            IsActive = false;
        }
        
        protected virtual void OnPreDeinitialize() { }

        protected void OnDeinitialize()
        {
            Deactivate();
            for (int i = 0; i < sceneServices.Count; i++)
            {
                sceneServices[i].Deinitialize();
            }
            _serviceMap.Clear();
            _isInitialized = false;
        }

        protected virtual void OnPostDeinitialize() { }

        protected void OnActivate()
        {
            for (int i = 0; i < sceneServices.Count; i++)
            {
                sceneServices[i].Activate();
            }
            IsActive = true;
        }

        public void AddService(SceneService service)
        {
            if (service == null) return;
            
            if (_serviceMap.ContainsKey(service.ServiceId) == true)
            {
                Debugger.LogWarning($"Service {service} already added.");
                return;
            }

            _serviceMap.Add(service.ServiceId, service);
        }

        public void RemoveService(SceneService service)
        {
            if (_serviceMap.ContainsValue(service) == true)
            {
                _serviceMap.Remove(nameof(service));
            }
        }

        private void RefreshServices()
        {
            sceneServices = GetComponentsInChildren<SceneService>().ToList();
            if (sceneCoreUI != null)
                sceneServices.Add(sceneCoreUI);
            _serviceMap.Clear();
            foreach (var service in sceneServices)
            {
                AddService(service);
            }
        }
    }
}
