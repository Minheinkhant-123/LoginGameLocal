using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Script
{
    public class MessageBox : MonoBehaviour
    {
        public TMP_Text outputText;

        public void Start()
        {
            outputText.text = Messagebox.message;
        }
        public void tryagain()
        {
            SceneManager.LoadScene(int.Parse(Messagebox.scenenum));
        }
    }
}
