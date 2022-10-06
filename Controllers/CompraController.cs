using Microsoft.AspNetCore.Mvc;
using ListaCompra.Models;
using ListaCompra.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListaCompra.Controllers
{
    public class CompraController : Controller
    {
        private CompraService service;

        public CompraController(CompraService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> IndexSuper()
        {
            List<ItemCompra> items = await this.service.GetItemsAsync("Super");
            return View(items);
        }

        public async Task<IActionResult> HistorialSuper()
        {
            List<ItemCompra> items = await this.service.GetItemsRemovedAsync("Super");
            return View(items);
        }

        public IActionResult CreateSuper()
        {
            return View();
        }


       

        [HttpPost]
        public async Task<IActionResult> CreateSuper(ItemCompra item)
        {
            await this.service.CreateItemsAsync(item.Nombre, item.Prioridad, item.Nota, "Super");
            return RedirectToAction("IndexSuper");
        }




        // DECORACIÓN
        public IActionResult CreateDeco()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateDeco(ItemCompra item)
        {
            await this.service.CreateItemsAsync(item.Nombre, item.Prioridad, item.Nota, "Deco");
            return RedirectToAction("IndexDeco");
        }
        public async Task<IActionResult> IndexDeco()
        {
            List<ItemCompra> items = await this.service.GetItemsAsync("Deco");
            return View(items);
        }
        public async Task<IActionResult> HistorialDeco()
        {
            List<ItemCompra> items = await this.service.GetItemsRemovedAsync("Deco");
            return View(items);
        }
        





        public async Task<IActionResult> Comprar(ItemCompra item)
        {
            await this.service.ComprarItemsAsync(item);
            return RedirectToAction("Index"+item.Categoria);
        }

        public async Task<IActionResult> Borrar(ItemCompra item)
        {
            await this.service.BorrarItemsAsync(item);
            return RedirectToAction("Historial" + item.Categoria);
        }

    }

}
