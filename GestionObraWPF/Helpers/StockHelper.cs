using GestionObraWPF.DTOs;
using GestionObraWPF.Model;
using GestionObraWPF.Servicios;
using System;
using System.Threading.Tasks;

namespace GestionObraWPF.Helpers
{
    public static class StockHelper
    {
        public async static void AgregarStock(long productoId, int cantidad)
        {
            var stock = await ApiProcessor.GetApi<StockDto>($"Stock/GetByLastOne/{productoId}");
            if (stock == null)
            {
                stock = new StockDto
                {
                    MaterialId = productoId,
                    StockActual = cantidad,
                    UsuarioId = UsuarioGral.UsuarioId,
                    StockMinimo = 0,
                    EstaEliminado = false,
                    FechaActualizacion = DateTime.Now
                };
                await ApiProcessor.PostApi(stock, "Stock/Insert");
            }
            else
            {
                stock.StockActual += cantidad;
                stock.FechaActualizacion = DateTime.Now;
                await ApiProcessor.PostApi(stock, $"Stock/Insert");
            }
        }

        public async static Task<bool> ConsultarStock(long productoId, int cantidad)
        {
            return await ApiProcessor.GetApi<bool>($"Stock/GetUpdate/{productoId}/{cantidad}");
        }

        public async static void QuitarStock(long productoId, int cantidad)
        {
            var stock = await ApiProcessor.GetApi<StockDto>($"Stock/GetByLastOne/{productoId}");
            if (stock != null && stock.StockActual > 0)
            {
                stock.StockActual -= cantidad;
                stock.FechaActualizacion = DateTime.Now;
                await ApiProcessor.PostApi(stock, $"Stock/Insert");
            }
        }
    }
}
