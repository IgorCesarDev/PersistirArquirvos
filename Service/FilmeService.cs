using Microsoft.EntityFrameworkCore;
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
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        public FilmeService(AppDbContext context)
        {
            _context = context;
        }
        public void AdicionarFilmes(IEnumerable<string> lines)
        {
            var option = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            Parallel.ForEach(lines, option, line =>
            {
                _semaphoreSlim.Wait();
                try
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
                    }
                    Console.WriteLine("roda");
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            });
            _context.SaveChanges();
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
