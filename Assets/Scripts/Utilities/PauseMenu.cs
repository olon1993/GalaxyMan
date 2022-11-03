using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool IsPaused = false;
        public GameObject PauseMenuUi;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        void Resume()
        {
            Cursor.visible = false;
            PauseMenuUi.SetActive(false);
            Time.timeScale = 1;
            IsPaused = false;
        }

        void Pause()
        {
            Cursor.visible = true;
            PauseMenuUi.SetActive(true);
            Time.timeScale = 0;
            IsPaused = true;
        }
    }
}
