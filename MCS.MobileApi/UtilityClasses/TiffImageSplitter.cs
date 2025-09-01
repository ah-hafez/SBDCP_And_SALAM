using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MobileApi.UtilityClasses
{
    public class TiffImageSplitter
    {
        // Retrive PageCount of a multi-page tiff image
        public int GetPageCount(Stream stream)
        {
            int pageCount = 0;

            Image img = Bitmap.FromStream(stream);

            pageCount = img.GetFrameCount(FrameDimension.Page);
            img.Dispose();

            return pageCount;
        }

        public int GetPageCount(Image img)
        {
            int pageCount = 0;

            pageCount = img.GetFrameCount(FrameDimension.Page);

            return pageCount;
        }

        // Retrive a specific Page from a multi-page tiff image
        public Image GetTiffImage(Stream stream, int pageNumber)
        {
            Image returnImage = null;

            try
            {
                Image sourceIamge = Bitmap.FromStream(stream);

                returnImage = GetTiffImage(sourceIamge, pageNumber);
                sourceIamge.Dispose();
            }
            catch (Exception)
            {
                returnImage = null;
            }

            return returnImage;
        }

        public Image GetTiffImage(Image sourceImage, int pageNumber)
        {
            MemoryStream ms = null;
            Image returnImage = null;

            try
            {
                ms = new MemoryStream();
                Guid objGuid = sourceImage.FrameDimensionsList[0];
                FrameDimension objDimension = new FrameDimension(objGuid);
                sourceImage.SelectActiveFrame(objDimension, pageNumber);
                sourceImage.Save(ms, ImageFormat.Tiff);
                returnImage = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                returnImage = null;
            }
            return returnImage;
        }
    }
}