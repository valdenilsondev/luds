using UnityEngine;
using System.Collections;

using System;

namespace Share.KinectUtils {
	public class UserMap : MonoBehaviour {
		
		private KinectManager Kinect;
		private static UserMap instance;
		
		// Public Floats to specify the width and height of the depth and color maps as % of the camera width and height
		// if percents are zero, they are calculated based on actual Kinect image´s width and height
		public float mapsPercentWidth = 0f;
		public float mapsPercentHeight = 0f;
		
		private Texture2D usersLblTex;
		private Color32[] usersMapColors;
		private ushort[] usersPrevState;
		private Rect usersMapRect;
		private int usersMapSize;
		
		private Texture2D usersClrTex;
		//Color[] usersClrColors;
		private Rect usersClrRect;
		
		// Color image data, if used
		private Color32[] colorImage;
		private byte[] usersColorMap;
		
		private Rect cameraRect;
		
		private ushort[] usersDepthMap;
		private float[] usersHistogramMap;
		
		// Image stream handles for the kinect
		private IntPtr colorStreamHandle;
		private IntPtr depthStreamHandle;
		
		private bool userMap = false;
		private bool colorMap = false;
		
		// draws the skeleton in the given texture
		public void DrawSkeleton(ref KinectWrapper.NuiSkeletonData skeletonData, ref bool[] playerJointsTracked)
		{
			int jointsCount = (int)KinectWrapper.NuiSkeletonPositionIndex.Count;
			
			for(int i = 0; i < jointsCount; i++)
			{
				int parent = KinectWrapper.GetSkeletonJointParent(i);
				
				if(playerJointsTracked[i] && playerJointsTracked[parent])
				{
					Vector3 posParent = KinectWrapper.MapSkeletonPointToDepthPoint(skeletonData.SkeletonPositions[parent]);
					Vector3 posJoint = KinectWrapper.MapSkeletonPointToDepthPoint(skeletonData.SkeletonPositions[i]);
					
					//				posParent.y = KinectWrapper.Constants.ImageHeight - posParent.y - 1;
					//				posJoint.y = KinectWrapper.Constants.ImageHeight - posJoint.y - 1;
					//				posParent.x = KinectWrapper.Constants.ImageWidth - posParent.x - 1;
					//				posJoint.x = KinectWrapper.Constants.ImageWidth - posJoint.x - 1;
					
					//Color lineColor = playerJointsTracked[i] && playerJointsTracked[parent] ? Color.red : Color.yellow;
					DrawLine(usersLblTex, (int)posParent.x, (int)posParent.y, (int)posJoint.x, (int)posJoint.y, Color.yellow);
				}
			}
			
			usersLblTex.Apply();
		}
		
		// draws a line in a texture
		private void DrawLine(Texture2D a_Texture, int x1, int y1, int x2, int y2, Color a_Color)
		{
			int width = KinectWrapper.Constants.DepthImageWidth;
			int height = KinectWrapper.Constants.DepthImageHeight;
			
			int dy = y2 - y1;
			int dx = x2 - x1;
			
			int stepy = 1;
			if (dy < 0)  {
				dy = -dy; 
				stepy = -1;
			}
			
			int stepx = 1;
			if (dx < 0)  {
				dx = -dx; 
				stepx = -1;
			}
			
			dy <<= 1;
			dx <<= 1;
			
			if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
				for(int x = -1; x <= 1; x++)
					for(int y = -1; y <= 1; y++)
						a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
			
			if (dx > dy) {
				int fraction = dy - (dx >> 1);
				
				while (x1 != x2)  {
					if (fraction >= 0)  {
						y1 += stepy;
						fraction -= dx;
					}
					
					x1 += stepx;
					fraction += dy;
					
					if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
						for(int x = -1; x <= 1; x++)
							for(int y = -1; y <= 1; y++)
								a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
				}
			} else {
				int fraction = dx - (dy >> 1);
				
				while (y1 != y2) {
					if (fraction >= 0) {
						x1 += stepx;
						fraction -= dy;
					}
					
					y1 += stepy;
					fraction += dx;
					
					if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
						for(int x = -1; x <= 1; x++)
							for(int y = -1; y <= 1; y++)
								a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
				}
			}
			
		}
		
