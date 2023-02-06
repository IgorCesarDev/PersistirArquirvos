using PersistirArquirvos.Data;
using PersistirArquirvos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistirArquirvos.Service
{
    public class FilmeService
    {
        private AppDbContext _context;

        public FilmeService(AppDbContext context)
        {
            _context = context;
        }
        public void AdicionarFilmes(IEnumerable<string> lines)
        {
            int cont=0;
            int contadd=0;
            foreach (string line in lines)
            {
                Filme filmeAtual = new Filme(line);
                Genero generoAtual = new Genero(line);

                Filme filmeExistente = _context.Filmes.FirstOrDefault(f => f.Titulo == filmeAtual.Titulo);

                if (filmeExistente == null)
                {
                    Genero generoExistente = _context.Generos.FirstOrDefault(g => g.Nome == generoAtual.Nome);
                    if (generoExistente != null)
                    {
                        filmeAtual.Generos.Add(generoExistente);
                        generoExistente.Filmes.Add(filmeAtual);
                    }
                    else
                    {
                        filmeAtual.Generos.Add(generoAtual);
                        generoAtual.Filmes.Add(filmeAtual);
                        _context.Generos.Add(generoAtual);
                    }
                    _context.Filmes.Add(filmeAtual);
                    _context.SaveChanges();
                }
            }
        }
    }
}
