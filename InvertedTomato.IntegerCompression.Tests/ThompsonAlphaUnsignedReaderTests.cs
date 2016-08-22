﻿using InvertedTomato.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace InvertedTomato.IntegerCompression.Tests {
    [TestClass]
    public class ThompsonAlphaUnsignedReaderTests {
        private ulong Read(string input) {
            return ThompsonAlphaUnsignedReader.ReadAll(ByteArrayUtility.ParseBinaryString(input)).First();
        }

        [TestMethod]
        public void Read_0() {
            Assert.AreEqual((ulong)0, Read("00000000"));
        }
        [TestMethod]
        public void Read_1() {
            Assert.AreEqual((ulong)1, Read("00000100"));
        }
        [TestMethod]
        public void Read_2() {
            Assert.AreEqual((ulong)2, Read("00000110"));
        }
        [TestMethod]
        public void Read_3() {
            Assert.AreEqual((ulong)3, Read("00001000"));
        }
        [TestMethod]
        public void Read_4() {
            Assert.AreEqual((ulong)4, Read("00001001"));
        }
        [TestMethod]
        public void Read_5() {
            Assert.AreEqual((ulong)5, Read("00001010"));
        }
        [TestMethod]
        public void Read_6() {
            Assert.AreEqual((ulong)6, Read("00001011"));
        }
        [TestMethod]
        public void Read_7() {
            Assert.AreEqual((ulong)7, Read("00001100 00000000"));
        }
        [TestMethod]
        public void Read_8() {
            Assert.AreEqual((ulong)8, Read("00001100 10000000"));
        }
        [TestMethod]
        public void Read_9() {
            Assert.AreEqual((ulong)9, Read("00001101 00000000"));
        }
        [TestMethod]
        public void Read_10() {
            Assert.AreEqual((ulong)10, Read("00001101 10000000"));
        }
        [TestMethod]
        public void Read_11() {
            Assert.AreEqual((ulong)11, Read("00001110 00000000"));
        }
        [TestMethod]
        public void Read_12() {
            Assert.AreEqual((ulong)12, Read("00001110 10000000"));
        }
        [TestMethod]
        public void Read_13() {
            Assert.AreEqual((ulong)13, Read("00001111 00000000"));
        }
        [TestMethod]
        public void Read_14() {
            Assert.AreEqual((ulong)14, Read("00001111 10000000"));
        }
        [TestMethod]
        public void Read_15() {
            Assert.AreEqual((ulong)15, Read("00010000 00000000"));
        }
        [TestMethod]
        public void Read_16() {
            Assert.AreEqual((ulong)16, Read("00010000 01000000"));
        }

        [TestMethod]
        public void Read_1_1_1() {
            var result = ThompsonAlphaUnsignedReader.ReadAll(ByteArrayUtility.ParseBinaryString("00000100 00001000 00010000"));
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual((ulong)1, result.ElementAt(0));
            Assert.AreEqual((ulong)1, result.ElementAt(1));
            Assert.AreEqual((ulong)1, result.ElementAt(2));
        }
        [TestMethod]
        public void Read_4_4_4() {
            var result = ThompsonAlphaUnsignedReader.ReadAll(ByteArrayUtility.ParseBinaryString("00001001 00001001 00001001"));
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual((ulong)4, result.ElementAt(0));
            Assert.AreEqual((ulong)4, result.ElementAt(1));
            Assert.AreEqual((ulong)4, result.ElementAt(2));
        }
    }
}
