using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Ludsgame;
using Share.EventSystem;

namespace Share.KinectUtils.Record {
	public class SkeletonInclination : MonoBehaviour {
		
		private SkeletonRecorder skeletonRecorder;
		private Events events;

		private string inclinationsFileName = "Inclinations.txt";
		private bool hasCalculated = false;

		public float[] inclinations;

		void Awake () {
		//	skeletonRecorder = SkeletonRecorder.Instance;
			events = Events.instance;
		}

		void OnEnable() {
			events.AddListener<GameOverEvents>(OnGameOver);
		}

		void OnDisable() {
			events.RemoveListener<GameOverEvents>(OnGameOver);
		}

		/// <summary>
		/// Calcula as inclinações baseando-se na última gravação
		/// </summary>
		private void CalculateSkeletonInclination() {
			//inclinations = new float[skeletonRecorder.GetSkeletonFramesLength()];
			SkeletonFrameEureka skeletonFrame;

			for (int i = 0; i < inclinations.Length; i++) {
//skeletonFrame = skeletonRecorder.GetSkeletonFrame(i);

			//	inclinations[i] = skeletonFrame.SkeletonPositions[(int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter].x - skeletonFrame.SkeletonPositions[(int)KinectWrapper.NuiSkeletonPositionIndex.Spine].x;
			}

			CreateFile();
			WriteInclinations();
			WriteTimes();

			SendTextFile();
		}

		/// <summary>
		/// Cria o arquivo
		/// </summary>
		private void CreateFile() {
			if(!File.Exists(inclinationsFileName)) {
				File.CreateText(inclinationsFileName);
			}
		}

		/// <summary>
		/// Escreve as inclinações no arquivo
		/// </summary>
		private void WriteInclinations() {
			StreamWriter sw = File.AppendText(inclinationsFileName);
			string inclination = "";

			sw.Write("$@$");
			for (int i = 0; i < inclinations.Length; i++) {
				inclination = inclinations[i] + ",";
				if(i == inclinations.Length - 1) {
					inclination = inclinations[i].ToString(FormatConfig.Nfi);
				}

				sw.Write(inclination);

			}
			sw.WriteLine("$@$");

			sw.Close();
		}

		/// <summary>
		/// Escreve os tempos no arquivo
		/// </summary>
		private void WriteTimes() {
			StreamWriter sw = File.AppendText(inclinationsFileName);
			int index = 0;
			string text = "";

			sw.Write("$@$");
			for (int i = 0; i < inclinations.Length; i++) {
				if(i%30 == 0){
					index++;
				}

				text = index.ToString() + ",";
				if(i == inclinations.Length - 1) {
					text = index.ToString();
				}

				sw.Write(text);
				
			}
			sw.WriteLine("$@$");
			
			sw.Close();
		}

		/// <summary>
		/// Tenta enviar os dados para o banco de dados
		/// </summary>
		private void SendTextFile() {
			try {
				// TODO: Salvar dados no banco de dados
			} catch (Exception ex) {
				// TODO: Tentar enviar os dados mais tarde
				return;
			}

			ClearTextFile();
		}

		/// <summary>
		/// Limpa o arquivo
		/// </summary>
		private void ClearTextFile() {
			File.WriteAllText(inclinationsFileName, string.Empty);
		}

		/// <summary>
		/// Evento de Game Over
		/// </summary>
		public void OnGameOver(GameOverEvents gameOverEvents) {
			if (!hasCalculated) {
				hasCalculated = true;
				CalculateSkeletonInclination ();
			}
		}
	}
}
