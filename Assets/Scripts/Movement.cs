using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] InputAction thrust;
    [SerializeField] float thrustSpeed = 1000f;
    [SerializeField] InputAction rotation;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem leftParticles;
    [SerializeField] ParticleSystem rightParticles;
    #endregion

    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        //InputAction nesneleri default olarak kapalı geldiğinden dolayı enable etmemiz gerekir.
        thrust.Enable();
        rotation.Enable();
    }

    void FixedUpdate()
    {
        ThrustMovement();
        RotationMovement();
    }

    private void ThrustMovement()
    {
        StartThrusting();
    }

    private void RotationMovement()
    {
        StartRotation();
    }

    private void StartThrusting()
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thrustSpeed * Time.fixedDeltaTime);
            if (!engineParticles.isPlaying)
            {
                engineParticles.Play();
            }

            //eğer eklediğimiz ses çalışmıyorsa çalıştır
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
        }
        else
        {
            StopSomethings();
        }
    }

    private void StartRotation()
    {
        //ReadValue<float> kullanmamızın sebebi pozitif veynegatif olacağından dolayı. 
        //ReadValue<float> değerinden dönecek olan değer -1 veya +1(float değer olarak alıyoruz) olacaktır. Bu değeri floabir değişken olarak rotationValue olarak saklıyoruz. 
        float rotationValue = rotation.ReadValue<float>();
        RotateToRight(rotationValue);
        RotateToLeft(rotationValue);

    }

    private void RotateToRight(float rotationValue)
    {
        if (rotationValue < 0)
        {
            rightParticles.Play();
            RotationMovementDirection(rotationSpeed);
        }
    }

    private void RotateToLeft(float rotationValue)
    {
        if (rotationValue > 0)
        {
            leftParticles.Play();
            RotationMovementDirection(-rotationSpeed);
        }
    }

    private void StopSomethings()
    {
        engineParticles.Stop();
        audioSource.Stop();
    }

    private void RotationMovementDirection(float rotationMovementValue)
    {
        //Unity fizik sistemi ile bizim uyguladığımız fizik sistemi karıştığı için RigidBody'nin default rotation sistemini kapatmamız gerekir.
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationMovementValue * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }

   
}
