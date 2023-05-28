using UnityEngine;
using System.Collections;

using BridgeGame.Player;
using Share.Managers;

namespace BridgeGame.GameUtilities {
	public class FinalPosition : MonoBehaviour {
		
		private Transform finalPosition;
		private GameObject player;
		private bool called = false;
        public static FinalPosition instance;
        private Animator pigAnim;
        public bool isFinalPos = false;

        void Awake()
        {
            instance = this;
        }

		void Start () {
			finalPosition = this.transform;
			player = GameObject.Find("pig_bridge");
            
		}
        //pausar o jogo, chamar o borg, se o borg tiver sido escolhido, chamar o game over em BridgeManager
        void OnTriggerEnter(Collider endpoint) 
        {
            if( endpoint.name == "joint15_head" || endpoint.name == "joint79" || endpoint.name == "joint79 1")
            {
				//condicao para que o iddle seja disparado apenas uma vez
				if(!isFinalPos){
					PlayerControl.instance.SetPigBridgeIddle();
				}
                isFinalPos = true;
                PlayerControl.instance.GetPlayer().GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
				PlayerControl.instance.ResetPlayerRotation();

            }
            
        }



	}
}
