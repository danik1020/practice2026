using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using task13;
using Xunit;

namespace task13.Tests
{
    public class StudentJsonTests
    {
        private readonly StudJsonSer _service = new();

        private Student CreateSampleStudent() => new()
        {
            FirstName = "Иван",
            LastName = null,
            BirthDate = new DateTime(2000, 5, 15),
            Grades = new List<Subject> { new() { Name = "Математика", Grade = 5 } }
        };

        [Fact]
        public void Serial_WhenLastNameIsNull_OmitsField()
        {
            var student = CreateSampleStudent();
            string json = _service.Serial(student);

            Assert.DoesNotContain("LastName", json);
        }

        [Fact]
        public void Serial_BirthDate_FormattedAsDdMmYyyy()
        {
            var student = CreateSampleStudent();
            string json = _service.Serial(student);

            Assert.Contains("15.05.2000", json);
        }

        [Fact]
        public void Deserial_ValidJson_ReturnsStudent()
        {
            string json = @"{
                ""FirstName"": ""Мария"",
                ""BirthDate"": ""01.01.2001"",
                ""Grades"": [{""subName"": ""Химия"", ""Grade"": 5}]
            }";

            var student = _service.Deserial(json);

            Assert.NotNull(student);
            Assert.Equal("Мария", student!.FirstName);
            Assert.Equal(new DateTime(2001, 1, 1), student.BirthDate);
        }

        [Fact]
        public void Deserial_InvalidDateFormat_ThrowsJsonException()
        {
            string json = @"{""FirstName"": ""Пётр"", ""BirthDate"": ""2000-05-15""}";

            Assert.Throws<JsonException>(() => _service.Deserial(json));
        }

        [Fact]
        public void Deserial_MissingRequiredField_ThrowsJsonException()
        {
            string json = @"{""BirthDate"": ""15.05.2000""}";

            Assert.Throws<JsonException>(() => _service.Deserial(json));
        }

        [Fact]
        public void SavefileAndLoadFromFile_RoundTrip_PreservesData()
        {
            var student = CreateSampleStudent();
            student.LastName = "Тестов";
            string tempFile = Path.Combine(Path.GetTempPath(), $"student_{Guid.NewGuid()}.json");

            try
            {
                _service.Savefile(student, tempFile);
                Assert.True(File.Exists(tempFile));

                var loaded = _service.LoadFromFile(tempFile);

                Assert.NotNull(loaded);
                Assert.Equal("Иван", loaded!.FirstName);
                Assert.Equal("Тестов", loaded.LastName);
                Assert.Equal(new DateTime(2000, 5, 15), loaded.BirthDate);
            }
            finally
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }
    }
}
