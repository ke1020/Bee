
namespace Bee.Base.Models.Tasks;

public static class TaskExtensions
{
    /// <summary>
    /// 合并图像集合
    /// </summary>
    /// <param name="source">源图像对象集合</param>
    /// <param name="newImages">要合并至源集合对象的新集合</param>
    public static void Merge(this ICollection<TaskItem> source, ICollection<TaskItem> newImages)
    {
        if (newImages == null)
        {
            return;
        }

        var existingSources = source.Select(i => i.FileName).ToHashSet();
        foreach (var newItem in newImages)
        {
            if (!existingSources.Contains(newItem.FileName))
            {
                source.Add(newItem);
            }
        }
    }
}