using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.EventsSystem;
using Runner.Managers;
using Share.Managers;
using Assets.Scripts.Share;
using Share.Controllers;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Share.Enums;
using Goalkeeper.Managers;

public class Score : MonoBehaviour {
 
	public Text item1Text;
	public Text item2Text;
	public Text item3Text;
	public Text item4Text;

	public static Score instance;
    private ScoreItem item1;
	private ScoreItem item2;
	private ScoreItem item3;
	public ScoreItem item4;

	bool iniciouBaits;

	void Awake(){
		instance = this;
	}

    void Start()
    {
		if (GameManagerShare.instance.game == Game.Pig) {
			item1 = PigRunnerManager.instance.scoreItems.Where (x => x.type == ScoreItemsType.Apple).FirstOrDefault ();
			item2 = PigRunnerManager.instance.scoreItems.Where (x => x.type == ScoreItemsType.Distance).FirstOrDefault ();
            
		}
		if (GameManagerShare.instance.game == Game.Goal_Keeper) {
			item1 = GoalkeeperManager.Instance().scoreItems.Where (x => x.type == ScoreItemsType.Defense).FirstOrDefault ();
			item2 = GoalkeeperManager.Instance().scoreItems.Where (x => x.type == ScoreItemsType.Goal).FirstOrDefault ();
		}
		if (GameManagerShare.instance.game == Game.Bridge) {
			item1 = BridgeManager.instance.scoreItems.Where (x => x.type == ScoreItemsType.Bridge_Distance).FirstOrDefault ();
			item2 = BridgeManager.instance.scoreItems.Where (x => x.type == ScoreItemsType.Hits).FirstOrDefault ();
		}

		if (GameManagerShare.instance.game == Game.Sup) {
			item1 = SupManager.instance.scoreItems.Where (x => x.type == ScoreItemsType.Apple).FirstOrDefault ();
			item2 = SupManager.instance.scoreItems.Where (x => x.type == ScoreItemsType.TimerSup).FirstOrDefault ();
			//item3 = SupManager.instance.scoreItems.Where (x => x.type == ScoreItemsType.TimerSup_sec).FirstOrDefault ();
		}
		if(GameManagerShare.instance.game == Game.Throw)
		{
			item1 = ThrowManager.Instance.scoreItems.Where (x => x.type == ScoreItemsType.Apple).FirstOrDefault ();
			//item4 = ThrowManager.Instance.scoreItems.Where (x => x.type == ScoreItemsType.TotalThrow).FirstOrDefault ();
			//item3 = ThrowManager.Instance.scoreItems.Where (x => x.type == ScoreItemsType.Miss).FirstOrDefault ();
			item2 = ThrowManager.Instance.scoreItems.Where (x => x.type == ScoreItemsType.Hits).FirstOrDefault ();
		}
		if(GameManagerShare.instance.game == Game.Fishing)
		{
			item4 = FishingManager.instance.scoreItems.Where (x => x.type == ScoreItemsType.Bait).FirstOrDefault ();
			item1 = FishingManager.instance.scoreItems.Where (x => x.type == ScoreItemsType.Fish1).FirstOrDefault ();
			item2 = FishingManager.instance.scoreItems.Where (x => x.type == ScoreItemsType.Fish2).FirstOrDefault ();
			item3 = FishingManager.instance.scoreItems.Where (x => x.type == ScoreItemsType.Fish3).FirstOrDefault ();
		}
    }

    void Update()
    {
		if (GameManagerShare.instance.game == Game.Pig) {
			item1Text.text = item1.GetValue().ToString();
			item2Text.text = item2.GetValue().ToString();			
		}
		if (GameManagerShare.instance.game == Game.Goal_Keeper) {
			item1Text.text = item1.GetValue().ToString();
			item2Text.text = item2.GetValue().ToString();
		}
		if (GameManagerShare.instance.game == Game.Bridge) {
			item1Text.text = item1.GetValue().ToString();		
		}
		if (GameManagerShare.instance.game == Game.Sup) {
			item1Text.text = item1.GetValue().ToString();		
			item2Text.text = item2.GetValue().ToString();		
		}
		if(GameManagerShare.instance.game == Game.Throw)
		{
			item1Text.text = item1.GetValue().ToString();		
			item2Text.text = item2.GetValue().ToString();
			//item3Text.text = item3.GetValue().ToString();
			//item4Text.text = item4.GetValue().ToString();;
		}
		if(GameManagerShare.instance.game == Game.Fishing)
		{
			if(!iniciouBaits){
				iniciouBaits = true;
				item4.SetValue(BucketBaitsControl.instance.GetNumberOfBaits());
			}
			item1Text.text = item1.GetValue().ToString();
			item2Text.text = item2.GetValue().ToString();
			item3Text.text = item3.GetValue().ToString();
			item4Text.text = item4.GetValue().ToString();
		}
    }

}
