using System;
using System.Collections;
using UnityEngine;
using VRTK;

public class TrainingRemoteBehavior : VRTK_InteractableObject {

    private VRTK_ControllerActions controllerActions;
    private Vector3 moveForce = Vector3.zero;
    private float moveTime = 0;

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    protected override void FixedUpdate() {
        if (moveTime > 0) {
            rb.AddForce(moveForce, ForceMode.Impulse);
            moveTime -= Time.deltaTime;
        } else {
            rb.velocity = Vector3.zero;
            if (base.IsUsing()) {
                //move();
            }
        }
    }

    public override void Grabbed(GameObject grabbingObject) {
        base.Grabbed(grabbingObject);

        controllerActions = grabbingObject.GetComponent<VRTK_ControllerActions>();
    }

    public override void StartUsing(GameObject usingObject) {
        base.StartUsing(usingObject);

        if (controllerActions) controllerActions.TriggerHapticPulse(4000, .5f, 0.01f);
        base.OnJointBreak(0);
        rb.useGravity = false;
        StartTraining();
    }

    public override void StopUsing(GameObject usingObject) {
        base.StopUsing(usingObject);
    }

    private void StartTraining() {
        moveForce = new Vector3(0, .1f, 0);
        moveTime = .5f;
    }

    private void move() {
        moveForce = new Vector3(.1f, 0, 0);
        moveTime = .5f;
    }
}
