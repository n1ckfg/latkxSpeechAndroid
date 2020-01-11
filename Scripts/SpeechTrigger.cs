using UnityEngine;
using System.Collections.Generic;

public class SpeechTrigger : MonoBehaviour {

    public LightningArtist latk;
    public string result;
    public bool isListening = false;

    [HideInInspector] public bool armed = false;

    private SpeechRecognizerManager _speechManager = null;
    private string countrySetting = "en-US";
    private int numResults = 1;

    public void listen() {
        if (!isListening) {
            isListening = true;
            _speechManager.StartListening(numResults, countrySetting);
            //_speechManager.StartListening(3, "en-US"); // Use english and return maximum three results.
            //_speechManager.StartListening (); // No parameters will use the device default language and returns maximum 5. results
        }
    }

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


    private void Update() {
        if (armed) {
            switch (result) {
                case "save":
                    if (!latk.isWritingFile) latk.armWriteFile = true;
                    break;
            }

            armed = false;
        }
    }

    private void OnDestroy() {
		if (_speechManager != null)	_speechManager.Release();
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
		isListening = false;

        try {
            string[] texts = results.Split(new string[] { SpeechRecognizerManager.RESULT_SEPARATOR }, System.StringSplitOptions.None);
            result = texts[0].ToLower();
            armed = true;
            Debug.Log("Speech results:\n   " + string.Join("\n   ", texts));
        } catch (UnityException e) { }
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

		isListening = false;
	}

}
