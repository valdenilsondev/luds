using UnityEngine;
using System.Collections;

using System.Runtime.InteropServices;
using Share.KinectUtils;

namespace Share.KinectUtils.Record {
	public class GestureEureka {
		public GestureEureka(int idGesture, KinectGestures.Gestures gesture, float[] gestureTimes) : this(gesture, gestureTimes) {
			this.idGesture = idGesture;
		}
		
		public GestureEureka(KinectGestures.Gestures gesture, float[] gestureTimes) {
			this.gesture = gesture;
			this.gestureTimes = gestureTimes;
		}

		public GestureEureka(KinectGestures.Gestures gesture, float[] gestureTimes, uint initialFrame, uint finalFrame) {
			this.gesture = gesture;
			this.gestureTimes = gestureTimes;
			this.initialFrame = initialFrame;
			this.finalFrame = finalFrame;
		}
		
		public int idGesture { get; set; }
		public uint initialFrame { get; set; }
		public uint finalFrame { get; set; }
		public string framesNumber { get; set; }
		public KinectGestures.Gestures gesture { get; set; }
		public float[] gestureTimes { get; set; }

		public override string ToString () {
			return string.Format ("[GestureEureka: idGesture={0}, initialFrame={1}, finalFrame={2}, framesNumber={3}, gesture={4}, gestureTimes={5}]", idGesture, initialFrame, finalFrame, framesNumber, gesture, gestureTimes);
		}
	}
	
	public class SkeletonFrameEureka {
		public SkeletonFrameEureka(){
			this.SkeletonPositions = new Vector4[25];
			this.SkeletonRotations = new Quaternion[25];
		}

		public SkeletonFrameEureka(int idFrame, Vector4[] skeletonPositions, Matrix4x4[] jointsOrientations) {
			this.idFrame = idFrame;
			this.SkeletonPositions = skeletonPositions;
			this.skeletonRotations = jointsOrientations;
		}

		public SkeletonFrameEureka(int idFrame, int idMatch, uint frameNumber, Vector4 pos, Vector4[] skeletonPositions) : this(frameNumber, pos, skeletonPositions){
			this.idFrame = idFrame;
			this.idMatch = idMatch;
		}
		
		public SkeletonFrameEureka(uint frameNumber, Vector4 pos, Vector4[] skeletonPositions) {
			this.dwFrameNumber = frameNumber;
			this.Position = pos;
			this.SkeletonPositions = skeletonPositions;
		}
		
		public int idMatch { get; set; }
		
		#region SkeletonFrame
		public int idFrame { get; set; }
		public uint dwFrameNumber { get; set; }
		#endregion
		
		#region SkeletonData
		public Vector4 Position { get; set; }
		[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
		public Vector4[] SkeletonPositions;
		public Quaternion[] SkeletonRotations;

		public Matrix4x4[] skeletonRotations;
		#endregion
	}
}