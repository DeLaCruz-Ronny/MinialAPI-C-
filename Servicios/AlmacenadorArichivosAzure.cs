using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace minimalAPIPeliculas.Servicios
{
    public class AlmacenadorArichivosAzure : IAlmacenadorArchivos
    {
        private string connectionString;

        public AlmacenadorArichivosAzure(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage")!;
        }
        public async Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            
            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(PublicAccessType.Blob);

            var extension = Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(nombreArchivo);
            var blobHttpHeader = new BlobHttpHeaders();
            blobHttpHeader.ContentType = archivo.ContentType;
            await blob.UploadAsync(archivo.OpenReadStream(), blobHttpHeader);
            return blob.Uri.ToString();
        }

        public async Task Borrar(string? ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return;
            }

            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            var nombreArchivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(nombreArchivo);
            await blob.DeleteIfExistsAsync();
        }
    }
}