using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaCompra.Models { 
    public class ItemCompra : TableEntity
    {
        private string _ID { get; set; }
        public string ID { get { return this._ID; } set { this.PartitionKey = value; this.RowKey = value; this._ID = value; } }
        public String Nombre { get; set; }
        public Boolean Comprado { get; set; }
        public string Nota { get; set; }
        public Boolean Prioridad { get; set; }
    }

}

