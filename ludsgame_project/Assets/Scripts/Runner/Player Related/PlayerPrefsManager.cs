using UnityEngine;
using System.Collections;

public class PlayerPrefsManager{
	private const string lateralAmountAnalog = "LATERAL_AMOUNT_ANALOG";
	private const string lateralAmountDigital = "LATERAL_AMOUNT_DIGITAL";
	private const string moveHandsensibility = "MOVE_HAND_SENSIBILITY";
	private const string movingSideAmount = "MOVING_SIDE_AMOUNT";
	private const string jumpAmountNeeded = "JUMP_AMOUNT_NEEDED";
	private const string rollAmountNeeded = "ROLL_AMOUNT_NEEDED";
	
	private const string initialMapSpeed = "INITIAL_MAP_SPEED";
	private const string mapSpeedLimit = "MAP_SPEED_LIMIT";
	private const string incrementSpeedInterval = "INCREMENT_SPEED_INTERVAL";
	private const string incrementSpeedValue = "INCREMENT_SPEED_VALUE";
	
	private const string playerDeficiency = "PLAYER_DEFICIENCY";
	private const string playerMiniGames = "PLAYER_MINIGAMES";
	private const string playerMiniGamesIndex = "PLAYER_MINIGAMES_INDEX";
	
	private const string clinicToken = "CLINIC_TOKEN";
	private const string physiotherapistToken = "PHYSIOTHERAPIST_TOKEN";
	private const string playerToken = "PLAYER_TOKEN";
	private const string mainToken = "MAIN_TOKEN";

	private const string player_name ="PLAYER_NAME";
	private const string player_id = "PLAYER_ID";
	private const string clinic_id = "CLINIC_ID";
	private const string physio_id = "PHYSIO_ID";
	
	private const string current_game_id = "CURRENT_GAME_ID";
	private const string current_match = "CURRENT_MATCH";
	private const string pig_tutorial = "PIG_TUTORIAL";
	private const string any_group_type = "ANY_GROUP_TYPE";
	private const string current_pig_group_type = "CURRRENT_PIG_GROUP_TYPE";
	private const string current_pig_level_group = "CURRRENT_PIG_LEVEL_GROUP";
	private const string move_with_hands_on = "MOVE_WITH_HANDS_ON";
	private const string move_to_sides = "MOVE_TO_SIDES";
	
	private const string number_of_lifes = "NUMBER_OF_LIFES";
	private const string borg_time = "BORG_TIME";
	
	private const string presentation_first_time = "PRESENTATION_FIRST_TIME";
	private const string games_first_time = "GAMES_FIRST_TIME";
	private const string borg_first_time = "BORG_FIRST_TIME";
	private const string gameOver_first_time = "GAMEOVER_FIRST_TIME";
	//====PIGRUNNER======
	private const string habilitarAgachar = "HABILITARAGACHAR";
	private const string habilitarPulo = "HABILITARPULO";
	private const string qtdObstaculos = "QTDOBSTACULOS";
	///====GOALKEEPER====
	private const string defs_limit = "DEFENSE_LIMIT";
	private const string goals_limit = "GOAL_LIMIT";
	private const string bar_speed = "BAR_SPEED";
	private const string bar_speed_increment = "BAR_SPEED_INCREMENT";
	private const string time_to_shoot = "TIME_TO_SHOOT";
	private const string gk_movement_setup = "GK_MOVEMENT_SETUP";
	///=======Bridge===========
	private const string lateral_sensibilit = "LATERAL_SENSIBILIT";
	private const string frontal_sensibilit = "FRONTAL_SENSIBILIT";//nao esta sendo usada
	private const string recover_sensibilit = "RECOVER_SENSIBILIT";
	private const string bridge_boost = "BRIDGE_BOOST";
	//=======FISHING========
	private const string fishingDifficult = "FISHING_DIFFICULT";
	private const string pullingStrengh = "PULLING_STRENGH";
	private const string big_pullingStrengh = "BIG_PULLING_STRENGH";
	private const string number_of_baits = "NUMBER_OF_BAITS";
	private const string fishing_setup_movements = "FISHING_SETUP_MOVEMENTS";//se eh elevaçao lateral ou default
	private const string fishingqt = "FISHINGQT";
	//========SUP==============
	private const string global_time = "GLOBAL_TIME";
	//=====THROW===============
	private const string throw_remaining_time = "THROW_REMAINING_TIME";
	private const string plaque_time = "PLAQUE_TIME";
	private const string throw_gesture_setup = "THROW_GESTURE_SETUP";
	private const string throw_right_diferentao = "THROW_RIGHT_DIFERENTAO";
	private const string throw_left_diferentao = "THROW_LEFT_DIFERENTAO";
	
