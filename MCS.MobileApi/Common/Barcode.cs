using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ZXing;
using ZXing.QrCode;

namespace MCS.MobileAPIs.Common
{
    public static class Barcode
    {
        public static string DrawBarcode(string barcodeText, BarcodeFormat symbology, int heigth, int width = 250, bool useDimensions = false)
        {
            var options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                //Width = width,
                //Height = heigth,
                PureBarcode = true
            };

            if (useDimensions)
            {
                options.Width = width;
                options.Height = heigth;
            }

            var qr = new ZXing.BarcodeWriter();
            qr.Options = options;
            qr.Format = symbology;
            var result = new Bitmap(qr.Write(barcodeText));
            Bitmap oImage = result; // I receive a bitmap.
            MemoryStream oMStream = new MemoryStream();
            oImage.Save(oMStream, ImageFormat.Png);
            //The Image is finally converted to Base64 string.

            return "data:image/png;base64," + Convert.ToBase64String(oMStream.ToArray());
        }

        public static string ImageToBase64(string _imagePath)
        {
            string _base64String = null;

            using (System.Drawing.Image _image = System.Drawing.Image.FromFile(_imagePath))
            {
                using (MemoryStream _mStream = new MemoryStream())
                {
                    _image.Save(_mStream, _image.RawFormat);
                    byte[] _imageBytes = _mStream.ToArray();
                    _base64String = Convert.ToBase64String(_imageBytes);

                    return "data:image/jpg;base64," + _base64String;
                }
            }
        }

    }
}