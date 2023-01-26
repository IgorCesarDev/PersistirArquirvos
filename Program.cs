using PersistirArquirvos.Data;
using PersistirArquirvos.Models;
using System;

class Program
{
    static void Main(string[] args)
    {
        var path = @"arquivo_caminho";
        var lines = File.ReadAllLines(path).Skip(1);
        foreach (var line in lines)
        {
            using (var _context = new AppDbContext())
            {
                var filme = new Filme(line);
                var genero = new Genero(line);
                filme.Generos.Add(genero);
                genero.Filmes.Add(filme);
                _context.Filmes.Add(filme);
                _context.Generos.Add(genero);
                _context.SaveChanges();
            }
        }

    }
}