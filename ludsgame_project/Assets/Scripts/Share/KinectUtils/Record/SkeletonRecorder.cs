using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.IO;
using System.Net;
using System;
using System.Text;
using Ludsgame;
using Share.Database;
using Share.KinectUtils;

public enum SkeletonRecordState {
	Idle,
	RecordingSkeletonAndGestures,
	RecordingSkeleton,
	RecodingGestures,
	PlayingSkeleton,
	PlayingGesture
}

namespace Share.KinectUtils.Record {
	public class SkeletonRecorder : MonoBehaviour {
		
		private static SkeletonRecorder instance;
		
		private int indexSkeletonFrame = 0;
		private int indexGesture = 0;
		public int id_match;
		
		private List<SkeletonFrameEureka> listSkeletonFrames = new List<SkeletonFrameEureka>();
		private List<GestureEureka> listGestures = new List<GestureEureka>();
		public SkeletonRecordState CurrentState { get; set; }
		private List<SkeletonFrameEureka> listSkeletonFramesPlayback = new List<SkeletonFrameEureka>();
		
		private void Awake() {
			instance = this;
			DontDestroyOnLoad(this);
		}
		
		
		private void Start() {
			//TODO: Fazer utilizando transaçao. Porque pode pegar o id de outra partida que esteja acontecendo ao mesmo tempo em maquinhas diferentes
			//MySQL.instance.InsertMatch (System.DateTime.Now);
		///	id_match = MySQL.instance.GetCurrentMatchId ();
		}
		
		#region Debugging
		private void PrintSkeletonList() {
			for (int i = 0; i < listSkeletonFrames.Count; i++) {
				Debug.Log (listSkeletonFrames [i].SkeletonPositions [(int)KinectWrapper.NuiSkeletonPositionIndex.Head]);
			}
			
			Debug.Log ("Fim");
		}
		
		private void PrintGestureList() {
			for (int i = 0; i < listGestures.Count; i++) {
				Debug.Log (listGestures [i].gesture);
			}
			
			Debug.Log ("Fim");
		}
		#endregion
		
		#region Playing
		public bool StartPlaySkeletonRecord() {
			if(CurrentState == SkeletonRecordState.Idle) {
				CurrentState = SkeletonRecordState.PlayingSkeleton;
				
				return true;
			}
			
			return false;
		}
		
		public bool StartPlayGestureRecord() {
			if (CurrentState == SkeletonRecordState.Idle) {
				CurrentState = SkeletonRecordState.PlayingGesture;
				
				return true;
			}
			
			return false;
		}

		public SkeletonFrameEureka GetSkeletonFrame(uint frame) {
			for (int i = 0; i < listSkeletonFramesPlayback.Count; i++) {
				if (listSkeletonFramesPlayback[i].dwFrameNumber == frame) {
					return listSkeletonFramesPlayback [i];
				}
			}
			
			return null;
		}
		
		public SkeletonFrameEureka PlaySkeletonRecord() {
			listSkeletonFramesPlayback = listSkeletonFrames;// MySQL.instance.GetAllJointsPositionFromMatch(104); // para teste 
			CurrentState = SkeletonRecordState.PlayingSkeleton; //teste
			if(CurrentState == SkeletonRecordState.PlayingSkeleton) {
				if(indexSkeletonFrame >= listSkeletonFramesPlayback.Count) {
					
					StopRecord();
					return null;
				}
				
				return listSkeletonFramesPlayback[indexSkeletonFrame++];
				//return listSkeletonFrames[indexSkeletonFrame++];
			}
			
			Debug.LogError ("Jogo não configurado para o esqueleto do jogador!");
			return null;
		}
		
		public GestureEureka PlayGestureRecord() {
			if(CurrentState == SkeletonRecordState.PlayingGesture) {
				if(indexGesture >= listGestures.Count) {
					StopRecord();
				}
				
				return listGestures[indexGesture++];
			}
			
			Debug.LogError ("Jogo não configurado para os gestos do jogador!");
			return null;
		}
		
		public void PauseRecord() {
			CurrentState = SkeletonRecordState.Idle;
		}
		
		public void StopRecord() {
			PauseRecord();
			
			indexGesture = 0;
			indexSkeletonFrame = 0;
		}
		
		public void RestartRecord() {
			indexGesture = 0;
			indexSkeletonFrame = 0;
		}
		#endregion
		
		#region Recording
		// Configura o jogo para gravar o esqueleto do jogador
		public void StartRecordingSkeleton() {
			CurrentState = SkeletonRecordState.RecordingSkeleton;
			
			listSkeletonFrames = new List<SkeletonFrameEureka>();
		}
		
		// Configura o jogo para gravar os gestos do jogador
		public void StartRecordingGestures() {
			CurrentState = SkeletonRecordState.RecodingGestures;
			
			listGestures = new List<GestureEureka>();
		}
		
