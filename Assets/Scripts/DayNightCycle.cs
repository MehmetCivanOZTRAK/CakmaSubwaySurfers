using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("Bir günün kaç saniye süreceği")]
    [SerializeField] private float _dayDuration = 120f;

    [Header("Light References")]
    [SerializeField] private Light _sunLight;
    [SerializeField] private Gradient _skyColorGradient;
    [Header("Star Settings")]
    [SerializeField] private ParticleSystem _starSystem;
    [SerializeField] private float _maxStarEmission = 100f;

    private float _currentTime = 0.25f;

    private void Update()
    {

        _currentTime += Time.deltaTime / _dayDuration;

        if (_currentTime >= 1f) _currentTime = 0f;

        float sunRotation = (_currentTime * 360f) - 90f;
        _sunLight.transform.localRotation = Quaternion.Euler(sunRotation, 170f, 0f);


        _sunLight.intensity = Mathf.Clamp01(Vector3.Dot(_sunLight.transform.forward, Vector3.down) + 0.1f);
        _sunLight.color = _skyColorGradient.Evaluate(_currentTime);

        AmbientLightUpdate();
    }
    private void AmbientLightUpdate()
    {
        var emission = _starSystem.emission;
        float starpower = Mathf.Clamp01(1 - _sunLight.intensity);
        emission.rateOverTime = starpower * _maxStarEmission;

        if (_currentTime > 0.75f && _currentTime < 1.75f)
        {
            if (starpower > 0.1f && !_starSystem.isPlaying)
            {
                _starSystem.Play();
            }
        }
        else
        {
            emission.rateOverTime = 0;
            if (_starSystem.particleCount > 0)
            {
                _starSystem.Clear();
            }
            if (starpower <= 0.1f && _starSystem.isPlaying)
            {
                _starSystem.Stop();
            }
        }


    }
}