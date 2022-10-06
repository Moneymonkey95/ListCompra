using Microsoft.Azure.Cosmos.Table;
using ListaCompra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaCompra.Services
{
    public class CompraService
    {
        private CloudTable tablaClientes;

        public CompraService(string azureKeys)
        {
            //PARA ACCEDER A LAS TABLAS Y LOS SERVICIOS NECESITAMOS  
            //UNA CLASE DE AZURESTORAGE 
            CloudStorageAccount account = CloudStorageAccount.Parse(azureKeys);
            CloudTableClient client = account.CreateCloudTableClient();
            this.tablaClientes = client.GetTableReference("ListaCompra");
            Task.Run(async () =>
            {
                await this.tablaClientes.CreateIfNotExistsAsync();
            });
        }

        public async Task CreateItemsAsync( string Nombre, Boolean Prioridad, string Nota)
        {
            string ID = await getMaxID();

            ItemCompra item = new ItemCompra
            {
                ID = ID,
                Nombre = Nombre,
                Comprado = false,
                Prioridad = Prioridad,
                Nota = Nota,
                PartitionKey = ID,
                RowKey = ID
            };

            //LAS CONSULTAS DE ACCION SE REALIZAN MEDIANTE OBJETOS 
            //DE TIPO TableOperation 
            //POSTERIORMENTE, SE EJECUTAN ESTAS CONSULTAS SOBRE LA PROPIA TABLE 
            TableOperation insert = TableOperation.Insert(item);
            await this.tablaClientes.ExecuteAsync(insert);
        }

        private async Task<string> getMaxID()
        {
            int maxID = 0;
            List<ItemCompra> lista = await GetItemsAsync();
            foreach (ItemCompra item in lista)
            {
                if (int.Parse(item.ID) > maxID) { maxID = int.Parse(item.ID); }
            }
            return (++maxID + "");
        }

        public async Task<List<ItemCompra>> GetItemsAsync()
        {
            TableQuery<ItemCompra> query = new TableQuery<ItemCompra>();
            TableQuerySegment<ItemCompra> segment = await this.tablaClientes.ExecuteQuerySegmentedAsync(query, null);
            List<ItemCompra> items = segment.Results;
            return items.Where(i => i.Comprado == false).OrderByDescending(i => i.Prioridad).ToList();
        }

        public async Task<List<ItemCompra>> GetItemsRemovedAsync()
        {
            TableQuery<ItemCompra> query = new TableQuery<ItemCompra>();
            TableQuerySegment<ItemCompra> segment = await this.tablaClientes.ExecuteQuerySegmentedAsync(query, null);
            List<ItemCompra> items = segment.Results;
            return items.Where(i => i.Comprado == true).OrderByDescending(i => i.Prioridad).ToList();
        }

        public async Task<ItemCompra> FindItemsAsync(string rowkey , string partitionkey)
        {
            TableOperation select = TableOperation.Retrieve<ItemCompra>(partitionkey, rowkey);
            TableResult result = await this.tablaClientes.ExecuteAsync(select);
            ItemCompra cliente = result.Result as ItemCompra;
            return cliente;
        }

        public async Task DeleteItemsAsync(ItemCompra item)
        {
            ItemCompra cliente = await this.FindItemsAsync(item.ID, item.ID);
            cliente.Comprado = true;
            TableOperation update = TableOperation.Replace(cliente);
            await this.tablaClientes.ExecuteAsync(update);
        }

    }

}
