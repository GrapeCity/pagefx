using System;

class Document
{
	public Guid Id;
}

class Loader
{
	private object _documentProperies;

	public Document Document { get; set; }

	public object DocumentProperties
	{
		get
		{
			if (_documentProperies == null && Document != null)
			{
				Avm.Object properties = new Avm.Object();
				
				properties.SetProperty("DocumentId", Document.Id.ToString());
				
				_documentProperies = properties;
			}

			return _documentProperies;
		}
	}
}

class Test
{
	static void Main()
	{
		var doc = new Document();
		var loader = new Loader { Document = doc };
		var id = ((Avm.Object)loader.DocumentProperties).GetProperty("DocumentId");
		Console.WriteLine(id);
		Console.WriteLine("<%END%>");
	}
}