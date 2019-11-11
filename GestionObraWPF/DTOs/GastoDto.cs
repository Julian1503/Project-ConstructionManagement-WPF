namespace GestionObraWPF.DTOs
{
    public class GastoDto : BaseDto
    {
        public long TipoGastoId { get; set; }
        public decimal Monto { get; set; }
        public long PresupuestoId { get; set; }
        public PresupuestoDto Presupuesto { get; set; }
        public TipoGastoDto TipoGasto { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is GastoDto)
            {
                GastoDto gasto = obj as GastoDto;
                return gasto.Id == Id && gasto.TipoGastoId == TipoGastoId &&
                    gasto.EstaEliminado == EstaEliminado && gasto.Monto == Monto &&
                    gasto.PresupuestoId == PresupuestoId && gasto.TipoGastoId == TipoGastoId;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & Monto.GetHashCode() & EstaEliminado.GetHashCode() & TipoGastoId.GetHashCode() & PresupuestoId.GetHashCode();
        }
    }
}
