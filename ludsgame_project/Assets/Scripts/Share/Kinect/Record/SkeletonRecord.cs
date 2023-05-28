#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Runtime.InteropServices;

[Serializable]
public class SkeletonRecord {
	
	public List<SkeletonFrame> frames = new List<SkeletonFrame>();

	public void AddFrame(SkeletonFrame frame) {
		frames.Add(frame);
	}

	public int CountFrames() {
		return frames.Count;
	}

	public SkeletonFrame GetFrame(int index) {
		return frames[index];

	}

}

[Serializable]
public class SkeletonFrame {

	public SkeletonFrame(){
		this.SkeletonPositions = new Vector4[25];
		this.SkeletonRotations = new Quaternion[25];
	}
	
	public SkeletonFrame(int idFrame, Vector4[] skeletonPositions, Matrix4x4[] jointsOrientations) {
		this.idFrame = idFrame;
		this.SkeletonPositions = skeletonPositions;
		this.skeletonRotations = jointsOrientations;
	}
	
	public SkeletonFrame(int idFrame, int idMatch, uint frameNumber, Vector4 pos, Vector4[] skeletonPositions) : this(frameNumber, pos, skeletonPositions){
		this.idFrame = idFrame;
		this.idMatch = idMatch;
	}

	public SkeletonFrame(uint frameNumber, Vector4 pos, Vector4[] skeletonPositions) {
		this.dwFrameNumber = frameNumber;
		this.SkeletonPositions = skeletonPositions;
		this.posSkeleton = pos;
	}
	
	public int idMatch;

	#region SkeletonFrame
	public int idFrame;
	public uint dwFrameNumber;
	#endregion

	#region SkeletonData
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
	public Vector4[] SkeletonPositions;

	public Quaternion[] SkeletonRotations;

	public Matrix4x4[] skeletonRotations;

	public Vector4 posSkeleton;
	#endregion
}
#endif