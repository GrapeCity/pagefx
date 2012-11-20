// ActionScript file
package DataDynamics
{
	public class Person
	{
		public var Name:String;
		public var Age:int;
		public var BirthDate:Date;
		
		public function Person(name:String="", age:int=0):void
		{
			Name = name;
			Age = age;
		}
		
		public function setBirthdate(d:Date):void
		{
			BirthDate = d;
		}
	}
}
