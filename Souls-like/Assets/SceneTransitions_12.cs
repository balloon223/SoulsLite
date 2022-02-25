using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class SceneTransitions_12 : MonoBehaviour
    {
        public Animator transitionAnim;
        float currentTime = 0f;
        public float startingTime = 10f;


        // Start is called before the first frame update
        void Start()
        {
            currentTime = startingTime;
        }

        // Update is called once per frame
        void Update()
        {
            currentTime -= 1 * Time.deltaTime;

            if (currentTime <= 0)
            {
                StartCoroutine(LoadScene());
            }
        }

        IEnumerator LoadScene()
        {
            transitionAnim.SetTrigger("end");
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}