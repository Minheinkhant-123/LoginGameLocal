using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SignUp;


public class SignUp : MonoBehaviour
{
    //public const string url = "http://localhost/api/register.php";
    public const string url = "http://103.91.190.179/api/register.php";
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmpasswordInput;
    public TMP_Text outputText;

    public class SignUpRequest
    {
        public string username;
        public string password;
    }

    public class SignUpResponse
    {
        public string status;
        public string message;
    }

    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(usernameInput.text)|| string.IsNullOrEmpty(passwordInput.text)
            || string.IsNullOrEmpty(confirmpasswordInput.text))
        {
            if (string.IsNullOrEmpty(usernameInput.text))
            { 
                usernameInput.placeholder.GetComponent<TextMeshProUGUI>().text = "Username empty.";
                usernameInput.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
                usernameInput.placeholder.GetComponent<TextMeshProUGUI>().fontSize = 9f;
            }

            if (string.IsNullOrEmpty(passwordInput.text))
            {
                passwordInput.placeholder.GetComponent<TextMeshProUGUI>().text = "Password empty.";
                passwordInput.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
                passwordInput.placeholder.GetComponent<TextMeshProUGUI>().fontSize = 9f;
            }

            if (string.IsNullOrEmpty(confirmpasswordInput.text))
            {
                confirmpasswordInput.placeholder.GetComponent<TextMeshProUGUI>().text = "Confirmpassword empty.";
                confirmpasswordInput.placeholder.GetComponent<TextMeshProUGUI>().color = Color.red;
                confirmpasswordInput.placeholder.GetComponent<TextMeshProUGUI>().fontSize = 9f;
            }
    }
        else
        {
            if (passwordInput.text != confirmpasswordInput.text)
            {
                outputText.text = "Your password doesn't match.";
            }
            else
            {
                StartCoroutine(PostRequest(url, usernameInput.text, passwordInput.text));
            }
        }
    }
    public IEnumerator PostRequest(string url, string username, string password)
    {
        // Create the request body
        SignUpRequest signUpRequest = new SignUpRequest { username = username, password = password };
        string jsonBody = JsonUtility.ToJson(signUpRequest);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);

        // Create a UnityWebRequest
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
                SignUpResponse response = JsonUtility.FromJson<SignUpResponse>(responseText);
                if (response != null && response.status == "success")
                {
                    SceneManager.LoadScene(0);
                }
                else 
                {
                    Messagebox.message = response.message;
                    Messagebox.scenenum = "1";
                    SceneManager.LoadScene(3);
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}