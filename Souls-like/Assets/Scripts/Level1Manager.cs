using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class Level1Manager : MonoBehaviour
    {
        public int numberOfEnemies = 10;

        // Update is called once per frame
        public void Update()
        {
            if (numberOfEnemies <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}