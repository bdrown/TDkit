using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using TDkit.Chemistry;

namespace TDkitTest.Chemistry
{
    [TestClass]
    public class ElementTests
    {
        Element nitrogen;
        Element iron;

        /// <summary>
        /// Most test will use nitrogen and iron as examples since they
        /// represent simple and complex cases, respectively.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            List<Isotope> n_dist = new List<Isotope>()
            {
                new Isotope(7, 14, 14.00307400443, 0.99636),
                new Isotope(7, 15, 15.00010889888, 0.00364)
            };
            nitrogen = new Element("N", 5, n_dist);

            List<Isotope> fe_dist = new List<Isotope>()
            {
                new Isotope(26, 54, 53.93960899, 0.05845),
                new Isotope(26, 56, 55.93493633, 0.91754),
                new Isotope(26, 57, 56.93539284, 0.02119),
                new Isotope(26, 58, 57.93327443, 0.00282)
            };
            iron = new Element("Fe", 26, fe_dist);
        }

        /// <summary>
        /// Test that ensures test suite is configured correctly.
        /// </summary>
        [TestMethod]
        public void Alway_True()
        {
            Assert.IsTrue(true);
        }

        /// <summary>
        /// The calculation of the monoisotopic mass should provide the most
        /// naturally abundant isotope mass.
        /// </summary>
        [TestMethod]
        public void Monoisotopic_Mass_Is_Most_Abundant()
        {
            // The most abundant isotope of nitrogen is also its smallest
            Assert.AreEqual(14.00307400443, nitrogen.MonoisotopicMass());

            // The most abundant isotope of iron is not its smallest
            Assert.AreEqual(55.93493633, iron.MonoisotopicMass());
        }

        /// <summary>
        /// The calculation of the average mass should give the weighted average 
        /// and shouldn't rely on all the weights adding to 1.
        /// </summary>
        [TestMethod]
        public void Average_Mass_Is_Weighted()
        {
            // Iron
            Assert.AreEqual(55.8451444338659, iron.AverageMass(), 0.0000000000001);

            // TODO: implement test in which weights sum to something other than 1.
        }
        
        /// <summary>
        /// Giving a valid symbol should return an Element instance that matches
        /// the expected data for that Element. Using carbon as a test case.
        /// </summary>
        [TestMethod]
        public void Valid_Symbol_Gives_Correct_Info_Carbon()
        {
            // Setup
            string expectedSymbol = "C";
            double expectedMonoMass = 12.0;
            Element ele = Element.GetElementFromSymbol(expectedSymbol);

            // Assertions
            Assert.AreEqual<double>(expectedMonoMass, ele.MonoisotopicMass());
            Assert.AreEqual<int>(6, ele.AtomicNumber);
        }

        /// <summary>
        /// Giving a valid symbol should return an Element instance that matches
        /// the expected data for that Element. Using iron as a test case.
        /// </summary>
        [TestMethod]
        public void Valid_Symbol_Give_Correct_Info_Iron()
        {
            // Setup
            string expectedSymbol = "Fe";
            double expectedMonoMass = 55.93493633;
            Element ele = Element.GetElementFromSymbol(expectedSymbol);

            // Assertions
            Assert.AreEqual<double>(expectedMonoMass, ele.MonoisotopicMass());
            Assert.AreEqual<int>(26, ele.AtomicNumber);
        }

        /// <summary>
        /// Symbols that do not correspond to existing elements should raise an exception
        /// </summary>
        [TestMethod]
        public void Invalid_Symbol_Raises_Exception()
        {
            string symbol = "Ci";
            Element ele;
            Assert.ThrowsException<ArgumentException>(() => ele = Element.GetElementFromSymbol(symbol));
        }

        /// <summary>
        /// Symbols that are longer than two characters should raise an exception
        /// </summary>
        [TestMethod]
        public void Long_Symbol_Raises_Exception()
        {
            string symbol = "Car";
            Element ele;
            Assert.ThrowsException<ArgumentException>(() => ele = Element.GetElementFromSymbol(symbol));
        }

        /// <summary>
        /// Providing an empty string as element symbol should raise an exception.
        /// </summary>
        [TestMethod]
        public void Empty_Symbol_Raises_Exception()
        {
            string symbol = "";
            Element ele;
            Assert.ThrowsException<ArgumentException>(() => ele = Element.GetElementFromSymbol(symbol));
        }
    }
}
