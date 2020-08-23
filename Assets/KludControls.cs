// GENERATED AUTOMATICALLY FROM 'Assets/KludControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @KludControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @KludControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""KludControls"",
    ""maps"": [
        {
            ""name"": ""Bust"",
            ""id"": ""31a5df80-1585-45bc-9449-cd67c18e19b5"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""ed6c75e6-01ff-4cc4-aaa4-4c47dbae61c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bd23ef7d-4197-456e-9714-ebf44206a901"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b0ea8405-2c83-4abf-848a-eb6485275b53"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a03e2e9d-bb9d-4bad-a9a7-8b3ccd82e7d1"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Bust
        m_Bust = asset.FindActionMap("Bust", throwIfNotFound: true);
        m_Bust_Fire = m_Bust.FindAction("Fire", throwIfNotFound: true);
        m_Bust_Point = m_Bust.FindAction("Point", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Bust
    private readonly InputActionMap m_Bust;
    private IBustActions m_BustActionsCallbackInterface;
    private readonly InputAction m_Bust_Fire;
    private readonly InputAction m_Bust_Point;
    public struct BustActions
    {
        private @KludControls m_Wrapper;
        public BustActions(@KludControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire => m_Wrapper.m_Bust_Fire;
        public InputAction @Point => m_Wrapper.m_Bust_Point;
        public InputActionMap Get() { return m_Wrapper.m_Bust; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BustActions set) { return set.Get(); }
        public void SetCallbacks(IBustActions instance)
        {
            if (m_Wrapper.m_BustActionsCallbackInterface != null)
            {
                @Fire.started -= m_Wrapper.m_BustActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_BustActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_BustActionsCallbackInterface.OnFire;
                @Point.started -= m_Wrapper.m_BustActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_BustActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_BustActionsCallbackInterface.OnPoint;
            }
            m_Wrapper.m_BustActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
            }
        }
    }
    public BustActions @Bust => new BustActions(this);
    public interface IBustActions
    {
        void OnFire(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
    }
}
