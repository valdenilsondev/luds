using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Share.Kinect
{
    public enum Movement
    {
		None,
        Jump,
        Roll, //pig
        MoveLeft,
        MoveRight,
		RaiseHand,
		Throw, //throw e fishing
		Pull, //girar carretel
		StopHand,
		RemarRight, //sup com braçada ou elevaçao
		RemarLeft, //sup
		ArmsUp,//so usa no sup
		BigPullR, //puxao forte no fishing
		BigPullL
    }
}
