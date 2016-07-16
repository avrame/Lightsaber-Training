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

    // Use this for initialization
    protected override void Start() {
        base.Start();

        blade = transform.FindChild("Blade").GetComponent<Blade>();

        lightSaberOnAudio = transform.FindChild("LightSaberOnAudio").GetComponent<AudioSource>();
        lightSaberOffAudio = transform.FindChild("LightSaberOffAudio").GetComponent<AudioSource>();
        lightSaberBuzzAudio = transform.FindChild("LightSaberBuzzAudio").GetComponent<AudioSource>();

        // This timer exists to better sync the retracting bade animation with the sound effect
        retractBladeTimer = new Timer();
        retractBladeTimer.Elapsed += new ElapsedEventHandler(DelayedRetractBlade);
        retractBladeTimer.AutoReset = false;
        retractBladeTimer.Interval = 750;
    }

    public override void Grabbed(GameObject grabbingObject) {
        base.Grabbed(grabbingObject);

        controllerActions = grabbingObject.GetComponent<VRTK_ControllerActions>();
        //controllerEvents = grabbingObject.GetComponent<VRTK_ControllerEvents>();

        if (controllerActions) controllerActions.TriggerHapticPulse(1000, .25f, 0.05f);
    }

    public override void StartUsing(GameObject usingObject) {
        base.StartUsing(usingObject);

        blade.extendBlade();

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
    
    private void DelayedRetractBlade(object source, ElapsedEventArgs e) {
        blade.retractBlade();
    }
}
