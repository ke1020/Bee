
namespace Bee.Base.Models.Tasks;

public static class TaskExtensions
{
    /// <summary>
    /// 合并图像集合
    /// </summary>
    /// <param name="source">源对象集合</param>
    /// <param name="newItems">要合并至源集合对象的新集合</param>
    public static void Merge(this ICollection<TaskItem> source, ICollection<TaskItem> newItems)
    {
        if (newItems == null)
        {
            return;
        }

        var existingSources = source.Select(i => i.Input).ToHashSet();
        foreach (var newItem in newItems)
        {
            if (!existingSources.Contains(newItem.Input))
            {
                source.Add(newItem);
            }
        }
    }
}