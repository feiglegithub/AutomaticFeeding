namespace NJIS.Common.Data
{
   public  interface IEntity<out TKey>
    {
        /// <summary>
        ///     获取 实体唯一标识，主键
        /// </summary>
        TKey Id { get; }
    }
}
