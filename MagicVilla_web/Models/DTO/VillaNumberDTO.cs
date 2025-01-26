using System.ComponentModel.DataAnnotations;

namespace MagicVilla_web.Models.DTO
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillId { get; set; }
        public string SpecialDetails { get; set; }

        public VillaDto Villa { get; set; }
    }
}
