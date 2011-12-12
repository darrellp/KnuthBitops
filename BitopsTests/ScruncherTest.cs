using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KnuthBitops;

namespace BitopsTests
{
	[TestClass]
	public class ScruncherTest
	{
		[TestMethod]
		public void TestScruncher()
		{
			Scruncher scruncher = new Scruncher(0xf0);
			Assert.AreEqual(0xaUL, scruncher.Scrunch(0xa0));
			scruncher = new Scruncher(0xf0f0);
			Assert.AreEqual(0xbdUL, scruncher.Scrunch(0xabcde));
		}
	}
}
