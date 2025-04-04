using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class LoginUsername
{
    public static string userid;
}
public static class Messagebox
{
    public static string message;
    public static string scenenum;
}
public class Login : MonoBehaviour
{
    //public const string url = "http://localhost/api/login.php";
    public const string url = "http://103.91.190.179/api/login.php";
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;


    public class LoginRequest
    {
        public string username;
        public string password;
    }

    public class LoginResponse
    {
        public string status;
        public string message;
        public string id;
        
    }


    public void LoginUser()
    {
        
        StartCoroutine(PostRequest(url, usernameInput.text, passwordInput.text));
    }

    public void SignUpUser()
    {
        SceneManager.LoadScene(1);
    }
    public IEnumerator PostRequest(string url, string username, string password)
    {
        LoginRequest loginRequest = new LoginRequest { username = username, password = password };
        string jsonBody = JsonUtility.ToJson(loginRequest);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            // Handle response
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Response: " + responseText);

                // Deserialize JSON response
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(responseText);
                if(response != null && response.status == "success")
                {
                    LoginUsername.userid = response.id;
                    SceneManager.LoadScene(2);
                }
                else
                {
                    Messagebox.message = "Your username or password is wrong.Please try again";
                    Messagebox.scenenum = "0";
                    SceneManager.LoadScene(3);
                }
            }
            else
            {
                Messagebox.message = "Your username or password is wrong.Please try again";
                Messagebox.scenenum = "0";
                SceneManager.LoadScene(3);
            }
        }
    }
}