using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.WinForms
{
    /// <summary>
    /// Carga una imagen desde memoria
    /// </summary>
    public class ImageMemorySchemeHandler : ResourceHandler
    {
        ///<summary>
        /// 
        /// </summary>
        public ImageMemorySchemeHandler()
        {

        }


        /// <summary>
        /// Search in the dictionary if exist and provide in the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public override CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback)
        {
            var uri = new Uri(request.Url);
            string url = System.Net.WebUtility.UrlDecode(request.Url);

            int localIndex = url.IndexOf("local/");
            string imageId = url.Substring(localIndex + 6);

            Bitmap image;
            ImageMemorySchemeHandlerFactory.Images.TryGetValue(imageId, out image);

            if (image != null)
            {
                Stream = new MemoryStream();
                image.Save(Stream, ImageFormat.Png);
                MimeType = GetMimeType(".png");
            }

            callback.Continue();
            return CefReturnValue.Continue;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ImageMemorySchemeHandlerFactory : ISchemeHandlerFactory
    {
        // Custom scheme
        public const string SchemeName = "memoryImage";

        // Static dictionary with image resources
        public static Dictionary<string, Bitmap> Images = new Dictionary<string, Bitmap>();

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new ImageMemorySchemeHandler();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="image"></param>
        public static void AddImage(string id, Bitmap image)
        {
            if (!Images.ContainsKey(id))
            {
                Images.Add(id, image);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Clear()
        {
            Images.Clear();
        }

    }

}
