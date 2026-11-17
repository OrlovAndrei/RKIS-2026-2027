using System;

class ViewCommand : ICommand
{
    public TodoList? TodoList { get; set; }
    public bool ShowIndex { get; set; }
    public bool ShowStatus { get; set; }
    public bool ShowDate { get; set; }

    public void Execute()
    {
        if (TodoList == null)
        {
            return;
        }

        TodoList.View(ShowIndex, ShowStatus, ShowDate);
    }
}