	//Movimentos para controle do jogo
	//=====================
	private const string jump_mov = "JUMP_MOV_01";
	private const string jump_mov2 = "JUMP_MOV_02";
	private const string squat_mov = "AQUAT_MOV";
	private const string move_left_mov = "MOVE_LEFT_MOV";
	private const string move_right_mov = "MOVE_RIGHT_MOV";
	private const string row_left_mov = "ROW_LEFT_MOV";
	private const string row_right_mov = "ROW_RIGHT_MOV";
	private const string row_mid_mov = "ROW_MID_MOV";
	private const string stop_right_hand = "STOP_RIGHT_HAND";
	private const string stop_left_hand = "STOP_LEFT_HAND";
	private const string cancel_stop_right_hand = "CANCEL_STOP_RIGHT_HAND";
	//private const string cancel_stop_left_hand = "CANCEL_STOP_LEFT_HAND";
	private const string TwoArms_Up = "TWOARMS_UP";
	
	private const string throw_left_mov = "THROW_LEFT_MOV";
	private const string throw_right_mov = "THROW_RIGHT_MOV";
	private const string throw_raise_right_mov = "THROW_RAISE_RIGHT_MOV";
	private const string throw_raise_left_mov = "THROW_RAISE_LEFT_MOV";
	private const string pullL_mov = "PULL_LEFT_MOV";
	private const string pullR_mov = "PULL_RIGHT_MOV";
	private const string remar_left = "REMAR_LEFT";
	private const string remar_right = "REMAR_RIGHT";
	private const string right_arm_elevation = "RIGHT_ARM_ELEVATION";
	private const string left_arm_elevation = "LEFT_ARM_ELEVATION";
	//=====================
	//idperfil config by game
	private const string idperfil_pig = "IDPERFIL_PIG";
	private const string idperfil_bridge = "IDPERFIL_BRIDGE";
	private const string idperfil_gk = "IDPERFIL_GK";
	private const string idperfil_fishing = "IDPERFIL_FISHING";
	private const string idperfil_throw = "IDPERFIL_THROW";
	private const string idperfil_sup = "IDPERFIL_SUP";
	private const string using_kinect = "USING_KINECT";
	//Victory e DEFEATED
	private const string victory = "VICTORY";
	private const string defeated = "DEFEATED";
	
	private const string user = "USER";
	private const string password = "PASSWORD";

	private const string circle_on = "CIRCLE";

	public static void ClearLogin(){
		PlayerPrefs.SetString (password, "");
		PlayerPrefs.SetString (user, "");
		PlayerPrefs.Save();
	}

	public static void SetPassword(string pass)
	{
		PlayerPrefs.SetString (password, pass);
		PlayerPrefs.Save();
	}
	public static string GetPassword()
	{
		return PlayerPrefs.GetString(password);
	}

	public static void SetUser(string _user)
	{
		PlayerPrefs.SetString (user, _user);
		PlayerPrefs.Save();
	}
	public static string GetUser()
	{
		return PlayerPrefs.GetString(user);
	}



	//Metodos GET e SET
	public static void SetUsingKinect(int usingK)
	{
		PlayerPrefs.SetInt (using_kinect, usingK);
		PlayerPrefs.Save();
	}
	public static int GetUsingKinect()
	{
		return PlayerPrefs.GetInt(using_kinect, 1);//0 false === 1 true
	}
	//GK metodo
	public static void SetTimeToShoot(int time)
	{
		PlayerPrefs.SetInt (time_to_shoot, time);
		PlayerPrefs.Save();
	}
	public static int GetTimeToShoot()
	{
		return PlayerPrefs.GetInt(time_to_shoot, 3);
	}
	
	public static void SetBarSpeedIncrement(float sp){
		PlayerPrefs.SetFloat(bar_speed_increment, sp);
		PlayerPrefs.Save();
	}
	
	public static float GetBarSpeedIncrement(){
		return PlayerPrefs.GetFloat (bar_speed_increment, 0.2f);
	}
	
	public static void SetBarSpeed(int sp){
		PlayerPrefs.SetInt(bar_speed, sp);
		PlayerPrefs.Save();
	}
	
	public static int GetBarSpeed(){
		return PlayerPrefs.GetInt (bar_speed, 1);// lenta=75 media = 100 rapida=120
	}
	
	public static void SetGoalsLimit(int gLmit){
		PlayerPrefs.SetInt(goals_limit, gLmit);
		PlayerPrefs.Save();
	}
	
	public static int GetGoalsLimit(){
		return PlayerPrefs.GetInt (goals_limit, 3);
	}
	
	public static void SetDefenseLimit(int dLmit){
		PlayerPrefs.SetInt(defs_limit, dLmit);
		PlayerPrefs.Save();
	}
	public static int GetDefenseLimit(){
		return PlayerPrefs.GetInt (defs_limit, 3);
	}
	
