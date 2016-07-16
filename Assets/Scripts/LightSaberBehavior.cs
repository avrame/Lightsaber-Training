using UnityEngine;
using VRTK;
using System.Timers;

public class LightSaberBehavior : VRTK_InteractableObject {

    private Blade blade;
    private VRTK_ControllerActions controllerActions;
    //private VRTK_ControllerEvents controllerEvents;
    private AudioSource lightSaberOnAudio;
    private AudioSource lightSaberOffAudio;
    private AudioSource lightSaberBuzzAudio;
    private Timer retractBladeTimer;
    private bool ejected = false;
    private float moveTime = 0;
    private CapsuleCollider lsCollider;

    // Use this for initialization
    protected override void Start() {
        base.Start();

        blade = transform.FindChild("Blade").GetComponent<Blade>();
        lsCollider = this.GetComponent<CapsuleCollider>();

        lightSaberOnAudio = transform.FindChild("LightSaberOnAudio").GetComponent<AudioSource>();
        lightSaberOffAudio = transform.FindChild("LightSaberOffAudio").GetComponent<AudioSource>();
        lightSaberBuzzAudio = transform.FindChild("LightSaberBuzzAudio").GetComponent<AudioSource>();

        // This timer exists to better sync the retracting bade animation with the sound effect
        retractBladeTimer = new Timer();
        retractBladeTimer.Elapsed += new ElapsedEventHandler(DelayedRetractBlade);
        retractBladeTimer.AutoReset = false;
        retractBladeTimer.Interval = 750;
    }

    protected override void FixedUpdate() {
        if (moveTime > 0) {
            moveTime -= Time.deltaTime;
        } else {
            rb.velocity = Vector3.zero;
        }
    }

    public override void Grabbed(GameObject grabbingObject) {
        base.Grabbed(grabbingObject);

        rb.useGravity = true;
        isUsable = true;

        controllerActions = grabbingObject.GetComponent<VRTK_ControllerActions>();
        //controllerEvents = grabbingObject.GetComponent<VRTK_ControllerEvents>();

        if (controllerActions) controllerActions.TriggerHapticPulse(1000, .25f, 0.05f);
    }

    public override void StartUsing(GameObject usingObject) {
        base.StartUsing(usingObject);

        blade.extendBlade();
        lsCollider.isTrigger = false;

        if (controllerActions) controllerActions.TriggerHapticPulse(4000, .5f, 0.01f);

        lightSaberOnAudio.Play();
        lightSaberBuzzAudio.PlayDelayed(1.5f);
    }

    public override void StopUsing(GameObject usingObject) {
        base.StopUsing(usingObject);

        // Start the timer for retracting the blade
        retractBladeTimer.Start();
        if (controllerActions) controllerActions.TriggerHapticPulse(4000, .5f, 0.01f);
        lightSaberBuzzAudio.Stop();
        lightSaberOffAudio.Play();
    }

    public void EjectFromR2D2() {
        if (!ejected) {
            ejected = true;
            rb.isKinematic = false;
            rb.AddForce(new Vector3(0, .25f, 0), ForceMode.Impulse);
            moveTime = .25f;
        }
    }

    private void DelayedRetractBlade(object source, ElapsedEventArgs e) {
        blade.retractBlade();
    }
}
