using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Rumbler : MonoBehaviour
{
    public CinemachineVirtualCamera camera;

    [SerializeField]
    private float _initialIntensity;

    [SerializeField]
    private float _duration;

    private float _shakeTimer;

    public void Rumble() 
    {
        this.ShakeCamera();
    }

    private void ShakeCamera()
    {
        _shakeTimer = _duration;
    }

    private void Update()
    {
        if (_shakeTimer > 0f)
        {
            _shakeTimer -= Time.deltaTime;
            var noise = this.camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = Mathf.Lerp(_initialIntensity, 0f, (_duration - _shakeTimer) / _duration);
        }
    }
}
