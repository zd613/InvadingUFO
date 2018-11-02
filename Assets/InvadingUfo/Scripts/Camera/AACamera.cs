using UnityEngine;

public class AACamera : BaseCamera
{
    [Header("targer")]
    public Transform antiAircraftGun;
    [Header("crosshair")]
    public Transform antiAircraftCrosshair;

    Vector3 offset;
    public float movingSpeed = 2.7f;//
    public float rotationSpeed = 2.7f;//2.7f

    [Header("zoon")]
    public float zoomSpeed = 4;
    public float minFieldOfView = 8;
    public float maxFieldOfView = 60;

    Status status = Status.ZoomOut;

    Vector3 defaultOffset;

    private void Awake()
    {
        offset = transform.position - antiAircraftGun.transform.position;
        defaultOffset = offset;
    }

    protected override void LateUpdate()
    {
        if (antiAircraftGun == null)
            return;
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - zoomSpeed, minFieldOfView, maxFieldOfView);

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView + zoomSpeed, minFieldOfView, maxFieldOfView);
        }



        base.LateUpdate();


    }

    protected override void Move()
    {
        var desiredPos = antiAircraftGun.position + CalculateOffset();
        transform.position = Vector3.Slerp(transform.position, desiredPos, Time.deltaTime * movingSpeed);
    }

    protected override void Rotate()
    {
        var lookRot = Quaternion.LookRotation(antiAircraftCrosshair.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
    }

    Vector3 CalculateOffset()
    {
        return antiAircraftGun.forward * offset.z + antiAircraftGun.right * offset.x
            + antiAircraftGun.up * offset.y;
    }

    public void ZoomIn()
    {
        Camera.main.fieldOfView = minFieldOfView;
        status = Status.ZoomIn;
    }

    public void ZoomOut()
    {
        Camera.main.fieldOfView = maxFieldOfView;
        status = Status.ZoomOut;
    }

    enum Status
    {
        ZoomIn,
        ZoomOut,
    }
}
