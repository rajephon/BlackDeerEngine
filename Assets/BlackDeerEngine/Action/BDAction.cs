using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDAction {
	private string name = "";
	public BDAction() {

	}
	public BDAction(string name) {
		this.name = name;
	}
	public string Name {
		get { return name; }
		set { this.name = Name; }
	}
	
}
