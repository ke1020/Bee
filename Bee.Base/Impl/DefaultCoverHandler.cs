
using Avalonia.Platform;

using Bee.Base.Abstractions;
using Bee.Base.Models;

using Microsoft.Extensions.Options;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Bee.Base.Impl;

/// <summary>
/// 默认封面处理器
/// </summary>
public class DefaultCoverHandler(IOptions<AppSettings> appSettings) : ICoverHandler
{
    private readonly string[] _availableImageFormats = ["jpg", "png", "bmp", "webp", "gif", "tif", "tiff", "tga"];
    /// <summary>
    /// 文件类型到 Uri 类型映射
    /// </summary>
    private readonly Dictionary<string, Uri> _fileTypeUriMaps = new()
    {
        // 视频
        { "mp4", ImageAssetConsts.Video },
        { "mkv", ImageAssetConsts.Video },
        { "avi", ImageAssetConsts.Video },
        { "webm", ImageAssetConsts.Video },
        { "flv", ImageAssetConsts.Video },
        { "mov", ImageAssetConsts.Video },
        { "wmv", ImageAssetConsts.Video },
        { "ogv", ImageAssetConsts.Video },
        { "ts", ImageAssetConsts.Video },
        { "mxf", ImageAssetConsts.Video },
        { "rm", ImageAssetConsts.Video },
        { "mpeg", ImageAssetConsts.Video },
        { "vob", ImageAssetConsts.Video },

        // 音频
        { "mp3", ImageAssetConsts.Audio },
        { "flac", ImageAssetConsts.Audio },
        { "ape", ImageAssetConsts.Audio },
        { "wav", ImageAssetConsts.Audio },
        { "aac", ImageAssetConsts.Audio },
        { "ogg", ImageAssetConsts.Audio },
        { "aiff", ImageAssetConsts.Audio },
        { "m4a", ImageAssetConsts.Audio },
        { "wma", ImageAssetConsts.Audio },
        { "ac3", ImageAssetConsts.Audio },
        { "mka", ImageAssetConsts.Audio },
        { "tak", ImageAssetConsts.Audio },
        { "amr", ImageAssetConsts.Audio },

        // 文档
        { "doc", ImageAssetConsts.Document },
        { "docx", ImageAssetConsts.Document },
        { "pdf", ImageAssetConsts.Document },
        { "xml", ImageAssetConsts.Document },
        { "xls", ImageAssetConsts.Document },
        { "xlsx", ImageAssetConsts.Document },
        { "txt", ImageAssetConsts.Document },
        { "md", ImageAssetConsts.Document },

        // 图表
        { ".puml", ImageAssetConsts.Uml }
    };

    private readonly AppSettings _appSettings = appSettings.Value;

    /// <summary>
    /// 获取封面
    /// </summary>
    /// <param name="file"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Stream> GetCoverAsync(string file, uint? width, uint? height, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var ext = Path.GetExtension(file).TrimStart('.').ToLower();

        // 如果是图片类型
        if (_availableImageFormats.Any(x => x.Equals(ext)))
        {
            // 根据原图片创建缩略图
            return await GetScaleStreamAsync(file, width, height, cancellationToken) ??
                AssetLoader.Open(new Uri(_appSettings.DefaultTaskImageUri))
                ;
        }

        if (_fileTypeUriMaps.TryGetValue(ext, out Uri? uri) && uri != null)
        {
            return AssetLoader.Open(uri);
        }

        return AssetLoader.Open(new Uri(_appSettings.DefaultTaskImageUri));
    }

    /// <summary>
    /// 根据源图缩放并返回 PNG 格式的内存流
    /// </summary>
    /// <param name="imageSource">源图</param>
    /// <param name="width">目标宽度</param>
    /// <param name="height">目标高度</param>
    /// <returns></returns>
    private async Task<Stream?> GetScaleStreamAsync(string imageSource, uint? w, uint? h, CancellationToken cancellationToken = default)
    {
        // 加载图片
        using var image = Image.Load(imageSource);

        Scale(image, w, h);

        var memoryStream = new MemoryStream();
        await image.SaveAsync(memoryStream, new PngEncoder(), cancellationToken);
        memoryStream.Position = 0;
        return memoryStream;
    }

    /// <summary>
    /// 缩放图片
    /// </summary>
    /// <param name="image"></param>
    /// <param name="w"></param>
    /// <param name="h"></param>
    private void Scale(Image image, uint? w, uint? h)
    {
        // 目标宽高
        int targetWidth = (int)(w ?? 80);
        int targetHeight = (int)(h ?? 80);

        int originalWidth = image.Width;
        int originalHeight = image.Height;

        // 计算缩放比例
        double scaleX = (double)targetWidth / originalWidth;
        double scaleY = (double)targetHeight / originalHeight;
        double scale = Math.Max(scaleX, scaleY);

        // 计算缩放后的尺寸
        int scaledWidth = (int)(originalWidth * scale);
        int scaledHeight = (int)(originalHeight * scale);

        // 计算裁剪区域
        int cropX = (scaledWidth - targetWidth) / 2;
        int cropY = (scaledHeight - targetHeight) / 2;

        // 确保裁剪区域不会超出缩放后的图像边界
        cropX = Math.Max(0, cropX);
        cropY = Math.Max(0, cropY);
        targetWidth = Math.Min(targetWidth, scaledWidth - cropX);
        targetHeight = Math.Min(targetHeight, scaledHeight - cropY);

        // 缩放图片
        image.Mutate(x => x.Resize(scaledWidth, scaledHeight));

        // 裁剪图片
        image.Mutate(x => x.Crop(new Rectangle(cropX, cropY, targetWidth, targetHeight)));
    }
}