	public static void SetGKmovementChoice(int moveChoice)
	{
		PlayerPrefs.SetInt(gk_movement_setup, moveChoice);
		PlayerPrefs.Save();
	}
	public static int GetGKSetupMovement(){
		return PlayerPrefs.GetInt (gk_movement_setup, 0);//0 DEFAULT 1 ELEVAÇAO
	}
	
	//bridge
	public static void SetLateralSensibilit(float Lsensibilit)
	{
		PlayerPrefs.SetFloat(lateral_sensibilit, Lsensibilit);
		PlayerPrefs.Save();
	}
	public static float GetLateralSensibilit()
	{
		return PlayerPrefs.GetFloat(lateral_sensibilit, 1);//baixa=13 media=20 alta=27
	}
	
	public static void SetBridgeBoost(int torso_speed)
	{
		PlayerPrefs.SetInt(bridge_boost, torso_speed);
		PlayerPrefs.Save();
	}
	public static int GetBridgeBoost()
	{
		return PlayerPrefs.GetInt(bridge_boost, 1);// lento=2 normal=3 alto =4
	}
	public static void SetJump1(int jump1id){
		PlayerPrefs.SetInt(jump_mov, jump1id);
		PlayerPrefs.Save();
	}
	
	public static int GetRecoverSensibility()
	{
		return PlayerPrefs.GetInt(recover_sensibilit, 3);//nao definido
	}
	public static void SetRecoverSensibility(int recover)
	{
		PlayerPrefs.SetInt(recover_sensibilit, recover);
		PlayerPrefs.Save();
	}
	public static int GetJump1(){
		int value = PlayerPrefs.GetInt (jump_mov, 25/*raiseknee right*/);
		return value;
	}
	
	public static void SetJump2(int jump2id){
		PlayerPrefs.SetInt(jump_mov2, jump2id);
		PlayerPrefs.Save();
	}
	
	public static int GetJump2(){
		int value = PlayerPrefs.GetInt (jump_mov2, 26/*raiseknee left*/);
		return value;
	}
	
	public static void SetSquat(int squatid){
		PlayerPrefs.SetInt(squat_mov, squatid);
		PlayerPrefs.Save();
	}
	
	public static int GetSquat(){
		int value = PlayerPrefs.GetInt (squat_mov, 18/*squat*/);
		return value;
	}
	
	public static void SetMoveLeft(int movLeftId){
		PlayerPrefs.SetInt(move_left_mov, movLeftId);
		PlayerPrefs.Save();
	}
	
	public static int GetMoveLeft(){
		int value = PlayerPrefs.GetInt (move_left_mov, 23/*moveleft*/);
		return value;
	}
	
	public static void SetMoveRight(int movRightId){
		PlayerPrefs.SetInt(move_right_mov, movRightId);
		PlayerPrefs.Save();
	}
	
	public static int GetMoveRight(){
		int value = PlayerPrefs.GetInt (move_right_mov, 24/*moveright*/);
		return value;
	}
	
	public static void SetRowRight(int rowRightId){
		PlayerPrefs.SetInt(row_right_mov, rowRightId);
		PlayerPrefs.Save();
	}
	
	public static int GetRowRight(){
		return PlayerPrefs.GetInt (row_right_mov, 39/*rowright*/);
	}
	
	public static void SetRowLeft(int rowLeftId){
		PlayerPrefs.SetInt(row_left_mov, rowLeftId);
		PlayerPrefs.Save();
	}
	public static int GetRowLeft(){
		return PlayerPrefs.GetInt (row_left_mov, 38/*rowleft*/);
	}
	
	public static void SetRowMid(int rowMidId){
		PlayerPrefs.SetInt(row_mid_mov, rowMidId);
		PlayerPrefs.Save();
	}
	public static int GetRowMid(){
		return PlayerPrefs.GetInt (row_mid_mov, 40/*rowmid*/);
	}
	
	public static int GetRemarRight(){
		return PlayerPrefs.GetInt (remar_right, 47/*rowmid*/);
	}
	
	public static void SetRemarRight(int remarRightId){
		PlayerPrefs.SetInt(remar_right, remarRightId);
		PlayerPrefs.Save();
	}
	
	public static int GetRemarLeft(){
		return PlayerPrefs.GetInt (remar_left, 46/*rowmid*/);
	}
	
	public static void SetRemarLeft(int remarLeftId){
		PlayerPrefs.SetInt(remar_left, remarLeftId);
		PlayerPrefs.Save();
	}
	
	//Pausar com a mao direita
	public static int GetStopRightHand(){
		return PlayerPrefs.GetInt(stop_right_hand, 41);
	}
	
