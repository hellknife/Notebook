using Notebook.Models;
using Notebook.Services;

namespace Notebook;

class Program
{
    private static NotebookService _notebook = null!;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        var contactStorage = new JsonStorage<Contact>();
        var groupStorage = new JsonStorage<Group>();
        _notebook = new NotebookService(contactStorage, groupStorage);

        _notebook.Load();
        SeedDemoDataIfEmpty();

        while (true)
        {
            ShowMenu();
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1": ShowAllContacts(); break;
                    case "2": AddContact(); break;
                    case "3": RemoveContact(); break;
                    case "4": SearchContacts(); break;
                    case "5": ShowGroups(); break;
                    case "6": AddGroup(); break;
                    case "7": ShowContactsByGroup(); break;
                    case "8": _notebook.Save(); Console.WriteLine("Сохранено."); break;
                    case "0": _notebook.Save(); return;
                    default: Console.WriteLine("Неверный пункт меню."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("=== ЗАПИСНАЯ КНИЖКА ===");
        Console.WriteLine("1. Показать все контакты");
        Console.WriteLine("2. Добавить контакт");
        Console.WriteLine("3. Удалить контакт");
        Console.WriteLine("4. Поиск контактов");
        Console.WriteLine("5. Показать группы");
        Console.WriteLine("6. Добавить группу");
        Console.WriteLine("7. Показать контакты по группе");
        Console.WriteLine("8. Сохранить");
        Console.WriteLine("0. Выход");
        Console.Write("\nВыберите пункт: ");
    }

    static void ShowAllContacts()
    {
        if (_notebook.Contacts.Count == 0)
        {
            Console.WriteLine("Контактов нет.");
            return;
        }

        Console.WriteLine($"\nВсего контактов: {_notebook.Contacts.Count}\n");
        foreach (var c in _notebook.Contacts)
            Console.WriteLine("• " + c.GetInfo());
    }

    static void AddContact()
    {
        Console.Write("ФИО: ");
        var name = Console.ReadLine() ?? "";
        Console.Write("Телефон: ");
        var phone = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        var email = Console.ReadLine() ?? "";
        Console.Write("Группа (Enter - Без группы): ");
        var group = Console.ReadLine() ?? "";
        Console.Write("Заметка: ");
        var note = Console.ReadLine() ?? "";

        var contact = new Contact(name)
        {
            Phone = phone,
            Email = email,
            Group = string.IsNullOrWhiteSpace(group) ? "Без группы" : group,
            Note = note
        };

        _notebook.AddContact(contact);
        Console.WriteLine("Контакт добавлен.");
    }

    static void RemoveContact()
    {
        Console.Write("Введите ФИО для удаления: ");
        var name = Console.ReadLine() ?? "";
        if (_notebook.RemoveContact(name))
            Console.WriteLine("Контакт удалён.");
        else
            Console.WriteLine("Контакт не найден.");
    }

    static void SearchContacts()
    {
        Console.Write("Поисковый запрос: ");
        var query = Console.ReadLine() ?? "";
        var results = _notebook.Search(query);

        if (results.Count == 0)
        {
            Console.WriteLine("Ничего не найдено.");
            return;
        }

        Console.WriteLine($"\nНайдено: {results.Count}");
        foreach (var c in results)
            Console.WriteLine("• " + c.GetInfo());
    }

    static void ShowGroups()
    {
        if (_notebook.Groups.Count == 0)
        {
            Console.WriteLine("Групп нет.");
            return;
        }

        Console.WriteLine("\nГруппы:");
        foreach (var g in _notebook.Groups)
            Console.WriteLine($"• {g.Name} — {g.Description}");
    }

    static void AddGroup()
    {
        Console.Write("Название группы: ");
        var name = Console.ReadLine() ?? "";
        Console.Write("Описание: ");
        var desc = Console.ReadLine() ?? "";

        _notebook.AddGroup(new Group(name) { Description = desc });
        Console.WriteLine("Группа добавлена.");
    }

    static void ShowContactsByGroup()
    {
        Console.Write("Название группы: ");
        var group = Console.ReadLine() ?? "";
        var list = _notebook.GetByGroup(group);

        if (list.Count == 0)
        {
            Console.WriteLine("В этой группе нет контактов.");
            return;
        }

        Console.WriteLine($"\nКонтакты в группе '{group}':");
        foreach (var c in list)
            Console.WriteLine("• " + c.GetInfo());
    }

    static void SeedDemoDataIfEmpty()
    {
        if (_notebook.Contacts.Count > 0) return;

        _notebook.AddGroup(new Group("Работа") { Description = "Коллеги" });
        _notebook.AddGroup(new Group("Друзья") { Description = "Личное" });
        _notebook.AddGroup(new Group("Семья") { Description = "Родные" });

        _notebook.AddContact(new Contact("Иванов Иван Иванович")
        {
            Phone = "+7-900-111-22-33",
            Email = "ivanov@example.com",
            Group = "Работа",
            Note = "Начальник отдела"
        });
        _notebook.AddContact(new Contact("Петрова Анна Сергеевна")
        {
            Phone = "+7-900-444-55-66",
            Email = "petrova@example.com",
            Group = "Друзья",
            Note = "Однокурсница"
        });
        _notebook.AddContact(new Contact("Сидоров Пётр Алексеевич")
        {
            Phone = "+7-900-777-88-99",
            Email = "sidorov@example.com",
            Group = "Семья",
            Note = "Брат"
        });

        _notebook.Save();
    }
}