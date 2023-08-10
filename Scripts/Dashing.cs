using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
  [Header("References")]
  public Transform orientation;
  public Transform playerCam;
  private Rigidbody rb;
  private playerController pc;

  [Header("Dashing")]
  public float dashForce;
  public float dashUpwardForce;
  public float dashDuration;

  [Header("Cooldown")]
  public float dashCd;
  private float dashCdTimer;

  [Header("Input")]
  public KeyCode dashKey = KeyCode.LeftShift;

  private void Start()
  {
    rb = GetComponent<Rigidbody>();
    pc = GetComponent<playerController>();
  }

  private void Update()
  {
    if (Input.GetKeyDown(dashKey))
        Dash();

    if(dashCdTimer > 0)
        dashCdTimer -= Time.deltaTime;
  }

  private void Dash()
  {
    if(dashCdTimer > 0)
        return;
    else dashCdTimer = dashCd;

    pc.dashing = true;

    Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

    delayedForceToApply = forceToApply;
    Invoke(nameof(DelayedDashForce), 0.025f);

    Invoke(nameof(ResetDash), dashDuration);

  }

  private Vector3 delayedForceToApply;
  private void DelayedDashForce()
  {
    rb.AddForce(delayedForceToApply, ForceMode.Impulse);

  }

  private void ResetDash()
  {
    pc.dashing = false;

  }
}
