using System.Text.Json;
using Notebook.Interfaces;

namespace Notebook.Services;

/// <summary>
/// Реализация хранилища в формате JSON.
/// Демонстрирует полиморфизм через интерфейс IStorage.
/// </summary>
public class JsonStorage<T> : IStorage<T>
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public void Save(IEnumerable<T> items, string path)
    {
        var json = JsonSerializer.Serialize(items, _options);
        File.WriteAllText(path, json);
    }

    public List<T> Load(string path)
    {
        if (!File.Exists(path))
            return new List<T>();

        var json = File.ReadAllText(path);
        if (string.IsNullOrWhiteSpace(json))
            return new List<T>();

        return JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
    }
}