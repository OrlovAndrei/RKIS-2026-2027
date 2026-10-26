using System;
using System.Collections.Generic;

namespace TodoListApp
{
    // Пункт 1: Статический класс глобального состояния приложения
    public static class AppInfo
    {
        public static TodoList Todos { get; set; } = new TodoList();
        // Пункт 4: Хранение списка всех профилей и Id текущего пользователя
        public static List<Profile> AllProfiles { get; set; } = new List<Profile>();
        public static Guid? CurrentProfileId { get; set; } = null;

        // Вспомогательное свойство для быстрого доступа к данным текущего юзера
        public static Profile? CurrentProfile
        {
            get
            {
                if (CurrentProfileId == null) return null;
                return AllProfiles.Find(p => p.Id == CurrentProfileId);
            }
        }
        public static Stack<ICommand> UndoStack { get; } = new Stack<ICommand>();
        public static Stack<ICommand> RedoStack { get; } = new Stack<ICommand>();

    }
}