	public static void SetStopRightHand(int stopRightId)
	{
		PlayerPrefs.SetInt(stop_right_hand, stopRightId);
		PlayerPrefs.Save ();
	}
	//Pausar com a mao esquerda
	public static int GetStopLeftHand(){
		return PlayerPrefs.GetInt(stop_left_hand, 42);
	}
	
	public static void SetStopLeftHand(int stopRightId)
	{
		PlayerPrefs.SetInt(stop_left_hand, stopRightId);
		PlayerPrefs.Save ();
	}
	
	//elevaçao frontal
	public static int GetTwoArmsUp()
	{
		return PlayerPrefs.GetInt(TwoArms_Up, 45);
	}
	public static void SetTwoArmsUp(int twoArmId)
	{
		PlayerPrefs.SetInt(TwoArms_Up, twoArmId);
		PlayerPrefs.Save ();
	}
	//elevaçao lateral direita
	public static int GetRightArmElevation()
	{
		return PlayerPrefs.GetInt(right_arm_elevation, 43);
	}
	public static void SetRightArmElevation(int rightArmId)
	{
		PlayerPrefs.SetInt(right_arm_elevation, rightArmId);
		PlayerPrefs.Save ();
	}
	//elevacao lateral esquerda
	public static int GetLeftArmElevation()
	{
		return PlayerPrefs.GetInt(left_arm_elevation, 44);
	}
	public static void SetLeftArmElevation(int leftArmId)
	{
		PlayerPrefs.SetInt(left_arm_elevation, leftArmId);
		PlayerPrefs.Save ();
	}
	
	//====THROW==========
	
	public static void SetThrowLeft(int throwLeftId){
		PlayerPrefs.SetInt(throw_left_mov, throwLeftId);
		PlayerPrefs.Save();
	}
	
	public static int GetThrowLeft(){
		return PlayerPrefs.GetInt (throw_left_mov, 21/*throwleft*/);
	}
	
	public static void SetThrowRight(int tRightId){
		PlayerPrefs.SetInt(throw_right_mov, tRightId);
		PlayerPrefs.Save();
	}
	
	public static int GetThrowRight(){
		return PlayerPrefs.GetInt (throw_right_mov, 22/*throwright*/);
	}
	public static void SetRemaingtimeThrow(float time){
		float value = time * 60;
		PlayerPrefs.SetFloat(throw_remaining_time, value);
		PlayerPrefs.Save();
	}
	public static float GetRemaininigtimethrow(){
		 
		return PlayerPrefs.GetFloat(throw_remaining_time, 30f);
	}
	public static void SetPlaqueTime(float time)
	{
		PlayerPrefs.SetFloat(plaque_time, time);//10 e o mais lento
		PlayerPrefs.Save();
	}
	public static float GetPlaqueTime()
	{
		return PlayerPrefs.GetFloat(plaque_time,8f);//10 e o mais lento
	}
	public static void SetThrowSetupMovements(int gesture)
	{	
		PlayerPrefs.SetInt(throw_gesture_setup, gesture);
		PlayerPrefs.Save();
	}
	/*public static int GetThrowSetupMovementes()
	{
		return PlayerPrefs.GetInt(throw_gesture_setup, 0);//0 default e 1 novo gesto
	}*/
	//levantar mao para arremeçar
	public static void SetTrow_RaiseRight(int throw_raiseRightId){
		PlayerPrefs.SetInt(throw_raise_right_mov, throw_raiseRightId);
		PlayerPrefs.Save();
	}
	
	public static int GetThrow_RaiseRight(){
		return PlayerPrefs.GetInt (throw_raise_right_mov, 1/*throwright*/);
	}
	public static void SetTrow_RaiseLeft(int throw_raiseRightId){
		PlayerPrefs.SetInt(throw_raise_left_mov, throw_raiseRightId);
		PlayerPrefs.Save();
	}
	public static int GetThrow_RaiseLeft(){
		return PlayerPrefs.GetInt (throw_raise_left_mov, 2/*throwleft*/);
	}
	//===arremeço com inclinaçao da cintura
	public static void SetThrowRight_Diferentao(int throw_r_diferentao)
	{
		PlayerPrefs.SetInt(throw_right_diferentao, throw_r_diferentao);
		PlayerPrefs.Save();
	}
	public static int GetThrowRight_Diferentao()
	{
		return PlayerPrefs.GetInt (throw_right_diferentao, 48);
	}
	public static void SetThrowLeft_Diferentao(int throw_l_diferentao)
	{
		PlayerPrefs.SetInt(throw_left_diferentao, throw_l_diferentao);
		PlayerPrefs.Save();
	}
	public static int GetThrowLeft_Diferentao()
	{
		return PlayerPrefs.GetInt (throw_left_diferentao, 49);
	}
	//=====Vitorias e Derrotas======
	public static void SetVictory(int vitoria)
	{
		PlayerPrefs.SetInt(victory, vitoria);
		PlayerPrefs.Save();
	}
	public static int GetVictory()
	{
		return PlayerPrefs.GetInt (victory, 1);
	}
	public static void SetDefeated(int derrota)
	{
		PlayerPrefs.SetInt(defeated, derrota);
		PlayerPrefs.Save();
	}
	public static int GetDefeated()
	{
		return PlayerPrefs.GetInt (defeated, 1);
	}

