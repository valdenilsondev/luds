#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Share.EventsSystem;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public enum RecordStatus {
	Idle,
	Recording,
	Playing,
}

namespace Share.Kinect.Record{
public class SkeletonRecorder : MonoBehaviour {
	
	private static SkeletonRecorder instance;
	private RecordStatus currentStatus = RecordStatus.Idle;
	private int currentFrame = -1;
	private SkeletonRecord skeletonRecord;

	void Awake () {
		instance = this;
	}

	void OnEnable() {
		Events.AddListener<StartRecordingEvent>(StartRecording);
		Events.AddListener<StopRecordingEvent>(StopRecording);
		Events.AddListener<PlayRecordingEvent>(Play);
	}

	void OnDisable() {
		Events.RemoveListener<StartRecordingEvent>(StartRecording);
		Events.RemoveListener<StopRecordingEvent>(StopRecording);
		Events.RemoveListener<PlayRecordingEvent>(Play);
	}

	private void Play() {
		currentFrame = 0;
		currentStatus = RecordStatus.Playing;
	}

	private void Pause() {
		currentStatus = RecordStatus.Idle;
	}

	public SkeletonFrame GetNextSkeletonFrame() {
		currentFrame++;

		if(currentFrame >= skeletonRecord.CountFrames()) {
			return null;
		}

		return skeletonRecord.GetFrame(currentFrame);
	}

	private void StartRecording() {
		skeletonRecord = new SkeletonRecord();
		currentStatus = RecordStatus.Recording;
	}

	public void Record(SkeletonFrame frame) {
		if(currentStatus == RecordStatus.Recording) {
			skeletonRecord.AddFrame(frame);
		}
	}

	private void StopRecording() {
		currentStatus = RecordStatus.Idle;
	}

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/Record.dat");
		
		bf.Serialize(file, skeletonRecord);
		file.Close();
	}
	
	public void Load() {
		if(File.Exists(Application.persistentDataPath + "/Record.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + "/Record.dat", FileMode.Open);
			SkeletonRecord data = (SkeletonRecord)bf.Deserialize(file);

			Debug.Log (data.frames.Count);
			for (int i = 0; i < data.frames.Count; i++) {
				//Debug.Log (data.frames[i].SkeletonPositions[0]);
			}

			file.Close();
		}
	}

	public static SkeletonRecorder Instance {
		get {
			if(instance == null) {
				instance = FindObjectOfType<SkeletonRecorder>();
				if(instance == null) {
					Debug.LogWarning("SkeletonRecorder não foi encontrado");
				}
			}
			
			return instance;
		}
	}
}
}
#endif