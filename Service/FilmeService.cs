using Microsoft.EntityFrameworkCore;
using PersistirArquirvos.Data;
using PersistirArquirvos.Models;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
        public async void AdicionarFilmes(IEnumerable<string> lines)
        {
            var filmesExistentes = await _context.Filmes.ToDictionaryAsync(f => f.Titulo);
            var generosExistentes = await _context.Generos.ToDictionaryAsync(g => g.Nome);

            var novosFilmes = new ConcurrentBag<Filme>();
            var novosGeneros = new ConcurrentBag<Genero>();

            Parallel.ForEach(lines, line =>
            {
                Filme filmeAtual = new Filme(line);
                Genero generoAtual = new Genero(line);

                if (!filmesExistentes.ContainsKey(filmeAtual.Titulo))
                {
                    if (generosExistentes.TryGetValue(generoAtual.Nome, out Genero generoExistente))
                    {
                        filmeAtual.Generos.Add(generoExistente);
                    }
                    else
                    {
                        novosGeneros.Add(generoAtual);
                        filmeAtual.Generos.Add(generoAtual);
                    }
                    novosFilmes.Add(filmeAtual);
                }
            });
            await _context.AddRangeAsync(novosFilmes);
            await _context.AddRangeAsync(novosGeneros);
            await _context.SaveChangesAsync();
        }
    }
}
