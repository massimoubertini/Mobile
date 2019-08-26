using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Xamarin.Forms;
using SkiaSharpFormsDemos.UWP;

[assembly: Dependency(typeof(PhotoLibrary))]

namespace SkiaSharpFormsDemos.UWP
{
    public class PhotoLibrary : IPhotoLibrary
    {
        public async Task<Stream> PickPhotoAsync()
        {
            // crea e inizializzare FileOpenPicker
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            // ottiene un file e restituire uno Stream
            StorageFile storageFile = await openPicker.PickSingleFileAsync();
            if (storageFile == null)
                return null;
            IRandomAccessStreamWithContentType raStream = await storageFile.OpenReadAsync();
            return raStream.AsStreamForRead();
        }

        public async Task<bool> SavePhotoAsync(byte[] data, string folder, string filename)
        {
            StorageFolder picturesDirectory = KnownFolders.PicturesLibrary;
            StorageFolder folderDirectory = picturesDirectory;
            // ottiene la cartella o crearla se necessario
            if (!string.IsNullOrEmpty(folder))
            {
                try
                {
                    folderDirectory = await picturesDirectory.GetFolderAsync(folder);
                }
                catch
                { }
                if (folderDirectory == null)
                {
                    try
                    {
                        folderDirectory = await picturesDirectory.CreateFolderAsync(folder);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            try
            {
                // crea il file
                StorageFile storageFile = await folderDirectory.CreateFileAsync(filename, CreationCollisionOption.GenerateUniqueName);
                // converte byte[] in buffer di Windows e lo scrive
                IBuffer buffer = WindowsRuntimeBuffer.Create(data, 0, data.Length, data.Length);
                await FileIO.WriteBufferAsync(storageFile, buffer);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}