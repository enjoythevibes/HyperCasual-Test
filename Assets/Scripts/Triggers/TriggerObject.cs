using System.Collections;
using System.Collections.Generic;
using enjoythevibes.Levels;
using enjoythevibes.Player;
using UnityEngine;

namespace enjoythevibes.Triggers
{
    public class TriggerObject : MonoBehaviour
    {
        // private enum TranslateType
        // {
        //     SubLevel,
        //     Level
        // }
        // [SerializeField]
        // private TranslateType translateType;
        public Level nextLevel;

        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<PlayerController>().TranslateToNextLevel(nextLevel, transform.position);
            // switch (translateType)
            // {
            //     case TranslateType.SubLevel:
            //         break;
            //     case TranslateType.Level:
            //         other.GetComponent<PlayerController>().TranslateToNextLevel(nextLevel, transform.position);
            //         break;
            // }
        }
    }
}