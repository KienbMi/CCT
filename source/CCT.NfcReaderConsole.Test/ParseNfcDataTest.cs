using System;
using CCT.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCT.NfcReaderConsole.Test
{
    [TestClass]
    public class ParseNfcDataTest
    {
        [TestMethod]
        public void T01_ParseNfcString_Ok()
        {
            //Arrange
            string expectedFirstName = "Max";
            string expectedLastName = "Mustermann";
            string expectedPhoneNumber = "066412345667";
            string nfcDataContent = $"{expectedFirstName};{expectedLastName};{expectedPhoneNumber};";

            //Act
            Person person = FunctionsCCT.ParseNfcDataToPerson(nfcDataContent);

            //Assert
            Assert.IsNotNull(person);
            Assert.AreEqual(expectedFirstName, person.FirstName);
            Assert.AreEqual(expectedLastName, person.LastName);
            Assert.AreEqual(expectedPhoneNumber, person.PhoneNumber);
            Assert.AreEqual(DateTime.Now.Date, person.RecordTime.Date);
            Assert.AreEqual(DateTime.Now.Hour, person.RecordTime.Hour);
        }

        [TestMethod]
        public void T02_ParseNfcString_notOk_NULL_String()
        {
            //Arrange
            string nfcDataContent = null;

            //Act
            Person person = FunctionsCCT.ParseNfcDataToPerson(nfcDataContent);

            //Assert
            Assert.IsNull(person);
        }

        [TestMethod]
        public void T03_ParseNfcString_notOk_missingFirstName()
        {
            //Arrange
            //string expectedFirstName = "Max";
            string expectedLastName = "Mustermann";
            string expectedPhoneNumber = "066412345667";
            string nfcDataContent = $"{expectedLastName};{expectedPhoneNumber};";

            //Act
            Person person = FunctionsCCT.ParseNfcDataToPerson(nfcDataContent);

            //Assert
            Assert.IsNull(person);
        }

        [TestMethod]
        public void T04_ParseNfcString_notOk_missingLastName()
        {
            //Arrange
            string expectedFirstName = "Max";
            //string expectedLastName = "Mustermann";
            string expectedPhoneNumber = "066412345667";
            string nfcDataContent = $"{expectedFirstName};{expectedPhoneNumber};";

            //Act
            Person person = FunctionsCCT.ParseNfcDataToPerson(nfcDataContent);

            //Assert
            Assert.IsNull(person);
        }

        [TestMethod]
        public void T05_ParseNfcString_notOk_missingPhone()
        {
            //Arrange
            string expectedFirstName = "Max";
            string expectedLastName = "Mustermann";
            //string expectedPhoneNumber = "066412345667";
            string nfcDataContent = $"{expectedFirstName};{expectedLastName};";

            //Act
            Person person = FunctionsCCT.ParseNfcDataToPerson(nfcDataContent);

            //Assert
            Assert.IsNull(person);
        }

        [TestMethod]
        public void T06_ParseNfcString_notOk_missingLastSemicolon()
        {
            //Arrange
            string expectedFirstName = "Max";
            string expectedLastName = "Mustermann";
            string expectedPhoneNumber = "066412345667";
            string nfcDataContent = $"{expectedFirstName};{expectedLastName};{expectedPhoneNumber}";

            //Act
            Person person = FunctionsCCT.ParseNfcDataToPerson(nfcDataContent);

            //Assert
            Assert.IsNull(person);
        }
    }
}
