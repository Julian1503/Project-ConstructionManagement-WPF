using GestionObraWPF.Constantes;
using System;

namespace GestionObraWPF.DTOs
{
    public class PresupuestoDto : BaseDto
    {
        public long Numero { get; set; }
        public string Descripcion { get; set; } = "";
        public DateTime FechaPresupuesto { get; set; } = DateTime.Now;
        public DateTime FechaFacturacion { get; set; } = DateTime.Now;
        public long? Cae { get; set; }
        public long? NumeroFacturacion { get; set; }
        public EstadoPresupuesto EstadoPresupuesto { get; set; }
        public long EmpresaId { get; set; } // nuevo  
        public EmpresaDto Empresa { get; set; } // nuevo  
        public string Titulo { get; set; } = "";
        public string Observacion { get; set; } = "";
        public decimal Beneficio { get; set; }
        public decimal ImprevistoPorcentual { get; set; }
        public decimal Impuestos { get; set; }
        public decimal SubTotal { get; set; }
        public decimal PrecioCliente { get; set; }
        public long ObraId { get;  set; }
        public ObraDto Obra { get;  set; }
        public EstadoDeCobro EstadoDeCobro { get; set; }
        public decimal Cobrado { get; set; }
        public decimal Descuento { get; set; }
        public decimal Interes { get; set; }
        public bool Facturado { get; set; }
        public decimal Percepciones { get; set; }
        public decimal Retenciones { get; set; }
        public decimal Iva { get; set; }
        public decimal Total => PrecioCliente + Iva + Retenciones + Interes - Descuento + Percepciones;
        public decimal TotalSinImpuestos => ((PrecioCliente - Descuento) + Interes);
        public decimal CobradoSinImpuestos => Cobrado>TotalSinImpuestos? Cobrado - (Iva + Retenciones + Percepciones):Cobrado;
        public override bool Equals(object obj)
        {
            if (obj is PresupuestoDto)
            {
                PresupuestoDto presupuesto = obj as PresupuestoDto;
                return presupuesto.Id == Id && presupuesto.Descripcion == Descripcion && presupuesto.Numero == Numero && presupuesto.FechaPresupuesto == FechaPresupuesto && presupuesto.ImprevistoPorcentual == ImprevistoPorcentual && presupuesto.EstaEliminado == EstaEliminado && presupuesto.EstadoPresupuesto == EstadoPresupuesto && presupuesto.Titulo == Titulo && presupuesto.FechaPresupuesto== FechaPresupuesto && presupuesto.EmpresaId== EmpresaId && presupuesto.Observacion== Observacion && presupuesto.PrecioCliente== PrecioCliente && presupuesto.Beneficio== Beneficio && presupuesto.Impuestos== Impuestos && presupuesto.SubTotal==SubTotal && presupuesto.ObraId == ObraId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() & Descripcion.GetHashCode() & EstaEliminado.GetHashCode() & EstadoPresupuesto.GetHashCode() & FechaPresupuesto.GetHashCode() & Titulo.GetHashCode() & ImprevistoPorcentual.GetHashCode() & EstadoPresupuesto.GetHashCode() & EmpresaId.GetHashCode() & Titulo.GetHashCode() & Observacion.GetHashCode() & PrecioCliente.GetHashCode() & Beneficio.GetHashCode() & Impuestos.GetHashCode() & SubTotal.GetHashCode() & ObraId.GetHashCode();
        }
    }
}
