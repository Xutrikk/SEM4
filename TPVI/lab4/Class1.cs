using System.Text.Json;

namespace DAL004;

public interface IRepository : IDisposable
{
    string BasePath { get; }
    Celebrity[] GetAllCelebrities();
    Celebrity? GetCelebrityById(int id);
    Celebrity[] GetCelebritiesBySurename(string surename);
    string? GetPhotoPathById(int id);


    int? addCelebrity(Celebrity celebrity);
    bool delCelebrityByID(int id);
    bool updCelebrityById(int id, Celebrity celebrity);
    int SaveChanges();
}

public record Celebrity(int Id, string Firstname, string Surname, string PhotoPath);

public class Repository : IRepository
{
    public static string JSONFileName = "Celebrities.json";
    public string BasePath { get; }
    public string filePath { get; }
    private List<Celebrity> _celebrities;

    public Repository(string dirPath)
    {
        this.BasePath = Path.Combine(Directory.GetCurrentDirectory(), dirPath);
        this.filePath = Path.Combine(BasePath, JSONFileName);

        if (!Directory.Exists(this.BasePath))
        {
            Directory.CreateDirectory(this.BasePath);
        }
        if (!File.Exists(this.filePath))
        {
            File.WriteAllText(this.filePath, "[]");
        }
        LoadData();
    }

    private void LoadData()
    {
        var json = File.ReadAllText(filePath);
        _celebrities = JsonSerializer.Deserialize<List<Celebrity>>(json) ?? new List<Celebrity>();
    }

    public Celebrity[] GetAllCelebrities() => _celebrities.ToArray();

    public Celebrity? GetCelebrityById(int id) => _celebrities.FirstOrDefault(c => c.Id == id);

    public Celebrity[] GetCelebritiesBySurename(string surename) =>
        _celebrities.Where(c => c.Surname.Equals(surename, StringComparison.OrdinalIgnoreCase)).ToArray();

    public string? GetPhotoPathById(int id) => GetCelebrityById(id)?.PhotoPath;


    public int? addCelebrity(Celebrity celebrity)
    {
        LoadData(); // Загружаем актуальные данные перед изменениями

        int newId = _celebrities.Count > 0 ? _celebrities.Max(c => c.Id) + 1 : 1;
        Celebrity newCelebrity = new Celebrity(newId, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);

        _celebrities.Add(newCelebrity);
        SaveChanges();

        return newId;
    }
 
    public bool delCelebrityByID(int id)
    {
        var celebrity = _celebrities.FirstOrDefault(c => c.Id == id);
        if (celebrity == null) return false;

        _celebrities.Remove(celebrity);
        SaveChanges();
        return true;
    }

    public bool updCelebrityById(int id, Celebrity newCelebrity)
    {
        var index = _celebrities.FindIndex(c => c.Id == id);
        if (index == -1) return false;

        _celebrities[index] = new Celebrity(id, newCelebrity.Firstname, newCelebrity.Surname, newCelebrity.PhotoPath);
        SaveChanges();
        return true;
    }

    public int SaveChanges()
    {
        var json = JsonSerializer.Serialize(_celebrities, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
        return _celebrities.Count;
    }

    public static Repository Create(string dir) => new Repository(dir);
    public void Dispose() { }
}
 