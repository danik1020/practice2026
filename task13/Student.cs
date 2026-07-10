using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.Json;
using System.IO;




namespace task13{
public class CustomDateConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (DateTime.TryParseExact(reader.GetString(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {return result;}
        throw new JsonException($"Неверный формат даты");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("dd.MM.yyyy"));
    }
}


public class Subject
{
  [JsonPropertyName("subName")]
  public string Name {get; set; }

  public int Grade {get; set; }
}

public class Student
{
    [JsonRequired]
    public string FirstName { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LastName { get; set; }
    [JsonConverter(typeof(CustomDateConverter))]
    public DateTime BirthDate { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<Subject>? Grades { get; set; }
}

public class StudJsonSer
  {
    public string Serial(Student student)
    {
      if (student == null )throw new ArgumentNullException(nameof(student));
      return JsonSerializer.Serialize(student);
    }

    public Student? Deserial(string json)
    {
      if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("json не может быть пустым");

            return JsonSerializer.Deserialize<Student>(json);
    }

    public void Savefile(Student student, string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllText(filePath, Serial(student));
        }

    public Student? LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл не найден", filePath);

        return Deserial(File.ReadAllText(filePath));
    }
  }

}
