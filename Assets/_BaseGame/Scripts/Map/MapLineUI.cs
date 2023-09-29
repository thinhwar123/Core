using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaseGame
{
    public class MapLineUI : MonoBehaviour
    {
        [SerializeField] GameObject linePass;
        // Start is called before the first frame update


        public void SetLineState(bool highlight)
        {
            if (linePass == null)
                linePass = gameObject.transform.GetChild(0).gameObject;
            linePass.SetActive(highlight);
            if(highlight)
            {
                gameObject.GetComponent<Image>().enabled = false;
            }
        }
    }
}
