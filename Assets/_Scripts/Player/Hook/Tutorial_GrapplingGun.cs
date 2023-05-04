using UnityEngine;

public class Tutorial_GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Tutorial_GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    //[SerializeField] private bool grappleToAll = false;
    [SerializeField] private LayerMask grappableLayer;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    //[SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 20;

/*    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }*/

    [Header("Launching:")]
    //[SerializeField] private bool launchToPoint = true;
    //[SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

/*    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;*/

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    [Header("Параметры крюка")]
    [SerializeField] private float grappleCooldown;     // кд 
    [SerializeField] private float timeToUnGrapple;     // время до отпускания крюка
    float lastHook;                                     // время последнего захвата
    [HideInInspector] public bool grappleHit;           // попали в стену
    [HideInInspector] public bool grappleHitEnemy;      // попали во врага
    Fighter enemy;
    [SerializeField] private float launchSpeedEnemy = 1;    // сила притягивания врагов
    [SerializeField] private float dampingEnemy = 1;        // дампинг джоинта


    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;

        //Debug.Log(grappableLayerNumber);
    }

    private void Update()
    {
        if (grappleHitEnemy)
        {
            grapplePoint = enemy.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.C) && Time.time - lastHook > grappleCooldown)
        {
            lastHook = Time.time;
            SetGrapplePoint();
            Invoke("UnGrapple", timeToUnGrapple);
        }
        else if (Input.GetKey(KeyCode.C))
        {
/*            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }*/

/*            if (launchToPoint && grappleRope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }*/
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            //grappleRope.enabled = false;
            //m_springJoint2D.enabled = false;

            //m_rigidbody.gravityScale = 1;
        }
        else
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos, true);
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetGrapplePoint()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;

        Debug.DrawRay(firePoint.position, distanceVector.normalized * 10, Color.yellow);

        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            //Debug.Log("Ray!");

            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized, 100, grappableLayer);
            if (_hit)
            {
                //Debug.Log("Hit");

                if (_hit.collider.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    //Debug.Log("Hit_Enemy");
                    grapplePoint = fighter.transform.position;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;
                    grappleHit = true;
                    grappleHitEnemy = true;
                    enemy = fighter;
                }
                else
                {
                    if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistnace)
                    {                        
                        grapplePoint = _hit.point;
                        grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                        grappleRope.enabled = true;
                        grappleHit = true;              // попали
                    }
                    else
                    {                        
                        grapplePoint = new Vector2(firePoint.position.x, firePoint.position.y) + distanceVector.normalized * maxDistnace;
                        grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                        grappleRope.enabled = true;
                        grappleHit = false;              // не попали
                    }
                }
            }
        }
    }

    public void Grapple()
    {
        Debug.Log("Grap");
        if (grappleHit)
        {
            // для врага
            if (grappleHitEnemy)
            {                
                //Debug.Log("GrapEnemy");
                enemy.TryGetComponent<SpringJoint2D>(out SpringJoint2D enemySJ);
                if (enemySJ)
                {
                    //Debug.Log("SJ");
                    enemySJ.autoConfigureDistance = false;                      // дистанцию задаём сами
                    enemySJ.connectedAnchor = firePoint.position;               // якорь - точку куда мы попали
                    enemySJ.frequency = launchSpeedEnemy;                       // сила запуска
                    enemySJ.dampingRatio = dampingEnemy;                        // дампинг
                    enemySJ.enabled = true;                                     // включаем джоинт
                }

                //Vector2 distanceVector = -(enemy.transform.position - firePoint.position).normalized;
                //enemy.TakeDamage(10, distanceVector, 30);                
            }
            // для стены
            else
            {
                m_springJoint2D.autoConfigureDistance = false;                      // дистанцию задаём сами
                m_springJoint2D.connectedAnchor = grapplePoint;                     // якорь - точку куда мы попали

                Vector2 distanceVector = firePoint.position - gunHolder.position;   // вектор для дистанции
                                                                                    //Debug.Log(distanceVector.magnitude);
                m_springJoint2D.distance = distanceVector.magnitude;                // дистанция? (0.45 получается)
                m_springJoint2D.frequency = launchSpeed;                            // сила запуска
                m_springJoint2D.enabled = true;                                     // включаем джоинт
            }
        }
        // если не попали никуда
        else
        {
            m_springJoint2D.autoConfigureDistance = true;
            m_springJoint2D.frequency = 0;
        }


        /*        m_springJoint2D.autoConfigureDistance = false;
                if (!launchToPoint && !autoConfigureDistance)
                {
                    m_springJoint2D.distance = targetDistance;
                    m_springJoint2D.frequency = targetFrequncy;
                }

                if (!launchToPoint)
                {
                    if (autoConfigureDistance)
                    {
                        m_springJoint2D.autoConfigureDistance = true;
                        m_springJoint2D.frequency = 0;
                    }

                    m_springJoint2D.connectedAnchor = grapplePoint;
                    m_springJoint2D.enabled = true;
                }
                else
                {
                    switch (launchType)
                    {
                        case LaunchType.Physics_Launch:
                            m_springJoint2D.connectedAnchor = grapplePoint;             // якорь - точку куда мы попали

                            Vector2 distanceVector = firePoint.position - gunHolder.position;   // тут вроде вместо gunHolder должен быть grapplePoint
                            //Debug.Log(distanceVector.magnitude);

                            m_springJoint2D.distance = distanceVector.magnitude;
                            m_springJoint2D.frequency = launchSpeed;
                            m_springJoint2D.enabled = true;
                            break;
                        case LaunchType.Transform_Launch:
                            //m_rigidbody.gravityScale = 0;
                            m_rigidbody.velocity = Vector2.zero;
                            break;
                    }
                }*/
    }

    void UnGrapple()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        if (grappleHitEnemy)
        {
            grappleHitEnemy = false;
            if (enemy.TryGetComponent<SpringJoint2D>(out SpringJoint2D enemySJ))
            {
                enemySJ.enabled = false;                                     // выключаем джоинт
                enemy = null;
            }
        }            
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }
}