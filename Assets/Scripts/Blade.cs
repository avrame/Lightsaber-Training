using UnityEngine;
using System.Collections;

public class Blade : MonoBehaviour {

    public float extendSpeed;

    private GameObject blade;
    private Vector2 beamLimits = new Vector2(0f, 1f);
    private float currentBeamSize;
    private float bladeExtendSpeed = 0f;

    public void extendBlade () {
        bladeExtendSpeed = extendSpeed;
    }

    public void retractBlade () {
        bladeExtendSpeed = -extendSpeed;
    }

    // Use this for initialization
    void Start () {
        blade = this.gameObject;
        currentBeamSize = beamLimits.x;
        blade.transform.localScale = new Vector3(1f, currentBeamSize, 1f);
        //setBladeSize();
    }
	
	// Update is called once per frame
	void Update () {
        setBladeSize();
    }

    private void setBladeSize () {
        currentBeamSize = Mathf.Clamp(blade.transform.localScale.y + (bladeExtendSpeed * Time.deltaTime), beamLimits.x, beamLimits.y);
        blade.transform.localScale = new Vector3(1f, currentBeamSize, 1f);
    }
}
