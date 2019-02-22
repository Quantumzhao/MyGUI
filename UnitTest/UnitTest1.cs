using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			MyGUI.Utilities.Point point = new MyGUI.Utilities.Point(1, 1);
			MyGUI.Utilities.Point point2 = point;
			point.X = 2;
			Assert.IsTrue(point2.X == 1);
		}
	}
}