		void Awake() {
			instance = this;
			Kinect = KinectManager.Instance;
		}
		
		void Update() {
			if(KinectManager.IsKinectInitialized()) {
				// If the players aren't all calibrated yet, draw the user map.
				if(userMap) {
					if(depthStreamHandle != IntPtr.Zero && KinectWrapper.PollDepth(depthStreamHandle, KinectWrapper.Constants.IsNearMode, ref usersDepthMap)) {
						UpdateUserMap();
					}
				}
				
				if(colorMap) {
					if(colorStreamHandle != IntPtr.Zero && KinectWrapper.PollColor(colorStreamHandle, ref usersColorMap, ref colorImage)) {
						UpdateColorMap();
					}
				}
			}
		}
		
		public int InitUserMapSettings() {
			int hr = 0;
			try {
				depthStreamHandle = IntPtr.Zero;
				hr = KinectWrapper.NuiImageStreamOpen(KinectWrapper.NuiImageType.DepthAndPlayerIndex, 
				                                      KinectWrapper.Constants.DepthImageResolution, 0, 2, IntPtr.Zero, ref depthStreamHandle);
				
				userMap = true;
			} catch {
				return hr;
			}
			
			InitUserMap();
			return hr;
		}
		
		public int InitColorMapSettins() {
			int hr = 0;
			try {
				colorStreamHandle = IntPtr.Zero;
				hr = KinectWrapper.NuiImageStreamOpen(KinectWrapper.NuiImageType.Color, 
				                                      KinectWrapper.Constants.ColorImageResolution, 0, 2, IntPtr.Zero, ref colorStreamHandle);
				
				colorMap = true;
			} catch {
				return hr;
			}
			
			InitColorMap();
			return hr;
		}
		
		private void InitUserMap() {
			CalculateMapWidthHeightPercent();
			
			// Initialize depth & label map related stuff
			usersMapSize = KinectWrapper.GetDepthWidth() * KinectWrapper.GetDepthHeight();
			usersLblTex = new Texture2D(KinectWrapper.GetDepthWidth(), KinectWrapper.GetDepthHeight());
			usersMapColors = new Color32[usersMapSize];
			usersPrevState = new ushort[usersMapSize];
			//usersMapRect = new Rect(Screen.width, Screen.height - usersLblTex.height / 2, -usersLblTex.width / 2, usersLblTex.height / 2);
			//usersMapRect = new Rect(cameraRect.width, cameraRect.height - cameraRect.height * MapsPercentHeight, -cameraRect.width * MapsPercentWidth, cameraRect.height * MapsPercentHeight);
			usersMapRect = new Rect(cameraRect.width - cameraRect.width * mapsPercentWidth, cameraRect.height, cameraRect.width * mapsPercentWidth, -cameraRect.height * mapsPercentHeight);
			
			usersDepthMap = new ushort[usersMapSize];
			usersHistogramMap = new float[8192];
		}
		
		private void InitColorMap() {
			CalculateMapWidthHeightPercent();
			
			// Initialize color map related stuff
			usersClrTex = new Texture2D(KinectWrapper.GetDepthWidth(), KinectWrapper.GetDepthHeight());
			//usersClrRect = new Rect(cameraRect.width, cameraRect.height - cameraRect.height * MapsPercentHeight, -cameraRect.width * MapsPercentWidth, cameraRect.height * MapsPercentHeight);
			usersClrRect = new Rect(cameraRect.width - cameraRect.width * mapsPercentWidth, cameraRect.height, cameraRect.width * mapsPercentWidth, -cameraRect.height * mapsPercentHeight);
			
			//if(ComputeUserMap)
			usersMapRect.x -= cameraRect.width * mapsPercentWidth; //usersClrTex.width / 2;
			
			colorImage = new Color32[KinectWrapper.GetDepthWidth() * KinectWrapper.GetDepthHeight()];
			usersColorMap = new byte[colorImage.Length << 2];
		}
		