	//=== pull right and left============
	public static void SetPullLeft(int pull1Id){
		PlayerPrefs.SetInt(pullL_mov, pull1Id);
		PlayerPrefs.Save();
	}
	
	public static int GetPullLeft(){
		return PlayerPrefs.GetInt (pullL_mov, 34/*pull left*/);
	}
	
	
	public static void SetPullRight(int pull2Id){
		PlayerPrefs.SetInt(pullR_mov, pull2Id);
		PlayerPrefs.Save();
	}
	
	public static int GetPullRight(){
		return PlayerPrefs.GetInt (pullR_mov, 35/*pull right*/);
	}
	
	
	//===================================================
	
	public static void SetBorgTime(int time){
		PlayerPrefs.SetInt(borg_time, time);
		PlayerPrefs.Save();
	}
	
	public static int GetBorgTime(){
		int value = PlayerPrefs.GetInt (borg_time, 1);
		return value*60; 
	}
	
	public static int GetPigRunnerLevelGroup(){
		return PlayerPrefs.GetInt(current_pig_level_group, 4);//de levelgroup vai de 1 a 5
	}
	//qt obstaculos ou dificuldade
	public static void SetPigRunnerLevelGroup(int level){
		PlayerPrefs.SetInt(current_pig_level_group, level);//pega do sistema 0, 1 ou 2  
		PlayerPrefs.Save();
	}
	
	public static int GetAnyGroupType(){
		return PlayerPrefs.GetInt(any_group_type, 3);
	}
	
	public static void SetAnyGroupType(int value){
		PlayerPrefs.SetInt (any_group_type, value);
		PlayerPrefs.Save();
	}
	
	public static int GetPigTutorial(){
		return PlayerPrefs.GetInt (pig_tutorial, 1);//0 default 1 elevaçoes
	}
	
	public static void SetPigTutorial(int value){
		PlayerPrefs.SetInt (pig_tutorial, value);
		PlayerPrefs.Save();
	}
	
	public static int GetPigRunnerGroupType(){
		int value = PlayerPrefs.GetInt (current_pig_group_type, 3);
		return value;
	}
	
	public static void SetPigRunnerGroupType(int level){
		PlayerPrefs.SetInt(current_pig_group_type, level);
		PlayerPrefs.Save();
	}
	
	public static void SetLateralAmountAnalog(float amount){
		PlayerPrefs.SetFloat(lateralAmountAnalog, amount);
		PlayerPrefs.Save();
	}
	
	public static void SetLateralAmountDigital(float amount){
		PlayerPrefs.SetFloat(lateralAmountDigital, amount);
		PlayerPrefs.Save();
	}
	
	public static void SetMoveHandSensibility(float amount){
		PlayerPrefs.SetFloat (moveHandsensibility, amount);
		PlayerPrefs.Save();
	}
	
	public static void SetMovingSideAmount(float amount){
		PlayerPrefs.SetFloat (movingSideAmount, amount);
		PlayerPrefs.Save();
	}
	
	public static void SetJumpAmountNeeded(float amount){
		PlayerPrefs.SetFloat (jumpAmountNeeded, amount);
		PlayerPrefs.Save();
	}
	
	public static void SetRollAmountNeeded(float amount){
		PlayerPrefs.SetFloat (rollAmountNeeded, amount);
		PlayerPrefs.Save();
	}
	
	public static float GetLateralAmountAnalog(){
		return PlayerPrefs.GetFloat(lateralAmountAnalog, 0.035f);
	}
	
	public static float GetLateralAmountDigital(){
		return PlayerPrefs.GetFloat(lateralAmountDigital, 0.035f);
	}
	
	public static float GetHandSensibility(){
		return PlayerPrefs.GetFloat(moveHandsensibility, 0.60f);
	}
	
	public static float GetMovingSideAmount(){
		return PlayerPrefs.GetFloat(movingSideAmount, 0.15f);
	}
	
	public static float GetJumpAmountNeeded(){
		return PlayerPrefs.GetFloat(jumpAmountNeeded, 0.27f);
	}
	
