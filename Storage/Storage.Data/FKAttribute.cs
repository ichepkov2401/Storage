namespace Storage.Data
{
    /// <summary>
    /// Аттрибут связи для JSON провайдера. Без него не будут работать навигационные свойства
    /// </summary>
    internal class FKAttribute : Attribute
    {
        public Type Relation { get; }

        public string FK { get; }

        public string PK { get; }

        public string FKNavigation { get; }

        public string PKNavigation { get; }

        /// <summary>
        /// Добавить к таблице имеющей внешний ключ
        /// </summary>
        /// <param name="relation">Тип сущности таблицы на которцю указывает связь</param>
        /// <param name="fK">Имя поля внешнего ключа</param>
        /// <param name="pk">Имя поля первичного ключа таблицы relation</param>
        /// <param name="fKNavigation">Имя навигационного свойства данной таблицы</param>
        /// <param name="pKNavigation">Имя навигационного свойcтва внешней таблицы (может быть простым или в виде списка)
        /// в зависимости от связи Один-к-одному или Один-к-многим</param>
        internal FKAttribute(Type relation, string fK, string pk, string fKNavigation, string pKNavigation)
        {
            Relation = relation;
            FK = fK;
            PK = pk;
            FKNavigation = fKNavigation;
            PKNavigation = pKNavigation;
        }
    }
}
