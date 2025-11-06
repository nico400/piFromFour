using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.Timeline.TimelineAsset;

public class PlayersConnectedStart : NetworkBehaviour
{

    [SerializeField] private PlayerConnected playerConnected;
    [SerializeField] private Animator animTransitionStart;

    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private float durationFade;

    [SyncVar(hook = nameof(OnPlayersConnectedChanged))]
    private int playersConnected;

    void Update()
    {
        if (!isServer) return;
        playersConnected = playerConnected.CheckPlayersConnected();
    }
    private void OnPlayersConnectedChanged(int oldValue, int newValue)
    {
        if (newValue >= 2)
        {
            StartCoroutine(WaitPlayerClientSpawn());
        }
    }
    IEnumerator WaitPlayerClientSpawn()
    {
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(1.75f);
        animTransitionStart.SetBool("IsClose", false);
    }
    private IEnumerator FadeIn()
    {
        float tempo = 0f;
        while (tempo < durationFade)
        {
            tempo += Time.deltaTime;
            infoText.alpha = Mathf.Clamp01(tempo / durationFade);
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float tempo = 0f;
        while (tempo < durationFade)
        {
            tempo += Time.deltaTime;
            infoText.alpha = Mathf.Clamp01(1 - (tempo / durationFade));
            yield return null;
        }
    }
}