	public static float GetRollAmountNeeded(){
		return PlayerPrefs.GetFloat(rollAmountNeeded, 1.55f);
	}
	//PlayerPrefsManager.SetAllowSquat((string)thing["habilitarAgachar"].b);
	public static void SetAllowSquat(string squat)
	{
		PlayerPrefs.SetString(habilitarAgachar, squat);
		PlayerPrefs.Save();
	}
	public static string GetAllowSquat()
	{
		return PlayerPrefs.GetString(habilitarAgachar, "true");//false
	}
	//SetAllowJump((string)thing["habilitarPulo"].b);
	public static void SetAllowJump(string pulo)
	{
		PlayerPrefs.SetString(habilitarPulo, "pulo");
		PlayerPrefs.Save();
	}
	public static string GetAllowJump()
	{
		return PlayerPrefs.GetString(habilitarPulo, "true");//false
	}
	public static void SetQtdObstacles(int qts_obs)
	{
		PlayerPrefs.SetInt(qtdObstaculos, qts_obs);
		PlayerPrefs.Save();
	}
	public static int GetQtObstacles()
	{
		int value = PlayerPrefs.GetInt(qtdObstaculos, 10);//false
		return value;
	}
	
	
	//PlayerPrefsManager.SetAllowJump((string)thing["habilitarPulo"].b);
	//PlayerPrefsManager.SetQtdObstacles((int)thing["qtdObstaculos"].n);
	//
	
	//========SET ID PERFIL GAMES============
	//0 = false, 1 == true
	public static void SetIdPerfilPigrunner(int value)
	{
		PlayerPrefs.SetInt(idperfil_pig, value);
		PlayerPrefs.Save();
	}
	public static int GetIdPerfilPigrunner(){
		int value = PlayerPrefs.GetInt(idperfil_pig, 0);
		return value;//PlayerPrefs.GetInt(idperfil_pig, 0);//0 pernas 1 braço
	}
	
	public static void SetIdPerfilBridge(int value)
	{
		PlayerPrefs.SetInt(idperfil_bridge, value);
		PlayerPrefs.Save();
	}
	public static int GetIdPerfilBridge(){
		return PlayerPrefs.GetInt(idperfil_bridge, 0);//nao precisa, nao existe gesto configuravel
	}
	
	public static void SetIdPerfilGK(int value)
	{
		PlayerPrefs.SetInt(idperfil_gk, value);
		PlayerPrefs.Save();//nao precisa
	}
	public static int GetIdPerfilGK(){
		return PlayerPrefs.GetInt(idperfil_gk, 0);
	}
	
	public static void SetIdPerfilFishing(int value)
	{
		PlayerPrefs.SetInt(idperfil_fishing, value);
		PlayerPrefs.Save();//nao precisa
	}
	public static int GetIdPerfilFishing(){
		return PlayerPrefs.GetInt(idperfil_fishing, 0);
	}
	
	public static void SetIdPerfilThrow(int value)
	{
		PlayerPrefs.SetInt(idperfil_throw, value);
		PlayerPrefs.Save();
	}
	public static int GetIdPerfilThrow(){
		return PlayerPrefs.GetInt(idperfil_throw, 0);
	}
	
	public static void SetIdPerfilSup(int value)
	{
		PlayerPrefs.SetInt(idperfil_sup, value);
		PlayerPrefs.Save();
	}
	public static int GetIdPerfilSup(){
		return PlayerPrefs.GetInt(idperfil_sup, 1);
	}
	
	//======setup movewith hands by game
	
	/*public static int GetMoveWithHands_PigRunner()
	{
		return PlayerPrefs.GetInt(move_with_hands_on, 0);//0 => hands = false e sides =true |====| 1=> hands = true e sides = false
	}
	public static int GetMoveToSides()
	{
		return PlayerPrefs.GetInt(move_to_sides, 1);//0 = false, 1 == true
	}
	//OS DOIS NUNCA PODEM SER IGUAIS
	public static void SetMoveWithHands_True_PigRunner()
	{
		PlayerPrefs.SetInt (move_with_hands_on, 1);
		PlayerPrefs.SetInt (move_to_sides, 0);
		PlayerPrefs.Save();
	}
	public static void SetMoveWithHands_False_PigRunner()
	{
		PlayerPrefs.SetInt (move_with_hands_on, 0);
		PlayerPrefs.SetInt (move_to_sides, 1);
		PlayerPrefs.Save();
	}*/
	//=================Sup=================
	/*public static int GetMoveWithHands_Sup()
	{
		return PlayerPrefs.GetInt(move_with_hands_on, 1);//0 => hands = false e sides =true |====| 1=> hands = true e sides = false
	}
	//OS DOIS NUNCA PODEM SER IGUAIS
	public static void SetMoveWithHands_True_Sup()
	{
		PlayerPrefs.SetInt (move_with_hands_on, 1);
		PlayerPrefs.SetInt (move_to_sides, 0);
		PlayerPrefs.Save();
	}
	public static void SetMoveWithHands_False_Sup()
	{
		PlayerPrefs.SetInt (move_with_hands_on, 0);
		PlayerPrefs.SetInt (move_to_sides, 1);
		PlayerPrefs.Save();
	}*/
	
