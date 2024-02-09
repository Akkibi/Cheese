using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private CustomInput input = null;
    [SerializeField] private Transform m_SpaceshipTransform;
    [SerializeField] private float m_RotationSpeed = 180.0f;
    [SerializeField] private float m_Speed = 500.0f;
    [SerializeField] private Transform m_CameraTransform;
    [SerializeField] private Transform m_Planet1; 
    [SerializeField] private float m_AttractionForce1 = 0.001f;
    [SerializeField] private Transform m_Planet2; 
    [SerializeField] private float m_AttractionForce2 = 0.0001f;
    [SerializeField] private Transform m_Planet3; 
    [SerializeField] private float m_AttractionForce3 = 0.0005f;
    [SerializeField] private Transform m_Planet4; 
    [SerializeField] private float m_AttractionForce4 = 0.00075f;
    private Vector2 m_MoveVector;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        input = new CustomInput();
    }
    void OnEnable()
    {
        input.Enable();
        input.Player.Movement.started += OnMovePerformed;
        input.Player.Movement.canceled += OnMoveCanceled;
    }

    void OnDisable()
    {
        input.Disable();
        input.Player.Movement.started -= OnMovePerformed;
        input.Player.Movement.canceled -= OnMoveCanceled;
    }

    void OnMovePerformed(InputAction.CallbackContext context)
    {
        m_MoveVector = context.ReadValue<Vector2>();
        Debug.Log("Move Vector: " + m_MoveVector);
        
    }
    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        m_MoveVector = Vector2.zero;
    }


    void Update()
    {
        Vector3 forward = m_CameraTransform.forward;

        Vector3 right = new Vector3(forward.z, 0, -forward.x).normalized;
        Vector3 direction = right * m_MoveVector.x + forward * m_MoveVector.y;
        Vector3 force = direction * m_Speed * Time.deltaTime;

        // Apply forces
        m_Rigidbody.AddForce(force);

        //rotate spaceship to face direction
        if (direction != Vector3.zero)
        {
            // Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            Vector3 velocity = m_Rigidbody.velocity;
            Quaternion toRotation = Quaternion.LookRotation(velocity.normalized);
            m_SpaceshipTransform.rotation = Quaternion.RotateTowards(m_SpaceshipTransform.rotation, toRotation, Time.deltaTime * m_RotationSpeed);
        }

        AddGravity(m_Planet1, m_AttractionForce1, 1);
        AddGravity(m_Planet2, m_AttractionForce2, 2);
        AddGravity(m_Planet3, m_AttractionForce3, 3);
        AddGravity(m_Planet4, m_AttractionForce4, 4);
    }
    void AddGravity(Transform attrationObject, float attractionIntensity, int id)
    {
        // Calculate attraction
        Vector3 toInputObject = attrationObject.position - transform.position;
        // Debug.Log("toInputObject, "+id+": " + toInputObject);
        float distance = toInputObject.magnitude;
        Vector3 attractionForce = toInputObject.normalized * ((attractionIntensity * attrationObject.localScale.x * 1 / distance * distance) - 0.01f);
        Debug.Log("attractionForce, " + id + ": " + attractionForce);
        m_Rigidbody.AddForce(attractionForce);
    }
}
