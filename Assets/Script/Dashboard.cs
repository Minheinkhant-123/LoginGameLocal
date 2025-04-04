using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SignUp;

namespace Assets.Script
{
    public class Dashboard : MonoBehaviour
    {
        public const string url = "http://localhost/api/dashboard.php";
        // public const string url = "http://103.91.190.179/api/dashboard.php";
        public const string diamondupdateurl = "http://localhost/api/updatediamond.php";
        // public const string diamondupdateurl = "http://103.91.190.179/api/updatediamond.php";
        public Slider healthBarImage;
        public TMP_Text diamondOutput;
        public TMP_Text heartOutput;

        public class DashboardRequest
        {
            public string userid;
            public string diamond;
        }
        public class DashboardResponse
        {
            public string status;
            public string message;
            public string heart;
            public string diamond;
        }
        void Start()
        {                   
                StartCoroutine(PostRequest(url, LoginUsername.userid));
        }
        public void UserData()
        {
            StartCoroutine(PostRequest(url, LoginUsername.userid));
        }
        public void Updatediamond()
        {
            diamondOutput.text = (int.Parse(diamondOutput.text) + 100).ToString();
            StartCoroutine(UpdateDiamond(diamondupdateurl, LoginUsername.userid, diamondOutput.text));
        }
        public void Logout()
        {
            SceneManager.LoadScene(0);
        }
        public IEnumerator PostRequest(string url, string userid)
        {        
            DashboardRequest signUpRequest = new DashboardRequest { userid = userid };
            string jsonBody = JsonUtility.ToJson(signUpRequest);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string responseText = request.downloadHandler.text;
                    Debug.Log("Response: " + responseText);

                    DashboardResponse response = JsonUtility.FromJson<DashboardResponse>(responseText);
                    if (response != null && response.status == "success")
                    {
                        diamondOutput.text = response.diamond;
                        healthBarImage.value = float.Parse(response.heart);
                    }
                }
                else
                {
                    Debug.LogError("Error: " + request.error);
                }
            }
        }

        public IEnumerator UpdateDiamond(string updatediamondurl,string userid,string diamond)
        {
            DashboardRequest signUpRequest = new DashboardRequest { userid = userid,diamond = diamond };
            string jsonBody = JsonUtility.ToJson(signUpRequest);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);

            using (UnityWebRequest request = new UnityWebRequest(updatediamondurl, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
            }
        }
    }
}
