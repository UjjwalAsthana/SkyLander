using System;
using UnityEngine;

public class LanderVisuals : MonoBehaviour
{
    [SerializeField] private GameObject landerExplosionVFX;
    private Lander lander;
    private void Awake()
    {
        lander = GetComponent<Lander>();
    }
    private void Start()
    {
        lander.OnLanded += Lander_OnLander;
    }

    private void Lander_OnLander(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.TooFastLanding:
            case Lander.LandingType.TooSteepAngle:
            case Lander.LandingType.WrongLandingArea:
                Instantiate(landerExplosionVFX, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
        }
    }
}
