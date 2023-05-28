using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FillGameSelection {

	public List<MiniGame> miniGamesList = new List<MiniGame>() {
		new MiniGame("Bridge", new List<CategoryType>(1){CategoryType.Torso}),
		new MiniGame("Sandbox", new List<CategoryType>(1){CategoryType.Arms}),
		new MiniGame("Soccer", new List<CategoryType>(1){CategoryType.Legs}),
		new MiniGame("Plane", new List<CategoryType>(1){CategoryType.Legs}),
		new MiniGame("PigRunner", new List<CategoryType>(1){CategoryType.Legs}),
		new MiniGame("Teste", new List<CategoryType>(1){CategoryType.Legs})};
	
}
