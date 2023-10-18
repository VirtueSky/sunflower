using System;
using System.Collections.Generic;
using UnityEngine;

namespace VirtueSky.Events
{
    public abstract class EventListenerMono : MonoBehaviour
    {
        [SerializeField] private BindingListener bindingListener;

        protected abstract void ToggleListenerEvent(bool isListenerEvent);

        private void Awake()
        {
            if (bindingListener == BindingListener.UNTIL_DESTROY)
            {
                ToggleListenerEvent(true);
            }
        }

        private void OnEnable()
        {
            if (bindingListener == BindingListener.UNTIL_DISABLE)
            {
                ToggleListenerEvent(true);
            }
        }

        private void OnDisable()
        {
            if (bindingListener == BindingListener.UNTIL_DISABLE)
            {
                ToggleListenerEvent(false);
            }
        }

        private void OnDestroy()
        {
            if (bindingListener == BindingListener.UNTIL_DESTROY)
            {
                ToggleListenerEvent(false);
            }
        }
    }

    public enum BindingListener
    {
        UNTIL_DISABLE,
        UNTIL_DESTROY
    }
}