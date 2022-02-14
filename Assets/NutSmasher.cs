using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NutSmasher : MonoBehaviour
{
	Quaternion _startRot;
	Quaternion _smashRot;
	bool _smashing;
	public float _smashDur;
	public AnimationCurve _smashCurve;
	public Transform _nut;
	MeshRenderer _nutMesh;
	ParticleSystem _nutParts;
	AudioSource _nutSound;
	AudioSource _woosh;
	public float _normFxTime;
	public Text _smashText;

	void Awake(){
		_startRot=transform.rotation;
		transform.Rotate(Vector3.forward*-90);
		_smashRot=transform.rotation;
		transform.rotation=_startRot;
		_nutMesh=_nut.GetComponent<MeshRenderer>();
		_nutParts=_nut.GetComponent<ParticleSystem>();
		_nutSound=_nut.GetComponent<AudioSource>();
		_woosh=GetComponent<AudioSource>();

		int smashCount=0;
		if(PlayerPrefs.HasKey("smashed")){
			smashCount=PlayerPrefs.GetInt("smashed");
		}
		_smashText.text="Virtual Nuts Smashed: "+smashCount;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Smash(){
		if(!_smashing)
			StartCoroutine(SmashR());
	}

	IEnumerator SmashR(){
		_smashing=true;
		float timer=0;
		bool fxPlay=false;
		_woosh.Play();
		while(timer<_smashDur){
			timer+=Time.deltaTime;
			if(!fxPlay&&timer/_smashDur>_normFxTime){
				_nutParts.Play();
				_nutMesh.enabled=false;
				_nutSound.Play();
				fxPlay=true;

				//update smash count
				_smashing=false;

				int smashCount=0;
				if(PlayerPrefs.HasKey("smashed")){
					smashCount=PlayerPrefs.GetInt("smashed");
				}
				smashCount++;
				_smashText.text="Virtual Nuts Smashed: "+smashCount;
				PlayerPrefs.SetInt("smashed",smashCount);
				PlayerPrefs.Save();
			}
			transform.rotation=Quaternion.SlerpUnclamped(_startRot,_smashRot,_smashCurve.Evaluate(timer/_smashDur));
			yield return null;
		}
		transform.rotation=_startRot;
		_nutMesh.enabled=true;

	}
}