	//=================================================
	public static void SetInitialMapSpeed(float value){
		PlayerPrefs.SetFloat(initialMapSpeed, value);//7, 8, 9
		PlayerPrefs.Save();
	}
	
	public static void SetMapSpeedLimit(float value){
		PlayerPrefs.SetFloat(mapSpeedLimit, value);
		PlayerPrefs.Save();
	}
	
	public static void SetIncrementSpeedInterval(float value){
		PlayerPrefs.SetFloat(incrementSpeedInterval, value);
		PlayerPrefs.Save();
	}
	
	public static void SetIncrementSpeedValue(float value){
		PlayerPrefs.SetFloat(incrementSpeedValue, value);
		PlayerPrefs.Save();
	}

	public static float GetInitialMapSpeed(){
		float value = PlayerPrefs.GetFloat(initialMapSpeed, 8);//6,8 e 10
		return value;
	}
	
	public static float GetMapSpeedLimit(){
		return PlayerPrefs.GetFloat(mapSpeedLimit, 20);//0=15  1=20  2=25
	}
	
	public static float GetIncrementSpeedInterval(){
		return PlayerPrefs.GetFloat(incrementSpeedInterval, 6);
	}
	
	public static float GetIncrementSpeedValue(){
		return PlayerPrefs.GetFloat(incrementSpeedValue, 0.02f);
	}
	
	public static int GetPlayerDeficiency(){
		return PlayerPrefs.GetInt(playerDeficiency, 0);
	}
	
	public static void SetPlayerDeficiency(int value){
		PlayerPrefs.SetInt(playerDeficiency, value);
		PlayerPrefs.Save();
	}
	//
	
	public static string GetMiniGames(){
		return PlayerPrefs.GetString (playerMiniGames);
	}
	
	public static void SetMiniGames(string value){
		PlayerPrefs.SetString(playerMiniGames, value);
		PlayerPrefs.Save();
	}
	
	public static string GetClinicToken() {
		return PlayerPrefs.GetString(clinicToken, "0");
	}
	
	public static void SetClinicToken(string token) {
		PlayerPrefs.SetString(clinicToken, token);
		PlayerPrefs.Save();
	}

	public static string GetMainToken() {
		return PlayerPrefs.GetString(mainToken, "0");
	}
	
	public static void SetMainToken(string token) {
		PlayerPrefs.SetString(mainToken, token);
		PlayerPrefs.Save();
	}

	public static string GetPhysiotherapistToken() {
		return PlayerPrefs.GetString(physiotherapistToken, "0");
	}
	
	public static void SetPhysiotherapistToken(string token) {
		PlayerPrefs.SetString(physiotherapistToken, token);
		PlayerPrefs.Save();
	}
	
	public static string GetPlayerToken() {
		return PlayerPrefs.GetString(playerToken, "0");
	}
	
	public static void SetPlayerToken(string token) {
		PlayerPrefs.SetString(playerToken, token);
		PlayerPrefs.Save();
	}
	
	
	public static string GetMiniGamesIndex(){
		return PlayerPrefs.GetString (playerMiniGamesIndex,"0");
	}
	
	public static void SetMiniGamesIndex(string value){
		PlayerPrefs.SetString(playerMiniGamesIndex, value);
		PlayerPrefs.Save();
	}
	
	
	public static int GetPlayerID(){
		return PlayerPrefs.GetInt (player_id, 0);
	}
	
	public static void SetPlayerID(int value){
		PlayerPrefs.SetInt(player_id, value);
		PlayerPrefs.Save();
	}
	public static string GetPlayerName(){
		return PlayerPrefs.GetString (player_name, " ");
	}
	
	public static void SetPlayerName(string name){
		PlayerPrefs.SetString(player_name, name);
		PlayerPrefs.Save();
	}
	
	public static int GetClinicID(){
		return PlayerPrefs.GetInt (clinic_id, 0);
	}
	
	public static void SetClinicID(int value){
		PlayerPrefs.SetInt(clinic_id, value);
		PlayerPrefs.Save();
	}
	
	
	public static int GetPhysioID(){
		return PlayerPrefs.GetInt (physio_id, 0);
	}
	
	public static void SetPhysioID(int value){
		PlayerPrefs.SetInt(physio_id, value);
		PlayerPrefs.Save();
	}
	
	public static int GetCurrentGameID(){
		return PlayerPrefs.GetInt (current_game_id, 5);
	}
	
	public static void SetCurrentGameID(int value){
		PlayerPrefs.SetInt(current_game_id, value);
		PlayerPrefs.Save();
	}
	
	public static int GetCurrentMatch()
	{
		return PlayerPrefs.GetInt(current_match, 0);
	}
	
