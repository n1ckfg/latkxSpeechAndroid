using UnityEngine;
using System.Collections.Generic;

public class SpeechSayTrigger : MonoBehaviour {

	public string[] triggerWords;


	private SpeechRecognizerManager _speechManager = null;
	private bool _isListening = false;
	private string _message = "";
	private float markTime = 0f;
	private bool hasBeenTriggered = false;
	private float markTriggerTime = 0f;
	private bool armMainTrigger = false;
	private string rememberTriggerWord = "";

	private void Start() {
		if (Application.platform != RuntimePlatform.Android) {
			Debug.Log ("Speech recognition is only available on Android platform.");
			return;
		}

		if (!SpeechRecognizerManager.IsAvailable ()) {
			Debug.Log ("Speech recognition is not available on this device.");
			return;
		}

		// We pass the game object's name that will receive the callback messages.
		_speechManager = new SpeechRecognizerManager(gameObject.name);
	}

    private void OnDestroy() {
		if (_speechManager != null)	_speechManager.Release ();
	}


    private void OnSpeechEvent(string e) {
		switch (int.Parse (e)) {
		case SpeechRecognizerManager.EVENT_SPEECH_READY:
			Debug.Log ("Ready for speech");
			break;
		case SpeechRecognizerManager.EVENT_SPEECH_BEGINNING:
			Debug.Log ("User started speaking");
			break;
		case SpeechRecognizerManager.EVENT_SPEECH_END:
			Debug.Log ("User stopped speaking");
			break;
		}
	}

    private void OnSpeechResults (string results) {
		_isListening = false;

		// Need to parse
		string[] texts = results.Split (new string[] { SpeechRecognizerManager.RESULT_SEPARATOR }, System.StringSplitOptions.None);
		result = texts[0];

		// ~ ~ ~ ~ ~
		textMeshRen.enabled = true;
		markTime = Time.realtimeSinceStartup;
		textMesh.text = result;
		// ~ ~ ~ ~ ~

		Debug.Log ("Speech results:\n   " + string.Join ("\n   ", texts));
	}

    private void OnSpeechError (string error) {
		switch (int.Parse (error)) {
		case SpeechRecognizerManager.ERROR_AUDIO:
			Debug.Log ("Error during recording the audio.");
			break;
		case SpeechRecognizerManager.ERROR_CLIENT:
			Debug.Log ("Error on the client side.");
			break;
		case SpeechRecognizerManager.ERROR_INSUFFICIENT_PERMISSIONS:
			Debug.Log ("Insufficient permissions. Do the RECORD_AUDIO and INTERNET permissions have been added to the manifest?");
			break;
		case SpeechRecognizerManager.ERROR_NETWORK:
			Debug.Log ("A network error occured. Make sure the device has internet access.");
			break;
		case SpeechRecognizerManager.ERROR_NETWORK_TIMEOUT:
			Debug.Log ("A network timeout occured. Make sure the device has internet access.");
			break;
		case SpeechRecognizerManager.ERROR_NO_MATCH:
			Debug.Log ("No recognition result matched.");
			break;
		case SpeechRecognizerManager.ERROR_NOT_INITIALIZED:
			Debug.Log ("Speech recognizer is not initialized.");
			break;
		case SpeechRecognizerManager.ERROR_RECOGNIZER_BUSY:
			Debug.Log ("Speech recognizer service is busy.");
			break;
		case SpeechRecognizerManager.ERROR_SERVER:
			Debug.Log ("Server sends error status.");
			break;
		case SpeechRecognizerManager.ERROR_SPEECH_TIMEOUT:
			Debug.Log ("No speech input.");
			break;
		default:
			break;
		}

		_isListening = false;
	}

    public void startRecognizer() {
        _speechManager.StartListening(1, "en-US"); // Use english and return maximum three results.
        //_speechManager.StartListening(3, "en-US"); // Use english and return maximum three results.
        //_speechManager.StartListening (); // No parameters will use the device default language and returns maximum 5. results
    }

    private void Update() {

	}

}
