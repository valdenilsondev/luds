using UnityEngine;
using System.Collections;

using Sandbox.GameUtils;
using Sandbox.PlayerControl;
using Share.Database;

namespace Sandbox.Database {
	public class LoadGamePreferences : MonoBehaviour {
		
		//classe que ira carregar as configuracoes previamente definidas do jogador (calibragem[lado do tesouro] e dificuldade[lvl inicial do jogo])
		
		// Use this for initialization
		void Start () {
		//	MySQL.instance.SetGamePreferences(Player.GetIdPlayer(), GameManager.GetIdGame());
		}
	}
}