using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[System.Serializable]
	public struct Card{
		public Transform _cardPrefab;
		public Color _border;
		public Color _main;
		public Color _text;
		public Color _bgPrim;
		public Color _bgSec;
	}
	public Card[] _cards;
	public CanvasGroup _albumCanvas;
	Transform _mainCam;
	Transform _pivot;
	bool _cardViewer;
	public float _spinSpeed;
	Transform _card;
	Vector3 _camStart;
	public Material _cardBorder;
	public Material _cardMain;
	public Material _bg;
	public ReflectionProbe _probe;
    // Start is called before the first frame update
    void Start()
    {
		_albumCanvas.alpha=1f;
		_mainCam=Camera.main.transform;
		_camStart=_mainCam.position;
		_cardViewer=false;
		_pivotTarget = new GameObject("pivot target").transform;
    }

	Vector3 _prevPos;
	public float _rotateLerp;
	Transform _pivotTarget;
    // Update is called once per frame
    void Update()
    {
		if(_cardViewer){
			if(Input.GetMouseButton(0)){
				float xDelta=Input.mousePosition.x-_prevPos.x;
				_pivotTarget.Rotate(Vector3.up*xDelta);
			}
			_pivot.rotation=Quaternion.Slerp(_pivot.rotation,_pivotTarget.rotation,Time.deltaTime*_rotateLerp);

			_prevPos=Input.mousePosition;
		}
    }

	public void LoadTest(int index){
		_albumCanvas.alpha=0f;
		_albumCanvas.interactable=false;
		_albumCanvas.blocksRaycasts=false;
		_card = Instantiate(_cards[index]._cardPrefab);
		_pivot = _card.GetChild(0);
		_mainCam.SetParent(_pivot);
		_pivotTarget.rotation=_pivot.rotation;
		_pivot.localPosition=Vector3.up*0.01f;
		_cardViewer=true;
		_cardBorder.SetColor("_Color",_cards[index]._border);
		_cardMain.SetColor("_Color",_cards[index]._main);
		_bg.SetColor("_Color",_cards[index]._bgPrim);
		_bg.SetColor("_Secondary",_cards[index]._bgSec);
		//bake reflection probe
		//_probe.RenderProbe();
	}

	IEnumerator ProbeRenderR(){
		yield return null;
		_probe.RenderProbe();
	}

	public void Unload(){
		_mainCam.SetParent(null);
		_mainCam.position=_camStart;
		_mainCam.rotation = Quaternion.identity;
		Destroy(_card.gameObject);
		_albumCanvas.alpha=1f;
		_albumCanvas.interactable=true;
		_albumCanvas.blocksRaycasts=true;
		_cardViewer=false;
	}
}
