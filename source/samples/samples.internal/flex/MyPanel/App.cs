using System;
using mx.core;
using mx.controls;
using mx.containers;
using Avm;

class MyPanel : Panel
{
	public MyPanel()
	{
		this.x = 10;
		this.y = 10;
		this.width = 500;
		this.height = 500;
	}
	
}

class MyApp : Application
{
	private MyPanel _panel;

	public MyApp()
	{
		_panel = new MyPanel();

		addChild(_panel);
	}
}