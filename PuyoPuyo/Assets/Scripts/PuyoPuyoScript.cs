using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoPuyoScript : MonoBehaviour
{
    [SerializeField]
    private int horizontalStartPosition;
    [SerializeField]
    private PuyoScript staticPuyo;
    [SerializeField]
    private PuyoScript movablePuyo;
    [SerializeField]
    private Transform leaver;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float horizontalSpeed;
    [SerializeField]
    private float baseSpeed;

    private float verticalSpeed = 0;
    private bool isRotating = false;
    private bool canSettle = false;
    private bool isMovingHorizontal = false;
    private bool isLocked = false;
    private float leaverFutureRotation = 0;
    private float futureHorizontalPosition = 0;
    private float futureVerticalPosition = 0;
    private float leaverAngleCheck = 0;

    void Start()
    {
        verticalSpeed = baseSpeed;
        staticPuyo.InitializePuyo(horizontalStartPosition, -1);
        movablePuyo.InitializePuyo(horizontalStartPosition, -2);
        SetNewAdvanceTarget();
    }

    void Update()
    {
        if (GridManager.instance.GetGameStarted())
        {
            Advance();
            PlayerInput();
            RotatePuyo();
            MovePuyo();
            Settle();
        }
    }

    private void Settle()
    {
        
        if (!isRotating && canSettle)
        {
            canSettle = false;
            staticPuyo.willFall = true;
            movablePuyo.willFall = true;
            if (!staticPuyo.CanMoveVertical())
            {
                staticPuyo.Settle();
                movablePuyo.Settle();
            }
            else if (!movablePuyo.CanMoveVertical())
            {
                movablePuyo.Settle();
                staticPuyo.Settle();
            }
            
            staticPuyo.transform.parent = GridManager.instance.transform;
            movablePuyo.transform.parent = GridManager.instance.transform;
            Destroy(gameObject);
        }
    }

    private void Advance()
    {
        if (!isLocked)
        {

            if (transform.position.y > futureVerticalPosition)
            {
                transform.Translate(Vector3.down * Time.deltaTime * verticalSpeed);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, futureVerticalPosition, transform.position.z);
                if (staticPuyo.CanMoveVertical() && movablePuyo.CanMoveVertical())
                {
                    SetNewAdvanceTarget();
                }
                else
                {
                    isLocked = true;
                    StartCoroutine(WaitToSettle());
                }
            }
        }
        else
        {
            if (staticPuyo.CanMoveVertical() && movablePuyo.CanMoveVertical())
            {
                StopAllCoroutines();
                isLocked = false;
            }
        }
    }

    IEnumerator WaitToSettle()
    {
        yield return new WaitForSeconds(1);
        canSettle = true;
    }

    private void SetNewAdvanceTarget()
    {
        futureVerticalPosition = transform.position.y - .5f;
        staticPuyo.UpdatePosY(1);
        movablePuyo.UpdatePosY(1);
    }

    private void MovablePuyoPosXLeftRotation()
    {
        if (transform.position.x - movablePuyo.transform.position.x < -0.25f)
        {
            if (movablePuyo.CanRotateLeft(-1, -1))
            {
                RotateLeaver(90);
                movablePuyo.UpdatePosX(-1);
                movablePuyo.UpdatePosY(-1);
            }
        }
        else if (transform.position.x - movablePuyo.transform.position.x > 0.25f)
        {
            if (movablePuyo.CanRotateLeft(1, 1))
            {
                RotateLeaver(90);
                movablePuyo.UpdatePosX(1);
                movablePuyo.UpdatePosY(1);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
                RotateLeaver(90);
                movablePuyo.UpdatePosX(1);
                staticPuyo.UpdatePosY(-1);
                futureVerticalPosition = transform.position.y;
            }
        }
        else
        {
            if (transform.position.y - movablePuyo.transform.position.y > -0.25f)
            {
                if (movablePuyo.CanRotateLeft(1, -1))
                {
                    RotateLeaver(90);
                    movablePuyo.UpdatePosX(1);
                    movablePuyo.UpdatePosY(-1);
                }
                else if (staticPuyo.CanMoveHorizontal(-1))
                {
                    Move(-0.5f);
                    staticPuyo.UpdatePosX(-1);
                    movablePuyo.UpdatePosY(-1);
                    RotateLeaver(90);
                }
            }
            else if (transform.position.y - movablePuyo.transform.position.y < 0.25f)
            {
                if (movablePuyo.CanRotateLeft(-1, 1))
                {
                    RotateLeaver(90);
                    movablePuyo.UpdatePosX(-1);
                    movablePuyo.UpdatePosY(1);
                }
                else if (staticPuyo.CanMoveHorizontal(1))
                {
                    Move(.5f);
                    staticPuyo.UpdatePosX(1);
                    movablePuyo.UpdatePosY(1);
                    RotateLeaver(90);

                }
            }
        }
    }

    private void MovablePuyoPosXRightRotation()
    {
        if (transform.position.x - movablePuyo.transform.position.x < -0.25f)
        {
            if (movablePuyo.CanRotateRight(-1, 1))
            {
                RotateLeaver(-90);
                movablePuyo.UpdatePosX(-1);
                movablePuyo.UpdatePosY(1);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                RotateLeaver(-90);
                movablePuyo.UpdatePosX(-1);
                staticPuyo.UpdatePosY(-1);
                futureVerticalPosition = transform.position.y;
            }
        }
        else if (transform.position.x - movablePuyo.transform.position.x > 0.25f)
        {
            if (movablePuyo.CanRotateRight(1, -1))
            {
                RotateLeaver(-90);
                movablePuyo.UpdatePosX(1);
                movablePuyo.UpdatePosY(-1);
            }
        }
        else
        {
            if (transform.position.y - movablePuyo.transform.position.y > -0.25f)
            {
                if (movablePuyo.CanRotateRight(-1, -1))
                {
                    RotateLeaver(-90);
                    movablePuyo.UpdatePosX(-1);
                    movablePuyo.UpdatePosY(-1);
                }
                else if (staticPuyo.CanMoveHorizontal(1))
                {
                    Move(0.5f);
                    staticPuyo.UpdatePosX(1);
                    movablePuyo.UpdatePosY(-1);
                    RotateLeaver(-90);

                }
            }
            else if (transform.position.y - movablePuyo.transform.position.y < 0.25f)
            {
                if (movablePuyo.CanRotateRight(1, 1))
                {
                    RotateLeaver(-90);
                    movablePuyo.UpdatePosX(1);
                    movablePuyo.UpdatePosY(1);
                }
                else if (staticPuyo.CanMoveHorizontal(-1))
                {
                    Move(-0.5f);
                    staticPuyo.UpdatePosX(-1);
                    movablePuyo.UpdatePosY(1);
                    RotateLeaver(-90);
                }
            }
        }
    }

    private void PlayerInput()
    {
        if (leaver != null)
        {
            if (!isRotating)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    SFXManager.instance.PlayClip(4);
                    MovablePuyoPosXLeftRotation();
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    SFXManager.instance.PlayClip(4);
                    MovablePuyoPosXRightRotation();
                }
            }
        }

        if (!isMovingHorizontal)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (staticPuyo.CanMoveHorizontal(-1) && movablePuyo.CanMoveHorizontal(-1))
                {
                    SFXManager.instance.PlayClip(3);
                    Move(-0.5f);
                    movablePuyo.UpdatePosX(-1);
                    staticPuyo.UpdatePosX(-1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (staticPuyo.CanMoveHorizontal(1) && movablePuyo.CanMoveHorizontal(1))
                {
                    SFXManager.instance.PlayClip(3);
                    Move(0.5f);
                    movablePuyo.UpdatePosX(1);
                    staticPuyo.UpdatePosX(1);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            verticalSpeed = baseSpeed * 5;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            verticalSpeed = baseSpeed;
        }
    }

    private void Move(float steps)
    {
        isMovingHorizontal = true;
        futureHorizontalPosition = transform.position.x + steps;
    }

    private void MovePuyo()
    {
        if (isMovingHorizontal)
        {
            if (transform.position.x > futureHorizontalPosition - .1f && transform.position.x < futureHorizontalPosition + .1f)
            {
                transform.position = new Vector3(futureHorizontalPosition, transform.position.y, transform.position.z);
                isMovingHorizontal = false;
            }
            else
            {
                Vector3 newPosition = new Vector3(futureHorizontalPosition, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * horizontalSpeed);
            }
        }
    }

    private void RotateLeaver(float rotationDegrees)
    {
        isRotating = true;
        leaverFutureRotation = leaver.rotation.eulerAngles.z + rotationDegrees;
        leaverAngleCheck = leaverFutureRotation;
        if (leaverAngleCheck < 0)
        {
            leaverAngleCheck += 360;
        }
    }

    private void RotatePuyo()
    {
        if (isRotating)
        {
            if (leaver.localRotation.eulerAngles.z > leaverAngleCheck - 1 && leaver.localRotation.eulerAngles.z < leaverAngleCheck + 1)
            {
                leaver.localRotation = Quaternion.Euler(new Vector3(leaver.localRotation.eulerAngles.x, leaver.localRotation.eulerAngles.y, leaverFutureRotation));
                isRotating = false;
            }
            else
            {
                Quaternion newAngle = Quaternion.Euler(leaver.localRotation.eulerAngles.x, leaver.localRotation.eulerAngles.y, leaverFutureRotation);
                leaver.localRotation = Quaternion.Lerp(leaver.localRotation, newAngle, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
