using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [EditorIcon("icon_controller")]
    public class TouchInputManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Event A finger touched the screen")]
        private InputEventTouchBegin inputEventTouchBegin;

        [SerializeField, Tooltip("Event A finger moved on the screen")]
        private InputEventTouchMove inputEventTouchMove;

        [SerializeField, Tooltip("Event A finger is touching the screen but hasn't moved")]
        private InputEventTouchStationary inputEventTouchStationary;

        [SerializeField, Tooltip("Event A finger was lifted from the screen. This is the final phase of a touch")]
        private InputEventTouchEnd inputEventTouchEnd;

        [SerializeField, Tooltip("The system cancelled tracking for the touch")]
        private InputEventTouchCancel inputEventTouchCancel;

        [SerializeField] private Vector3 touchPosition;

        private bool _mouseDown;
        private bool _mouseUpdate;

        private void Update()
        {
#if UNITY_EDITOR
            if (UnityEngine.Device.SystemInfo.deviceType != DeviceType.Desktop)
            {
                HandleTouch();
            }
            else
            {
                HandleMouse();
            }
#else
            HandleTouch();
#endif
        }

        void HandleTouch()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (inputEventTouchBegin != null)
                        {
                            inputEventTouchBegin.Raise(touch.position);
                        }

                        break;
                    case TouchPhase.Moved:
                        if (inputEventTouchMove != null)
                        {
                            inputEventTouchMove.Raise(touch.position);
                        }

                        break;
                    case TouchPhase.Stationary:
                        if (inputEventTouchStationary != null)
                        {
                            inputEventTouchStationary.Raise(touch.position);
                        }

                        break;
                    case TouchPhase.Ended:
                        if (inputEventTouchEnd != null)
                        {
                            inputEventTouchEnd.Raise(touch.position);
                        }

                        break;
                    case TouchPhase.Canceled:
                        if (inputEventTouchCancel != null)
                        {
                            inputEventTouchCancel.Raise(touch.position);
                        }

                        break;
                }

                touchPosition = touch.position;
            }
        }

        void HandleMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_mouseDown)
                {
                    _mouseDown = true;
                    _mouseUpdate = true;
                    if (inputEventTouchBegin != null)
                    {
                        inputEventTouchBegin.Raise(Input.mousePosition);
                    }

                    touchPosition = Input.mousePosition;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _mouseDown = false;
                _mouseUpdate = false;
                if (inputEventTouchEnd != null)
                {
                    inputEventTouchEnd.Raise(Input.mousePosition);
                }

                touchPosition = Input.mousePosition;
            }

            if (_mouseDown && _mouseUpdate)
            {
                if (inputEventTouchMove != null)
                {
                    inputEventTouchMove.Raise(Input.mousePosition);
                }

                touchPosition = Input.mousePosition;
            }
        }
    }
}