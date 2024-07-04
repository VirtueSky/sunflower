using UnityEngine;
using UnityEngine.EventSystems;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [EditorIcon("icon_controller")]
    public class TouchManager : BaseMono
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

        [SerializeField, Tooltip("Use mouse in editor")]
        private bool useMouse = false;

        [SerializeField, Tooltip("Event start click the mouse button")]
        private InputEventMouseDown inputEventMouseDown;

        [SerializeField, Tooltip("Event hold the mouse button")]
        private InputEventMouseUpdate inputEventMouseUpdate;

        [SerializeField, Tooltip("Event releases the mouse button")]
        private InputEventMouseUp inputEventMouseUp;

        [SerializeField, Tooltip("Disable when touching UI")]
        private bool ignoreUI = true;

        [SerializeField] private Vector3 touchPosition;

        private bool _mouseDown;
        private bool _mouseUpdate;

        public override void Tick()
        {
            base.Tick();
            if (ignoreUI)
            {
                if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null)
                {
                    HandleTouch();
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    HandleTouch();
                }
            }
#if UNITY_EDITOR
            if (useMouse)
            {
                if (ignoreUI)
                {
                    if (EventSystem.current.currentSelectedGameObject == null)
                    {
                        HandleMouse();
                    }
                }
                else
                {
                    HandleMouse();
                }
            }

#endif
        }

        void HandleTouch()
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (inputEventTouchBegin != null)
                    {
                        inputEventTouchBegin.Raise(touch);
                    }

                    break;
                case TouchPhase.Moved:
                    if (inputEventTouchMove != null)
                    {
                        inputEventTouchMove.Raise(touch);
                    }

                    break;
                case TouchPhase.Stationary:
                    if (inputEventTouchStationary != null)
                    {
                        inputEventTouchStationary.Raise(touch);
                    }

                    break;
                case TouchPhase.Ended:
                    if (inputEventTouchEnd != null)
                    {
                        inputEventTouchEnd.Raise(touch);
                    }

                    break;
                case TouchPhase.Canceled:
                    if (inputEventTouchCancel != null)
                    {
                        inputEventTouchCancel.Raise(touch);
                    }

                    break;
            }

            touchPosition = touch.position;
        }

        void HandleMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_mouseDown)
                {
                    _mouseDown = true;
                    _mouseUpdate = true;
                    if (inputEventMouseDown != null)
                    {
                        inputEventMouseDown.Raise(Input.mousePosition);
                    }

                    touchPosition = Input.mousePosition;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _mouseDown = false;
                _mouseUpdate = false;
                if (inputEventMouseUp != null)
                {
                    inputEventMouseUp.Raise(Input.mousePosition);
                }

                touchPosition = Input.mousePosition;
            }

            if (_mouseDown && _mouseUpdate)
            {
                if (inputEventMouseUpdate != null)
                {
                    inputEventMouseUpdate.Raise(Input.mousePosition);
                }

                touchPosition = Input.mousePosition;
            }
        }
    }
}