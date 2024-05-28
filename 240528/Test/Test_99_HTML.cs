using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class Test_99_HTML : TestBase
{
    readonly string url = "https://atentsexample.azurewebsites.net/monster";

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(url); // 웹페이지 가져오는 요청 만들기
        yield return www.SendWebRequest(); // 위에서 만든 요청을 전송하고 결과 기다리기 (전송 결과가 돌아올 때까지 대기)

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}