		private void CalculateMapWidthHeightPercent() {
			if(Camera.main == null) {
				Debug.Log ("Camera null");
			}
			cameraRect = Camera.main.pixelRect;
			
			// calculate map width and height in percent, if needed
			if(mapsPercentWidth == 0f)
				mapsPercentWidth = (KinectWrapper.GetDepthWidth() / 2) / cameraRect.width;
			if(mapsPercentHeight == 0f)
				mapsPercentHeight = (KinectWrapper.GetDepthHeight() / 2) / cameraRect.height;
		}
		
		// Update the User Map
		void UpdateUserMap() {
			int numOfPoints = 0;
			Array.Clear(usersHistogramMap, 0, usersHistogramMap.Length);
			
			// Calculate cumulative histogram for depth
			for (int i = 0; i < usersMapSize; i++) {
				// Only calculate for depth that contains users
				if ((usersDepthMap[i] & 7) != 0) {
					usersHistogramMap[usersDepthMap[i] >> 3]++;
					numOfPoints++;
				}
			}
			
			if (numOfPoints > 0) {
				for (int i = 1; i < usersHistogramMap.Length; i++) {
					usersHistogramMap[i] += usersHistogramMap[i-1];
				}
				
				for (int i = 0; i < usersHistogramMap.Length; i++) {
					usersHistogramMap[i] = 1.0f - (usersHistogramMap[i] / numOfPoints);
				}
			}
			
			// dummy structure needed by the coordinate mapper
			KinectWrapper.NuiImageViewArea pcViewArea = new KinectWrapper.NuiImageViewArea {
				eDigitalZoom = 0,
				lCenterX = 0,
				lCenterY = 0
			};
			
			// Create the actual users texture based on label map and depth histogram
			Color32 clrClear = Color.clear;
			for (int i = 0; i < usersMapSize; i++) {
				// Flip the texture as we convert label map to color array
				int flipIndex = i; // usersMapSize - i - 1;
				
				ushort userMap = (ushort)(usersDepthMap[i] & 7);
				ushort userDepth = (ushort)(usersDepthMap[i] >> 3);
				
				ushort nowUserPixel = userMap != 0 ? (ushort)((userMap << 13) | userDepth) : userDepth;
				ushort wasUserPixel = usersPrevState[flipIndex];
				
				// draw only the changed pixels
				if(nowUserPixel != wasUserPixel) {
					usersPrevState[flipIndex] = nowUserPixel;
					
					if (userMap == 0) {
						usersMapColors[flipIndex] = clrClear;
					} else {
						if(colorImage != null) {
							int x = i % KinectWrapper.Constants.ColorImageWidth;
							int y = i / KinectWrapper.Constants.ColorImageWidth;
							
							int cx, cy;
							int hr = KinectWrapper.NuiImageGetColorPixelCoordinatesFromDepthPixelAtResolution(
								KinectWrapper.Constants.ColorImageResolution,
								KinectWrapper.Constants.ColorImageResolution,
								ref pcViewArea,
								x, y, usersDepthMap[i],
								out cx, out cy);
							
							if(hr == 0) {
								int colorIndex = cx + cy * KinectWrapper.Constants.ColorImageWidth;
								//colorIndex = usersMapSize - colorIndex - 1;
								if(colorIndex >= 0 && colorIndex < usersMapSize) {
									Color32 colorPixel = colorImage[colorIndex];
									usersMapColors[flipIndex] = colorPixel;  // new Color(colorPixel.r / 256f, colorPixel.g / 256f, colorPixel.b / 256f, 0.9f);
									usersMapColors[flipIndex].a = 230; // 0.9f
								}
							}
						} else {
							// Create a blending color based on the depth histogram
							float histDepth = usersHistogramMap[userDepth];
							Color c = new Color(histDepth, histDepth, histDepth, 0.9f);
							
							switch(userMap % 4) {
							case 0:
								usersMapColors[flipIndex] = Color.red * c;
								break;
							case 1:
								usersMapColors[flipIndex] = Color.green * c;
								break;
							case 2:
								usersMapColors[flipIndex] = Color.blue * c;
								break;
							case 3:
								usersMapColors[flipIndex] = Color.magenta * c;
								break;
							}
						}
					}
					
				}
			}
			
			// Draw it!
			usersLblTex.SetPixels32(usersMapColors);
			
			if(!Kinect.DisplaySkeletonLines) {
				usersLblTex.Apply();
			}
		}
		