	public static void SetCurrentMatch(int value)
	{
		PlayerPrefs.SetInt(current_match, value);
		PlayerPrefs.Save();
	}
	
	public static void SetNumberOfLifes(int value)
	{
		PlayerPrefs.SetInt(number_of_lifes, value);
		PlayerPrefs.Save();
	}
	
	public static int GetNumberOfLifes()
	{
		return PlayerPrefs.GetInt(number_of_lifes, 3);
	}
	//fishing
	public static void SetFishingQuantity(int qt)
	{
		PlayerPrefs.SetInt(fishingqt, qt);
		PlayerPrefs.Save();
	}
	
	public static int GetFishingQuantity()
	{
		return PlayerPrefs.GetInt(fishingqt, 5);
	}
	//
	public static void SetFishingDifficult(int dif)
	{
		PlayerPrefs.SetInt(fishingqt, dif);
		PlayerPrefs.Save();
	}
	
	public static int GetFishingDifficult()
	{
		return PlayerPrefs.GetInt(fishingDifficult, 1);//   0...5
	}
	//
	public static void SetPullingStrengh(float strengh)
	{
		PlayerPrefs.SetFloat(pullingStrengh, strengh);
		PlayerPrefs.Save();
	}
	public static float GetPullingStrengh()
	{
		return PlayerPrefs.GetFloat(big_pullingStrengh, 40);
	}
	public static void SetBigPullingStrengh(float bigstrengh)
	{
		PlayerPrefs.SetFloat(big_pullingStrengh, bigstrengh);
		PlayerPrefs.Save();
	}
	public static float GetBigPullingStrengh()
	{
		return PlayerPrefs.GetFloat(big_pullingStrengh, 100);
	}
	public static void SetNumberOfBaits(int baits)
	{
		PlayerPrefs.SetInt(number_of_baits, baits);
		PlayerPrefs.Save();
	}
	public static int GetNumberofBaits()
	{
		return PlayerPrefs.GetInt(number_of_baits, 10);
	}
	public static void SetFishMovementSetup(int gesture)
	{
		PlayerPrefs.SetInt(fishing_setup_movements, gesture);
		PlayerPrefs.Save ();
	}
	public static int GetFishMovementSetup()
	{
		return PlayerPrefs.GetInt(fishing_setup_movements, 0);//0 default e 1 elevaçao
	}
	//=========SUP=============
	public static void SetRemaingTime(float time)
	{
		float value = time * 60;
		PlayerPrefs.SetFloat(global_time, value);
		PlayerPrefs.Save();
	}
	public static float GetRemaingTime()
	{
		return PlayerPrefs.GetFloat(global_time, 180);
	}
	//configurar gesto padrao ou variaçao
	/*public static void SetSupSetuMovement(int gesture)
	{
		PlayerPrefs.SetInt(sup_setup_movements, gesture); //0 default e 1 elevaçao
		PlayerPrefs.Save();
	}
	public static int GetSupSetupMovement()
	{
		return PlayerPrefs.GetInt(sup_setup_movements, 1);//0 default e 1 elevaçao. No sup o preferivel e elevaçao
	}*/
	//=========================
	public static void SetPresentationFirstTime(int sit)
	{
		// 0 = false
		// 1 = true
		PlayerPrefs.SetInt(presentation_first_time, sit);
		PlayerPrefs.Save();
	}
	
	public static int IsPresentationFirstTime()
	{
		return PlayerPrefs.GetInt(presentation_first_time, 1);
	}
	
	
	public static void SetGamesFirstTime(string s)
	{
		//string com situaçao dos games separados por "@".
		//pig,gk,bridge, throw,sup,fishing
		PlayerPrefs.SetString(games_first_time, s);
		PlayerPrefs.Save();
	}
	
	public static string GetGamesFirstTime()
	{
		return PlayerPrefs.GetString(games_first_time, "1@1@1@1@1@1");
	}
	
	public static void SetBorgFirstTime(int b)
	{
		//1 - true
		//2 - false
		PlayerPrefs.SetInt(borg_first_time, b);
		PlayerPrefs.Save();
	}
	
	public static int GetBorgFirstTime()
	{
		return PlayerPrefs.GetInt(borg_first_time, 1);
	}
	
	public static void SetGameOverFirstTime(int g)
	{
		//1 - true
		//2 - false
		PlayerPrefs.SetInt(gameOver_first_time, g);
		PlayerPrefs.Save();
	}
	
	public static int GetGameOverFirstTime()
	{
		return PlayerPrefs.GetInt(gameOver_first_time, 1);
	}


	public static void SetCircleOn(int b)
	{
		//1 - true
		//0 - false
		PlayerPrefs.SetInt(circle_on, b);
		PlayerPrefs.Save();
	}

	public static int GetIsCircleOn()
	{
		return PlayerPrefs.GetInt(circle_on, 1);
	}

}