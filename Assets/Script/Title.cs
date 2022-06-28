using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    bool Setumei = false;
    bool Rankingbool = false;

    public GameObject SetumeiPanel;
    public GameObject RankPanel;

    AudioSource audioSource;
    public AudioClip SE_Button;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Setumei) SetumeiPanel.SetActive(true);
        else SetumeiPanel.SetActive(false);

        if (Rankingbool) RankPanel.SetActive(true);
        else RankPanel.SetActive(false);
    }

    public void StartButton()
    {
        audioSource.PlayOneShot(SE_Button);
        SceneManager.LoadSceneAsync("Game");
    }

    public void TutorialButton()
    {
        audioSource.PlayOneShot(SE_Button);
        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void ExitButton()
    {
        audioSource.PlayOneShot(SE_Button);
        Application.Quit();
    }

    public void SetumeiButton()
    {
        if (Setumei) Setumei = false;
        else Setumei = true;

        Rankingbool = false;
    }
    public void RankingboolButton()
    {
        if (Rankingbool) Rankingbool = false;
        else Rankingbool = true;

        Setumei = false;
    }

}