		// Update the Color Map
		void UpdateColorMap() {
			usersClrTex.SetPixels32(colorImage);
			usersClrTex.Apply();
		}

		/// <summary>
		/// returns the raw depth/user data, if ComputeUserMap is true
		/// </summary>
		public ushort[] GetRawDepthMap() {
			return usersDepthMap;
		}
		
		/// <summary>
		/// returns the depth data for a specific pixel
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public ushort GetDepthForPixel(int x, int y) {
			int index = y * KinectWrapper.Constants.DepthImageWidth + x;
			
			if(index >= 0 && index < usersDepthMap.Length)
				return usersDepthMap[index];
			else
				return 0;
		}
		
		/// <summary>
		/// Draw the Histogram Map on the GUI.
		/// </summary>
		void OnGUI() {
			if(Kinect.IsInitialized()) {
				if(userMap && (/**(allUsers.Count == 0) ||*/ Kinect.DisplayUserMap)) {
					GUI.DrawTexture(usersMapRect, usersLblTex);
				}
				
				if(colorMap && (/**(allUsers.Count == 0) ||*/ Kinect.DisplayColorMap)) {
					GUI.DrawTexture(usersClrRect, usersClrTex);
				}
			}
		}
		
		public static UserMap Instance() {
			if(instance == null){
				instance = FindObjectOfType<UserMap>();
				if(instance == null){
					Debug.LogError("Nenhum GameObject do tipo UserMap foi encontrado");
				}
			}
			
			return instance;
		}
		
		#region OverlayDemo
		// returns the depth map position for a 3d joint position
		public Vector2 GetDepthMapPosForJointPos (Vector3 posJoint)
		{
			Vector3 vDepthPos = KinectWrapper.MapSkeletonPointToDepthPoint (posJoint);
			Vector2 vMapPos = new Vector2 (vDepthPos.x, vDepthPos.y);
			
			return vMapPos;
		}
		
		// returns the color map position for a depth 2d position
		public Vector2 GetColorMapPosForDepthPos (Vector2 posDepth)
		{
			int cx, cy;
			
			KinectWrapper.NuiImageViewArea pcViewArea = new KinectWrapper.NuiImageViewArea  {
				eDigitalZoom = 0,
				lCenterX = 0,
				lCenterY = 0
			};
			
			/*int hr = */KinectWrapper.NuiImageGetColorPixelCoordinatesFromDepthPixelAtResolution (
				KinectWrapper.Constants.ColorImageResolution,
				KinectWrapper.Constants.ColorImageResolution,
				ref pcViewArea,
				(int)posDepth.x, (int)posDepth.y, GetDepthForPixel ((int)posDepth.x, (int)posDepth.y),
				out cx, out cy);
			
			return new Vector2 (cx, cy);
		}
		
		// returns the color image texture,if ComputeColorMap is true
		public Texture2D GetUsersClrTex () { 
			return usersClrTex;
		}
		#endregion
	}
}
