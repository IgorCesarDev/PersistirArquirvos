using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PersistirArquirvos.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Filme> Filmes { get; set; } = new();
        public Genero()
        {

        }
        public Genero(string line)
        {
            var columns = line.Split('\t');
            var columnsId = BuscarId(columns[0]);

            this.Id = columnsId;
            this.Nome = columns[8].Replace(",", "/");
        }
        private int BuscarId(string colunaId)
        {
            Match resultado = Regex.Match(colunaId, @"[0123456789]{7}");
            var resultadoID = resultado.Value;
            return int.Parse(resultadoID);
        }
    }
}
