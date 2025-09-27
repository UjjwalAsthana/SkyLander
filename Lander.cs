using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.7f;
    public static Lander Instance { get; private set; }
    public event EventHandler OnCoinPickup;
    public event EventHandler OnFuelPickup;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        internal State state;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }
    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }
    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }
    private float fuelAmount;
    private float fuelAmountMax = 10f;
    private Rigidbody2D rb;
    private State state;
    private void Awake()
    {
        Instance = this;
        fuelAmount = fuelAmountMax;

        state = State.WaitingToStart;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (GameInput.Instance.IsUpActionPressed() || GameInput.Instance.IsLeftActionPressed() || GameInput.Instance.IsRightActionPressed())
                {
                    rb.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }
                break;
            case State.Normal:
                if (fuelAmount <= 0f)
                {
                    return;
                }

                if (GameInput.Instance.IsUpActionPressed() || GameInput.Instance.IsLeftActionPressed() || GameInput.Instance.IsRightActionPressed())
                {
                    consumeFuel();
                }

                if (GameInput.Instance.IsUpActionPressed())
                {
                    //moving up with some force
                    float upForce = 700f;
                    rb.AddForce(upForce * transform.up * Time.deltaTime);
                }

                if (GameInput.Instance.IsLeftActionPressed())
                {
                    //rotating speed to left
                    float turnLeft = +100f;
                    rb.AddTorque(turnLeft * Time.deltaTime);
                }

                if (GameInput.Instance.IsRightActionPressed())
                {
                    //rotating speed to right
                    float turnRight = -100f;
                    rb.AddTorque(turnRight * Time.deltaTime);
                }
                break;
            case State.GameOver:
                break;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed!!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        float softLanding = 3f;
        float relativeVelocityMagnitude = collision.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLanding)
        {
            Debug.Log("Landed too Hard!!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = 0.90f;
        if (dotVector < minDotVector)
        {
            Debug.Log("Landed On a too steep angle!!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }
        Debug.Log("Successful Landing");

        float maxLandingAngleScore = 100;
        float scoreMultiplier = 10f;
        float currentAngleScore = maxLandingAngleScore - Math.Abs(dotVector - 1f) * scoreMultiplier * maxLandingAngleScore;

        float maxLandingSpeedScore = 100;
        float currentLandingScore = (softLanding - relativeVelocityMagnitude) * maxLandingSpeedScore;

        Debug.Log("Current Landing Angle:" + currentAngleScore);
        Debug.Log("Current Landing Speed:" + currentLandingScore);

        int score = Mathf.RoundToInt((currentAngleScore + currentLandingScore) * landingPad.GetScoremultiplier());
        Debug.Log("Total Score:" + score);
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landingPad.GetScoremultiplier(),
            score = score,
        });
        SetState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            if (fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            OnFuelPickup?.Invoke(this, EventArgs.Empty);
            fuelPickup.DestroySelf();
        }

        if (collision.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }
    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });
    }
    private void consumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public float GetFuel()
    {
        return fuelAmount;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }
    public float GetSpeedX()
    {
        return rb.linearVelocityX;
    }

    public float GetSpeedY()
    {
        return rb.linearVelocityY;
    }
}