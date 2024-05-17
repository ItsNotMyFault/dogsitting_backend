namespace dogsitting_backend.ApplicationServices.dto
{
    public class ReservationDto
    {
        public required DateTime DateFrom { get; set; }
        public required DateTime DateTo { get; set; }
        public required int LodgerCount { get; set; }
        public required string Notes { get; set; }
    }
}