		// Configura o jogo para gravar o esqueleto e os gestos do jogador
		public void StartRecording() {
			CurrentState = SkeletonRecordState.RecordingSkeletonAndGestures;

			listSkeletonFrames = new List<SkeletonFrameEureka>();
			listGestures = new List<GestureEureka>();
		}
		private bool saved = false;
		// Para a gravação e grava as informações no banco de dados
		public void StopRecording(bool saveOnDatabase) {
			if(saveOnDatabase) {
				// TODO: Save on database
				//DataBase.instance.SaveSkelFramesList(listSkeletonFrames);
				if(!saved){
					SaveSkelFramesListOnDatabase(listSkeletonFrames);
					listSkeletonFramesPlayback =  listSkeletonFrames; 
					saved = true;
					Debug.Log ("Fim");
				}
			}
			
			CurrentState = SkeletonRecordState.Idle;
		}
		
		public bool AddSkeletonFrame (ref KinectWrapper.NuiSkeletonFrame skeletonFrame, int playerID) {
			return AddSkeletonFrame(new SkeletonFrameEureka (KinectManager.Instance.initial_frame_id, id_match, skeletonFrame.dwFrameNumber, skeletonFrame.SkeletonData [playerID].Position, skeletonFrame.SkeletonData [playerID].SkeletonPositions));
		}
		
		int frametest;
		public bool AddSkeletonFrame (SkeletonFrameEureka skeletonFrame) {
			if (CurrentState == SkeletonRecordState.RecordingSkeleton || CurrentState == SkeletonRecordState.RecordingSkeletonAndGestures) {
				//skeletonFrame.idMatch = id_match;
				//skeletonFrame.idFrame = Kinect.Instance.initial_frame_id;
				listSkeletonFrames.Add(skeletonFrame);
				//print("gravando? " + frametest++);
				// Debug apenas
				//			if (listSkeletonFrames.Count == 500) {
				//				PrintSkeletonList();
				//				StopRecording(true);
				//				return false;
				
				//			}
				
				return true;
			}
			
			//Debug.LogError ("Jogo não configurado para gravar o esqueleto do jogador!");
			return false;
		}
		
		public void AddGesture (ref KinectGestures.GestureData gestureData)	{
			if (CurrentState == SkeletonRecordState.RecodingGestures || CurrentState == SkeletonRecordState.RecordingSkeletonAndGestures) {
				listGestures.Add (new GestureEureka (gestureData.gesture, gestureData.gestureTimes));
				
				// Debug apenas
				//			if (listGestures.Count == 3) {
				//				PrintGestureList ();
				//			}
			} else {
				Debug.LogError ("Jogo não configurado para gravar os gestos do jogador!");
			}
		}
		
		
		
		//Salva os frames na tabela frames e as jointpositions
		public void SaveSkelFramesListOnDatabase(List<SkeletonFrameEureka> skelFrameList){
			StringBuilder sb = new StringBuilder();
			StringBuilder sb2 = new StringBuilder();
			
			string match = id_match.ToString();
			string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\insertedFrames"+match+".txt";
			string folder2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\insertedJoints"+match+".txt";
			
			foreach (SkeletonFrameEureka f in skelFrameList) {
				string posX = f.Position.x.ToString(FormatConfig.Nfi);
				string posY = f.Position.y.ToString(FormatConfig.Nfi);
				string posZ = f.Position.z.ToString(FormatConfig.Nfi);
				string posW = f.Position.w.ToString(FormatConfig.Nfi);
				
				sb.AppendLine(f.idFrame+"$@$"+posX + ", " + posY + ", " + posZ + ", "+ posW + "$@$" + f.dwFrameNumber + "$@$" + f.idMatch);
				string jointPosText= String.Empty;
				
				for(int i = 0; i<20; i++){
					jointPosText = jointPosText + ("$@$"+ (f.SkeletonPositions[i].x + ", " + f.SkeletonPositions[i].y + ", " + f.SkeletonPositions[i].z + ", " + f.SkeletonPositions[i].w));
				}
				
				sb2.AppendLine("$@$"+f.idFrame+jointPosText);
			}
			System.IO.File.WriteAllText(folder, sb.ToString());
			System.IO.File.WriteAllText(folder2, sb2.ToString());
			
			//MySQL.instance.SetCurrentFolder (folder);
		//	MySQL.instance.InsertFrameUsingFile ();
			
		//	MySQL.instance.SetCurrentFolder (folder2);
		//	MySQL.instance.ChamandoInsert();
		}
		
		
		#endregion
		
		public static SkeletonRecorder Instance {
			get {
				if(instance == null) {
					instance = FindObjectOfType<SkeletonRecorder>();
					if(instance == null) {
						Debug.LogError("Nenhum GameObject do tipo SkeletonRecorder foi encontrado");
					}
				}
				
				return instance;
			}
		}

		public int GetSkeletonFramesLength() {
			return listSkeletonFramesPlayback.Count;
		}

		public SkeletonFrameEureka GetSkeletonFrame(int index) {
			return listSkeletonFramesPlayback[index];
		}
	}
}