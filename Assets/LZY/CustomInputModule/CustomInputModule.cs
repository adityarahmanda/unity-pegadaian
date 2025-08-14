using UnityEngine;
using UnityEngine.EventSystems;

namespace LZY.InputModule
{
    public class CustomInputModule : StandaloneInputModule
    {
        public static CustomInputModule Instance
        {
            get
            {
                if (_instance == null)
                    _instance = EventSystem.current?.currentInputModule as CustomInputModule;
                return _instance;
            }
        }

        private static CustomInputModule _instance;

        private ICustomInputModuleComponent[] _inputModuleComponents;

        protected override void Awake()
        {
            base.Awake();
            if (_instance == null)
                _instance = this;
            else
                Destroy(this);
            _inputModuleComponents = GetComponents<ICustomInputModuleComponent>();
        }

        public T GetInputModuleComponent<T>() where T : ICustomInputModuleComponent
        {
            foreach (var inputModuleComponent in _inputModuleComponents)
            {
                if (inputModuleComponent.GetType() == typeof(T))
                    return (T)inputModuleComponent;
            }

            return default;
        }

        public void ProcessTouch(Touch touch)
        {
            Input.simulateMouseWithTouches = true;
            var pointerData = GetTouchPointerEventData(touch, out _, out _);
            var released = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
            ProcessTouchPress(pointerData, !released, released);
        }
    }

    public interface ICustomInputModuleComponent
    {
    }
}
