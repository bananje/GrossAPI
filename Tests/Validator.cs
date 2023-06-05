using GrossAPI.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    internal class Validator
    {
        [TestMethod]
        public void PasswordHasherVerifier_SimplePassword_ReturnedTrue()
        {
            //arrange
            string password = "2344234vg!df";
            string hash = Crypter.HashPassword(password);
            bool expected = true;
            //act
            bool verify = Crypter.VerifyPassword(password, hash);
            //assert
            Assert.AreEqual(expected, verify);
        }
    }
}
