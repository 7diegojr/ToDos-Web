using ToDoPlatform.Models;

namespace ToDoPlatform.ViewModels;

// transporta os dados do dashboard para a View

public class HomeVM
{
    public int TotalTasks { get; set; }
    public int OpenTasks { get; set; }
    public int EndedTasks { get; set; }

    // exibe uma lista de entidades sem transformar cada campo individualmente
    public List<ToDo> ToDos { get; set; } // lista de tarefas abertas a serem exibidas
}