using System.ComponentModel.DataAnnotations;

namespace SGEP.Models
{
    public class TestingModel
    {
        [Range(0, 10, ErrorMessage="AsdasdaSDASDASdasdasD")]
        public int Teste { get; set; }
    }
}