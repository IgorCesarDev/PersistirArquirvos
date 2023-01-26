using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PersistirArquirvos.Models
{
    public class Filme
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public List<Genero> Generos { get; set; } = new();
        public int FaixaEtaria { get; set; }
        public Filme()
        {

        }
        public Filme(string line)
        {
            var columns = line.Split('\t');
            var columnsId = BuscarId(columns[0]);

            this.Id = columnsId;
            this.Titulo = columns[2];
            this.SubTitulo = columns[3];
            this.FaixaEtaria = SorteiaData();
        }

        private int SorteiaData()
        {
            Random random = new Random();
            int numAletorio = random.Next(1, 4);
            if (numAletorio == 1) { return 10; }
            if (numAletorio == 2) { return 15; }
            if (numAletorio == 3) { return 18; }
            return 999;
        }

        public static int BuscarId(string colunaId)
        {
            Match resultado = Regex.Match(colunaId, @"[0123456789]{7}");
            var resultadoID = resultado.Value;
            return int.Parse(resultadoID);
        }
    }
}
