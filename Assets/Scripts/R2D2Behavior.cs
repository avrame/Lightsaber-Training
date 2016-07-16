using UnityEngine;
using VRTK;

public class R2D2Behavior : VRTK_InteractableObject {

    LightSaberBehavior lightSaberBehavior;
    private AudioSource R2D2ChirpAudio;

    // Use this for initialization
    protected override void Start() {
        base.Start();

        lightSaberBehavior = transform.FindChild("LightSaber").GetComponent<LightSaberBehavior>();

        R2D2ChirpAudio = transform.FindChild("R2D2ChirpAudio").GetComponent<AudioSource>();
    }

    public override void StartTouching(GameObject touchingObject) {
        base.StartTouching(touchingObject);

        R2D2ChirpAudio.Play();
        lightSaberBehavior.EjectFromR2D2();
    }
}
