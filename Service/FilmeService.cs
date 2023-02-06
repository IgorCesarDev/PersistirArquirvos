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

            Parallel.ForEach(lines, line =>
            {
                Filme filmeAtual = new Filme(line);
                Genero generoAtual = new Genero(line);

                var novosFilmes = new ConcurrentBag<Filme>();

                if (!filmesExistentes.ContainsKey(filmeAtual.Titulo))
                {
                    if (generosExistentes.TryGetValue(generoAtual.Nome, out Genero generoExistente))
                    {
                        filmeAtual.Generos.Add(generoExistente);
                        generoExistente.Filmes.Add(filmeAtual);
                        Console.WriteLine("f");
                    }
                    else
                    {
                        filmeAtual.Generos.Add(generoAtual);
                        generoAtual.Filmes.Add(filmeAtual);
                        _context.Generos.Add(generoAtual);
                        Console.WriteLine("t");
                    }
                    _context.Filmes.Add(filmeAtual);
                    novosFilmes.Add(filmeAtual);
                }
                Console.WriteLine("roda");
            });
            await _context.AddRangeAsync(novosFilmes);
           await _context.SaveChangesAsync();
        }


        //foreach (string line in lines)
        //{
        //    Filme filmeAtual = new Filme(line);
        //    Genero generoAtual = new Genero(line);

        //    Filme filmeExistente = _context.Filmes.FirstOrDefault(f => f.Titulo == filmeAtual.Titulo);

        //    if (filmeExistente == null)
        //    {
        //        Genero generoExistente = _context.Generos.FirstOrDefault(g => g.Nome == generoAtual.Nome);
        //        if (generoExistente != null)
        //        {
        //            filmeAtual.Generos.Add(generoExistente);
        //            generoExistente.Filmes.Add(filmeAtual);
        //            Console.WriteLine("s");
        //        }
        //        else
        //        {
        //            filmeAtual.Generos.Add(generoAtual);
        //            generoAtual.Filmes.Add(filmeAtual);
        //            _context.Generos.Add(generoAtual);
        //            Console.WriteLine("a");
        //        }
        //        _context.Filmes.Add(filmeAtual);
        //    }
        //    Console.WriteLine("f");
        //}
        //_context.SaveChanges();

    }
}
