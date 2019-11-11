using System;

namespace GestionObraWPF.DTOs
{
    public class ComprobanteDto : BaseDto
    {
    

        //public override bool Equals(object obj)
        //{
        //    if (obj is ComprobanteDto)
        //    {
        //        ComprobanteDto comprobante = obj as ComprobanteDto;
        //        return comprobante.Id == Id && comprobante.EstaEliminado == EstaEliminado
        //            &&  comprobante.UsuarioId == UsuarioId && comprobante.SubRubroId == SubRubroId
        //             && comprobante.Monto == Monto && comprobante.Detalle == Detalle && comprobante.Fecha == Fecha
        //             && comprobante.NumeroComprobante == NumeroComprobante && comprobante.Discriminator == Discriminator;
        //    }
        //    return false;
        //}
        //public override int GetHashCode()
        //{
        //    return Id.GetHashCode() & EstaEliminado.GetHashCode() 
        //         & UsuarioId.GetHashCode() & SubRubroId.GetHashCode()
        //        & Monto.GetHashCode() & Detalle.GetHashCode() & Fecha.GetHashCode() & NumeroComprobante.GetHashCode() & Discriminator.GetHashCode();
        //}
    }
}