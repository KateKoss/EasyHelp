using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcApplication1.Models;
using System.Collections.Generic;

namespace MvcApplication1.Tests.Controllers
{
    [TestClass]
    public class UnitTestAlgorithmKmeans
    {
        [TestMethod]
        public void TestMethodMinIndex()
        {
            // Arrange              
            double[] distances = {1, 2, 3, 4, 5};
            int expected = 0;
            Algorithm a = new Algorithm();         

            // Act  
            int actual = a.MinIndex(distances);

            // Assert  
            Assert.AreEqual(expected, actual, 0.001, "Min index not correct");  
        }

        [TestMethod]
        public void TestMethodMinIndex2()
        {
            // Arrange              
            double[] distances = { 5, 2, 3, 4, 1 };
            int expected = 4;
            Algorithm a = new Algorithm();

            // Act  
            int actual = a.MinIndex(distances);

            // Assert  
            Assert.AreEqual(expected, actual, 0.001, "Min index not correct");
        }

        [TestMethod]
        public void TestMethodNormalize()
        {            
            // Arrange 
            Algorithm a = new Algorithm();
            List<DataPoint> rawDataToCluster = new List<DataPoint>();
                        
            a.setN(2);

            DataPoint p = new DataPoint(2);
            p.pointId = "0";
            p.a[0] = 1;
            p.a[1] = 2;
            rawDataToCluster.Add(p);
            p = new DataPoint(2);
            p.pointId = "1";
            p.a[0] = 2;
            p.a[1] = 3;
            rawDataToCluster.Add(p);
            a.setRawDataToCluster(rawDataToCluster);            
            
            List<DataPoint> expected = new List<DataPoint>();
            p = new DataPoint(2);
            
            p.a[0] = 0;
            p.a[1] = 0;
            expected.Add(p);
            p = new DataPoint(2);
            
            p.a[0] = 2;
            p.a[1] = 3;
            expected.Add(p);

            // Act  
            a.NormalizeData();

            // Assert 
            List<DataPoint> actual = new List<DataPoint>();
            actual = a.getNormalizedDataToCluster();
            Assert.AreEqual(expected[0].a[0], actual[0].a[0], 0.001, "Normalize not corect");
        }
    }
}
