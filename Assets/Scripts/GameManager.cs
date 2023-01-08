using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public GameObject cameraShake;
	public GameObject uiController;

	public GameObject player;

	public AudioSource audioSource;
	public AudioClip sfxDeath;
	public AudioClip sfxWin;

	CameraShake camShakeScript;
	UiController uiControllerScript;
    private void Start()
    {
		camShakeScript = cameraShake.GetComponent<CameraShake>();
		uiControllerScript = uiController.GetComponent<UiController>();
	}

    private void Update()
	{
	}

	public void ShakeCamera(float intensity = 5f, float timing = 0.5f)
    {
		camShakeScript.startShake(intensity, timing);
	}

	public void NextLevel(string levelName)
	{
		StartCoroutine(NextLevelCoroutine(levelName));
	}
	IEnumerator NextLevelCoroutine(string levelName)
	{
		audioSource.PlayOneShot(sfxWin);
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(levelName);

	}

	public void FinishGame()
	{
		StartCoroutine(EndGameCoroutine());
	}

	IEnumerator EndGameCoroutine()
	{
		audioSource.PlayOneShot(sfxDeath);
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("GameOver");

	}
}
