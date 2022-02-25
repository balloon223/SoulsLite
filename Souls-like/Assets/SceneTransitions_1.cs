using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class SceneTransitions_1 : MonoBehaviour
    {
        public Animator transitionAnim;
        PlayerManager playerManager;
        // Start is called before the first frame update
        void Start()
        {
            playerManager = FindObjectOfType<PlayerManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerManager.canMoveToRoom2)
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