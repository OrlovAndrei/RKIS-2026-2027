public class ReadCommand : TaskCommand
{
    protected override void Run()
    {
        Console.WriteLine($"Задача №{Index + 1}:");
        Console.WriteLine(TodoList[Index].GetFullInfo());
    }
}
