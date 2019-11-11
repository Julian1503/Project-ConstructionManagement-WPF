using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.Servicios
{
    public static class CloudImage
    {
        public static Cloudinary cloudinary { get; set; }
        public static Account Account { get; set; }
        public static void Inicializar()
        {
            Account = new Account(
               Constantes.Constantes.USUARIOCLOUD,
              Constantes.Constantes.TOKEN,
             Constantes.Constantes.SECRETTOKEN);
            cloudinary = new Cloudinary(Account);
        }

        public static string BuscarImagen()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription($"{op.FileName}")
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                return $@"{uploadResult.Uri}";
            }
            return "";
        }
    }
}
